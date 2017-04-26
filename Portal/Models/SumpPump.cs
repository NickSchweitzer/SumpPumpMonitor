using System;

namespace CodingMonkeyNet.SumpPumpMonitor.Portal.Models
{
    public class SumpPump
    {
        public string PumpId { get; set; }
        public string Name { get; set; }
        public Double MaxWaterLevel { get; set; }
        public int MaxRunTimeNoChange { get; set; }     // Seconds
        public bool InError { get; set; }

        public Double CurrentWaterLevel { get; set; }
        public bool PumpRunning { get; set; }
        public DateTime LastDataRecorded { get; set; }

        public string Data
        {
            get { return string.Format("./data/{0}", PumpId); }
        }
    }
}
