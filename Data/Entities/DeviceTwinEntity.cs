using System;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Entities
{
    public class DeviceTwinEntity
    {
        public string DeviceId { get; set; }
        public SumpPumpSettingPair Properties { get; set; }
    }

    public class SumpPumpSettingPair
    {
        public SumpPumpSetting Desired { get; set; }
        public SumpPumpSetting Reported { get; set; }
    }

    internal class DeviceTwinUpdateEntity
    {
        public string DeviceId { get; set; }
        public SumpPumpSettingUpdate Properties { get; set; }
    }

    internal class SumpPumpSettingUpdate
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