using System;
using Newtonsoft.Json;

namespace CodingMonkeyNet.SumpPumpMonitor.IoT.Messages
{
    public class DataPointPayload
    {
        [JsonProperty(PropertyName = "deviceId", Order = 1)]
        public string DeviceId { get; set; }
        [JsonProperty(PropertyName = "timeStamp", Order = 2)]
        public DateTime Timestamp { get; set; }
        [JsonProperty(PropertyName = "waterLevel", Order = 3)]
        public Double WaterLevel { get; set; }
        [JsonProperty(PropertyName = "pumpRunning", Order = 4)]
        public bool PumpRunning { get; set; }
    }
}
