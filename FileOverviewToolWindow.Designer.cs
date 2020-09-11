namespace CK3ScriptEditor
{
    partial class FileOverviewToolWindow
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TreeView = new DarkUI.Controls.DarkTreeView();
            this.SuspendLayout();
            // 
            // TreeView
            // 
            this.TreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TreeView.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TreeView.Location = new System.Drawing.Point(3, 28);
            this.TreeView.MaxDragChange = 20;
            this.TreeView.Name = "TreeView";
            this.TreeView.Size = new System.Drawing.Size(842, 542);
            this.TreeView.TabIndex = 0;
            this.TreeView.Text = "darkTreeView1";
            // 
            // FileOverviewToolWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TreeView);
            this.DockText = "File Outliner";
            this.MinimumSize = new System.Drawing.Size(500, 0);
            this.Name = "FileOverviewToolWindow";
            this.Size = new System.Drawing.Size(859, 573);
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkTreeView TreeView;
    }
}
