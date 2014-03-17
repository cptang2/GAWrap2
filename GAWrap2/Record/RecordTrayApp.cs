using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace GAWrap2
{
    class RecordTrayApp : Form
    {
        NotifyIcon trayIcon;
        Record recorder;
        string dir;

        public RecordTrayApp(string dir)
        {
            this.dir = dir;
        }

        [STAThread]
        public static void initRecordApp(string dir)
        {
            RecordTrayApp app = new RecordTrayApp(dir);
            Application.Run(app);
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.
            startRecord(dir);

            base.OnLoad(e);

            //Make context menu:
            ContextMenu trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Stop Recording (F11)", stopRecord);

            // Make tray icon:
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Record GUI Automation";
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

            //Add context menu:
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }
        
        void startRecord(string dir)
        {
            recorder = new Record(dir);
            recorder.start(this);
        }

        void stopRecord(object sender, EventArgs e)
        {
            if (recorder != null)
            {
                recorder.stop();
                recorder = null;
                System.Windows.MessageBox.Show("Recording stopped");
                this.Close();
            }
        }

        void OnExit(object sender, EventArgs e)
        {
            stopRecord(this, null);
            this.Close();
        }

        protected override void Dispose(bool isDisposing)
        {
            stopRecord(this, null);

            if (isDisposing)
            {
                // Release the icon resource.
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // RecordTrayApp
            // 
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "RecordTrayApp";
            this.ResumeLayout(false);
        }
    }
}
