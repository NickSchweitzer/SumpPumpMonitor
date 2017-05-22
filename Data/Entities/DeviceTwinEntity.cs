using System;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Entities
{
    public class DeviceTwinEntity<T>
        where T: new()
    {
        public string DeviceId { get; set; }
        public TwinSettingPair<T> Properties { get; set; }
    }

    public class TwinSettingPair<T>
        where T : new()
    {
        public T Desired { get; set; }
        public T Reported { get; set; }
    }

    internal class DeviceTwinUpdateEntity<T>
        where T : new()
    {
        public string DeviceId { get; set; }
        public TwinSettingUpdate<T> Properties { get; set; }
    }

    internal class TwinSettingUpdate<T>
        where T : new()
    {
        public T Desired { get; set; }
    }
}