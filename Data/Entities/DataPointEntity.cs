using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Entities
{
    public class DataPointEntity : TableEntity
    {
        public double WaterLevel { get; set; }
        public bool PumpRunning { get; set; }
    }
}