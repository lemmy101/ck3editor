namespace CK3ScriptEditor
{
    partial class TabOpenWindowsDlg
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
            this.openFileList = new DarkUI.Controls.DarkListView();
            this.SuspendLayout();
            // 
            // openFileList
            // 
            this.openFileList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.openFileList.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.openFileList.Location = new System.Drawing.Point(0, 0);
            this.openFileList.Name = "openFileList";
            this.openFileList.Size = new System.Drawing.Size(800, 450);
            this.openFileList.TabIndex = 0;
            this.openFileList.Text = "darkListView1";
            this.openFileList.DoubleClick += new System.EventHandler(this.openFileList_DoubleClick);
            this.openFileList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.openFileList_KeyDown);
            this.openFileList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.openFileList_KeyUp);
            // 
            // TabOpenWindowsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.openFileList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "TabOpenWindowsDlg";
            this.Text = "TabOpenWindowsDlg";
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkListView openFileList;
    }
}