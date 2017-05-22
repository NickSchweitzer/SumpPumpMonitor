using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingMonkeyNet.SumpPumpMonitor.Portal.Models
{
    public class PumpConfiguration
    {
        public string Name { get; set; }
        public double MaxWaterLevel { get; set; }
        public double MaxRunTimeNoChange { get; set; }
    }
}
