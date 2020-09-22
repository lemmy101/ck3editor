namespace CK3ScriptEditor
{
    partial class ScriptObjectExplorer
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
            this.tree = new DarkUI.Controls.DarkTreeView();
            this.showModOnly = new DarkUI.Controls.DarkCheckBox();
            this.showOveridden = new DarkUI.Controls.DarkCheckBox();
            this.showEmpty = new DarkUI.Controls.DarkCheckBox();
            this.findTextBox = new DarkUI.Controls.DarkTextBox();
            this.darkLabel1 = new DarkUI.Controls.DarkLabel();
            this.SuspendLayout();
            // 
            // tree
            // 
            this.tree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tree.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tree.Location = new System.Drawing.Point(-3, 60);
            this.tree.MaxDragChange = 20;
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(426, 568);
            this.tree.TabIndex = 0;
            this.tree.Text = "darkTreeView1";
            this.tree.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tree_MouseClick);
            this.tree.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.tree_MouseDoubleClick);
            this.tree.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tree_MouseUp);
            // 
            // showModOnly
            // 
            this.showModOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showModOnly.AutoSize = true;
            this.showModOnly.Checked = true;
            this.showModOnly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showModOnly.Location = new System.Drawing.Point(12, 644);
            this.showModOnly.Name = "showModOnly";
            this.showModOnly.Size = new System.Drawing.Size(96, 17);
            this.showModOnly.TabIndex = 1;
            this.showModOnly.Text = "Show inherited";
            this.showModOnly.CheckedChanged += new System.EventHandler(this.showModOnly_CheckedChanged);
            // 
            // showOveridden
            // 
            this.showOveridden.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showOveridden.AutoSize = true;
            this.showOveridden.Checked = true;
            this.showOveridden.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showOveridden.Location = new System.Drawing.Point(119, 644);
            this.showOveridden.Name = "showOveridden";
            this.showOveridden.Size = new System.Drawing.Size(106, 17);
            this.showOveridden.TabIndex = 2;
            this.showOveridden.Text = "Show overridden";
            this.showOveridden.CheckedChanged += new System.EventHandler(this.showOveridden_CheckedChanged);
            // 
            // showEmpty
            // 
            this.showEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.showEmpty.AutoSize = true;
            this.showEmpty.Location = new System.Drawing.Point(231, 644);
            this.showEmpty.Name = "showEmpty";
            this.showEmpty.Size = new System.Drawing.Size(136, 17);
            this.showEmpty.TabIndex = 3;
            this.showEmpty.Text = "Show empty categories";
            this.showEmpty.CheckedChanged += new System.EventHandler(this.showEmpty_CheckedChanged);
            // 
            // findTextBox
            // 
            this.findTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.findTextBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.findTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.findTextBox.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.findTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.findTextBox.Location = new System.Drawing.Point(54, 33);
            this.findTextBox.Name = "findTextBox";
            this.findTextBox.Size = new System.Drawing.Size(369, 20);
            this.findTextBox.TabIndex = 4;
            this.findTextBox.TextChanged += new System.EventHandler(this.findTextBox_TextChanged);
            // 
            // darkLabel1
            // 
            this.darkLabel1.AutoSize = true;
            this.darkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel1.Location = new System.Drawing.Point(18, 36);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.Size = new System.Drawing.Size(30, 13);
            this.darkLabel1.TabIndex = 5;
            this.darkLabel1.Text = "Find:";
            // 
            // ScriptObjectExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.darkLabel1);
            this.Controls.Add(this.findTextBox);
            this.Controls.Add(this.showEmpty);
            this.Controls.Add(this.showOveridden);
            this.Controls.Add(this.showModOnly);
            this.Controls.Add(this.tree);
            this.DockText = "ScriptObject Explorer";
            this.Name = "ScriptObjectExplorer";
            this.Size = new System.Drawing.Size(433, 670);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DarkUI.Controls.DarkTreeView tree;
        private DarkUI.Controls.DarkCheckBox showModOnly;
        private DarkUI.Controls.DarkCheckBox showOveridden;
        private DarkUI.Controls.DarkCheckBox showEmpty;
        private DarkUI.Controls.DarkTextBox findTextBox;
        private DarkUI.Controls.DarkLabel darkLabel1;
    }
}
