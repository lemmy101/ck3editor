namespace CK3ScriptEditor
{
    partial class ScriptWindow
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
            this.Editor = new ICSharpCode.TextEditor.TextEditorControl();
            this.SuspendLayout();
            // 
            // textEditorControl1
            // 
            this.Editor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Editor.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Editor.Highlighting = null;
            this.Editor.Location = new System.Drawing.Point(4, 28);
            this.Editor.Name = "Editor";
            this.Editor.Size = new System.Drawing.Size(812, 590);
            this.Editor.TabIndex = 0;
            this.Editor.Text = "textEditorControl1";
            this.Editor.Click += new System.EventHandler(this.textEditorControl1_Click);
            this.Editor.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textEditorControl1_KeyUp);
            // 
            // ScriptWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Editor);
            this.Name = "ScriptWindow";
            this.Size = new System.Drawing.Size(820, 621);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
