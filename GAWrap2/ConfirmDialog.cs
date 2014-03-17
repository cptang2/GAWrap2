using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GAWrap2
{
    class ConfirmDialog : Form
    {
        private Button btnYes;
        private Button btnNo;
        private Label label1;
        bool _canRecord;

        /// <summary>
        /// Gets the confirmation by the user
        /// </summary>
        public bool canRecord
        {
            get
            {
                return _canRecord;
            }
        }

        private ConfirmDialog()
		{
			InitializeComponent();
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(37, 50);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 23);
            this.btnYes.TabIndex = 0;
            this.btnYes.Text = "Yes";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnNo
            // 
            this.btnNo.Location = new System.Drawing.Point(136, 50);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 23);
            this.btnNo.TabIndex = 1;
            this.btnNo.Text = "No";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(216, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Overwrite any existing data in this test case?";
            // 
            // ConfirmDialog
            // 
            this.ClientSize = new System.Drawing.Size(241, 86);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ConfirmDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        public static bool GetConf()
        {
            using (ConfirmDialog cD = new ConfirmDialog())
            {
                cD.ShowDialog();
                return cD.canRecord;
            }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            _canRecord = true;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
