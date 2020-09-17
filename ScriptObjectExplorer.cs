using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
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

        public void UpdateScriptExplorer()
        {
            namespaces.Clear();
            tree.Nodes.Clear();

            tree.SuspendLayout();
            tree.SuspendNodeEvents();

            var c = Core.Instance.BaseCK3Library.ContextData.ToList().OrderBy(a => a.Key.ToString());
            foreach (var keyValuePair in c)
            {
                DarkTreeNode activities = new DarkTreeNode(keyValuePair.Key.ToString());
                tree.Nodes.Add(activities);
                FillBranch(activities, keyValuePair.Key);
            }
         
            tree.ResumeNodeEvents();
            tree.ResumeLayout(true);
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
                    if (e.Name == "education_intrigue_1")
                    {

                    }
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
        private void FillBranchFiles(DarkTreeNode parent, ScriptContext context)
        {
            namespaces.Clear();
            var names = Core.Instance.GetNameSet(context, this.showModOnly.CheckState != CheckState.Checked).OrderBy(a => a).ToList();

            NamespaceItems defaultNamespace = new NamespaceItems();

            foreach (var name in names)
            {
                ScriptObject e = Core.Instance.Get( context, name);
                string ff = e.Filename.Substring(e.Filename.LastIndexOf("/")+1).Replace(".txt", "");

                if (e != null)
                {
                    var namesp = defaultNamespace;
                    if (!string.IsNullOrEmpty(ff))
                    {
                        if (namespaces.ContainsKey(ff))
                        {
                            namesp = namespaces[ff];
                        }
                        else
                        {
                            namesp = new NamespaceItems();
                            namespaces[ff] = namesp;
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
    }
}
