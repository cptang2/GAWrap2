using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace GAWrap2.Playback
{
    class Startup
    {
        // Read instructions from csv file and replay them:
        public static void run()
        {
            //Wait for the time specified in the config file
            //Thread.Sleep((new Config(Path.Combine(Playback.data.dir, @"config.xml"))).start);

            if (File.Exists(Path.Combine(Playback.data.dir, "setup.bat")))
            {
                ProcessStartInfo setup = new ProcessStartInfo(Path.Combine(Playback.data.dir, "setup.bat"));
                Process setupProc = Process.Start(setup);
                setupProc.WaitForExit();
            }

            //Run playback and record the results in a results file:
            Replay rObj = new Replay();

            if (rObj.playSteps())
                Playback.data.resultSW.WriteLine("Successful run");
            else
                Playback.data.resultSW.WriteLine("Failed run");

            Playback.data.resultSW.Close();

            Console.WriteLine("Finished");
        }
    }
}
