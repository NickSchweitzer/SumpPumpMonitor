using System;
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

namespace CodingMonkeyNet.SumpPumpMonitor.Emulator
{
    [NotifyPropertyChanged]
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer waterLevelTimer;
        private readonly DispatcherTimer receiveMessageTimer;
        private readonly string DeviceId;
        private DeviceClient iotClient;
        private const Microsoft.Azure.Devices.Client.TransportType ClientTransportType = Microsoft.Azure.Devices.Client.TransportType.Amqp;

        public MainWindow()
        {
            DeviceId = GetDeviceId();

            // When the emulator is started via the command button, this timer controls the sending of messages to Azure IoT Hub
            waterLevelTimer = new DispatcherTimer();
            waterLevelTimer.Tick += waterLevelTimer_Tick;
            waterLevelTimer.Interval = new TimeSpan(0, 0, 1);           // Every second

            // As soon as the emulator program starts, we connect to IoT hub to receive any messages from Azure IoT Hub
            receiveMessageTimer = new DispatcherTimer();
            receiveMessageTimer.Tick += receiveMessageTimer_Tick;
            receiveMessageTimer.Interval = new TimeSpan(0, 0, 10);      // Every 10 seconds

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
            receiveMessageTimer.Start();
            base.OnInitialized(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            receiveMessageTimer.Stop();
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
            //await Task.Delay(500);
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
            }
        }

        private async void receiveMessageTimer_Tick(object sender, EventArgs e)
        {
            Message receivedMessage = await iotClient.ReceiveAsync();
            if (receivedMessage != null)
            {
                SumpPumpSettingsMessage sumpPumpMsg = JsonConvert.DeserializeObject<SumpPumpSettingsMessage>(Encoding.ASCII.GetString(receivedMessage.GetBytes()));
                MaxWaterLevel = sumpPumpMsg.MaxWaterLevel;
                await iotClient.CompleteAsync(receivedMessage);
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

        public int FillTime { get; set; }   // Seconds to fill
        public int DutyCycle { get; set; }  // Seconds to empty

        public int TurnPumpOffLevel { get; set; }   // Inches - Simulates when the pump should turn off
        public int TurnPumpOnLevel { get; set; }    // Inches - Simulates when the pump should turn on
        public double MaxWaterLevel { get; set; }      // Inches - When should the system send an alert?

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
