using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GAWrap2.Playback
{
    //Intended to hold metaData for the application:
    class MetaData
    {
        public readonly Config cfg;       //Config information
        public readonly string dir, file;
        public readonly StreamWriter resultSW;
        public readonly int threads;

        public MetaData(string directory, int threads)
        {
            this.dir = directory;
            this.file = Path.Combine(dir, "testcase.csv");
            cfg = new Config(Path.Combine(dir, @"config.xml"));
            resultSW = new StreamWriter(CreateResultFolder(dir + "\\Results"));
            this.threads = threads;
        }

        static string CreateResultFolder(string rDir)
        {
            if (!Directory.Exists(rDir))
                Directory.CreateDirectory(rDir);

            return Path.Combine(rDir, "results.txt");
        }
    }
}
