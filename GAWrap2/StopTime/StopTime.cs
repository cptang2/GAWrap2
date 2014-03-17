using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;

namespace GAWrap2
{
    class SetTime
    {
        [DllImport("Kernel32.dll")]
        private extern static void GetSystemTime(ref SYSTEMTIME lpSystemTime);
        [DllImport("Kernel32.dll")]
        private extern static uint SetSystemTime(ref SYSTEMTIME lpSystemTime);

        private struct SYSTEMTIME
        {
            public ushort year;
            public ushort month;
            public ushort dayOfWeek;
            public ushort day;
            public ushort hour;
            public ushort minute;
            public ushort second;
            public ushort milliseconds;
        }

        Thread stopTime;

        public SetTime()
        {
            stopTime = new Thread(new ThreadStart(_stop));
        }

        public void stop()
        {
            if (!stopTime.IsAlive)
                stopTime.Start();
        }

        public void unStop()
        {
            if (stopTime != null && stopTime.IsAlive)
                stopTime.Abort();
        }

        private string GetTime()
        {
            // Call the native GetSystemTime method 
            // with the defined structure.
            SYSTEMTIME stime = new SYSTEMTIME();
            GetSystemTime(ref stime);

            return ("Current Time: " + stime.hour + ":" + stime.minute);
        }

        private void _stop()
        {
            SYSTEMTIME systime = new SYSTEMTIME();
            GetSystemTime(ref systime);

            // Set the system clock:
            systime.year = 2014; systime.month = 1; systime.day = 15; systime.hour = 8;
            systime.minute = 0; systime.second = 0; systime.milliseconds = 0;

            SetSystemTime(ref systime);

            while (true)
            {
                System.Threading.Thread.Sleep(500);

                SetSystemTime(ref systime);
            }
        }
    }
}
