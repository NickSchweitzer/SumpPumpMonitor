using System;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Utilities
{
    public static class TableEntityExtensions
    {
        public static string ToRowKey(this DateTime dt)
        {
            return string.Format("{0:D19}", DateTime.MaxValue.Ticks - dt.Ticks);
        }

        public static DateTime FromRowKey(this string rowKey)
        {
            return new DateTime(DateTime.MaxValue.Ticks - Int64.Parse(rowKey));
        }

        public static double TotalSeconds(this TimeSpan timeSpan)
        {
            return timeSpan.Ticks * 1E-07;
        }
    }
}