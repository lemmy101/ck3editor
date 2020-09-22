namespace CK3ScriptEditor
{
    partial class CloneToModDialog
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
            this.chosenFilename = new DarkUI.Controls.DarkTextBox();
            this.darkLabel1 = new DarkUI.Controls.DarkLabel();
            this.darkGroupBox1 = new DarkUI.Controls.DarkGroupBox();
            this.cloneToExistingTag = new DarkUI.Controls.DarkGroupBox();
            this.existingFileView = new DarkUI.Controls.DarkTreeView();
            this.newFileRadio = new DarkUI.Controls.DarkRadioButton();
            this.cloneToExisting = new DarkUI.Controls.DarkRadioButton();
            this.OK = new DarkUI.Controls.DarkButton();
            this.Cancel = new DarkUI.Controls.DarkButton();
            this.filenameButton = new DarkUI.Controls.DarkButton();
            this.darkGroupBox1.SuspendLayout();
            this.cloneToExistingTag.SuspendLayout();
            this.SuspendLayout();
            // 
            // chosenFilename
            // 
            this.chosenFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chosenFilename.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.chosenFilename.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.chosenFilename.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.chosenFilename.Location = new System.Drawing.Point(77, 32);
            this.chosenFilename.Name = "chosenFilename";
            this.chosenFilename.ReadOnly = true;
            this.chosenFilename.Size = new System.Drawing.Size(270, 20);
            this.chosenFilename.TabIndex = 0;
            // 
            // darkLabel1
            // 
            this.darkLabel1.AutoSize = true;
            this.darkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel1.Location = new System.Drawing.Point(19, 35);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.Size = new System.Drawing.Size(52, 13);
            this.darkLabel1.TabIndex = 1;
            this.darkLabel1.Text = "Filename:";
            // 
            // darkGroupBox1
            // 
            this.darkGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkGroupBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.darkGroupBox1.Controls.Add(this.filenameButton);
            this.darkGroupBox1.Controls.Add(this.darkLabel1);
            this.darkGroupBox1.Controls.Add(this.chosenFilename);
            this.darkGroupBox1.Location = new System.Drawing.Point(32, 12);
            this.darkGroupBox1.Name = "darkGroupBox1";
            this.darkGroupBox1.Size = new System.Drawing.Size(428, 77);
            this.darkGroupBox1.TabIndex = 2;
            this.darkGroupBox1.TabStop = false;
            this.darkGroupBox1.Text = "Clone to new file";
            // 
            // cloneToExistingTag
            // 
            this.cloneToExistingTag.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cloneToExistingTag.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.cloneToExistingTag.Controls.Add(this.existingFileView);
            this.cloneToExistingTag.Location = new System.Drawing.Point(32, 100);
            this.cloneToExistingTag.Name = "cloneToExistingTag";
            this.cloneToExistingTag.Size = new System.Drawing.Size(428, 453);
            this.cloneToExistingTag.TabIndex = 3;
            this.cloneToExistingTag.TabStop = false;
            this.cloneToExistingTag.Text = "Clone to existing file";
            // 
            // existingFileView
            // 
            this.existingFileView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.existingFileView.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.existingFileView.Location = new System.Drawing.Point(7, 26);
            this.existingFileView.MaxDragChange = 20;
            this.existingFileView.Name = "existingFileView";
            this.existingFileView.Size = new System.Drawing.Size(415, 421);
            this.existingFileView.TabIndex = 0;
            this.existingFileView.Text = "darkTreeView1";
            this.existingFileView.SelectedNodesChanged += new System.EventHandler(this.existingFileView_SelectedNodesChanged);
            // 
            // newFileRadio
            // 
            this.newFileRadio.AutoSize = true;
            this.newFileRadio.Checked = true;
            this.newFileRadio.Location = new System.Drawing.Point(12, 12);
            this.newFileRadio.Name = "newFileRadio";
            this.newFileRadio.Size = new System.Drawing.Size(14, 13);
            this.newFileRadio.TabIndex = 4;
            this.newFileRadio.TabStop = true;
            this.newFileRadio.CheckedChanged += new System.EventHandler(this.newFileRadio_CheckedChanged);
            // 
            // cloneToExisting
            // 
            this.cloneToExisting.AutoSize = true;
            this.cloneToExisting.Location = new System.Drawing.Point(12, 100);
            this.cloneToExisting.Name = "cloneToExisting";
            this.cloneToExisting.Size = new System.Drawing.Size(14, 13);
            this.cloneToExisting.TabIndex = 5;
            this.cloneToExisting.CheckedChanged += new System.EventHandler(this.cloneToExisting_CheckedChanged);
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(298, 559);
            this.OK.Name = "OK";
            this.OK.Padding = new System.Windows.Forms.Padding(5);
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 6;
            this.OK.Text = "OK";
            this.OK.Click += new System.EventHandler(this.OK_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(379, 559);
            this.Cancel.Name = "Cancel";
            this.Cancel.Padding = new System.Windows.Forms.Padding(5);
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 7;
            this.Cancel.Text = "Cancel";
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // filenameButton
            // 
            this.filenameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.filenameButton.Location = new System.Drawing.Point(354, 30);
            this.filenameButton.Name = "filenameButton";
            this.filenameButton.Padding = new System.Windows.Forms.Padding(5);
            this.filenameButton.Size = new System.Drawing.Size(55, 23);
            this.filenameButton.TabIndex = 8;
            this.filenameButton.Text = "...";
            this.filenameButton.Click += new System.EventHandler(this.filenameButton_Click);
            // 
            // CloneToModDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 593);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.Controls.Add(this.cloneToExisting);
            this.Controls.Add(this.newFileRadio);
            this.Controls.Add(this.cloneToExistingTag);
            this.Controls.Add(this.darkGroupBox1);
            this.FlatBorder = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CloneToModDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Clone ScriptObject To Mod...";
            this.darkGroupBox1.ResumeLayout(false);
            this.darkGroupBox1.PerformLayout();
            this.cloneToExistingTag.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DarkUI.Controls.DarkTextBox chosenFilename;
        private DarkUI.Controls.DarkLabel darkLabel1;
        private DarkUI.Controls.DarkGroupBox darkGroupBox1;
        private DarkUI.Controls.DarkGroupBox cloneToExistingTag;
        private DarkUI.Controls.DarkTreeView existingFileView;
        private DarkUI.Controls.DarkRadioButton newFileRadio;
        private DarkUI.Controls.DarkRadioButton cloneToExisting;
        private DarkUI.Controls.DarkButton OK;
        private DarkUI.Controls.DarkButton Cancel;
        private DarkUI.Controls.DarkButton filenameButton;
    }
}