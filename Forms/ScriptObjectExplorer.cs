#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DarkUI.Config;
using DarkUI.Controls;
using DarkUI.Docking;
using JominiParse;

#endregion

namespace CK3ScriptEditor
{
    public partial class ScriptObjectExplorer : DarkToolWindow
    {
        private readonly ContextMenu menu = new ContextMenu();

        private readonly Dictionary<string, NamespaceItems> namespaces = new Dictionary<string, NamespaceItems>();

        public ScriptObjectExplorer()
        {
            DockArea = DarkDockArea.Left;
            AutoScaleMode = AutoScaleMode.Inherit;
            InitializeComponent();
        }

        public string AddToPath { get; set; }

        private string CurrentSelectedPath
        {
            get
            {
                if (tree.SelectedNodes.Count > 0)
                {
                    var sel = tree.SelectedNodes[0];

                    var path = "";

                    while (sel != null)
                    {
                        path = sel.Text + ">" + path;
                        sel = sel.ParentNode;
                    }

                    if (AddToPath != null)
                        return path + ">" + AddToPath;

                    return path;
                }

                return "";
            }
        }

        private void SelectPath(string path)
        {
            var p = path.Split('>');

            var nodes = tree.Nodes;

            foreach (var s in p)
            foreach (var node in nodes)
                if (node.Text == s)
                {
                    node.Expanded = true;
                    tree.SelectNode(node);
                    nodes = node.Nodes;
                    break;
                }
        }

        public void UpdateScriptExplorer()
        {
            if (Core.Instance.ModCK3Library == null)
                return;

            var path = CurrentSelectedPath;
            namespaces.Clear();
            tree.Nodes.Clear();

            switch (EditorGlobals.FontSize)
            {
                case FontSize.Normal:
                    tree.Font = this.findTextBox.Font = darkLabel1.Font =  new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    break;
                case FontSize.Large:
                    tree.Font = this.findTextBox.Font = darkLabel1.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    break;
            }

            tree.SuspendLayout();
            tree.SuspendNodeEvents();

            var c = Core.Instance.BaseCK3Library.ContextData.ToList().OrderBy(a => a.Key.ToString()).ToList();

            var parents = new Dictionary<string, DarkTreeNode>();
            foreach (var keyValuePair in c)
            {
                var activities = new DarkTreeNode(keyValuePair.Key.ToString());

                var parent = tree.Nodes;

                if (keyValuePair.Value.Category != null)
                {
                    if (parents.ContainsKey(keyValuePair.Value.Category))
                    {
                        parent = parents[keyValuePair.Value.Category].Nodes;
                    }
                    else
                    {
                        var category = new DarkTreeNode(keyValuePair.Value.Category);
                        parents[keyValuePair.Value.Category] = category;

                        tree.Nodes.Add(category);
                        parent = parents[keyValuePair.Value.Category].Nodes;
                    }
                }

                activities.Tag = keyValuePair.Key;

                parent.Add(activities);

                var ordered = parent.ToList().OrderBy(a => a.Text);

                parent.Clear();

                parent.AddRange(ordered);

                FillBranch(activities, keyValuePair.Key);

                if (!showEmpty.Checked || findTextBox.Text.Trim().Length != 0) 
                    RemoveBlank(activities);
            }

            var orderedRoot = tree.Nodes.ToList().OrderBy(a => a.Text);

            tree.Nodes.Clear();

            tree.Nodes.AddRange(orderedRoot);

            RemoveBlankFinalCleanup();

            tree.ResumeNodeEvents();
            tree.ResumeLayout(true);
            SelectPath(path);
        }

        private void RemoveBlankFinalCleanup()
        {
            for (var index = 0; index < tree.Nodes.Count; index++)
            {
                var darkTreeNode = tree.Nodes[index];

                for (var i = 0; i < darkTreeNode.Nodes.Count; i++)
                {
                    var treeNode = darkTreeNode.Nodes[i];

                    if (!(treeNode.Tag is ScriptObject))
                        if (treeNode.Nodes.Count == 0)
                        {
                            darkTreeNode.Nodes.Remove(treeNode);
                            i--;
                        }
                }

                if (darkTreeNode.Nodes.Count == 0)
                {
                    tree.Nodes.Remove(darkTreeNode);
                    index--;
                }
            }
        }

