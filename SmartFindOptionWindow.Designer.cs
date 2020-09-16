namespace CK3ScriptEditor
{
    partial class SmartFindOptionWindow
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
            this.darkGroupBox1 = new DarkUI.Controls.DarkGroupBox();
            this.contextFind = new DarkUI.Controls.DarkRadioButton();
            this.rawTextFind = new DarkUI.Controls.DarkRadioButton();
            this.darkGroupBox2 = new DarkUI.Controls.DarkGroupBox();
            this.doesFindOverridden = new DarkUI.Controls.DarkCheckBox();
            this.checkAllTypes = new DarkUI.Controls.DarkButton();
            this.uncheckAllTypes = new DarkUI.Controls.DarkButton();
            this.darkGroupBox4 = new DarkUI.Controls.DarkGroupBox();
            this.checkAllScopes = new DarkUI.Controls.DarkButton();
            this.uncheckAllScopes = new DarkUI.Controls.DarkButton();
            this.scopeProvinces = new DarkUI.Controls.DarkCheckBox();
            this.scopeTitles = new DarkUI.Controls.DarkCheckBox();
            this.scopeSchemes = new DarkUI.Controls.DarkCheckBox();
            this.scopeCharacters = new DarkUI.Controls.DarkCheckBox();
            this.doesFindEffectFuncs = new DarkUI.Controls.DarkCheckBox();
            this.doesTriggerFuncs = new DarkUI.Controls.DarkCheckBox();
            this.doesFindValues = new DarkUI.Controls.DarkCheckBox();
            this.doesFindFuncParams = new DarkUI.Controls.DarkCheckBox();
            this.doesFindScopes = new DarkUI.Controls.DarkCheckBox();
            this.darkLabel1 = new DarkUI.Controls.DarkLabel();
            this.searchText = new DarkUI.Controls.DarkTextBox();
            this.findInFiles = new DarkUI.Controls.DarkButton();
            this.caseSensitive = new DarkUI.Controls.DarkCheckBox();
            this.wholeWordOnly = new DarkUI.Controls.DarkCheckBox();
            this.findInModFiles = new DarkUI.Controls.DarkCheckBox();
            this.findInBaseFiles = new DarkUI.Controls.DarkCheckBox();
            this.darkGroupBox3 = new DarkUI.Controls.DarkGroupBox();
            this.darkGroupBox5 = new DarkUI.Controls.DarkGroupBox();
            this.showChildren = new DarkUI.Controls.DarkCheckBox();
            this.darkGroupBox1.SuspendLayout();
            this.darkGroupBox2.SuspendLayout();
            this.darkGroupBox4.SuspendLayout();
            this.darkGroupBox3.SuspendLayout();
            this.darkGroupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // darkGroupBox1
            // 
            this.darkGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkGroupBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.darkGroupBox1.Controls.Add(this.contextFind);
            this.darkGroupBox1.Controls.Add(this.rawTextFind);
            this.darkGroupBox1.Location = new System.Drawing.Point(0, 171);
            this.darkGroupBox1.Name = "darkGroupBox1";
            this.darkGroupBox1.Size = new System.Drawing.Size(356, 65);
            this.darkGroupBox1.TabIndex = 0;
            this.darkGroupBox1.TabStop = false;
            this.darkGroupBox1.Text = "Find Type:";
            // 
            // contextFind
            // 
            this.contextFind.AutoSize = true;
            this.contextFind.Checked = true;
            this.contextFind.Location = new System.Drawing.Point(114, 28);
            this.contextFind.Name = "contextFind";
            this.contextFind.Size = new System.Drawing.Size(61, 17);
            this.contextFind.TabIndex = 1;
            this.contextFind.TabStop = true;
            this.contextFind.Text = "Context";
            this.contextFind.CheckedChanged += new System.EventHandler(this.contextFind_CheckedChanged);
            // 
            // rawTextFind
            // 
            this.rawTextFind.AutoSize = true;
            this.rawTextFind.Location = new System.Drawing.Point(17, 28);
            this.rawTextFind.Name = "rawTextFind";
            this.rawTextFind.Size = new System.Drawing.Size(71, 17);
            this.rawTextFind.TabIndex = 0;
            this.rawTextFind.Text = "Raw Text";
            this.rawTextFind.CheckedChanged += new System.EventHandler(this.rawTextFind_CheckedChanged);
            // 
            // darkGroupBox2
            // 
            this.darkGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkGroupBox2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.darkGroupBox2.Controls.Add(this.darkGroupBox5);
            this.darkGroupBox2.Controls.Add(this.doesFindOverridden);
            this.darkGroupBox2.Controls.Add(this.checkAllTypes);
            this.darkGroupBox2.Controls.Add(this.uncheckAllTypes);
            this.darkGroupBox2.Controls.Add(this.darkGroupBox4);
            this.darkGroupBox2.Controls.Add(this.doesFindEffectFuncs);
            this.darkGroupBox2.Controls.Add(this.doesTriggerFuncs);
            this.darkGroupBox2.Controls.Add(this.doesFindValues);
            this.darkGroupBox2.Controls.Add(this.doesFindFuncParams);
            this.darkGroupBox2.Controls.Add(this.doesFindScopes);
            this.darkGroupBox2.Location = new System.Drawing.Point(0, 242);
            this.darkGroupBox2.Name = "darkGroupBox2";
            this.darkGroupBox2.Size = new System.Drawing.Size(356, 699);
            this.darkGroupBox2.TabIndex = 2;
            this.darkGroupBox2.TabStop = false;
            this.darkGroupBox2.Text = "Context Find Options:";
            // 
            // doesFindOverridden
            // 
            this.doesFindOverridden.AutoSize = true;
            this.doesFindOverridden.Checked = true;
            this.doesFindOverridden.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doesFindOverridden.Location = new System.Drawing.Point(194, 69);
            this.doesFindOverridden.Name = "doesFindOverridden";
            this.doesFindOverridden.Size = new System.Drawing.Size(136, 17);
            this.doesFindOverridden.TabIndex = 16;
            this.doesFindOverridden.Text = "Search Mod Overidden";
            // 
            // checkAllTypes
            // 
            this.checkAllTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkAllTypes.Location = new System.Drawing.Point(275, 19);
            this.checkAllTypes.Name = "checkAllTypes";
            this.checkAllTypes.Padding = new System.Windows.Forms.Padding(5);
            this.checkAllTypes.Size = new System.Drawing.Size(75, 23);
            this.checkAllTypes.TabIndex = 7;
            this.checkAllTypes.Text = "Check All";
            this.checkAllTypes.Click += new System.EventHandler(this.checkAllTypes_Click);
            // 
            // uncheckAllTypes
            // 
            this.uncheckAllTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uncheckAllTypes.Location = new System.Drawing.Point(194, 19);
            this.uncheckAllTypes.Name = "uncheckAllTypes";
            this.uncheckAllTypes.Padding = new System.Windows.Forms.Padding(5);
            this.uncheckAllTypes.Size = new System.Drawing.Size(75, 23);
            this.uncheckAllTypes.TabIndex = 8;
            this.uncheckAllTypes.Text = "Uncheck All";
            this.uncheckAllTypes.Click += new System.EventHandler(this.uncheckAllTypes_Click);
            // 
            // darkGroupBox4
            // 
            this.darkGroupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkGroupBox4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.darkGroupBox4.Controls.Add(this.checkAllScopes);
            this.darkGroupBox4.Controls.Add(this.uncheckAllScopes);
            this.darkGroupBox4.Controls.Add(this.scopeProvinces);
            this.darkGroupBox4.Controls.Add(this.scopeTitles);
            this.darkGroupBox4.Controls.Add(this.scopeSchemes);
            this.darkGroupBox4.Controls.Add(this.scopeCharacters);
            this.darkGroupBox4.Location = new System.Drawing.Point(25, 187);
            this.darkGroupBox4.Name = "darkGroupBox4";
            this.darkGroupBox4.Size = new System.Drawing.Size(316, 201);
            this.darkGroupBox4.TabIndex = 15;
            this.darkGroupBox4.TabStop = false;
            this.darkGroupBox4.Text = "Scopes";
            // 
            // checkAllScopes
            // 
            this.checkAllScopes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkAllScopes.Location = new System.Drawing.Point(223, 19);
            this.checkAllScopes.Name = "checkAllScopes";
            this.checkAllScopes.Padding = new System.Windows.Forms.Padding(5);
            this.checkAllScopes.Size = new System.Drawing.Size(75, 23);
            this.checkAllScopes.TabIndex = 16;
            this.checkAllScopes.Text = "Check All";
            this.checkAllScopes.Click += new System.EventHandler(this.checkAllScopes_Click);
            // 
            // uncheckAllScopes
            // 
            this.uncheckAllScopes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uncheckAllScopes.Location = new System.Drawing.Point(142, 19);
            this.uncheckAllScopes.Name = "uncheckAllScopes";
            this.uncheckAllScopes.Padding = new System.Windows.Forms.Padding(5);
            this.uncheckAllScopes.Size = new System.Drawing.Size(75, 23);
            this.uncheckAllScopes.TabIndex = 17;
            this.uncheckAllScopes.Text = "Uncheck All";
            this.uncheckAllScopes.Click += new System.EventHandler(this.uncheckAllScopes_Click);
            // 
            // scopeProvinces
            // 
            this.scopeProvinces.AutoSize = true;
            this.scopeProvinces.Checked = true;
            this.scopeProvinces.CheckState = System.Windows.Forms.CheckState.Checked;
            this.scopeProvinces.Location = new System.Drawing.Point(28, 97);
            this.scopeProvinces.Name = "scopeProvinces";
            this.scopeProvinces.Size = new System.Drawing.Size(68, 17);
            this.scopeProvinces.TabIndex = 18;
            this.scopeProvinces.Text = "Province";
            // 
            // scopeTitles
            // 
            this.scopeTitles.AutoSize = true;
            this.scopeTitles.Checked = true;
            this.scopeTitles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.scopeTitles.Location = new System.Drawing.Point(28, 74);
            this.scopeTitles.Name = "scopeTitles";
            this.scopeTitles.Size = new System.Drawing.Size(46, 17);
            this.scopeTitles.TabIndex = 17;
            this.scopeTitles.Text = "Title";
            // 
            // scopeSchemes
            // 
            this.scopeSchemes.AutoSize = true;
            this.scopeSchemes.Checked = true;
            this.scopeSchemes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.scopeSchemes.Location = new System.Drawing.Point(28, 51);
            this.scopeSchemes.Name = "scopeSchemes";
            this.scopeSchemes.Size = new System.Drawing.Size(65, 17);
            this.scopeSchemes.TabIndex = 14;
            this.scopeSchemes.Text = "Scheme";
            // 
            // scopeCharacters
            // 
            this.scopeCharacters.AutoSize = true;
            this.scopeCharacters.Checked = true;
            this.scopeCharacters.CheckState = System.Windows.Forms.CheckState.Checked;
            this.scopeCharacters.Location = new System.Drawing.Point(28, 28);
            this.scopeCharacters.Name = "scopeCharacters";
            this.scopeCharacters.Size = new System.Drawing.Size(72, 17);
            this.scopeCharacters.TabIndex = 13;
            this.scopeCharacters.Text = "Character";
            // 
            // doesFindEffectFuncs
            // 
            this.doesFindEffectFuncs.AutoSize = true;
            this.doesFindEffectFuncs.Checked = true;
            this.doesFindEffectFuncs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doesFindEffectFuncs.Location = new System.Drawing.Point(25, 150);
            this.doesFindEffectFuncs.Name = "doesFindEffectFuncs";
            this.doesFindEffectFuncs.Size = new System.Drawing.Size(103, 17);
            this.doesFindEffectFuncs.TabIndex = 12;
            this.doesFindEffectFuncs.Text = "Effect Functions";
            // 
            // doesTriggerFuncs
            // 
            this.doesTriggerFuncs.AutoSize = true;
            this.doesTriggerFuncs.Checked = true;
            this.doesTriggerFuncs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doesTriggerFuncs.Location = new System.Drawing.Point(25, 127);
            this.doesTriggerFuncs.Name = "doesTriggerFuncs";
            this.doesTriggerFuncs.Size = new System.Drawing.Size(108, 17);
            this.doesTriggerFuncs.TabIndex = 11;
            this.doesTriggerFuncs.Text = "Trigger Functions";
            // 
            // doesFindValues
            // 
            this.doesFindValues.AutoSize = true;
            this.doesFindValues.Checked = true;
            this.doesFindValues.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doesFindValues.Location = new System.Drawing.Point(25, 92);
            this.doesFindValues.Name = "doesFindValues";
            this.doesFindValues.Size = new System.Drawing.Size(58, 17);
            this.doesFindValues.TabIndex = 10;
            this.doesFindValues.Text = "Values";
            // 
            // doesFindFuncParams
            // 
            this.doesFindFuncParams.AutoSize = true;
            this.doesFindFuncParams.Checked = true;
            this.doesFindFuncParams.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doesFindFuncParams.Location = new System.Drawing.Point(25, 69);
            this.doesFindFuncParams.Name = "doesFindFuncParams";
            this.doesFindFuncParams.Size = new System.Drawing.Size(123, 17);
            this.doesFindFuncParams.TabIndex = 9;
            this.doesFindFuncParams.Text = "Function Parameters";
            // 
            // doesFindScopes
            // 
            this.doesFindScopes.AutoSize = true;
            this.doesFindScopes.Checked = true;
            this.doesFindScopes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doesFindScopes.Location = new System.Drawing.Point(25, 34);
            this.doesFindScopes.Name = "doesFindScopes";
            this.doesFindScopes.Size = new System.Drawing.Size(96, 17);
            this.doesFindScopes.TabIndex = 7;
            this.doesFindScopes.Text = "Saved Scopes";
            // 
            // darkLabel1
            // 
            this.darkLabel1.AutoSize = true;
            this.darkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel1.Location = new System.Drawing.Point(14, 49);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.Size = new System.Drawing.Size(31, 13);
            this.darkLabel1.TabIndex = 4;
            this.darkLabel1.Text = "Text:";
            // 
            // searchText
            // 
            this.searchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.searchText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.searchText.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.searchText.Location = new System.Drawing.Point(51, 47);
            this.searchText.Name = "searchText";
            this.searchText.Size = new System.Drawing.Size(305, 20);
            this.searchText.TabIndex = 0;
            this.searchText.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.searchText_KeyPress);
            this.searchText.KeyUp += new System.Windows.Forms.KeyEventHandler(this.searchText_KeyUp);
            // 
            // findInFiles
            // 
            this.findInFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.findInFiles.Location = new System.Drawing.Point(269, 947);
            this.findInFiles.Name = "findInFiles";
            this.findInFiles.Padding = new System.Windows.Forms.Padding(5);
            this.findInFiles.Size = new System.Drawing.Size(75, 23);
            this.findInFiles.TabIndex = 6;
            this.findInFiles.Text = "Find";
            this.findInFiles.Click += new System.EventHandler(this.findInFiles_Click);
            // 
            // caseSensitive
            // 
            this.caseSensitive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.caseSensitive.AutoSize = true;
            this.caseSensitive.Location = new System.Drawing.Point(148, 82);
            this.caseSensitive.Name = "caseSensitive";
            this.caseSensitive.Size = new System.Drawing.Size(94, 17);
            this.caseSensitive.TabIndex = 16;
            this.caseSensitive.Text = "Case sensitive";
            // 
            // wholeWordOnly
            // 
            this.wholeWordOnly.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.wholeWordOnly.AutoSize = true;
            this.wholeWordOnly.Location = new System.Drawing.Point(248, 82);
            this.wholeWordOnly.Name = "wholeWordOnly";
            this.wholeWordOnly.Size = new System.Drawing.Size(105, 17);
            this.wholeWordOnly.TabIndex = 17;
            this.wholeWordOnly.Text = "Whole word only";
            // 
            // findInModFiles
            // 
            this.findInModFiles.AutoSize = true;
            this.findInModFiles.Checked = true;
            this.findInModFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.findInModFiles.Location = new System.Drawing.Point(145, 19);
            this.findInModFiles.Name = "findInModFiles";
            this.findInModFiles.Size = new System.Drawing.Size(71, 17);
            this.findInModFiles.TabIndex = 17;
            this.findInModFiles.Text = "Mod Files";
            // 
            // findInBaseFiles
            // 
            this.findInBaseFiles.AutoSize = true;
            this.findInBaseFiles.Checked = true;
            this.findInBaseFiles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.findInBaseFiles.Location = new System.Drawing.Point(26, 19);
            this.findInBaseFiles.Name = "findInBaseFiles";
            this.findInBaseFiles.Size = new System.Drawing.Size(97, 17);
            this.findInBaseFiles.TabIndex = 16;
            this.findInBaseFiles.Text = "Base CK3 Files";
            // 
            // darkGroupBox3
            // 
            this.darkGroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkGroupBox3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.darkGroupBox3.Controls.Add(this.findInBaseFiles);
            this.darkGroupBox3.Controls.Add(this.findInModFiles);
            this.darkGroupBox3.Location = new System.Drawing.Point(3, 114);
            this.darkGroupBox3.Name = "darkGroupBox3";
            this.darkGroupBox3.Size = new System.Drawing.Size(353, 51);
            this.darkGroupBox3.TabIndex = 2;
            this.darkGroupBox3.TabStop = false;
            this.darkGroupBox3.Text = "Search in:";
            // 
            // darkGroupBox5
            // 
            this.darkGroupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkGroupBox5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.darkGroupBox5.Controls.Add(this.showChildren);
            this.darkGroupBox5.Location = new System.Drawing.Point(25, 394);
            this.darkGroupBox5.Name = "darkGroupBox5";
            this.darkGroupBox5.Size = new System.Drawing.Size(316, 288);
            this.darkGroupBox5.TabIndex = 19;
            this.darkGroupBox5.TabStop = false;
            this.darkGroupBox5.Text = "Results";
            // 
            // showChildren
            // 
            this.showChildren.AutoSize = true;
            this.showChildren.Checked = true;
            this.showChildren.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showChildren.Location = new System.Drawing.Point(28, 28);
            this.showChildren.Name = "showChildren";
            this.showChildren.Size = new System.Drawing.Size(126, 17);
            this.showChildren.TabIndex = 13;
            this.showChildren.Text = "Show results children";
            this.showChildren.CheckedChanged += new System.EventHandler(this.showChildren_CheckedChanged);
            // 
            // SmartFindOptionWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.darkGroupBox3);
            this.Controls.Add(this.wholeWordOnly);
            this.Controls.Add(this.caseSensitive);
            this.Controls.Add(this.findInFiles);
            this.Controls.Add(this.searchText);
            this.Controls.Add(this.darkLabel1);
            this.Controls.Add(this.darkGroupBox2);
            this.Controls.Add(this.darkGroupBox1);
            this.DockText = "Smart Find";
            this.Name = "SmartFindOptionWindow";
            this.Size = new System.Drawing.Size(362, 987);
            this.Enter += new System.EventHandler(this.SmartFindOptionWindow_Enter);
            this.darkGroupBox1.ResumeLayout(false);
            this.darkGroupBox1.PerformLayout();
            this.darkGroupBox2.ResumeLayout(false);
            this.darkGroupBox2.PerformLayout();
            this.darkGroupBox4.ResumeLayout(false);
            this.darkGroupBox4.PerformLayout();
            this.darkGroupBox3.ResumeLayout(false);
            this.darkGroupBox3.PerformLayout();
            this.darkGroupBox5.ResumeLayout(false);
            this.darkGroupBox5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DarkUI.Controls.DarkGroupBox darkGroupBox1;
        private DarkUI.Controls.DarkRadioButton contextFind;
        private DarkUI.Controls.DarkRadioButton rawTextFind;
        private DarkUI.Controls.DarkGroupBox darkGroupBox2;
        private DarkUI.Controls.DarkLabel darkLabel1;
        private DarkUI.Controls.DarkTextBox searchText;
        private DarkUI.Controls.DarkButton findInFiles;
        private DarkUI.Controls.DarkCheckBox doesFindScopes;
        private DarkUI.Controls.DarkGroupBox darkGroupBox4;
        private DarkUI.Controls.DarkCheckBox scopeProvinces;
        private DarkUI.Controls.DarkCheckBox scopeTitles;
        private DarkUI.Controls.DarkCheckBox scopeSchemes;
        private DarkUI.Controls.DarkCheckBox scopeCharacters;
        private DarkUI.Controls.DarkCheckBox doesFindEffectFuncs;
        private DarkUI.Controls.DarkCheckBox doesTriggerFuncs;
        private DarkUI.Controls.DarkCheckBox doesFindValues;
        private DarkUI.Controls.DarkCheckBox doesFindFuncParams;
        private DarkUI.Controls.DarkButton checkAllTypes;
        private DarkUI.Controls.DarkButton uncheckAllTypes;
        private DarkUI.Controls.DarkCheckBox caseSensitive;
        private DarkUI.Controls.DarkCheckBox wholeWordOnly;
        private DarkUI.Controls.DarkCheckBox findInModFiles;
        private DarkUI.Controls.DarkCheckBox findInBaseFiles;
        private DarkUI.Controls.DarkGroupBox darkGroupBox3;
        private DarkUI.Controls.DarkButton checkAllScopes;
        private DarkUI.Controls.DarkButton uncheckAllScopes;
        private DarkUI.Controls.DarkCheckBox doesFindOverridden;
        private DarkUI.Controls.DarkGroupBox darkGroupBox5;
        private DarkUI.Controls.DarkCheckBox showChildren;
    }
}
