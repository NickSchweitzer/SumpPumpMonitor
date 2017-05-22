using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Windows;
using System.Configuration;
using System.Net.NetworkInformation;
using System.Windows.Threading;
using System.Threading.Tasks;
using PostSharp.Patterns.Model;
using Prism.Commands;
using Newtonsoft.Json;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Message = Microsoft.Azure.Devices.Client.Message;

using CodingMonkeyNet.SumpPumpMonitor.IoT.Messages;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json.Serialization;

namespace CodingMonkeyNet.SumpPumpMonitor.Emulator
{
    [NotifyPropertyChanged]
    public partial class MainWindow : Window
    {
        private const string ConfigFile = "config.json";
        private readonly DispatcherTimer waterLevelTimer;
        private readonly string DeviceId;
        private bool highWaterError = false;
        private DeviceClient iotClient;
        private SumpPumpSettings settings = new SumpPumpSettings();
        private const Microsoft.Azure.Devices.Client.TransportType ClientTransportType = Microsoft.Azure.Devices.Client.TransportType.Mqtt;

        public MainWindow()
        {
            DeviceId = GetDeviceId();

            // When the emulator is started via the command button, this timer controls the sending of messages to Azure IoT Hub
            waterLevelTimer = new DispatcherTimer();
            waterLevelTimer.Tick += waterLevelTimer_Tick;
            waterLevelTimer.Interval = new TimeSpan(0, 0, 1);           // Every second

            ToggleEmulatorCommand = new DelegateCommand<string>(ToggleEmulator, CanStartEmulator);

            InitializeComponent();
            DataContext = this;

            FillTime = 5 * 60;  // 5 min
            DutyCycle = 10;     // 10 seconds

            TurnPumpOffLevel = 1;  // 1 inch
            TurnPumpOnLevel = 10; // 10 inches
            EmulatorAction = "Start Emulator";
        }

