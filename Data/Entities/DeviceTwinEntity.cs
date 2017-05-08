using System;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Entities
{
    public class DeviceTwinQueryEntity
    {
        public string DeviceId { get; set; }
        public SumpPumpSettingQueryPair Properties { get; set; }
    }

    public class SumpPumpSettingQueryPair
    {
        public SumpPumpSetting Desired { get; set; }
        public SumpPumpSetting Reported { get; set; }
    }

    public class DeviceTwinUpdateEntity
    {
        public string DeviceId { get; set; }
        public SumpPumpSettingUpdate Properties { get; set; }
    }

    public class SumpPumpSettingUpdate
    {
        public SumpPumpSetting Desired { get; set; }
    }

    public class SumpPumpSetting
    {
        public string DeviceName { get; set; }
        public double MaxWaterLevel { get; set; }
        public double MaxRunTimeNoChange { get; set; }
    }
}