using System;

namespace CodingMonkeyNet.SumpPumpMonitor.API.Models
{
    public class DutyCycle
    {
        public string PumpId { get; set; }
        public DateTime TimeStamp { get; set; }
        public DutyCycleType Type { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double StartLevel { get; set; }
        public double EndLevel { get; set; }
    }

    public enum DutyCycleType
    {
        Empty = 0,
        Fill = 1
    }
}
