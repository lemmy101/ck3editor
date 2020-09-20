namespace CK3ScriptEditor
{
    partial class NewFileFolder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.NewText = new DarkUI.Controls.DarkTextBox();
            this.darkLabel1 = new DarkUI.Controls.DarkLabel();
            this.OK = new DarkUI.Controls.DarkButton();
            this.Cancel = new DarkUI.Controls.DarkButton();
            this.SuspendLayout();
            // 
            // NewText
            // 
            this.NewText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.NewText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NewText.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NewText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.NewText.Location = new System.Drawing.Point(82, 24);
            this.NewText.Name = "NewText";
            this.NewText.Size = new System.Drawing.Size(359, 23);
            this.NewText.TabIndex = 0;
            // 
            // darkLabel1
            // 
            this.darkLabel1.AutoSize = true;
            this.darkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel1.Location = new System.Drawing.Point(25, 26);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.Size = new System.Drawing.Size(38, 13);
            this.darkLabel1.TabIndex = 1;
            this.darkLabel1.Text = "Name:";
            // 
            // OK
            // 
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(285, 53);
            this.OK.Name = "OK";
            this.OK.Padding = new System.Windows.Forms.Padding(5);
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 2;
            this.OK.Text = "OK";
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(366, 53);
            this.Cancel.Name = "Cancel";
            this.Cancel.Padding = new System.Windows.Forms.Padding(5);
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 3;
            this.Cancel.Text = "Cancel";
            // 
            // NewFileFolder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 88);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.darkLabel1);
            this.Controls.Add(this.NewText);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "NewFileFolder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NewFileFolder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DarkUI.Controls.DarkTextBox NewText;
        private DarkUI.Controls.DarkLabel darkLabel1;
        private DarkUI.Controls.DarkButton OK;
        private DarkUI.Controls.DarkButton Cancel;
    }
}