        private void RemoveBlank(DarkTreeNode activities)
        {
            var darkTreeNode = activities;

            for (var i = 0; i < darkTreeNode.Nodes.Count; i++)
            {
                var treeNode = darkTreeNode.Nodes[i];

                for (var j = 0; j < treeNode.Nodes.Count; j++)
                {
                    var treeNode2 = treeNode.Nodes[j];
                    if (!(treeNode2.Tag is ScriptObject))
                        if (treeNode2.Nodes.Count == 0)
                        {
                            treeNode.Nodes.Remove(treeNode2);
                            j--;
                        }
                }

                if (!(treeNode.Tag is ScriptObject))
                    if (treeNode.Nodes.Count == 0)
                    {
                        darkTreeNode.Nodes.Remove(treeNode);
                        i--;
                    }
            }

            if (darkTreeNode.Nodes.Count == 0) tree.Nodes.Remove(darkTreeNode);
        }

        private void FillBranch(DarkTreeNode parent, ScriptContext context)
        {
            namespaces.Clear();
            var names = Core.Instance.GetNameSet(context, showModOnly.CheckState != CheckState.Checked).OrderBy(a => a)
                .ToList();

            var defaultNamespace = new NamespaceItems();

            foreach (var name in names)
            {
                var e = Core.Instance.Get(context, name);

                if (e != null)
                {
                    var namesp = defaultNamespace;
                    if (!string.IsNullOrEmpty(e.Namespace))
                    {
                        if (namespaces.ContainsKey(e.Namespace))
                        {
                            namesp = namespaces[e.Namespace];
                        }
                        else
                        {
                            namesp = new NamespaceItems();
                            namespaces[e.Namespace] = namesp;
                        }
                    }

                    namesp.objList.Add(e);
                }
            }


            var namelist = namespaces.Keys.ToList();

            foreach (var n in namelist)
            {
                var d = new DarkTreeNode(n);
                parent.Nodes.Add(d);
                foreach (var scriptObject in namespaces[n].objList)
                {
                    if (findTextBox.Text.Trim().Length != 0)
                        if (!scriptObject.Name.ToLower().Contains(findTextBox.Text.Trim().ToLower()))
                            continue;
                    var dob = new DarkTreeNode(scriptObject.Name);

                    dob.Tag = scriptObject;
                    if (!scriptObject.ScriptFile.IsBase)
                    {
                        dob.TextColor = Color.LightBlue;
                        parent.TextColor = Color.LightBlue;

                        if (parent.ParentNode != null) parent.ParentNode.TextColor = Color.LightBlue;
                        d.TextColor = Color.LightBlue;
                    }
                    else
                    {
                        if (scriptObject.Overridden)
                        {
                            dob.TextColor = Color.LightCoral;
                            if (parent.TextColor == Colors.LightText)
                            {
                                parent.TextColor = Color.LightCoral;

                                if (parent.ParentNode != null)
                                    if (parent.ParentNode.TextColor == Colors.LightText)
                                        parent.ParentNode.TextColor = Color.LightCoral;
                            }


                            if (d.TextColor == Colors.LightText)
                                d.TextColor = Color.LightCoral;
                        }
                    }

                    if (!scriptObject.Overridden || showOveridden.Checked)
                        d.Nodes.Add(dob);
                }
            }


            foreach (var scriptObject in defaultNamespace.objList)
            {
                if (findTextBox.Text.Trim().Length != 0)
                    if (!scriptObject.Name.ToLower().Contains(findTextBox.Text.Trim().ToLower()))
                        continue;
                var dob = new DarkTreeNode(scriptObject.Name);

                dob.Tag = scriptObject;

                if (!scriptObject.ScriptFile.IsBase)
                {
                    dob.TextColor = Color.LightBlue;
                    parent.TextColor = Color.LightBlue;

                    if (parent.ParentNode != null) parent.ParentNode.TextColor = Color.LightBlue;
                }
                else
                {
                    if (scriptObject.Overridden)
                    {
                        dob.TextColor = Color.LightCoral;
                        if (parent.TextColor == Colors.LightText)
                        {
                            parent.TextColor = Color.LightCoral;

                            if (parent.ParentNode != null)
                                if (parent.ParentNode.TextColor == Colors.LightText)
                                    parent.ParentNode.TextColor = Color.LightCoral;
                        }
                    }
                }

                if (!scriptObject.Overridden || showOveridden.Checked)
                    parent.Nodes.Add(dob);
            }
        }

        private void showModOnly_CheckedChanged(object sender, EventArgs e)
        {
            UpdateScriptExplorer();
        }

