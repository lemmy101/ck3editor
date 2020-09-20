using System;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Docking;
using JominiParse;

namespace CK3ScriptEditor
{
    public partial class SmartFindOptionWindow : DarkToolWindow
    {


        public SmartFindOptionWindow()
        {
            DockArea = DarkDockArea.Right;
            InitializeComponent();
        }


        private void findInFiles_Click(object sender, EventArgs e)
        {
            DoFind();
        }

        private void DoFind()
        {
            SmartFindOptions options = new SmartFindOptions();

            options.TextToFind = searchText.Text;
            options.CaseSensitive = caseSensitive.Checked;
            options.FindWholeWorld = wholeWordOnly.Checked;
            options.SearchBase = findInBaseFiles.Checked;
            options.SearchMod = findInModFiles.Checked;

            options.SmartDoFind = true;
            options.SearchRHS = doesRHS.Checked;
            options.SearchLHS = doLHS.Checked;

            options.SearchOverridden = doesFindOverridden.Checked;
            options.ShowChildren = showChildren.Checked;
            if (scopeCharacters.Checked)
                options.ScopesToSearch.Add(ScopeType.character);
            if (scopeProvinces.Checked)
                options.ScopesToSearch.Add(ScopeType.province);
            if (scopeSchemes.Checked)
                options.ScopesToSearch.Add(ScopeType.scheme);
            if (scopeTitles.Checked)
                options.ScopesToSearch.Add(ScopeType.landed_title);

            if (activityScope.Checked)
                options.ScopesToSearch.Add(ScopeType.activity);
            if (armyScope.Checked)
                options.ScopesToSearch.Add(ScopeType.army);
            if (casusScope.Checked)
                options.ScopesToSearch.Add(ScopeType.casus_belli);
            if (combatScope.Checked)
                options.ScopesToSearch.Add(ScopeType.combat);
            if (combatSideScope.Checked)
                options.ScopesToSearch.Add(ScopeType.combat_side);
            if (councilTaskScope.Checked)
                options.ScopesToSearch.Add(ScopeType.council_task);
            if (cultureGScope.Checked)
                options.ScopesToSearch.Add(ScopeType.culture_group);
            if (cultureScope.Checked)
                options.ScopesToSearch.Add(ScopeType.culture);
            if (factionScope.Checked)
                options.ScopesToSearch.Add(ScopeType.faction);
            if (armyScope.Checked)
                options.ScopesToSearch.Add(ScopeType.army);
            if (faithScope.Checked)
                options.ScopesToSearch.Add(ScopeType.faith);
            if (greatHolyScope.Checked)
                options.ScopesToSearch.Add(ScopeType.great_holy_war);
            if (tavcScope.Checked)
                options.ScopesToSearch.Add(ScopeType.title_and_vassal_change);
            if (religionScope.Checked)
                options.ScopesToSearch.Add(ScopeType.religion);
            if (holyOrderScope.Checked)
                options.ScopesToSearch.Add(ScopeType.holy_order);
            if (storyScope.Checked)
                options.ScopesToSearch.Add(ScopeType.story_cycle);
            if (warScope.Checked)
                options.ScopesToSearch.Add(ScopeType.war);
            if (dynastyScsope.Checked)
                options.ScopesToSearch.Add(ScopeType.dynasty);
            if (secretScope.Checked)
                options.ScopesToSearch.Add(ScopeType.secret);
            if (mercScope.Checked)
                options.ScopesToSearch.Add(ScopeType.mercenary_company);
            if (houseScope.Checked)
                options.ScopesToSearch.Add(ScopeType.dynasty_house);

            if (noneScope.Checked)
            {
                options.ScopesToSearch.Add(ScopeType.none);
                options.ScopesToSearch.Add(ScopeType.any);
                options.ScopesToSearch.Add(ScopeType.inheritparent);
            }


            var results = Core.Instance.DoSmartFind(options);

            CK3ScriptEd.Instance.searchResults.SetResults(options, results);
        }

        private void uncheckAllTypes_Click(object sender, EventArgs e)
        {
            doesFindOverridden.Checked = false;
            doesRHS.Checked = false;
            doLHS.Checked = false;
       
        }

        private void checkAllTypes_Click(object sender, EventArgs e)
        {
            doesFindOverridden.Checked = true;
            doesRHS.Checked = true;
            doLHS.Checked = true;
      
        }

        private void uncheckAllScopes_Click(object sender, EventArgs e)
        {
            scopeCharacters.Checked = false;
            scopeProvinces.Checked = false;
            scopeSchemes.Checked = false;
            scopeTitles.Checked = false;

            activityScope.Checked = false;
            armyScope.Checked = false;
            casusScope.Checked = false;
            combatScope.Checked = false;
            combatSideScope.Checked = false;
            councilTaskScope.Checked = false;
            cultureGScope.Checked = false;
            cultureScope.Checked = false;
            factionScope.Checked = false;
            armyScope.Checked = false;
            faithScope.Checked = false;
            greatHolyScope.Checked = false;
            tavcScope.Checked = false;
            religionScope.Checked = false;
            holyOrderScope.Checked = false;
            storyScope.Checked = false;
            warScope.Checked = false;
            dynastyScsope.Checked = false;
            secretScope.Checked = false;
            mercScope.Checked = false;
            houseScope.Checked = false;

            noneScope.Checked = false;
        }

        private void checkAllScopes_Click(object sender, EventArgs e)
        {
            scopeCharacters.Checked = true;
            scopeProvinces.Checked = true;
            scopeSchemes.Checked = true;
            scopeTitles.Checked = true;


            activityScope.Checked = true;
            armyScope.Checked = true;
            casusScope.Checked = true;
            combatScope.Checked = true;
            combatSideScope.Checked = true;
            councilTaskScope.Checked = true;
            cultureGScope.Checked = true;
            cultureScope.Checked = true;
            factionScope.Checked = true;
            armyScope.Checked = true;
            faithScope.Checked = true;
            greatHolyScope.Checked = true;
            tavcScope.Checked = true;
            religionScope.Checked = true;
            holyOrderScope.Checked = true;
            storyScope.Checked = true;
            warScope.Checked = true;
            dynastyScsope.Checked = true;
            secretScope.Checked = true;
            mercScope.Checked = true;
            houseScope.Checked = true;
            noneScope.Checked = true;
        }

        private void SmartFindOptionWindow_Enter(object sender, EventArgs e)
        {
            searchText.Focus();
        }

        private void searchText_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void searchText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                DoFind();
            }
        }

        private void showChildren_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
