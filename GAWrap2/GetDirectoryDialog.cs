using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace GAWrap2
{
    public class GetDirectoryDialog : Form
    {
        Button btnOk;
        Button btnCancel;
        Button btnBrowse;
        TextBox _directory;

        Container components = null;

        private GetDirectoryDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets the directory entered by the user.
        /// </summary>
        public string directory
        {
            get
            {
                return _directory.Text;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this._directory = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(144, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(64, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(144, 40);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(64, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            // 
            // _directory
            // 
            this._directory.Location = new System.Drawing.Point(16, 8);
            this._directory.Name = "_directory";
            this._directory.Size = new System.Drawing.Size(112, 20);
            this._directory.TabIndex = 0;
            // 
            // btnBrowse
            // 
            this.btnBrowse.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnBrowse.Location = new System.Drawing.Point(16, 40);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(64, 23);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // GetDirectoryDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(216, 73);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this._directory);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GetDirectoryDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Specify a directory";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public static string GetDirectory()
        {
            using (GetDirectoryDialog dialog = new GetDirectoryDialog())
            {
                dialog.ShowDialog();

                if (dialog.directory.Length > 0)
                    return dialog.directory;
                else
                    return null;
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fBD = new FolderBrowserDialog();

            fBD.ShowDialog();

            if (fBD.SelectedPath.Length > 0)
            {
                _directory.Text = fBD.SelectedPath;
            }
            else
            {
                // If the user clicks Cancel, return null and not the empty string.
                return;
            }
        }
    }
}