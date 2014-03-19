using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace GAWrap2.Steps
{
    class StepsIO : Undo
    {
        public StepsIO() { }

        //Separate file into steps
        public void parse(List<Step> s, string file)
        {
            if (!File.Exists(file))
                return;

            string[] lns = File.ReadAllLines(file);
            string imDir = Path.GetDirectoryName(file);

            //Get events from the file
            foreach (string ln in lns)
                parseLn(s, ln, imDir);
        }

        //Line parse logic
        void parseLn(List<Step> s, string ln, string imDir)
        {
            if (ln.Split(',')[0] == "image")
            {
                s.Add(new Step());
                Bitmap temp = new Bitmap(imDir + "\\" + ln.Split(',')[1]); //Enable deleting images while editor is loaded by using a temp variable
                s[s.Count - 1].image = new Bitmap(temp);                  
                (new Thread(new ThreadStart(temp.Dispose))).Start();

                s[s.Count - 1].image.Tag = ln.Split(',')[1];         // Label image with its original name
            }
            else
                s[s.Count - 1].events.Add(ln);
        }

        //Write steps in memory into a file
        public void writeTo(List<Step> steps, string file, int length)
        {
            if (length == -1)
                length = steps.Count;

            StreamWriter sW = new StreamWriter(file);
            for (int i = 0; i < steps.Count && i < length; i++)
            {
                sW.WriteLine("image,{0}", steps[i].image.Tag); 
                steps[i].events.ForEach((item) => { sW.WriteLine(item); });
            }

            sW.Close();
        }
    }
}
