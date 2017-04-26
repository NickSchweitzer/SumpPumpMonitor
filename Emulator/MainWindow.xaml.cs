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
        private readonly DispatcherTimer timer;
        private readonly string DeviceId;
        private Device IoTDevice;
        private const Microsoft.Azure.Devices.Client.TransportType ClientTransportType = Microsoft.Azure.Devices.Client.TransportType.Amqp;

        public MainWindow()
        {
            DeviceId = GetDeviceId();
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1); // Every second

            ToggleEmulatorCommand = new DelegateCommand<string>(ToggleEmulator, CanStartEmulator);

            InitializeComponent();
            DataContext = this;

            FillTime = 5 * 60;  // 5 min
            DutyCycle = 10;     // 10 seconds

            MinWaterLevel = 1;  // 1 inch
            MaxWaterLevel = 10; // 10 inches
            EmulatorAction = "Start Emulator";
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            if (PumpStatus) // Pump Running - Emptying
            {
                double decrementAmount = (double)(MaxWaterLevel - MinWaterLevel) / DutyCycle;
                double waterLevel = CurrentWaterLevel - decrementAmount;
                if (waterLevel <= MinWaterLevel)
                {
                    CurrentWaterLevel = MinWaterLevel;
                    PumpStatus = false;
                }
                else
                    CurrentWaterLevel = waterLevel;
            }
            else            // Pump Stopped - Filling
            {
                double incrementAmount = (double)(MaxWaterLevel - MinWaterLevel) / FillTime;
                double waterLevel = CurrentWaterLevel + incrementAmount;
                if (waterLevel >= MaxWaterLevel)
                {
                    CurrentWaterLevel = MaxWaterLevel;
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
            string iotHubConnectionString = GetConnectionString();
            if (string.IsNullOrEmpty(iotHubConnectionString) || string.IsNullOrEmpty(DeviceId))
                return; // Shouldn't happen - figure out what to do here

            using (var manager = RegistryManager.CreateFromConnectionString(iotHubConnectionString))
            {
                IoTDevice = await manager.GetDeviceAsync(DeviceId);
                if (IoTDevice == null)
                    IoTDevice = await manager.AddDeviceAsync(new Device(DeviceId));
            }
        }

        private async Task SendDataPoint()
        {
            string hostName = ConfigurationManager.AppSettings["HostName"];
            using (var client = DeviceClient.Create(
                hostName,
                new DeviceAuthenticationWithRegistrySymmetricKey(DeviceId, IoTDevice.Authentication.SymmetricKey.PrimaryKey),
                ClientTransportType))
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
                    await client.SendEventAsync(message);
                }
            }
        }

        private string GetConnectionString()
        {
            string hostName = ConfigurationManager.AppSettings["HostName"];
            string sharedAccessKeyName = ConfigurationManager.AppSettings["SharedAccessKeyName"];
            string sharedAccessKey = ConfigurationManager.AppSettings["SharedAccessKey"];
            return string.Format("HostName={0};SharedAccessKeyName={1};SharedAccessKey={2}", hostName, sharedAccessKeyName, sharedAccessKey);
        }

        public int FillTime { get; set; }   // Seconds to fill
        public int DutyCycle { get; set; }  // Seconds to empty

        public int MinWaterLevel { get; set; }  // Inches
        public int MaxWaterLevel { get; set; }  // Inches

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
            if (IoTDevice == null)
                await RegisterEmulator();

            if (IoTDevice != null)
            {
                if (timer.IsEnabled)
                {
                    EmulatorAction = "Start Emulator";
                    timer.Stop();
                }
                else
                {
                    EmulatorAction = "Stop Emulator";
                    CurrentWaterLevel = 0;
                    timer.Start();
                }
            }
        }   
    }
}
