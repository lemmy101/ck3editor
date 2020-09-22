namespace CK3ScriptEditor
{
    partial class BasicFind
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
            this.darkGroupBox1 = new DarkUI.Controls.DarkGroupBox();
            this.wholeWordOnly = new DarkUI.Controls.DarkCheckBox();
            this.caseSensitive = new DarkUI.Controls.DarkCheckBox();
            this.FindText = new DarkUI.Controls.DarkTextBox();
            this.darkLabel1 = new DarkUI.Controls.DarkLabel();
            this.findNext = new DarkUI.Controls.DarkButton();
            this.replaceNext = new DarkUI.Controls.DarkButton();
            this.darkLabel2 = new DarkUI.Controls.DarkLabel();
            this.ReplaceText = new DarkUI.Controls.DarkTextBox();
            this.ReplaceAll = new DarkUI.Controls.DarkButton();
            this.darkGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // darkGroupBox1
            // 
            this.darkGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkGroupBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.darkGroupBox1.Controls.Add(this.wholeWordOnly);
            this.darkGroupBox1.Controls.Add(this.caseSensitive);
            this.darkGroupBox1.Location = new System.Drawing.Point(12, 107);
            this.darkGroupBox1.Name = "darkGroupBox1";
            this.darkGroupBox1.Size = new System.Drawing.Size(393, 85);
            this.darkGroupBox1.TabIndex = 12;
            this.darkGroupBox1.TabStop = false;
            this.darkGroupBox1.Text = "Find Options";
            // 
            // wholeWordOnly
            // 
            this.wholeWordOnly.AutoSize = true;
            this.wholeWordOnly.Location = new System.Drawing.Point(21, 55);
            this.wholeWordOnly.Name = "wholeWordOnly";
            this.wholeWordOnly.Size = new System.Drawing.Size(105, 17);
            this.wholeWordOnly.TabIndex = 19;
            this.wholeWordOnly.Text = "Whole word only";
            // 
            // caseSensitive
            // 
            this.caseSensitive.AutoSize = true;
            this.caseSensitive.Checked = true;
            this.caseSensitive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.caseSensitive.Location = new System.Drawing.Point(21, 32);
            this.caseSensitive.Name = "caseSensitive";
            this.caseSensitive.Size = new System.Drawing.Size(94, 17);
            this.caseSensitive.TabIndex = 18;
            this.caseSensitive.Text = "Case sensitive";
            // 
            // FindText
            // 
            this.FindText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FindText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.FindText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FindText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FindText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.FindText.Location = new System.Drawing.Point(53, 16);
            this.FindText.Name = "FindText";
            this.FindText.Size = new System.Drawing.Size(263, 20);
            this.FindText.TabIndex = 0;
            this.FindText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FindText_KeyDown);
            // 
            // darkLabel1
            // 
            this.darkLabel1.AutoSize = true;
            this.darkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel1.Location = new System.Drawing.Point(21, 19);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.Size = new System.Drawing.Size(30, 13);
            this.darkLabel1.TabIndex = 2;
            this.darkLabel1.Text = "Find:";
            // 
            // findNext
            // 
            this.findNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.findNext.Location = new System.Drawing.Point(322, 14);
            this.findNext.Name = "findNext";
            this.findNext.Padding = new System.Windows.Forms.Padding(5);
            this.findNext.Size = new System.Drawing.Size(90, 23);
            this.findNext.TabIndex = 3;
            this.findNext.Text = "Find Next...";
            this.findNext.Click += new System.EventHandler(this.findNext_Click);
            // 
            // replaceNext
            // 
            this.replaceNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.replaceNext.Location = new System.Drawing.Point(322, 42);
            this.replaceNext.Name = "replaceNext";
            this.replaceNext.Padding = new System.Windows.Forms.Padding(5);
            this.replaceNext.Size = new System.Drawing.Size(90, 23);
            this.replaceNext.TabIndex = 7;
            this.replaceNext.Text = "Replace Next...";
            this.replaceNext.Click += new System.EventHandler(this.replaceNext_Click);
            // 
            // darkLabel2
            // 
            this.darkLabel2.AutoSize = true;
            this.darkLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel2.Location = new System.Drawing.Point(2, 46);
            this.darkLabel2.Name = "darkLabel2";
            this.darkLabel2.Size = new System.Drawing.Size(50, 13);
            this.darkLabel2.TabIndex = 6;
            this.darkLabel2.Text = "Replace:";
            // 
            // ReplaceText
            // 
            this.ReplaceText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReplaceText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.ReplaceText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ReplaceText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReplaceText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.ReplaceText.Location = new System.Drawing.Point(53, 44);
            this.ReplaceText.Name = "ReplaceText";
            this.ReplaceText.Size = new System.Drawing.Size(263, 20);
            this.ReplaceText.TabIndex = 1;
            // 
            // ReplaceAll
            // 
            this.ReplaceAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ReplaceAll.Location = new System.Drawing.Point(322, 71);
            this.ReplaceAll.Name = "ReplaceAll";
            this.ReplaceAll.Padding = new System.Windows.Forms.Padding(5);
            this.ReplaceAll.Size = new System.Drawing.Size(90, 23);
            this.ReplaceAll.TabIndex = 8;
            this.ReplaceAll.Text = "Replace All...";
            this.ReplaceAll.Click += new System.EventHandler(this.ReplaceAll_Click);
            // 
            // BasicFind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 204);
            this.Controls.Add(this.ReplaceAll);
            this.Controls.Add(this.replaceNext);
            this.Controls.Add(this.darkLabel2);
            this.Controls.Add(this.ReplaceText);
            this.Controls.Add(this.findNext);
            this.Controls.Add(this.darkLabel1);
            this.Controls.Add(this.FindText);
            this.Controls.Add(this.darkGroupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BasicFind";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Find...";
            this.darkGroupBox1.ResumeLayout(false);
            this.darkGroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DarkUI.Controls.DarkGroupBox darkGroupBox1;
        private DarkUI.Controls.DarkLabel darkLabel1;
        private DarkUI.Controls.DarkButton findNext;
        private DarkUI.Controls.DarkCheckBox wholeWordOnly;
        private DarkUI.Controls.DarkCheckBox caseSensitive;
        private DarkUI.Controls.DarkButton replaceNext;
        private DarkUI.Controls.DarkLabel darkLabel2;
        private DarkUI.Controls.DarkTextBox ReplaceText;
        public DarkUI.Controls.DarkTextBox FindText;
        private DarkUI.Controls.DarkButton ReplaceAll;
    }
}