using System;

using Microsoft.WindowsAzure.Storage.Table;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Entities
{
    public class DutyCycleEntity : TableEntity
    {
        public bool IsEmptying { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double StartLevel { get; set; }
        public double EndLevel { get; set; }
    }
}
