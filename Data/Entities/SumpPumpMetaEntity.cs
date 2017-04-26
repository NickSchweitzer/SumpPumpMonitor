using System;

using Microsoft.WindowsAzure.Storage.Table;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Entities
{
    public class SumpPumpMetaEntity : TableEntity
    {
        public string Name { get; set; }
        public Double MaxWaterLevel { get; set; }
        public int MaxRunTimeNoChange { get; set; }     // Seconds
        public bool InError { get; set; }
    }
}