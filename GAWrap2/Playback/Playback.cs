using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading;

namespace GAWrap2.Playback
{
    class Playback
    {
        public static MetaData data;
        const int threads = 4;
        static SetTime time;

        public static void Init(string[] files, PlaybackTrayApp PTA)
        {
            time = new SetTime();
            time.stop();

            foreach (string f in files)
            {
                data = new MetaData(f, threads);
                Startup.run();
            }

            time.unStop();

            //Close caller:
            Action close = () => PTA.Close();
            PTA.Invoke(close);
        }

        /// <summary>
        /// Make sure no key is held down by the system.
        /// </summary>
        public static void clear()
        {
            time.unStop();

            //Up key everything:
            for (int i = 1; i < 150; i++)
                KeyboardInput.KeyUp(i);
        }
    }
}
