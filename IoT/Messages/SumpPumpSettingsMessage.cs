using System;
using System.Collections.Generic;
using System.Text;

namespace CodingMonkeyNet.SumpPumpMonitor.IoT.Messages
{
    public class SumpPumpSettingsMessage
    {
        public Double MaxWaterLevel { get; set; }       // Inches
        public int MaxRunTimeNoChange { get; set; }     // Seconds
    }
}
