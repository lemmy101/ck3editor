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
            this.darkGroupBox2 = new DarkUI.Controls.DarkGroupBox();
            this.darkGroupBox5 = new DarkUI.Controls.DarkGroupBox();
            this.showChildren = new DarkUI.Controls.DarkCheckBox();
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
            this.doesRHS = new DarkUI.Controls.DarkCheckBox();
            this.darkLabel1 = new DarkUI.Controls.DarkLabel();
            this.searchText = new DarkUI.Controls.DarkTextBox();
            this.findInFiles = new DarkUI.Controls.DarkButton();
            this.caseSensitive = new DarkUI.Controls.DarkCheckBox();
            this.wholeWordOnly = new DarkUI.Controls.DarkCheckBox();
            this.findInModFiles = new DarkUI.Controls.DarkCheckBox();
            this.findInBaseFiles = new DarkUI.Controls.DarkCheckBox();
            this.darkGroupBox3 = new DarkUI.Controls.DarkGroupBox();
            this.doLHS = new DarkUI.Controls.DarkCheckBox();
            this.activityScope = new DarkUI.Controls.DarkCheckBox();
            this.secretScope = new DarkUI.Controls.DarkCheckBox();
            this.combatScope = new DarkUI.Controls.DarkCheckBox();
            this.combatSideScope = new DarkUI.Controls.DarkCheckBox();
            this.tavcScope = new DarkUI.Controls.DarkCheckBox();
            this.faithScope = new DarkUI.Controls.DarkCheckBox();
            this.greatHolyScope = new DarkUI.Controls.DarkCheckBox();
            this.warScope = new DarkUI.Controls.DarkCheckBox();
            this.religionScope = new DarkUI.Controls.DarkCheckBox();
            this.storyScope = new DarkUI.Controls.DarkCheckBox();
            this.casusScope = new DarkUI.Controls.DarkCheckBox();
            this.dynastyScsope = new DarkUI.Controls.DarkCheckBox();
            this.houseScope = new DarkUI.Controls.DarkCheckBox();
            this.factionScope = new DarkUI.Controls.DarkCheckBox();
            this.cultureScope = new DarkUI.Controls.DarkCheckBox();
            this.armyScope = new DarkUI.Controls.DarkCheckBox();
            this.holyOrderScope = new DarkUI.Controls.DarkCheckBox();
            this.councilTaskScope = new DarkUI.Controls.DarkCheckBox();
            this.mercScope = new DarkUI.Controls.DarkCheckBox();
            this.cultureGScope = new DarkUI.Controls.DarkCheckBox();
            this.noneScope = new DarkUI.Controls.DarkCheckBox();
            this.darkGroupBox2.SuspendLayout();
            this.darkGroupBox5.SuspendLayout();
            this.darkGroupBox4.SuspendLayout();
            this.darkGroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // darkGroupBox2
            // 
            this.darkGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkGroupBox2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.darkGroupBox2.Controls.Add(this.doLHS);
            this.darkGroupBox2.Controls.Add(this.darkGroupBox5);
            this.darkGroupBox2.Controls.Add(this.doesFindOverridden);
            this.darkGroupBox2.Controls.Add(this.checkAllTypes);
            this.darkGroupBox2.Controls.Add(this.uncheckAllTypes);
            this.darkGroupBox2.Controls.Add(this.darkGroupBox4);
            this.darkGroupBox2.Controls.Add(this.doesRHS);
            this.darkGroupBox2.Location = new System.Drawing.Point(0, 171);
            this.darkGroupBox2.Name = "darkGroupBox2";
            this.darkGroupBox2.Size = new System.Drawing.Size(356, 770);
            this.darkGroupBox2.TabIndex = 2;
            this.darkGroupBox2.TabStop = false;
            this.darkGroupBox2.Text = "Context Find Options:";
            // 
            // darkGroupBox5
            // 
            this.darkGroupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.darkGroupBox5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.darkGroupBox5.Controls.Add(this.showChildren);
            this.darkGroupBox5.Location = new System.Drawing.Point(25, 554);
            this.darkGroupBox5.Name = "darkGroupBox5";
            this.darkGroupBox5.Size = new System.Drawing.Size(316, 199);
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
            // doesFindOverridden
            // 
            this.doesFindOverridden.AutoSize = true;
            this.doesFindOverridden.Checked = true;
            this.doesFindOverridden.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doesFindOverridden.Location = new System.Drawing.Point(25, 81);
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
            this.darkGroupBox4.Controls.Add(this.noneScope);
            this.darkGroupBox4.Controls.Add(this.cultureGScope);
            this.darkGroupBox4.Controls.Add(this.mercScope);
            this.darkGroupBox4.Controls.Add(this.councilTaskScope);
            this.darkGroupBox4.Controls.Add(this.holyOrderScope);
            this.darkGroupBox4.Controls.Add(this.armyScope);
            this.darkGroupBox4.Controls.Add(this.cultureScope);
            this.darkGroupBox4.Controls.Add(this.factionScope);
            this.darkGroupBox4.Controls.Add(this.houseScope);
            this.darkGroupBox4.Controls.Add(this.dynastyScsope);
            this.darkGroupBox4.Controls.Add(this.casusScope);
            this.darkGroupBox4.Controls.Add(this.storyScope);
            this.darkGroupBox4.Controls.Add(this.religionScope);
            this.darkGroupBox4.Controls.Add(this.warScope);
            this.darkGroupBox4.Controls.Add(this.greatHolyScope);
            this.darkGroupBox4.Controls.Add(this.faithScope);
            this.darkGroupBox4.Controls.Add(this.tavcScope);
            this.darkGroupBox4.Controls.Add(this.combatSideScope);
            this.darkGroupBox4.Controls.Add(this.combatScope);
            this.darkGroupBox4.Controls.Add(this.secretScope);
            this.darkGroupBox4.Controls.Add(this.activityScope);
            this.darkGroupBox4.Controls.Add(this.checkAllScopes);
            this.darkGroupBox4.Controls.Add(this.uncheckAllScopes);
            this.darkGroupBox4.Controls.Add(this.scopeProvinces);
            this.darkGroupBox4.Controls.Add(this.scopeTitles);
            this.darkGroupBox4.Controls.Add(this.scopeSchemes);
            this.darkGroupBox4.Controls.Add(this.scopeCharacters);
            this.darkGroupBox4.Location = new System.Drawing.Point(25, 118);
            this.darkGroupBox4.Name = "darkGroupBox4";
            this.darkGroupBox4.Size = new System.Drawing.Size(316, 430);
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
            this.scopeProvinces.Location = new System.Drawing.Point(28, 121);
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
            this.scopeTitles.Location = new System.Drawing.Point(28, 98);
            this.scopeTitles.Name = "scopeTitles";
            this.scopeTitles.Size = new System.Drawing.Size(85, 17);
            this.scopeTitles.TabIndex = 17;
            this.scopeTitles.Text = "Landed Title";
            // 
            // scopeSchemes
            // 
            this.scopeSchemes.AutoSize = true;
            this.scopeSchemes.Checked = true;
            this.scopeSchemes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.scopeSchemes.Location = new System.Drawing.Point(28, 75);
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
            this.scopeCharacters.Location = new System.Drawing.Point(28, 52);
            this.scopeCharacters.Name = "scopeCharacters";
            this.scopeCharacters.Size = new System.Drawing.Size(72, 17);
            this.scopeCharacters.TabIndex = 13;
            this.scopeCharacters.Text = "Character";
            // 
            // doesRHS
            // 
            this.doesRHS.AutoSize = true;
            this.doesRHS.Checked = true;
            this.doesRHS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doesRHS.Location = new System.Drawing.Point(25, 48);
            this.doesRHS.Name = "doesRHS";
            this.doesRHS.Size = new System.Drawing.Size(123, 17);
            this.doesRHS.TabIndex = 10;
            this.doesRHS.Text = "rhs (Right hand side)";
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
            // doLHS
            // 
            this.doLHS.AutoSize = true;
            this.doLHS.Checked = true;
            this.doLHS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.doLHS.Location = new System.Drawing.Point(25, 25);
            this.doLHS.Name = "doLHS";
            this.doLHS.Size = new System.Drawing.Size(115, 17);
            this.doLHS.TabIndex = 20;
            this.doLHS.Text = "lhs (Left hand side)";
            // 
            // activityScope
            // 
            this.activityScope.AutoSize = true;
            this.activityScope.Checked = true;
            this.activityScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.activityScope.Location = new System.Drawing.Point(28, 144);
            this.activityScope.Name = "activityScope";
            this.activityScope.Size = new System.Drawing.Size(60, 17);
            this.activityScope.TabIndex = 19;
            this.activityScope.Text = "Activity";
            // 
            // secretScope
            // 
            this.secretScope.AutoSize = true;
            this.secretScope.Checked = true;
            this.secretScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.secretScope.Location = new System.Drawing.Point(28, 167);
            this.secretScope.Name = "secretScope";
            this.secretScope.Size = new System.Drawing.Size(57, 17);
            this.secretScope.TabIndex = 20;
            this.secretScope.Text = "Secret";
            // 
            // combatScope
            // 
            this.combatScope.AutoSize = true;
            this.combatScope.Checked = true;
            this.combatScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.combatScope.Location = new System.Drawing.Point(28, 190);
            this.combatScope.Name = "combatScope";
            this.combatScope.Size = new System.Drawing.Size(62, 17);
            this.combatScope.TabIndex = 21;
            this.combatScope.Text = "Combat";
            // 
            // combatSideScope
            // 
            this.combatSideScope.AutoSize = true;
            this.combatSideScope.Checked = true;
            this.combatSideScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.combatSideScope.Location = new System.Drawing.Point(28, 213);
            this.combatSideScope.Name = "combatSideScope";
            this.combatSideScope.Size = new System.Drawing.Size(86, 17);
            this.combatSideScope.TabIndex = 22;
            this.combatSideScope.Text = "Combat Side";
            // 
            // tavcScope
            // 
            this.tavcScope.Checked = true;
            this.tavcScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tavcScope.Location = new System.Drawing.Point(28, 236);
            this.tavcScope.Name = "tavcScope";
            this.tavcScope.Size = new System.Drawing.Size(137, 17);
            this.tavcScope.TabIndex = 23;
            this.tavcScope.Text = "Title & Vassal Change";
            // 
            // faithScope
            // 
            this.faithScope.AutoSize = true;
            this.faithScope.Checked = true;
            this.faithScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.faithScope.Location = new System.Drawing.Point(28, 259);
            this.faithScope.Name = "faithScope";
            this.faithScope.Size = new System.Drawing.Size(49, 17);
            this.faithScope.TabIndex = 24;
            this.faithScope.Text = "Faith";
            // 
            // greatHolyScope
            // 
            this.greatHolyScope.AutoSize = true;
            this.greatHolyScope.Checked = true;
            this.greatHolyScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.greatHolyScope.Location = new System.Drawing.Point(28, 328);
            this.greatHolyScope.Name = "greatHolyScope";
            this.greatHolyScope.Size = new System.Drawing.Size(99, 17);
            this.greatHolyScope.TabIndex = 25;
            this.greatHolyScope.Text = "Great Holy War";
            // 
            // warScope
            // 
            this.warScope.AutoSize = true;
            this.warScope.Checked = true;
            this.warScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.warScope.Location = new System.Drawing.Point(28, 305);
            this.warScope.Name = "warScope";
            this.warScope.Size = new System.Drawing.Size(46, 17);
            this.warScope.TabIndex = 26;
            this.warScope.Text = "War";
            // 
            // religionScope
            // 
            this.religionScope.AutoSize = true;
            this.religionScope.Checked = true;
            this.religionScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.religionScope.Location = new System.Drawing.Point(28, 282);
            this.religionScope.Name = "religionScope";
            this.religionScope.Size = new System.Drawing.Size(64, 17);
            this.religionScope.TabIndex = 27;
            this.religionScope.Text = "Religion";
            // 
            // storyScope
            // 
            this.storyScope.AutoSize = true;
            this.storyScope.Checked = true;
            this.storyScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.storyScope.Location = new System.Drawing.Point(28, 351);
            this.storyScope.Name = "storyScope";
            this.storyScope.Size = new System.Drawing.Size(79, 17);
            this.storyScope.TabIndex = 28;
            this.storyScope.Text = "Story Cycle";
            // 
            // casusScope
            // 
            this.casusScope.AutoSize = true;
            this.casusScope.Checked = true;
            this.casusScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.casusScope.Location = new System.Drawing.Point(182, 52);
            this.casusScope.Name = "casusScope";
            this.casusScope.Size = new System.Drawing.Size(77, 17);
            this.casusScope.TabIndex = 29;
            this.casusScope.Text = "Casus Belli";
            // 
            // dynastyScsope
            // 
            this.dynastyScsope.AutoSize = true;
            this.dynastyScsope.Checked = true;
            this.dynastyScsope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.dynastyScsope.Location = new System.Drawing.Point(182, 74);
            this.dynastyScsope.Name = "dynastyScsope";
            this.dynastyScsope.Size = new System.Drawing.Size(64, 17);
            this.dynastyScsope.TabIndex = 30;
            this.dynastyScsope.Text = "Dynasty";
            // 
            // houseScope
            // 
            this.houseScope.AutoSize = true;
            this.houseScope.Checked = true;
            this.houseScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.houseScope.Location = new System.Drawing.Point(182, 98);
            this.houseScope.Name = "houseScope";
            this.houseScope.Size = new System.Drawing.Size(57, 17);
            this.houseScope.TabIndex = 31;
            this.houseScope.Text = "House";
            // 
            // factionScope
            // 
            this.factionScope.AutoSize = true;
            this.factionScope.Checked = true;
            this.factionScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.factionScope.Location = new System.Drawing.Point(182, 121);
            this.factionScope.Name = "factionScope";
            this.factionScope.Size = new System.Drawing.Size(61, 17);
            this.factionScope.TabIndex = 32;
            this.factionScope.Text = "Faction";
            // 
            // cultureScope
            // 
            this.cultureScope.AutoSize = true;
            this.cultureScope.Checked = true;
            this.cultureScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cultureScope.Location = new System.Drawing.Point(182, 144);
            this.cultureScope.Name = "cultureScope";
            this.cultureScope.Size = new System.Drawing.Size(59, 17);
            this.cultureScope.TabIndex = 33;
            this.cultureScope.Text = "Culture";
            // 
            // armyScope
            // 
            this.armyScope.AutoSize = true;
            this.armyScope.Checked = true;
            this.armyScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.armyScope.Location = new System.Drawing.Point(182, 190);
            this.armyScope.Name = "armyScope";
            this.armyScope.Size = new System.Drawing.Size(49, 17);
            this.armyScope.TabIndex = 34;
            this.armyScope.Text = "Army";
            // 
            // holyOrderScope
            // 
            this.holyOrderScope.AutoSize = true;
            this.holyOrderScope.Checked = true;
            this.holyOrderScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.holyOrderScope.Location = new System.Drawing.Point(181, 213);
            this.holyOrderScope.Name = "holyOrderScope";
            this.holyOrderScope.Size = new System.Drawing.Size(76, 17);
            this.holyOrderScope.TabIndex = 35;
            this.holyOrderScope.Text = "Holy Order";
            // 
            // councilTaskScope
            // 
            this.councilTaskScope.AutoSize = true;
            this.councilTaskScope.Checked = true;
            this.councilTaskScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.councilTaskScope.Location = new System.Drawing.Point(181, 236);
            this.councilTaskScope.Name = "councilTaskScope";
            this.councilTaskScope.Size = new System.Drawing.Size(88, 17);
            this.councilTaskScope.TabIndex = 36;
            this.councilTaskScope.Text = "Council Task";
            // 
            // mercScope
            // 
            this.mercScope.AutoSize = true;
            this.mercScope.Checked = true;
            this.mercScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mercScope.Location = new System.Drawing.Point(181, 259);
            this.mercScope.Name = "mercScope";
            this.mercScope.Size = new System.Drawing.Size(108, 17);
            this.mercScope.TabIndex = 37;
            this.mercScope.Text = "Mercenary Group";
            // 
            // cultureGScope
            // 
            this.cultureGScope.AutoSize = true;
            this.cultureGScope.Checked = true;
            this.cultureGScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cultureGScope.Location = new System.Drawing.Point(182, 167);
            this.cultureGScope.Name = "cultureGScope";
            this.cultureGScope.Size = new System.Drawing.Size(91, 17);
            this.cultureGScope.TabIndex = 38;
            this.cultureGScope.Text = "Culture Group";
            // 
            // noneScope
            // 
            this.noneScope.AutoSize = true;
            this.noneScope.Checked = true;
            this.noneScope.CheckState = System.Windows.Forms.CheckState.Checked;
            this.noneScope.Location = new System.Drawing.Point(181, 291);
            this.noneScope.Name = "noneScope";
            this.noneScope.Size = new System.Drawing.Size(52, 17);
            this.noneScope.TabIndex = 39;
            this.noneScope.Text = "None";
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
            this.DockText = "Smart Find";
            this.Name = "SmartFindOptionWindow";
            this.Size = new System.Drawing.Size(362, 987);
            this.Enter += new System.EventHandler(this.SmartFindOptionWindow_Enter);
            this.darkGroupBox2.ResumeLayout(false);
            this.darkGroupBox2.PerformLayout();
            this.darkGroupBox5.ResumeLayout(false);
            this.darkGroupBox5.PerformLayout();
            this.darkGroupBox4.ResumeLayout(false);
            this.darkGroupBox4.PerformLayout();
            this.darkGroupBox3.ResumeLayout(false);
            this.darkGroupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DarkUI.Controls.DarkGroupBox darkGroupBox2;
        private DarkUI.Controls.DarkLabel darkLabel1;
        private DarkUI.Controls.DarkTextBox searchText;
        private DarkUI.Controls.DarkButton findInFiles;
        private DarkUI.Controls.DarkGroupBox darkGroupBox4;
        private DarkUI.Controls.DarkCheckBox scopeProvinces;
        private DarkUI.Controls.DarkCheckBox scopeTitles;
        private DarkUI.Controls.DarkCheckBox scopeSchemes;
        private DarkUI.Controls.DarkCheckBox scopeCharacters;
        private DarkUI.Controls.DarkCheckBox doesRHS;
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
        private DarkUI.Controls.DarkCheckBox doLHS;
        private DarkUI.Controls.DarkCheckBox secretScope;
        private DarkUI.Controls.DarkCheckBox activityScope;
        private DarkUI.Controls.DarkCheckBox mercScope;
        private DarkUI.Controls.DarkCheckBox councilTaskScope;
        private DarkUI.Controls.DarkCheckBox holyOrderScope;
        private DarkUI.Controls.DarkCheckBox armyScope;
        private DarkUI.Controls.DarkCheckBox cultureScope;
        private DarkUI.Controls.DarkCheckBox factionScope;
        private DarkUI.Controls.DarkCheckBox houseScope;
        private DarkUI.Controls.DarkCheckBox dynastyScsope;
        private DarkUI.Controls.DarkCheckBox casusScope;
        private DarkUI.Controls.DarkCheckBox storyScope;
        private DarkUI.Controls.DarkCheckBox religionScope;
        private DarkUI.Controls.DarkCheckBox warScope;
        private DarkUI.Controls.DarkCheckBox greatHolyScope;
        private DarkUI.Controls.DarkCheckBox faithScope;
        private DarkUI.Controls.DarkCheckBox tavcScope;
        private DarkUI.Controls.DarkCheckBox combatSideScope;
        private DarkUI.Controls.DarkCheckBox combatScope;
        private DarkUI.Controls.DarkCheckBox cultureGScope;
        private DarkUI.Controls.DarkCheckBox noneScope;
    }
}
