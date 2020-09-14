namespace CK3ScriptEditor
{
    partial class ObjectDetailsExplorer
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
            this.referencedByList = new DarkUI.Controls.DarkListView();
            this.darkGroupBox1 = new DarkUI.Controls.DarkGroupBox();
            this.darkListView2 = new DarkUI.Controls.DarkListView();
            this.referencesList = new DarkUI.Controls.DarkListView();
            this.darkGroupBox2 = new DarkUI.Controls.DarkGroupBox();
            this.darkGroupBox3 = new DarkUI.Controls.DarkGroupBox();
            this.scopesList = new DarkUI.Controls.DarkListView();
            this.darkListView3 = new DarkUI.Controls.DarkListView();
            this.darkGroupBox1.SuspendLayout();
            this.darkGroupBox2.SuspendLayout();
            this.darkGroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // referencedByList
            // 
            this.referencedByList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.referencedByList.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.referencedByList.Location = new System.Drawing.Point(12, 21);
            this.referencedByList.Name = "referencedByList";
            this.referencedByList.Size = new System.Drawing.Size(405, 246);
            this.referencedByList.TabIndex = 0;
            this.referencedByList.Text = "referencedByList";
            this.referencedByList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.referencedByList_MouseDoubleClick);
            // 
            // darkGroupBox1
            // 
            this.darkGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkGroupBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.darkGroupBox1.Controls.Add(this.referencedByList);
            this.darkGroupBox1.Location = new System.Drawing.Point(3, 28);
            this.darkGroupBox1.Name = "darkGroupBox1";
            this.darkGroupBox1.Size = new System.Drawing.Size(423, 273);
            this.darkGroupBox1.TabIndex = 1;
            this.darkGroupBox1.TabStop = false;
            this.darkGroupBox1.Text = "Referenced by";
            // 
            // darkListView2
            // 
            this.darkListView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkListView2.Location = new System.Drawing.Point(12, 21);
            this.darkListView2.Name = "darkListView2";
            this.darkListView2.Size = new System.Drawing.Size(411, 86);
            this.darkListView2.TabIndex = 2;
            this.darkListView2.Text = "triggeredByList";
            // 
            // referencesList
            // 
            this.referencesList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.referencesList.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.referencesList.Location = new System.Drawing.Point(23, 21);
            this.referencesList.Name = "referencesList";
            this.referencesList.Size = new System.Drawing.Size(394, 243);
            this.referencesList.TabIndex = 0;
            this.referencesList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.referencesList_MouseDoubleClick);
            // 
            // darkGroupBox2
            // 
            this.darkGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkGroupBox2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.darkGroupBox2.Controls.Add(this.referencesList);
            this.darkGroupBox2.Controls.Add(this.darkListView2);
            this.darkGroupBox2.Location = new System.Drawing.Point(6, 307);
            this.darkGroupBox2.Name = "darkGroupBox2";
            this.darkGroupBox2.Size = new System.Drawing.Size(423, 270);
            this.darkGroupBox2.TabIndex = 2;
            this.darkGroupBox2.TabStop = false;
            this.darkGroupBox2.Text = "References";
            // 
            // darkGroupBox3
            // 
            this.darkGroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkGroupBox3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.darkGroupBox3.Controls.Add(this.scopesList);
            this.darkGroupBox3.Controls.Add(this.darkListView3);
            this.darkGroupBox3.Location = new System.Drawing.Point(6, 583);
            this.darkGroupBox3.Name = "darkGroupBox3";
            this.darkGroupBox3.Size = new System.Drawing.Size(423, 270);
            this.darkGroupBox3.TabIndex = 3;
            this.darkGroupBox3.TabStop = false;
            this.darkGroupBox3.Text = "Scopes";
            // 
            // scopesList
            // 
            this.scopesList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scopesList.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scopesList.Location = new System.Drawing.Point(23, 21);
            this.scopesList.Name = "scopesList";
            this.scopesList.Size = new System.Drawing.Size(394, 243);
            this.scopesList.TabIndex = 0;
            this.scopesList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.scopesList_MouseDoubleClick);
            // 
            // darkListView3
            // 
            this.darkListView3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkListView3.Location = new System.Drawing.Point(12, 21);
            this.darkListView3.Name = "darkListView3";
            this.darkListView3.Size = new System.Drawing.Size(411, 86);
            this.darkListView3.TabIndex = 2;
            this.darkListView3.Text = "triggeredByList";
            // 
            // ObjectDetailsExplorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.darkGroupBox3);
            this.Controls.Add(this.darkGroupBox2);
            this.Controls.Add(this.darkGroupBox1);
            this.DockText = "Object Details";
            this.Name = "ObjectDetailsExplorer";
            this.Size = new System.Drawing.Size(429, 1068);
            this.darkGroupBox1.ResumeLayout(false);
            this.darkGroupBox2.ResumeLayout(false);
            this.darkGroupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DarkUI.Controls.DarkListView referencedByList;
        private DarkUI.Controls.DarkGroupBox darkGroupBox1;
        private DarkUI.Controls.DarkListView darkListView2;
        private DarkUI.Controls.DarkListView referencesList;
        private DarkUI.Controls.DarkGroupBox darkGroupBox2;
        private DarkUI.Controls.DarkGroupBox darkGroupBox3;
        private DarkUI.Controls.DarkListView scopesList;
        private DarkUI.Controls.DarkListView darkListView3;
    }
}
