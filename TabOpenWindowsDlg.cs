#region

using System;
using System.Windows.Forms;
using DarkUI.Controls;
using DarkUI.Forms;

#endregion

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
            openFileList.Items.Clear();
            foreach (var instanceOpenScriptWindow in CK3ScriptEd.Instance.OpenScriptWindows)
            {
                var s = instanceOpenScriptWindow.Filename;

                var ss = s.ToRelativeFilename().Substring(s.ToRelativeFilename().LastIndexOf("/") + 1);

                var col = 50;

                while (ss.Length < col)
                    ss += " ";

                if (instanceOpenScriptWindow.ScriptFile.IsBase)
                    ss += "Base: ";
                else
                    ss += "Mod:  ";

                ss += s.ToRelativeFilename().Substring(0, s.ToRelativeFilename().LastIndexOf("/"));

                var i = new DarkListItem(ss);
                i.Tag = instanceOpenScriptWindow;
                openFileList.Items.Add(i);
            }

            openFileList.SelectItem(1);
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
                    var sel = openFileList.SelectedIndices[0] - 1;
                    if (sel < 0)
                        sel = openFileList.Items.Count - 1;
                    openFileList.SelectItem(sel);
                    openFileList.EnsureVisible();
                }
                else
                {
                    var sel = openFileList.SelectedIndices[0] + 1;
                    if (sel >= openFileList.Items.Count)
                        sel = 0;
                    openFileList.SelectItem(sel);
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