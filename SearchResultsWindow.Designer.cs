namespace CK3ScriptEditor
{
    partial class SearchResultsWindow
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
            this.resultsTree = new DarkUI.Controls.DarkTreeView();
            this.SuspendLayout();
            // 
            // resultsTree
            // 
            this.resultsTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsTree.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resultsTree.Location = new System.Drawing.Point(3, 28);
            this.resultsTree.MaxDragChange = 20;
            this.resultsTree.Name = "resultsTree";
            this.resultsTree.Size = new System.Drawing.Size(1857, 306);
            this.resultsTree.TabIndex = 0;
            this.resultsTree.Text = "darkTreeView1";
            this.resultsTree.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.resultsTree_MouseDoubleClick);
            // 
            // SearchResultsWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.resultsTree);
            this.DockText = "Smart Find Results";
            this.Name = "SearchResultsWindow";
            this.Size = new System.Drawing.Size(1863, 334);
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkTreeView resultsTree;
    }
}
