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

            {

                DarkTreeNode activities = new DarkTreeNode("Activities");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.Activities);

            }
            {
                DarkTreeNode bookmarks = new DarkTreeNode("Buildings");
                tree.Nodes.Add(bookmarks);
                FillBranch(bookmarks, ScriptContext.Buildings);

            }
            {
                DarkTreeNode bookmarks = new DarkTreeNode("Bookmarks");
                tree.Nodes.Add(bookmarks);
                FillBranch(bookmarks, ScriptContext.Bookmark);

            }
            {
                DarkTreeNode bookmarks = new DarkTreeNode("Casus Belli Types");
                tree.Nodes.Add(bookmarks);
                FillBranch(bookmarks, ScriptContext.CasusBelliType);

            }

            {

                DarkTreeNode activities = new DarkTreeNode("Characters");
                tree.Nodes.Add(activities);

                FillBranchFiles(activities, ScriptContext.Characters);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("Character Interactions");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.CharacterInteractions);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("CouncilPositions");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.CouncilPositions);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("CouncilTasks");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.CouncilTasks);

            }

            {
                DarkTreeNode decisions = new DarkTreeNode("Decisions");
                tree.Nodes.Add(decisions);
                FillBranch(decisions, ScriptContext.Decisions);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("Defines");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.Defines);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("DynastyLegacies");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.DynastyLegacies);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("DynastyPerks");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.DynastyPerks);

            }
            {
                DarkTreeNode events = new DarkTreeNode("Events");
                tree.Nodes.Add(events);
                FillBranch(events, ScriptContext.Events);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("EventBackgrounds");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.EventBackgrounds);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("EventThemes");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.EventThemes);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("Factions");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.Factions);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("Focuses");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.Focuses);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("GameRules");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.GameRules);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("Governments");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.Governments);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("Holdings");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.Holdings);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("HookTypes");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.HookTypes);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("ImportantActions");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.ImportantActions);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("LandedTitles");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.LandedTitles);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("Laws");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.Laws);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("Lifestyles");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.Lifestyles);

            }

            {

                DarkTreeNode activities = new DarkTreeNode("LifestylePerks");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.LifestylePerks);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("ScriptedModifiers");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.ScriptedModifiers);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("StaticModifiers");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.StaticModifiers);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("Nicknames");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.Nicknames);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("OnActions");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.OnActions);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("OptionModifiers");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.OptionModifiers);

            }

            {
                DarkTreeNode r = new DarkTreeNode("Religion");
                tree.Nodes.Add(r);
             
                {

                    DarkTreeNode activities = new DarkTreeNode("Doctrines");
                    r.Nodes.Add(activities);
                    FillBranch(activities, ScriptContext.Doctrines);

                }
                {

                    DarkTreeNode activities = new DarkTreeNode("FervorModifiers");
                    r.Nodes.Add(activities);
                    FillBranch(activities, ScriptContext.FervorModifiers);

                }
                {

                    DarkTreeNode activities = new DarkTreeNode("HolySites");
                    tree.Nodes.Add(activities);
                    FillBranch(activities, ScriptContext.HolySites);

                }
                {

                    DarkTreeNode activities = new DarkTreeNode("ReligionFamilies");
                    tree.Nodes.Add(activities);
                    FillBranch(activities, ScriptContext.ReligionFamilys);

                }
                {

                    DarkTreeNode activities = new DarkTreeNode("Religions");
                    tree.Nodes.Add(activities);
                    FillBranch(activities, ScriptContext.Religions);

                }
            }
            {

                DarkTreeNode activities = new DarkTreeNode("Schemes");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.Schemes);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("ScriptedCharacterTemplates");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.ScriptedCharacterTemplates);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("ScriptedEffects");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.ScriptedEffects);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("ScriptedLists");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.ScriptedLists);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("ScriptedRelations");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.ScriptedRelations);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("ScriptedRules");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.ScriptedRules);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("ScriptedTriggers");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.ScriptedTriggers);

            }
            {
                DarkTreeNode scriptValues = new DarkTreeNode("ScriptedValues");
                tree.Nodes.Add(scriptValues);
                FillBranch(scriptValues, ScriptContext.ScriptValues);
            }
            {

                DarkTreeNode activities = new DarkTreeNode("SecretTypes");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.SecretTypes);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("StoryCycles");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.StoryCycles);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("SuccessionElections");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.SuccessionElections);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("Traits");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.Traits);

            }
            {

                DarkTreeNode activities = new DarkTreeNode("VassalContracts");
                tree.Nodes.Add(activities);
                FillBranch(activities, ScriptContext.VassalContracts);

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
            var names = Core.Instance.GetNameSet(this.showModOnly.CheckState != CheckState.Checked, context).OrderBy(a => a).ToList();

            NamespaceItems defaultNamespace = new NamespaceItems();

            foreach (var name in names)
            {
                ScriptObject e = Core.Instance.Get(name, context);
              
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
            var names = Core.Instance.GetNameSet(this.showModOnly.CheckState != CheckState.Checked, context).OrderBy(a => a).ToList();

            NamespaceItems defaultNamespace = new NamespaceItems();

            foreach (var name in names)
            {
                ScriptObject e = Core.Instance.Get(name, context);

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
