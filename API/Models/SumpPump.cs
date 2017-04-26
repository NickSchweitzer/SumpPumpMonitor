using System;

namespace CodingMonkeyNet.SumpPumpMonitor.API.Models
{
    public class SumpPump
    {
        public string PumpId { get; set; }
        public string Name { get; set; }
        public Double MaxWaterLevel { get; set; }
        public int MaxRunTimeNoChange { get; set; }     // Seconds
        public bool InError { get; set; }

        public string Data
        {
            get { return string.Format("./data/{0}", PumpId); }
        }
    }
}
