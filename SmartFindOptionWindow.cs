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

        private void contextFind_CheckedChanged(object sender, EventArgs e)
        {
            rawTextFind.Checked = !contextFind.Checked;
        }

        private void rawTextFind_CheckedChanged(object sender, EventArgs e)
        {
            contextFind.Checked = !rawTextFind.Checked;
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

            options.SmartDoFind = contextFind.Checked;
            options.SearchValues = doesFindValues.Checked;

            options.SearchOverridden = doesFindOverridden.Checked;
            options.SearchConditionFunctions = doesConditionFuncs.Checked;
            options.SearchValues = doesFindValues.Checked;
            options.SearchEffectFunctions = doesFindEffectFuncs.Checked;
            options.SearchFunctionParameters = doesFindFuncParams.Checked;
            options.SearchSavedScopes = doesFindScopes.Checked;
            options.ShowChildren = showChildren.Checked;
            if (scopeCharacters.Checked)
                options.ScopesToSearch.Add(ScopeType.character);
            if (scopeProvinces.Checked)
                options.ScopesToSearch.Add(ScopeType.province);
            if (scopeSchemes.Checked)
                options.ScopesToSearch.Add(ScopeType.scheme);
            if (scopeTitles.Checked)
                options.ScopesToSearch.Add(ScopeType.title);

            var results = Core.Instance.DoSmartFind(options);

            CK3ScriptEd.Instance.searchResults.SetResults(options, results);
        }

        private void uncheckAllTypes_Click(object sender, EventArgs e)
        {
            doesConditionFuncs.Checked = false;
            doesFindOverridden.Checked = false;
            doesFindValues.Checked = false;
            doesFindEffectFuncs.Checked = false;
            doesFindFuncParams.Checked = false;
            doesFindScopes.Checked = false;

        }

        private void checkAllTypes_Click(object sender, EventArgs e)
        {
            doesConditionFuncs.Checked = true;
            doesFindOverridden.Checked = true;
            doesFindValues.Checked = true;
            doesFindEffectFuncs.Checked = true;
            doesFindFuncParams.Checked = true;
            doesFindScopes.Checked = true;

        }

        private void uncheckAllScopes_Click(object sender, EventArgs e)
        {
            scopeCharacters.Checked = false;
            scopeProvinces.Checked = false;
            scopeSchemes.Checked = false;
            scopeTitles.Checked = false;
        }

        private void checkAllScopes_Click(object sender, EventArgs e)
        {
            scopeCharacters.Checked = true;
            scopeProvinces.Checked = true;
            scopeSchemes.Checked = true;
            scopeTitles.Checked = true;

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
