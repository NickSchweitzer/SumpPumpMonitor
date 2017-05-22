using System;
using System.Collections.Generic;
using System.Text;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Entities
{
    public class SumpPumpSettingEntity
    {
        public string DeviceName { get; set; }
        public double MaxWaterLevel { get; set; }
        public double MaxRunTimeNoChange { get; set; }
    }
}