using System;

namespace CodingMonkeyNet.SumpPumpMonitor.IoT
{
    public static class DateTimeExtensions
    {
        public static double TotalSeconds(this TimeSpan timeSpan)
        {
            return timeSpan.Ticks * 1E-07;
        }
    }
}