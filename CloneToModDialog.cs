﻿#region

using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DarkUI.Controls;
using DarkUI.Forms;
using JominiParse;

#endregion

namespace CK3ScriptEditor
{
    public partial class CloneToModDialog : DarkForm
    {
        internal string ChosenFilename;

        public CloneToModDialog()
        {
            InitializeComponent();
        }

        public ScriptObject ScriptObject { get; set; }

        public ScriptContext Context { get; set; }

        public void Init(RefFilename fromFilename, int lineStart, int lineEnd, ScriptObject scriptObject)
        {
            OK.Enabled = false;
            var context = scriptObject.Context;
            Context = context;
            if (scriptObject.Namespace != null)
                cloneToExistingTag.Text += " with namespace: \"" + scriptObject.Namespace + "\"";

            ScriptObject = scriptObject;
            var dirFromContext = Core.Instance.GetBaseDirectoryFromContext(context).ToRelativeFilename();

            var root = new DarkTreeNode(context.ToString());

            var str = Core.Instance.ModCK3Library.GetDirectoryListFromContext(context, scriptObject.Namespace);
            for (var index = 0; index < str.Count; index++)
            {
                var item = root;
                var file = str[index];

                file = file.RemoveFromPath(dirFromContext);

                var parts = file.SplitDirectories();

                foreach (var part in parts)
                {
                    var next = new DarkTreeNode(part);
                    var f = item.Nodes.Where(a => a.Text == part);
                    if (f.Any())
                        next = f.First();
                    else
                        item.Nodes.Add(next);
                    item = next;
                    item.Expanded = true;
                    if (part.Contains(".txt"))
                        item.Tag = file;
                }
            }

            if (root.Nodes.Count > 0)
                existingFileView.Nodes.Add(root);

            root.Expanded = true;
        }

        private void newFileRadio_CheckedChanged(object sender, EventArgs e)
        {
            cloneToExisting.Checked = !newFileRadio.Checked;
        }

        private void cloneToExisting_CheckedChanged(object sender, EventArgs e)
        {
            newFileRadio.Checked = !cloneToExisting.Checked;
        }

        private void filenameButton_Click(object sender, EventArgs e)
        {
            var saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.InitialDirectory =
                Core.Instance.GetModDirectoryFromContext(Context).ToFullWindowsFilename();


            var dir = Core.Instance.GetModDirectoryFromContext(Context).ToFullWindowsFilename();

            dir = dir.ToLower();

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            var done = false;

            while (!done)
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (!saveFileDialog1.FileName.Replace("\\", "/").Contains(
                        Core.Instance.GetModDirectoryFromContext(Context).ToRelativeFilename()))
                    {
                        MessageBox.Show("Error: File must be in " +
                                        Core.Instance.GetModDirectoryFromContext(Context).ToRelativeFilename() +
                                        " directory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        done = true;
                        var f = saveFileDialog1.FileName.Replace("\\", "/");
                        var r = Core.Instance.GetModDirectoryFromContext(Context).ToFullFilename();

                        chosenFilename.Text = saveFileDialog1.FileName = f.Replace(r, "");
                        newFileRadio.Checked = true;
                        ChosenFilename = chosenFilename.Text;
                        OK.Enabled = true;
                    }
                }
                else
                {
                    done = true;
                }
        }

        private void OK_Click(object sender, EventArgs e)
        {
            // do the clone!!!
        }

        internal string GetTextToImplant(RefFilename fullPath)
        {
            var text = File.ReadAllText(fullPath.ToFullWindowsFilename());

            var lines = text.Split('\n').ToList();

            var from = ScriptObject.LineStart - 1;
            var to = ScriptObject.LineEnd - 1;

            var toImplant = lines.GetRange(from, to - from + 1);

            return string.Join("\n", toImplant);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
        }

        private void existingFileView_SelectedNodesChanged(object sender, EventArgs e)
        {
            if (existingFileView.SelectedNodes.Count > 0 && existingFileView.SelectedNodes[0].Tag != null)
            {
                ChosenFilename = (existingFileView.SelectedNodes[0].Tag as RefFilename).ToRelativeFilename();
                cloneToExisting.Checked = true;
                OK.Enabled = true;
            }
        }
    }
}