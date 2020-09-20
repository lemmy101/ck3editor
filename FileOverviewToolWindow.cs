using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Collections;
using DarkUI.Controls;
using DarkUI.Docking;
using JominiParse;

namespace CK3ScriptEditor
{
    public partial class FileOverviewToolWindow : DarkToolWindow
    {
        public FileOverviewToolWindow()
        {
            DockArea = DarkDockArea.Left;
            InitializeComponent();

            TreeView.MouseDoubleClick += TreeView_MouseDoubleClick;
        }

        private void TreeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(TreeView.SelectedNodes.Count > 0)
            {
                var sel = TreeView.SelectedNodes[0];

                var tag = sel.Tag as ScriptObject;
                if ((tag as ScriptObject) == null)
                    return;

                CK3ScriptEd.Instance.Goto(tag.Filename, tag.LineStart-1, IsBaseFile);

       
            }

        }

        public void Clear()
        {
            TreeView.Nodes.Clear();
            TreeView.SelectedNodes.Clear();

        }
        public void UpdateTree(string filename, int lineNumber, bool isBaseFile)
        {
            IsBaseFile = isBaseFile;
            TreeView.Nodes.Clear();
            TreeView.SelectedNodes.Clear();
            // Build dummy nodes
            var childCount = 0;

            
            TreeView.SuspendNodeEvents();
            var file = Core.Instance.GetFile(filename, isBaseFile);

            foreach (var scriptObject in file.Map.Values) 
            {
                var node = new DarkTreeNode(scriptObject.Name);

                node.Tag = scriptObject;

                FillTreeData(node, scriptObject, lineNumber);

                TreeView.Nodes.Add(node);
            }

            TreeView.ResumeNodeEvents();

            UpdateTreeSelection(filename, lineNumber);
        }

        public bool IsBaseFile { get; set; }

        Dictionary<object, DarkTreeNode> fileOverviewNodes = new Dictionary<object, DarkTreeNode>();
       
        private void FillTreeData(DarkTreeNode treeNode, ScriptObject ev, int lineNumber)
        {
            fileOverviewNodes.Clear();
            var triggeredBy = new DarkTreeNode("Triggered By");
            bool hasReferencedBy = false;
          
            /*foreach (var eventConnection in ev.Connections)
            {
                if (eventConnection.To == ev)
                {
                    DarkTreeNode n = new DarkTreeNode(eventConnection.From.Name);
                    n.Tag = eventConnection.FromCommand;
                    fileOverviewNodes[n.Tag] = n;
                    triggeredBy.Nodes.Add(n);
                    hasReferencedBy = true;
                }
            }
            */
            if (hasReferencedBy)
            {
              //  treeNode.Nodes.Add(triggeredBy);
            }

            foreach (var scriptObject in ev.Children)
            {
                AddTreeNode(treeNode, scriptObject);
            }

            treeNode.Expanded = false;
            //    treeNode.Nodes.Add(new TreeNode(decision.picture.ToString()) { Tag = decision.picture });
        }

        private void AddTreeNode(DarkTreeNode treeNode, ScriptObject obj)
        {
            if (obj == null)
                return;

            var n = new DarkTreeNode(obj.Name);
            n.Tag = obj;
            treeNode.Nodes.Add(n);

            n.Expanded = false;
        }

        private int CaretLine = 0;
        public void UpdateTreeSelection(string filename, int caretLine)
        {
            caretLine++;
            
            CaretLine = caretLine;
         TreeView.SelectedNodes.Clear();
            var n = TreeView.Nodes;
            foreach (var ndd in TreeView.Nodes)
            {
                ndd.Expanded = false;
            }

            DarkTreeNode chosen = null;
            DoNodeSelection(caretLine, n, out chosen);
            if(chosen != null)
            {
                TreeView.SelectedNodes.Clear();
                TreeView.SelectedNodes.Add(chosen);
                if (TreeView.SelectedNodes.Count > 0 && TreeView.SelectedNodes[0].ParentNode != null)
                    TreeView.SelectedNodes[0].ParentNode.Expanded = true;
                else if (TreeView.SelectedNodes.Count > 0 && TreeView.SelectedNodes[0].Nodes.Count > 0)
                    TreeView.SelectedNodes[0].Expanded = true;

            }

            // TreeView.Invalidate();

            TreeView.EnsureVisible();

            TreeView.Invalidate();
            TreeView.Update();
        }

        private void DoNodeSelection(int caretLine, ObservableList<DarkTreeNode> n, out DarkTreeNode chosen)
        {
            bool brk = false;

            DarkTreeNode res = null;
            for (var index = n.Count - 1; index >= 0; index--)
            {
                var treeViewNode = n[index];
                var so = (treeViewNode.Tag as ScriptObject);

                if (so != null)
                {
                    if (so.LineStart <= caretLine && so.LineEnd >= caretLine)
                    {
                        res = treeViewNode;
                        //    brk = true;
                    }

                    DarkTreeNode nchosen;
                    DoNodeSelection(caretLine, treeViewNode.Nodes, out nchosen);

                    if (nchosen != null)
                    {
                        chosen = nchosen;
                        return;
                    }

                    if (res != null)
                    {
                        chosen = res;
                        return;
                    }

                }
            }

            chosen = null;
        }
    }
    /*
      * 
     private void DoNodeSelection(int caretLine, ObservableList<DarkTreeNode> n)
     {
         bool brk = false;
         for (var index = 0; index < n.Count; index++)
         {
             var treeViewNode = n[index];
             var so = (treeViewNode.Tag as ScriptObject);

             if (so != null)
             {
                 if (so.LineStart <= caretLine && (so.LineEnd >= caretLine))
                 {
                     TreeView.SelectedNodes.Clear();
                     TreeView.SelectedNodes.Add(treeViewNode);
                     TreeView.Invalidate();
                     treeViewNode.Expanded = true;
                     brk = true;
                 }

                 DoNodeSelection(caretLine, treeViewNode.Nodes);

                 if (brk)
                     return;
             }
         }
     }
      */
}