        private void tree_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (tree.SelectedNodes.Count > 0)
            {
                var sel = tree.SelectedNodes[0];

                var tag = sel.Tag as ScriptObject;
                if (tag == null)
                    return;

                CK3ScriptEd.Instance.Goto(tag.Filename, tag.LineStart - 1, tag.ScriptFile.IsBase);
            }
        }

        private void showOveridden_CheckedChanged(object sender, EventArgs e)
        {
            UpdateScriptExplorer();
        }

        private void tree_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void tree_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                tree.Invalidate();
                menu.MenuItems.Clear();

                if (tree.SelectedNodes.Count > 0 && tree.SelectedNodes[0].Tag != null)
                {
                    var selectedO = tree.SelectedNodes[0].Tag;

                    if (selectedO is ScriptContext)
                    {
                        var c = (ScriptContext) selectedO;

                        var newObject = new MenuItem("&New " + c);
                        newObject.Click += NewObject_Click;
                        newObject.Tag = selectedO;
                        menu.MenuItems.Add(newObject);
                    }
                    else if (selectedO is ScriptObject)
                    {
                        var so = selectedO as ScriptObject;

                        if (so.Topmost.Filename.IsBase)
                        {
                            var newObject = new MenuItem("&Clone " + (selectedO as ScriptObject).Name + " to " +
                                                         Core.Instance.ModCK3Library.Name);
                            newObject.Click += CloneObject_OnClick;
                            newObject.Tag = selectedO;
                            menu.MenuItems.Add(newObject);
                        }
                    }

                    if (menu.MenuItems.Count > 0) menu.Show(tree, new Point(e.X, e.Y));
                }
            }
        }

        private void CloneObject_OnClick(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;

            var toClone = menuItem.Tag as ScriptObject;

            if (toClone != null)
                ScriptCloningManager.Instance.Clone(toClone);
        }

        private void NewObject_Click(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;

            var context = (ScriptContext) menuItem.Tag;

            var dlg = new NewScriptObjectDialog();

            dlg.Text = "Create new " + context + " script object.";
            dlg.Init(context);
            if (dlg.ShowDialog(CK3ScriptEd.Instance) == DialogResult.OK)
            {
                var dir = Core.Instance.GetBaseDirectoryFromContext(dlg.Context);

                var fullPath = dir.Append(dlg.ChosenFilename);
                fullPath.IsBase = false;

                var exists = fullPath.Exists;
                var textToImplant = dlg.soName.Text + " = { \n\n\n}";

                if (exists)
                {
                    // need to insert it into the file....
                    var text = File.ReadAllText(fullPath.ToFullWindowsFilename());
                    text = text.Replace("\r", "");
                    var lines = text.Split('\n').ToList();

                    var lines2 = textToImplant.Split('\n').ToList();

                    lines.AddRange(lines2);


                    using (var fs = new FileStream(fullPath.ToFullWindowsFilename(), FileMode.Create))
                    {
                        // create a new file....
                        using (var outputFile = new StreamWriter(fs, Encoding.UTF8))
                        {
                            foreach (var line in lines)
                                outputFile.WriteLine(line);
                        }
                    }
                }
                else
                {
                    using (var fs = new FileStream(fullPath.ToFullWindowsFilename(), FileMode.Create))
                    {
                        // create a new file....
                        using (var outputFile = new StreamWriter(fs, Encoding.UTF8))
                        {
                            var lines = textToImplant.Split('\n').ToList();

                            foreach (var line in lines)
                                outputFile.WriteLine(line);
                        }
                    }
                }


                Core.Instance.ModCK3Library.EnsureFile(fullPath, context);
                Core.Instance.LoadCK3File(fullPath, false, true);
                CK3ScriptEd.Instance.projectExplorer.FillProjectView();
                CK3ScriptEd.Instance.soExplorer.UpdateScriptExplorer();
                CK3ScriptEd.Instance.CloseDocument(true, fullPath);
                var newLine = Core.Instance.ModCK3Library.GetFile(fullPath).Map[dlg.soName.Text].LineStart - 1;
                CK3ScriptEd.Instance.Goto(fullPath, newLine, false);
                CK3ScriptEd.Instance.Goto(fullPath, newLine, false);


                CK3ScriptEd.Instance.UpdateAllWindows();
            }
        }

        private void showEmpty_CheckedChanged(object sender, EventArgs e)
        {
            UpdateScriptExplorer();
        }

        private void findTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateScriptExplorer();
        }

        private class NamespaceItems
        {
            public readonly List<ScriptObject> objList = new List<ScriptObject>();
        }
    }
}