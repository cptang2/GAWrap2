using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace PlayBack
{
    class Startup
    {
        // Read instructions from csv file and replay them:
        public static void run()
        {
            //Wait for the time specified in the config file
            Thread.Sleep((new Config(Path.Combine(Playback.data.dir, @"config.xml"))).start);

            //Run playback and record the results in a results file:
            Replay rObj = new Replay();

            if (rObj.playSteps())
                Playback.data.rF.WriteLine("Successful run");
            else
                Playback.data.rF.WriteLine("Failed run");

            Console.WriteLine("Finished");
        }
    }
}
