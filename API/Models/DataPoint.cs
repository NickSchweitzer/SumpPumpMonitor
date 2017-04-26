using System;

namespace CodingMonkeyNet.SumpPumpMonitor.API.Models
{
    public class DataPoint
    {
        public string PumpId { get; set; }
        public DateTime TimeStamp { get; set; }
        public Double WaterLevel { get; set; }
        public bool PumpRunning { get; set; }
    }
}
