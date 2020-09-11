namespace CK3ScriptEditor
{
    partial class ProjectExplorer
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
            this.projectTree = new DarkUI.Controls.DarkTreeView();
            this.SuspendLayout();
            // 
            // projectTree
            // 
            this.projectTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.projectTree.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.projectTree.Location = new System.Drawing.Point(3, 28);
            this.projectTree.MaxDragChange = 20;
            this.projectTree.Name = "projectTree";
            this.projectTree.Size = new System.Drawing.Size(523, 883);
            this.projectTree.TabIndex = 0;
            this.projectTree.Text = "darkTreeView1";
            this.projectTree.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.projectTree_MouseDoubleClick);
            // 
            // ProjectExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.projectTree);
            this.DockText = "Project Explorer";
            this.Name = "ProjectExplorer";
            this.Size = new System.Drawing.Size(529, 914);
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkTreeView projectTree;
    }
}
