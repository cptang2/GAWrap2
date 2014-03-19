using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;

// Assembly from http://globalmousekeyhook.codeplex.com/
using MouseKeyboardActivityMonitor;
using MouseKeyboardActivityMonitor.Controls;

namespace GAWrap2.Playback
{
    class PlaybackTrayApp : Form
    {
        NotifyIcon trayIcon;
        readonly string[] files;
        Thread replayThread;                // Replay thread
        MouseKeyEventProvider spKeys;       // Listen for special keys to  stop playback

        public PlaybackTrayApp(string[] files)
        {
            this.files = files;

            spKeys = new MouseKeyEventProvider();
            spKeys.KeyDown += Key;
        }

        [STAThread]
        public static void initPlaybackApp(string[] files)
        {
            PlaybackTrayApp app = new PlaybackTrayApp(files);
            Application.Run(app);
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);

            this.replayThread = AllocPlayback();
            this.replayThread.Start();

            //Make context menu:
            ContextMenu trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Stop Playback (F11)", StopPlayback);

            // Make tray icon:
            trayIcon = new NotifyIcon();
            trayIcon.Text = "Playback GUI Automation";
            trayIcon.Icon = new Icon(SystemIcons.Application, 40, 40);

            //Add context menu:
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
        }

        Thread AllocPlayback()
        {
            Action playObj = () => Playback.Init(files, this);

            return new Thread(new ThreadStart(playObj));
        }

        void StopPlayback(object sender, EventArgs e)
        {
            this.OnExit(this, null);
        }

        void OnExit(object sender, EventArgs e)
        {
            if (replayThread != null && replayThread.IsAlive)
            {
                Playback.clear();
                replayThread.Abort();
            }

            this.Close();
        }

        //Stop playback if F11 is pressed
        void Key(object sender, KeyEventArgs e)
        {
            if ((int)e.KeyCode == 122)
            {
                this.OnExit(this, null);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                trayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.ClientSize = new System.Drawing.Size(292, 266);
            this.Name = "PlaybackTrayApp";
            this.ResumeLayout(false);
        }
    }
}
