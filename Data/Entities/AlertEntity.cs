using System;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Entities
{
    public class AlertEntity : DataPointEntity
    {
        public AlertType Type { get; set; }
    }

    public enum AlertType : int
    {
        HighWater = 0,              // Sump Pump Reached High Water Mark
        PumpNotStopped = 1,         // Pump still running with no water level change
    }
}