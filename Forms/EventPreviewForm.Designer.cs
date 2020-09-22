namespace CK3ScriptEditor
{
    partial class EventPreviewForm
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
            this.eventRepresentation = new CK3ScriptEditor.EventRepresentation();
            this.SuspendLayout();
            // 
            // eventRepresentation
            // 
            this.eventRepresentation.Location = new System.Drawing.Point(3, 12);
            this.eventRepresentation.Name = "eventRepresentation";
            this.eventRepresentation.Size = new System.Drawing.Size(1203, 681);
            this.eventRepresentation.TabIndex = 0;
            // 
            // EventPreviewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1398, 879);
            this.Controls.Add(this.eventRepresentation);
            this.Name = "EventPreviewForm";
            this.Text = "EventPreviewForm";
            this.ResumeLayout(false);

        }

        #endregion

        private EventRepresentation eventRepresentation;
    }
}