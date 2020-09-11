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
using DarkUI.Controls;
using DarkUI.Docking;
using JominiParse;

namespace CK3ScriptEditor
{
    public partial class ProjectExplorer : DarkToolWindow
    {
        public ProjectExplorer()
        {
            DockArea = DarkDockArea.Left;
            InitializeComponent();
            FillProjectView();
        }

        public void FillProjectView()
        {
            projectTree.Nodes.Clear();

            DarkTreeNode ck3 = new DarkTreeNode("base");
            DarkTreeNode mod = new DarkTreeNode(Core.Instance.ModCK3Library.Name);

            projectTree.Nodes.Add(ck3);
            projectTree.Nodes.Add(mod);
            string startDir = Globals.CK3Path;//"D:/SteamLibrary/steamapps/common/Crusader Kings III/";
            string startModDir = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name;
            Fill(ck3, startDir, false);
            Fill(mod, startModDir, true);
        }

        List<string> allowedDirs = new List<string>()
        {
            "common",
            "events",
            "gui",
            "gfx",
            "history",
            "localization",
        };
        private void Fill(DarkTreeNode node, string startDir, bool mod)
        {
            var dirs = Directory.GetDirectories(startDir);

            for (var index = 0; index < dirs.Length; index++)
            {
                var dir = dirs[index];
                dir = dir.Replace("\\", "/");
                var s = dir.Substring(dir.LastIndexOf("/") + 1);
                if (allowedDirs.Contains(s))
                {
                    FillDir(node, dir, mod);
                }
            }
        }

        private void FillDir(DarkTreeNode node, string dir, bool mod)
        {
            string d = dir.Replace("\\", "/");
            d = d.Substring(d.LastIndexOf("/") + 1);
            DarkTreeNode sub = new DarkTreeNode(d);
            sub.Tag = dir;
            var dirs = Directory.GetDirectories(dir);
            node.Nodes.Add(sub);
            foreach (var sd in dirs)
            {
                FillDir(sub, sd, mod);
            }

            var files = Directory.GetFiles(dir);

            foreach (var file in files)
            {
                string ff = file.Replace("\\", "/");
                string ffshort = ff;
                if (!mod)
                    ffshort = ff.Substring(ff.LastIndexOf("game/") + 5);
                else
                    ffshort = ff.Substring(ff.LastIndexOf(Core.Instance.ModCK3Library.Name + "/") + Core.Instance.ModCK3Library.Name.Length+1);
                ff = ff.Substring(ff.LastIndexOf("/") + 1);
                DarkTreeNode f = new DarkTreeNode(ff);
                f.Tag = ffshort;
                
                sub.Nodes.Add(f);

            }
        }

        private void projectTree_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (projectTree.SelectedNodes.Count > 0)
            {
                var sel = projectTree.SelectedNodes[0];

                var tag = sel.Tag as string;

                bool fromBase = false;

                while (sel != null)
                {
                    if(sel.Text == "base" && sel.ParentNode == null)
                        fromBase = true;
                    sel = sel.ParentNode;
                }

                CK3ScriptEd.Instance.LoadCK3File(tag, fromBase);

            }
        }
    }
}