        protected async override void OnInitialized(EventArgs e)
        {
            await RegisterEmulator();
            base.OnInitialized(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            waterLevelTimer.Stop();
            iotClient.Dispose();
            base.OnClosed(e);
        }

        private async void waterLevelTimer_Tick(object sender, EventArgs e)
        {
            if (PumpStatus) // Pump Running - Emptying
            {
                double decrementAmount = (double)(TurnPumpOnLevel - TurnPumpOffLevel) / DutyCycle;
                double waterLevel = CurrentWaterLevel - decrementAmount;
                if (waterLevel <= TurnPumpOffLevel)
                {
                    CurrentWaterLevel = TurnPumpOffLevel;
                    PumpStatus = false;
                }
                else
                    CurrentWaterLevel = waterLevel;
            }
            else            // Pump Stopped - Filling
            {
                double incrementAmount = (double)(TurnPumpOnLevel - TurnPumpOffLevel) / FillTime;
                double waterLevel = CurrentWaterLevel + incrementAmount;
                if (waterLevel >= TurnPumpOnLevel)
                {
                    CurrentWaterLevel = TurnPumpOnLevel;
                    PumpStatus = true;
                }
                else
                    CurrentWaterLevel = waterLevel;
            }

            if (CurrentWaterLevel > MaxWaterLevel && !highWaterError)
            {
                highWaterError = true;
                SendAlert(AlertPayloadType.HighWater);
            }

            await SendDataPoint();
        }

        private string GetDeviceId()
        {
            return (
                from nic in NetworkInterface.GetAllNetworkInterfaces()
                where nic.OperationalStatus == OperationalStatus.Up
                select nic.GetPhysicalAddress().ToString()
                ).FirstOrDefault();
        }

        private async Task RegisterEmulator()
        {
            string hostName = ConfigurationManager.AppSettings["HostName"];
            string sharedAccessKeyName = ConfigurationManager.AppSettings["SharedAccessKeyName"];
            string sharedAccessKey = ConfigurationManager.AppSettings["SharedAccessKey"];
            string iotHubConnectionString = string.Format("HostName={0};SharedAccessKeyName={1};SharedAccessKey={2}", hostName, sharedAccessKeyName, sharedAccessKey);

            if (string.IsNullOrEmpty(iotHubConnectionString) || string.IsNullOrEmpty(DeviceId))
                return; // Shouldn't happen - figure out what to do here

            using (var manager = RegistryManager.CreateFromConnectionString(iotHubConnectionString))
            {
                var iotDevice = await manager.GetDeviceAsync(DeviceId);
                if (iotDevice == null)
                    iotDevice = await manager.AddDeviceAsync(new Device(DeviceId));

                iotClient = DeviceClient.Create(hostName,
                    new DeviceAuthenticationWithRegistrySymmetricKey(DeviceId, iotDevice.Authentication.SymmetricKey.PrimaryKey),
                    ClientTransportType);

                await iotClient.OpenAsync();
                await GetInitialConfiguration();
                await iotClient.SetDesiredPropertyUpdateCallback(OnDesiredPropertyChanged, null);
            }
        }

        private async Task GetInitialConfiguration()
        {
            // First - Read settings from a file
            SumpPumpSettings fileSettings = null;
            SumpPumpSettings twinSettings = null;
            if (File.Exists(ConfigFile))
                fileSettings = JsonConvert.DeserializeObject<SumpPumpSettings>(File.ReadAllText(ConfigFile));

            // Now grab the device twin settings
            var twin = await iotClient.GetTwinAsync();
            if (twin != null)
                twinSettings = JsonConvert.DeserializeObject<SumpPumpSettings>(twin.Properties.Desired.ToString());

            // Take the best version of the settings
            if (twinSettings != null)
                settings = twinSettings;
            else if (fileSettings != null)
                settings = fileSettings;

            // Report back to the device twin that we took the new settings
            if (twinSettings != null)
                await SaveSettings(fileSettings, twinSettings);
        }

        private async Task OnDesiredPropertyChanged(TwinCollection desiredProperties, object userContext)
        {
            string jsonPatch = desiredProperties.ToJson();
            JsonSerializer ser = new JsonSerializer();
            using (var reader = new StringReader(jsonPatch))
            {
                ser.Populate(reader, settings);
            }

            NotifyPropertyChangedServices.SignalPropertyChanged(this, "DeviceName");
            NotifyPropertyChangedServices.SignalPropertyChanged(this, "MaxWaterLevel");

            // Sending null for currentSettings will force a save and update of Device Twin
            await SaveSettings(null, settings);
        }

        private async Task SaveSettings(SumpPumpSettings currentSettings, SumpPumpSettings newSettings)
        {
            if (currentSettings == null || currentSettings.DeviceName != newSettings.DeviceName || currentSettings.MaxWaterLevel != newSettings.MaxWaterLevel)
            {
                // Save settings to a file
                string newSettingsJson = JsonConvert.SerializeObject(newSettings);
                using (var writer = File.CreateText(ConfigFile))
                {
                    writer.WriteLine(newSettingsJson);
                    writer.Flush();
                    writer.Close();
                }

                // Tell Azure we took their settings
                var serializerSettings = new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                await iotClient.UpdateReportedPropertiesAsync(JsonConvert.DeserializeObject<TwinCollection>(newSettingsJson, serializerSettings));
            }
        }

        private async Task SendAlert(AlertPayloadType type)
        {
            var payload = new AlertPayload
            {
                DeviceId = DeviceId,
                Timestamp = DateTime.Now,
                WaterLevel = CurrentWaterLevel,
                PumpRunning = PumpStatus,
                Type = type
            };

            var json = JsonConvert.SerializeObject(payload);
            using (var message = new Message(Encoding.UTF8.GetBytes(json)))
            {
                message.Properties.Add("DeviceName", DeviceId);
                await iotClient.SendEventAsync(message);
            }
        }

        private async Task SendDataPoint()
        {
            var payload = new DataPointPayload
            {
                DeviceId = DeviceId,
                Timestamp = DateTime.Now,
                WaterLevel = CurrentWaterLevel,
                PumpRunning = PumpStatus
            };

            var json = JsonConvert.SerializeObject(payload);
            using (var message = new Message(Encoding.UTF8.GetBytes(json)))
            {
                message.Properties.Add("DeviceName", DeviceId);
                await iotClient.SendEventAsync(message);
            }
        }

        public string DeviceName
        {
            get { return settings.DeviceName; }
            set { settings.DeviceName = value; }
        }

        public double MaxWaterLevel                 // Inches - When should the system send an alert?
        {
            get { return settings.MaxWaterLevel; }
            set { settings.MaxWaterLevel = value; }
        }      

        public int FillTime { get; set; }   // Seconds to fill
        public int DutyCycle { get; set; }  // Seconds to empty

        public int TurnPumpOffLevel { get; set; }   // Inches - Simulates when the pump should turn off
        public int TurnPumpOnLevel { get; set; }    // Inches - Simulates when the pump should turn on

        public double CurrentWaterLevel { get; set; }    // Inches
        public bool PumpStatus { get; set; }

        public string EmulatorAction { get; set; }

        public DelegateCommand<string> ToggleEmulatorCommand { get; private set; }

        public bool CanStartEmulator(string value)
        {
            return true;
        }

        public async void ToggleEmulator(string value)
        {
            if (iotClient == null)
                await RegisterEmulator();

            if (iotClient != null)
            {
                if (waterLevelTimer.IsEnabled)
                {
                    EmulatorAction = "Start Emulator";
                    waterLevelTimer.Stop();
                }
                else
                {
                    EmulatorAction = "Stop Emulator";
                    CurrentWaterLevel = 0;
                    waterLevelTimer.Start();
                }
            }
        }   
    }
}