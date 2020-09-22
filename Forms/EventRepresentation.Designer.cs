namespace CK3ScriptEditor
{
    partial class EventRepresentation
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
            this.topTitle = new System.Windows.Forms.PictureBox();
            this.location = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.topTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.location)).BeginInit();
            this.SuspendLayout();
            // 
            // topTitle
            // 
            this.topTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.topTitle.Location = new System.Drawing.Point(3, 3);
            this.topTitle.Name = "topTitle";
            this.topTitle.Size = new System.Drawing.Size(1177, 77);
            this.topTitle.TabIndex = 0;
            this.topTitle.TabStop = false;
            // 
            // location
            // 
            this.location.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.location.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.location.Location = new System.Drawing.Point(3, 59);
            this.location.Name = "location";
            this.location.Size = new System.Drawing.Size(1177, 469);
            this.location.TabIndex = 1;
            this.location.TabStop = false;
            // 
            // EventRepresentation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.topTitle);
            this.Controls.Add(this.location);
            this.Name = "EventRepresentation";
            this.Size = new System.Drawing.Size(1183, 735);
            ((System.ComponentModel.ISupportInitialize)(this.topTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.location)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox topTitle;
        private System.Windows.Forms.PictureBox location;
    }
}
