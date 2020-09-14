namespace CK3ScriptEditor
{
    partial class IntellisenseDlg
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
            this.suggestionListbox = new DarkUI.Controls.DarkListView();
            this.SuspendLayout();
            // 
            // suggestionListbox
            // 
            this.suggestionListbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.suggestionListbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.suggestionListbox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.suggestionListbox.Location = new System.Drawing.Point(0, 0);
            this.suggestionListbox.Name = "suggestionListbox";
            this.suggestionListbox.Size = new System.Drawing.Size(800, 450);
            this.suggestionListbox.TabIndex = 1;
            this.suggestionListbox.SelectedIndicesChanged += new System.EventHandler(this.suggestionListbox_SelectedIndicesChanged);
            this.suggestionListbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.suggestionListbox_KeyPress);
            this.suggestionListbox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.suggestionListbox_KeyUp);
            this.suggestionListbox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.suggestionListbox_MouseDoubleClick);
            // 
            // IntellisenseDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ControlBox = false;
            this.Controls.Add(this.suggestionListbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "IntellisenseDlg";
            this.Text = "IntellisenseDlg";
            this.ResumeLayout(false);

        }

        #endregion

        public DarkUI.Controls.DarkListView suggestionListbox;
    }
}