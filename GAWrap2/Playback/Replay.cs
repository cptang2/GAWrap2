using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace GAWrap2.Playback
{
    
    //Each step consists of an image and the corresponding events
    //This does not load all of the steps into memory like in editor
    struct StepLite
    {
        public string image;
        public List<string> events;


        public StepLite(string image)
        {
            this.image = image;
            events = new List<string>();
        }


        public void setImage(string image)
        {
            this.image = image;
        }
    }
    

    class Replay
    {
        ParseTCs TCParser = new ParseTCs();

        private List<StepLite> sList;
        private Rectangle bounds = Screen.GetBounds(Point.Empty);


        public Replay() 
        {
            sList = TCParser.readTCs(Playback.data.file, -1);
            //printEvents();
        }


        //Replay events in csv file:
        public bool playSteps()
        {
            foreach (StepLite e in sList)
            {
                Playback.data.resultSW.WriteLine("Step: {0}", e.image);

                foreach (string s in e.events)
                {
                    if (!handleStep(s, e.image))
                        return false;
                }
            }

            return true;
        }


        //Handles method invokes for mouse and keyboard input
        private bool handleStep(string ev, string image)
        {
            string[] coms = ev.Split(',');

            //Handle non-click events:
            switch (coms[0])
            {
                case ("detent"):
                    MouseInput.mouseWheel(int.Parse(coms[1]), int.Parse(coms[2]), int.Parse(coms[3]));
                    return true;

                case ("keydown"):
                    KeyboardInput.KeyDown(int.Parse(coms[1]));
                    return true;

                case ("keyup"):
                    KeyboardInput.KeyUp(int.Parse(coms[1]));
                    return true;
            }

            return checkClick(coms, image);
        }


        //Handle click event
        private bool checkClick(string[] coms, string image)
        {
            string[] dwn = { "Left", "Right", "Middle", 
                             "doubleclickLeft", "doubleclickRight", "doubleclickMiddle" };

            MouseInput.move(int.Parse(coms[1]), int.Parse(coms[2]));

            if (dwn.Contains(coms[0]))
                if (!compareToScreen(image))
                    return false;

            MouseInput.mouse(coms[0], int.Parse(coms[1]), int.Parse(coms[2]));
            return true;
        }


        //Compare the image to the screen for an allotted time:
        private bool compareToScreen(string image)
        {
            using (Bitmap png = new Bitmap(Path.Combine(Playback.data.dir, image)))
            {
                using (Bitmap screen = new Bitmap(bounds.Width, bounds.Height))
                {
                    int index = 0;
                    while (index < (int)(Playback.data.cfg.timeout / 1000))
                    {
                        //Get current screen bitmap:
                        using (Graphics g = Graphics.FromImage(screen))
                        {
                            g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                        }

                        if (CompareImages.driver(png, screen))
                            break;

                        Thread.Sleep(1000);
                        index++;
                    }

                    Playback.data.resultSW.WriteLine();

                    if (!checkSave((Bitmap)screen.Clone(), image, index))
                        return false;
                }
            }

            return true;
        }


        //Check whether the image needs to be saved:
        private bool checkSave(Bitmap screen, string image, int index)
        {
            if (Playback.data.cfg.record)
                screen.Save(Path.Combine(Playback.data.dir, ("Results\\" + image)));

            if (index == (int)(Playback.data.cfg.timeout / 1000))
            {
                screen.Save(Path.Combine(Playback.data.dir, ("Results\\" + image)));

                //Up key everything:
                for (int i = 1; i < 150; i++)
                    KeyboardInput.KeyUp(i);

                return false;
            }

            return true;
        }


        //Print what is stored in the eventList data structure
        private void printEvents()
        {
            foreach (StepLite e in sList)
            {
                Console.WriteLine("Image: {0}", e.image);
                Playback.data.resultSW.WriteLine("Image: {0}", e.image);

                foreach (string s in e.events)
                {
                    Console.WriteLine("event: {0}", s);
                    Playback.data.resultSW.WriteLine("Image: {0}", s);
                }

                Playback.data.resultSW.WriteLine();
                Console.Write('\n');
            }
        }
    }
}
