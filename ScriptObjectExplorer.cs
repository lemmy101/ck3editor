using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Config;
using DarkUI.Controls;
using DarkUI.Docking;
using JominiParse;

namespace CK3ScriptEditor
{
    public partial class ScriptObjectExplorer : DarkToolWindow
    {
        public ScriptObjectExplorer()
        {
            DockArea = DarkDockArea.Left;
            InitializeComponent();
        }
        public string AddToPath { get; set; }

        string CurrentSelectedPath
        {
            get
            {

                if (tree.SelectedNodes.Count > 0)
                {
                    var sel = tree.SelectedNodes[0];

                    string path = "";

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
            string[] p = path.Split('>');

            var nodes = tree.Nodes;

            foreach (var s in p)
            {
                foreach (var node in nodes)
                {
                    if (node.Text == s)
                    {
                        node.Expanded = true;
                        tree.SelectNode(node);
                        nodes = node.Nodes;
                        break;
                    }
                }
            }
        }

        public void UpdateScriptExplorer()
        {
            if (Core.Instance.ModCK3Library == null)
                return;

            string path = CurrentSelectedPath;
            namespaces.Clear();
            tree.Nodes.Clear();

            tree.SuspendLayout();
            tree.SuspendNodeEvents();

            List<KeyValuePair<ScriptContext, ScriptLibrary.ContextInfo>> c = Core.Instance.BaseCK3Library.ContextData.ToList().OrderBy(a => a.Key.ToString()).ToList();

            Dictionary<string, DarkTreeNode> parents = new Dictionary<string, DarkTreeNode>();
            foreach (var keyValuePair in c)
            {
                DarkTreeNode activities = new DarkTreeNode(keyValuePair.Key.ToString());

                var parent = tree.Nodes;

                if (keyValuePair.Value.Category != null)
                {
                    if (parents.ContainsKey(keyValuePair.Value.Category))
                        parent = parents[keyValuePair.Value.Category].Nodes;
                    else
                    {
                        DarkTreeNode category = new DarkTreeNode(keyValuePair.Value.Category.ToString());
                        parents[keyValuePair.Value.Category] = category;
                        
                        tree.Nodes.Add(category);
                        parent = parents[keyValuePair.Value.Category].Nodes;
                    }
                }
               

                {
                    activities.Tag = keyValuePair.Key;
                }
                parent.Add(activities);

                var ordered = (parent).ToList().OrderBy(a => a.Text);

                parent.Clear();

                parent.AddRange(ordered);
                
                FillBranch(activities, keyValuePair.Key);
            }

            {

                var ordered = (tree.Nodes).ToList().OrderBy(a => a.Text);

                tree.Nodes.Clear();

                tree.Nodes.AddRange(ordered);

            }

            tree.ResumeNodeEvents();
            tree.ResumeLayout(true);
            SelectPath(path);
        }

        class NamespaceItems
        {
            public List<ScriptObject> objList = new List<ScriptObject>();
        }

        Dictionary<string, NamespaceItems> namespaces = new Dictionary<string,NamespaceItems>();

        private void FillBranch(DarkTreeNode parent, ScriptContext context)
        {
            namespaces.Clear();
            var names = Core.Instance.GetNameSet(context, this.showModOnly.CheckState != CheckState.Checked).OrderBy(a => a).ToList();

            NamespaceItems defaultNamespace = new NamespaceItems();

            foreach (var name in names)
            {
                ScriptObject e = Core.Instance.Get(context, name);
              
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
                DarkTreeNode d = new DarkTreeNode(n);
                parent.Nodes.Add(d);
                foreach (var scriptObject in namespaces[n].objList)
                {
                    DarkTreeNode dob = new DarkTreeNode(scriptObject.Name);

                    dob.Tag = scriptObject;
                    if (!scriptObject.ScriptFile.IsBase)
                    {
                        dob.TextColor = Color.LightBlue;
                        parent.TextColor = Color.LightBlue;

                        if (parent.ParentNode != null)
                        {
                            parent.ParentNode.TextColor = Color.LightBlue;

                        }
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
                                {
                                    if (parent.ParentNode.TextColor == Colors.LightText)
                                        parent.ParentNode.TextColor = Color.LightCoral;

                                }
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
                DarkTreeNode dob = new DarkTreeNode(scriptObject.Name);

                dob.Tag = scriptObject;

                if (!scriptObject.ScriptFile.IsBase)
                {
                    dob.TextColor = Color.LightBlue;
                    parent.TextColor = Color.LightBlue;

                    if (parent.ParentNode != null)
                    {
                        parent.ParentNode.TextColor = Color.LightBlue;

                    }
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
                            {
                                if (parent.ParentNode.TextColor == Colors.LightText)
                                    parent.ParentNode.TextColor = Color.LightCoral;

                            }
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
                if ((tag as ScriptObject) == null)
                    return;

                CK3ScriptEd.Instance.Goto(tag.Filename, tag.LineStart - 1, (tag as ScriptObject).ScriptFile.IsBase);

            }
        }
        
        private void showOveridden_CheckedChanged(object sender, EventArgs e)
        {
            UpdateScriptExplorer();

        }
        ContextMenu menu = new ContextMenu();
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
                        ScriptContext c = (ScriptContext)selectedO;

                        MenuItem newObject = new MenuItem("&New " + c.ToString());
                        newObject.Click += NewObject_Click;
                        newObject.Tag = selectedO;
                        menu.MenuItems.Add(newObject);

                    }

                    if (menu.MenuItems.Count > 0)
                    {
                        menu.Show(tree, new Point(e.X, e.Y));

                    }
                }

            }
        }

        private void NewObject_Click(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;

            var context = (ScriptContext)menuItem.Tag;

            NewScriptObjectDialog dlg = new NewScriptObjectDialog();

            dlg.Text = "Create new " + context + " script object.";
            dlg.Init(context);
            if (dlg.ShowDialog(CK3ScriptEd.Instance) == DialogResult.OK)
            {

                RefFilename dir = Core.Instance.GetBaseDirectoryFromContext(dlg.Context);

                RefFilename fullPath = dir.Append(dlg.ChosenFilename);
                fullPath.IsBase = false;

                bool exists = fullPath.Exists;
                string textToImplant = dlg.soName.Text + " = { \n\n\n}";
                
                if (exists)
                {
                    // need to insert it into the file....
                    string text = System.IO.File.ReadAllText(fullPath.ToFullWindowsFilename());
                    text = text.Replace("\r", "");
                    var lines = text.Split(new char[] { '\n' }).ToList();

                    var lines2 = textToImplant.Split(new char[] { '\n' }).ToList();

                    lines.AddRange(lines2);



                    using (FileStream fs = new FileStream(fullPath.ToFullWindowsFilename(), FileMode.Create))
                    {
                        // create a new file....
                        using (StreamWriter outputFile = new StreamWriter(fs, Encoding.UTF8))
                        {

                            foreach (string line in lines)
                                outputFile.WriteLine(line);
                        }
                    }
                }
                else
                {
                    using (FileStream fs = new FileStream(fullPath.ToFullWindowsFilename(), FileMode.Create))
                    {
                        // create a new file....
                        using (StreamWriter outputFile = new StreamWriter(fs, Encoding.UTF8))
                        {
                            var lines = textToImplant.Split(new char[] { '\n' }).ToList();
                          
                            foreach (string line in lines)
                                outputFile.WriteLine(line);
                        }
                    }
                }



                Core.Instance.ModCK3Library.EnsureFile(fullPath, context);
                Core.Instance.LoadCK3File(fullPath, false, true);
                CK3ScriptEd.Instance.projectExplorer.FillProjectView();
                CK3ScriptEd.Instance.soExplorer.UpdateScriptExplorer();
                CK3ScriptEd.Instance.CloseDocument(true, fullPath);
                int newLine = Core.Instance.ModCK3Library.GetFile(fullPath).Map[dlg.soName.Text].LineStart - 1;
                CK3ScriptEd.Instance.Goto(fullPath, newLine, false); CK3ScriptEd.Instance.Goto(fullPath, newLine, false);


                CK3ScriptEd.Instance.UpdateAllWindows();
            }
        }
    }
}
