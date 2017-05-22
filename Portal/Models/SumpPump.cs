using System;

namespace CodingMonkeyNet.SumpPumpMonitor.Portal.Models
{
    public class SumpPump
    {
        public string PumpId { get; set; }
        public PumpConfiguration Desired { get; set; }
        public PumpConfiguration Reported { get; set; }
        public DataPoint LastPoint { get; set; }

        public string Data
        {
            get { return string.Format("./data/{0}", PumpId); }
        }

        public string Alerts
        {
            get { return string.Format("./alerts/{0}", PumpId); }
        }
    }
}
