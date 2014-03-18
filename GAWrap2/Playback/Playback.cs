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

        public static void Init(string[] directories, PlaybackTrayApp PTA)
        {
            foreach (string d in directories)
            {
                data = new MetaData(d, threads);
                Startup.run();
            }

            Action close = () => PTA.Close();
            PTA.Invoke(close);
        }

        /// <summary>
        /// Make sure no key is held down by the system.
        /// </summary>
        public static void clear()
        {
            //Up key everything:
            for (int i = 1; i < 150; i++)
                KeyboardInput.KeyUp(i);

        }
    }
}
