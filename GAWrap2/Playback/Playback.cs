using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Threading;

namespace PlayBack
{
    class Playback
    {
        public static mData data;
        const int threads = 4;

        static void Init(string[] args)
        {
            List<string> files = new List<string>();

            files = getInput();

            foreach (string f in files)
            {
                data = new mData(f, threads);
                Startup.run();
            }

            Console.ReadKey();
        }

        //Get user input for file to replay
        private static List<string> getInput()
        {
            string file = null;
            List<string> files = new List<string>();

            while (!File.Exists(file))
            {
                Console.Write("File to replay: ");
                file = Console.ReadLine();
            }

            files.Add(file);

            return files;
        }

    }
}
