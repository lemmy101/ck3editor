using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Controls;
using DarkUI.Docking;
using JominiParse;

namespace CK3ScriptEditor
{
    public partial class SearchResultsWindow : DarkToolWindow
    {
        public SearchResultsWindow()
        {
            DockArea = DarkDockArea.Bottom;
            InitializeComponent();
            resultsTree.ItemHeight += 4;
        }

        public void SetResults(SmartFindOptions options, List<SmartFindResults> results)
        {
            resultsTree.SuspendNodeEvents();
            resultsTree.Nodes.Clear();
       
            var basenode = new DarkTreeNode("base");

            if (options.SearchBase)
            {
                 int n = FillResults(basenode, results, false, options.ShowChildren);
                 if (basenode.Nodes.Count > 0)
                 {
                     basenode.Text = basenode.Text + " - (" + n + " results in "+ basenode.Nodes.Count+" files)";
                     resultsTree.Nodes.Add(basenode);
                     basenode.Expanded = true;
                 }

            }

            var modnode = new DarkTreeNode(Core.Instance.ModCK3Library.Name);
            if (options.SearchMod)
            {
                int n = FillResults(modnode, results, true, options.ShowChildren);
                if (modnode.Nodes.Count > 0)
                {
                    modnode.Text = modnode.Text + " - (" + n + " results in " + modnode.Nodes.Count + " files)";
                    resultsTree.Nodes.Add(modnode);
                    modnode.Expanded = true;
                }
            }

            resultsTree.ResumeNodeEvents();
        }

        private int FillResults(DarkTreeNode parent, List<SmartFindResults> results, bool mod, bool showChildren)
        {
            List<DarkTreeNode> toAdd = new List<DarkTreeNode>();

            int col2 = 7;

            foreach (var smartFindResultse in results)
            {
                if (smartFindResultse.IsModResult == mod)
                {
                    var str = smartFindResultse.ScriptObject.LineStart + ": ";

                    while (str.Length < col2)
                    {
                        str += " ";
                    }

                    str += smartFindResultse.ScriptObject.Name;

                    var val = smartFindResultse.ScriptObject.GetStringValue();

                    if (val != null)
                        str = str + " " +smartFindResultse.ScriptObject.Op+ " " + val;

                 

                   

                    DarkTreeNode node = new DarkTreeNode(str);
                    node.Tag = smartFindResultse.ScriptObject;
                    node.TextColor = Color.White;
                    toAdd.Add(node);


                    if (smartFindResultse.ScriptObject.Children.Count > 0 && showChildren)
                    {
                        foreach (var scriptObjectChild in smartFindResultse.ScriptObject.Children)
                        {
                            str = "";
                            while (str.Length < col2+3)
                            {
                                str += " ";
                            }

                            str += scriptObjectChild.Name;

                            val = scriptObjectChild.GetStringValue();

                            if (val != null)
                                str = str + " " + scriptObjectChild.Op + " " + val;

                            DarkTreeNode c = new DarkTreeNode(str);
                            c.Tag = scriptObjectChild;
                            node.Nodes.Add(c);
                            
                        }
                        node.Expanded = true;
                    }
                    
                }
            }

            var group = toAdd.GroupBy(a => (a.Tag as ScriptObject).Topmost.ScriptFile);

            foreach (var g in group)
            {
                DarkTreeNode file = new DarkTreeNode(g.Key.Filename + " - (" + g.ToList().Count + " results)"); 
                file.Nodes.AddRange(g.ToList());
                parent.Nodes.Add(file);
            }

            return toAdd.Count;
        }

        private void resultsTree_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (resultsTree.SelectedNodes.Count > 0)
            {
                var so = resultsTree.SelectedNodes[0].Tag as ScriptObject;

                if (so != null)
                {
                    CK3ScriptEd.Instance.Goto(so.Topmost.ScriptFile.Filename, so.LineStart-1, so.Topmost.ScriptFile.IsBase);
                }
            }
            
        }
    }
}
