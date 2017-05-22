using System;
using Newtonsoft.Json;

namespace CodingMonkeyNet.SumpPumpMonitor.IoT.Messages
{
    public class AlertPayload
    {
        [JsonProperty(PropertyName = "deviceId", Order = 1)]
        public string DeviceId { get; set; }
        [JsonProperty(PropertyName = "timeStamp", Order = 2)]
        public DateTime Timestamp { get; set; }
        [JsonProperty(PropertyName = "type", Order = 3)]
        public AlertPayloadType Type { get; set; }
        [JsonProperty(PropertyName = "waterLevel", Order = 4)]
        public Double WaterLevel { get; set; }
        [JsonProperty(PropertyName = "pumpRunning", Order = 5)]
        public bool PumpRunning { get; set; }
    }

    public enum AlertPayloadType : int
    {
        HighWater = 0,              // Sump Pump Reached High Water Mark
        PumpNotStopped = 1,         // Pump still running with no water level change
    }
}