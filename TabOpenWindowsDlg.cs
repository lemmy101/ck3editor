using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Controls;
using DarkUI.Forms;

namespace CK3ScriptEditor
{
    public partial class TabOpenWindowsDlg : DarkForm
    {
        public TabOpenWindowsDlg()
        {
            InitializeComponent();

            Fill();
        }

        public void Fill()
        {
            this.openFileList.Items.Clear();
            foreach (var instanceOpenScriptWindow in CK3ScriptEd.Instance.OpenScriptWindows)
            {

                var s = instanceOpenScriptWindow.Filename;

                string ss = s.Substring(s.LastIndexOf("/") + 1);

                int col = 50;

                while (ss.Length < col)
                    ss += " ";

                if (instanceOpenScriptWindow.ScriptFile.IsBase)
                {
                    ss += "Base: ";
                }
                else
                {
                    ss += "Mod:  ";
                }

                ss += s.Substring(0, s.LastIndexOf("/"));

                var i = new DarkListItem(ss);
                i.Tag = instanceOpenScriptWindow;
                openFileList.Items.Add(i);
            }
            this.openFileList.SelectItem(1);
        }

        private void openFileList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey)
            {
                var scriptWindow = openFileList.Items[openFileList.SelectedIndices[0]].Tag as ScriptWindow;

                CK3ScriptEd.Instance.ActivateWindow(scriptWindow);
                Close();
            }
        }

        private void openFileList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                if (e.Shift)
                {
                    int sel = this.openFileList.SelectedIndices[0] - 1;
                    if (sel < 0)
                        sel = openFileList.Items.Count-1;
                    this.openFileList.SelectItem(sel);
                    openFileList.EnsureVisible();

                }
                else
                {
                    int sel = this.openFileList.SelectedIndices[0] + 1;
                    if (sel >= openFileList.Items.Count)
                        sel = 0;
                    this.openFileList.SelectItem(sel);
                    openFileList.EnsureVisible();

                }

            }
        }

        private void openFileList_DoubleClick(object sender, EventArgs e)
        {
            var scriptWindow = openFileList.Items[openFileList.SelectedIndices[0]].Tag as ScriptWindow;

            CK3ScriptEd.Instance.ActivateWindow(scriptWindow);
            Close();
        }
    }
}
