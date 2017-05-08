using System;

using Microsoft.WindowsAzure.Storage.Table;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Entities
{
    public class AlertEntity : TableEntity
    {
        public int Type { get; set; }
        public Double WaterLevel { get; set; }
        public bool PumpRunning { get; set; }
    }

    public enum SumpPumpErrorType
    {
        HighWater = 0,              // Sump Pump Reached High Water Mark
        PumpNotStopped = 1,         // Pump still running with no water level change
    }
}
