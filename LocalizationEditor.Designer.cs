namespace CK3ScriptEditor
{
    partial class LocalizationEditor
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
            this.localTextEdit = new ICSharpCode.TextEditor.TextEditorControl();
            this.done = new DarkUI.Controls.DarkButton();
            this.cancel = new DarkUI.Controls.DarkButton();
            this.SuspendLayout();
            // 
            // localTextEdit
            // 
            this.localTextEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.localTextEdit.Highlighting = null;
            this.localTextEdit.Location = new System.Drawing.Point(0, 0);
            this.localTextEdit.Name = "localTextEdit";
            this.localTextEdit.ShowLineNumbers = false;
            this.localTextEdit.ShowVRuler = false;
            this.localTextEdit.Size = new System.Drawing.Size(1300, 497);
            this.localTextEdit.TabIndex = 0;
            // 
            // done
            // 
            this.done.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.done.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.done.Location = new System.Drawing.Point(1223, 501);
            this.done.Name = "done";
            this.done.Padding = new System.Windows.Forms.Padding(5);
            this.done.Size = new System.Drawing.Size(75, 23);
            this.done.TabIndex = 1;
            this.done.Text = "Done";
            this.done.Click += new System.EventHandler(this.done_Click);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Location = new System.Drawing.Point(1147, 501);
            this.cancel.Name = "cancel";
            this.cancel.Padding = new System.Windows.Forms.Padding(5);
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 2;
            this.cancel.Text = "Cancel";
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // LocalizationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 526);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.done);
            this.Controls.Add(this.localTextEdit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LocalizationEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LocalizationEditor";
            this.ResumeLayout(false);

        }

        #endregion
        private DarkUI.Controls.DarkButton done;
        private DarkUI.Controls.DarkButton cancel;
        public ICSharpCode.TextEditor.TextEditorControl localTextEdit;
    }
}