﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace GAWrap2.Steps
{
    class Step
    {
        public Bitmap image = null;
        public List<string> events = new List<string>();

        public Step() { }

        //Clone referenced objects to new step object
        public Step(Step s)
        {
            this.image = (Bitmap)s.image.Clone();
            s.events.ForEach((item) => { events.Add(item); });
        }

        public Step(Bitmap image)
        {
            this.image = (Bitmap)image.Clone();
        }

        public Step(List<string> events)
        {
            events.ForEach((item) => { this.events.Add(item); });
        }

        public void close()
        {
            if (this.image != null)
                this.image.Dispose();

            events.Clear();
        }

        ~Step()
        {
            close();
        }
    }
}
