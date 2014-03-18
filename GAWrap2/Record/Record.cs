using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

// Assembly from http://globalmousekeyhook.codeplex.com/
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.Controls;


namespace GAWrap2.Record
{
    class Record
    {
        MouseKeyEventProvider mKEP;         // Record a test case
        MouseKeyEventProvider spKeys;       // Listen for special keys to pause recording
        readonly string dir;                // Full path to the test case directory
        REvents rE;                         // Record events object
        RecordTrayApp caller;

        public Record(string dir)
        {
            this.dir = dir;

            mKEP = new MouseKeyEventProvider();
            mKEP.Enabled = false;
            mKEP.HookType = HookType.Global;

            spKeys = new MouseKeyEventProvider();
            spKeys.Enabled = true;
            spKeys.HookType = HookType.Global;
            spKeys.KeyDown += KeyDown;
        }

        /// <summary>
        /// Start Recording test case
        /// </summary>
        /// <param name="dir">Full path to the directory to write to</param>
        public void start(RecordTrayApp caller)
        {
            try
            {
                mKEP.Enabled = true;
                this.caller = caller;
                rE = new REvents(mKEP, dir);
            }
            catch
            {
                MessageBox.Show("Unable to initialize REvents");
                mKEP.Enabled = false;
                caller.Close();
            }
        }

        /// <summary>
        /// Stop Recording
        /// </summary>
        public void stop()
        {
            if (!mKEP.Enabled)
                return;

            rE.close();
            mKEP.Enabled = false;
        }

        //Pause recording if printscreen is pressed
        //Stop recording if F11 is pressed
        void KeyDown(object sender, KeyEventArgs e)
        {
            if ((int)e.KeyCode == 44) //print screen
            {
                mKEP.Enabled = !mKEP.Enabled;

                if (!mKEP.Enabled)
                    MessageBox.Show("Paused");
            }
            else if ((int)e.KeyCode == 122)
            {
                stop();

                caller.Close();
            }
        }
    }
}
