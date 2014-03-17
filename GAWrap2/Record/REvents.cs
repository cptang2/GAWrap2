using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.Controls;

namespace GAWrap2
{
    class REvents
    {
        readonly string dir;
        StreamWriter sW;                    // test case csv file within the target directory
        SetTime timeStop = new SetTime();   // Object used to stop time


        public REvents(MouseKeyEventProvider mKEP, string dir)
        {
            this.dir = dir;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);


            //Copy default config file
            string cfg = Path.Combine(dir, "config.xml");

            if (File.Exists(cfg))
                File.Delete(cfg);

            File.Copy(Path.Combine(Environment.CurrentDirectory, "blankConfig.xml"), cfg);

            //Start recording input
            sW = new StreamWriter(Path.Combine(dir, "testcase.csv"));

            addEvents(mKEP);

            timeStop.stop();
        }

        //Add methods to mKEP events
        void addEvents(MouseKeyEventProvider mKEP)
        {
            mKEP.MouseDownExt += this.MouseDownExt;
            mKEP.MouseUp += this.MouseUp;
            mKEP.MouseWheel += this.MouseWheel;
            mKEP.KeyUp += this.KeyUp;
            mKEP.KeyDown += this.KeyDown;
        }

        /// <summary>
        /// Close test case file
        /// </summary>
        public void close()
        {
            if (sW != null)
            {
                timeStop.unStop();
                sW.Close();
            }
        }

        //Take a snapshot before handling mousedown event:
        void MouseDownExt(object sender, MouseEventExtArgs e)
        {
            saveStep(dir);

            sW.WriteLine(e.Timestamp);
            sW.WriteLine("{0},{1},{2}", e.Button.ToString(), e.X, e.Y);
        }

        void MouseUp(object sender, MouseEventArgs e)
        {
            sW.WriteLine("up{0},{1},{2}", e.Button, e.X, e.Y);
        }

        void MouseWheel(object sender, MouseEventArgs e)
        {
            sW.WriteLine("detent,{0},{1},{2}", e.X, e.Y, e.Delta);
        }

        void KeyUp(object sender, KeyEventArgs e)
        {
            sW.WriteLine("keyup,{0}", (int)e.KeyCode);
        }

        void KeyDown(object sender, KeyEventArgs e)
        {
            sW.WriteLine("keydown,{0}", (int)e.KeyCode);
        }

        //Save current screen:
        void saveStep(string path)
        {
            Rectangle bounds = Screen.GetBounds(Point.Empty);

            //Save the full screen:
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }

                int post = 0;
                while (File.Exists(Path.Combine(path, post + ".png")))
                {
                    post++;
                }

                bitmap.Save(Path.Combine(path, post + ".png"));
                sW.WriteLine("image,{0}", post + ".png");
            }
        }
    }
}
