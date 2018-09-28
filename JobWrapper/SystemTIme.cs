using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobWrapper
{
    public static class SystemTime
    {
        public static readonly double MicroSecPerTick;
        public static Func<DateTime> UtcDateTime;
        public static Action<int> WaitCalled;

        public static DateTime UtcNow { get; }
    }
}
