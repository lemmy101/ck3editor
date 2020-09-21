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

        public string AddToPath { get; set; }

        string CurrentSelectedPath
        {
            get
            {

                if (projectTree.SelectedNodes.Count > 0)
                {
                    var sel = projectTree.SelectedNodes[0];

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

            var nodes = projectTree.Nodes;

            foreach (var s in p)
            {
                foreach (var node in nodes)
                {
                    if (node.Text == s)
                    {
                        node.Expanded = true;
                        projectTree.SelectNode(node);
                        nodes = node.Nodes;
                        break;
                    }
                }
            }
        }
        public void FillProjectView()
        {
            if (Core.Instance.ModCK3Library == null)
                return;

            string path = CurrentSelectedPath;


            projectTree.Nodes.Clear();
            projectTree.SuspendNodeEvents();
            DarkTreeNode ck3 = new DarkTreeNode("base");
            DarkTreeNode mod = new DarkTreeNode(Core.Instance.ModCK3Library.Name);

            projectTree.Nodes.Add(ck3);
            projectTree.Nodes.Add(mod);
            string startDir = Globals.CK3Path;//"D:/SteamLibrary/steamapps/common/Crusader Kings III/";
            string startModDir = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name;
            Fill(ck3, startDir, false);
            Fill(mod, startModDir, true);
         //   mod.Tag = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/";
            projectTree.ResumeNodeEvents();

            SelectPath(path);

            AddToPath = null;
        }


        ContextMenu menu = new ContextMenu();
        List<string> allowedDirs = new List<string>()
        {
            "common",
            "events",
            "gui",
            "gfx",
            "history",
            "map_data",
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

            string dirShort = null;
            if (!mod)
                dirShort = dir.Substring(dir.LastIndexOf("game/") + 5);
            else
                dirShort = dir.Substring(dir.LastIndexOf(Core.Instance.ModCK3Library.Name + "/") + Core.Instance.ModCK3Library.Name.Length + 1);


            sub.Tag = dirShort;
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
                    ffshort = ff.Substring(ff.LastIndexOf(Core.Instance.ModCK3Library.Name + "/") + Core.Instance.ModCK3Library.Name.Length + 1);
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

                var fromBase = IsFromBase(sel);

                if (tag != null)
                {
                    tag = tag.Replace("\\", "/");
                    tag = tag.Replace(Core.Instance.ModCK3Library.Path, "");
                    tag = tag.Replace(Globals.CK3Path, "");

                    if (!tag.EndsWith(".txt") && !tag.EndsWith(".yml"))
                        return;

                    CK3ScriptEd.Instance.LoadCK3File(tag, fromBase);
                }

            }
        }

        private static bool IsFromBase(DarkTreeNode sel)
        {
            bool fromBase = false;

            while (sel != null)
            {
                if (sel.Text == "base" && sel.ParentNode == null)
                    fromBase = true;
                sel = sel.ParentNode;
            }

            return fromBase;
        }

        private void projectTree_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                projectTree.Invalidate();
                MenuItem newFile = new MenuItem("&New file");
                MenuItem newFolder = new MenuItem("&New folder");
                MenuItem delete = new MenuItem("&Delete");
                MenuItem overrideFolder = new MenuItem("&Override folder");
                MenuItem overrideFile = new MenuItem("&Override file");
                // Define the MenuItem object to display for the PictureBox.

                // Clear all previously added MenuItems.
                menu.MenuItems.Clear();
                
                if (projectTree.SelectedNodes.Count > 0 && projectTree.SelectedNodes[0].Tag != null)
                {
                    var selected = projectTree.SelectedNodes[0].Tag.ToString();

                    var p = projectTree.SelectedNodes[0];
                    while (p.ParentNode != null)
                    {
                        p = p.ParentNode;
                    }

                    bool isBase = false;
                    if (p.Text == "base")
                        isBase = true;

                    selected = selected.Replace("\\", "/");

                    newFile.Tag = selected;
                    newFolder.Tag = selected;
                    delete.Tag = selected;
                    overrideFolder.Tag = selected;
                    overrideFile.Tag = selected;

                    if (isBase)
                        selected = Globals.CK3Path + selected + "/";
                    else
                        selected = Core.Instance.ModCK3Library.Path + selected + "/";


                    bool directory = Directory.Exists(selected);

                    if (directory)
                    {
                        if (!isBase)
                        {
                            menu.MenuItems.Add(newFolder);
                            menu.MenuItems.Add(newFile);
                            menu.MenuItems.Add(delete);

                        }
                        else
                            menu.MenuItems.Add(overrideFolder);
                    }
                    else
                    {
                        if (!isBase)
                            menu.MenuItems.Add(delete);
                        else
                            menu.MenuItems.Add(overrideFile);
                    }

                    newFile.Click += NewFile_Click;
                    newFolder.Click += NewFolder_Click;
                    delete.Click += Delete_Click;
                    overrideFile.Click += OverrideFileOnClick;
                    overrideFolder.Click += OverrideFolderOnClick;
                    menu.Show(projectTree, new Point(e.X, e.Y));

                }



            }
        }

        private void OverrideFolderOnClick(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;

            string tag = menuItem.Tag.ToString();

            string from = tag;

            from = Globals.CK3Path + from + "/";
        

            OverrideFolder(from);

            CK3ScriptEd.Instance.UpdateAllWindows();
        }

        private void OverrideFolder(string folder)
        {
            string[] dirs = Directory.GetDirectories(folder);

            foreach (var dir in dirs)
            {
                OverrideFolder(dir);
            }

            string[] files = Directory.GetFiles(folder);

            for (var index = 0; index < files.Length; index++)
            {
                var file = files[index];
                file = file.Replace("\\", "/");

                file = file.Replace(Globals.CK3Path, "");

                string toCreate = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/";
                toCreate += file;
                CreateFile(toCreate);
            }
       }

        private void OverrideFileOnClick(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;

            string tag = menuItem.Tag.ToString();

            string toCreate = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/";
            toCreate += tag;
            CreateFile(toCreate);
            CK3ScriptEd.Instance.UpdateAllWindows();
        }

        private void CreateFile(string filename)
        {
            string dir = filename.Substring(0, filename.LastIndexOf("/"));

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            if (!File.Exists(filename))
            {
             
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    using (System.IO.TextWriter writeFile = new StreamWriter(fs, Encoding.UTF8))
                    {

                    }
                }

                 filename = filename.Replace(Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/", "");

                 if (!Core.Instance.BaseCK3Library.FileMap.ContainsKey(filename))
                     return;

                Core.Instance.ModCK3Library.AddFile(filename);
                Core.Instance.LoadCK3File(filename, false, true);

                var f = Core.Instance.BaseCK3Library.FileMap[filename];

                foreach (var mapValue in f.Map.Values)
                {
                    mapValue.Overridden = true;
                }
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;

            string tag = menuItem.Tag.ToString();
        }

        private void NewFolder_Click(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;

            string tag = menuItem.Tag.ToString();
            tag = Core.Instance.ModCK3Library.Path + tag + "/";

            NewFileFolder f = new NewFileFolder();

            f.Dir = tag;
            f.Text = "Create new folder...";
            if (f.ShowDialog(CK3ScriptEd.Instance) == DialogResult.OK)
            {
                string t = f.NewText.Text;

                AddToPath = t;

                string dir = tag + "/" + t;

                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                CK3ScriptEd.Instance.UpdateAllWindows();
            }
        }

        private void NewFile_Click(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;

            string tag = menuItem.Tag.ToString();
            tag = Core.Instance.ModCK3Library.Path + tag;

            NewFileFolder f = new NewFileFolder();

            f.Dir = tag;
            f.Text = "Create new file...";

            if (f.ShowDialog(CK3ScriptEd.Instance) == DialogResult.OK)
            {
                string t = f.NewText.Text;

                string file = t;
                if (!file.EndsWith(".txt"))
                    file = file + ".txt";

                AddToPath = file;
                file = tag + "/" + file;
                if (!File.Exists(file))
                {
                    var file2 = file.Replace(Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/", "");

                    if(Core.Instance.ModCK3Library.AddFile(file2))
                    {
                        
                        using (FileStream fs = new FileStream(file, FileMode.Create))
                        {
                            using (System.IO.TextWriter writeFile = new StreamWriter(fs, Encoding.UTF8))
                            {

                            }
                        }
                        file = file2;

                        Core.Instance.LoadCK3File(file, false, true);


                        if (Core.Instance.BaseCK3Library.FileMap.ContainsKey(file))
                        {
                            var ff = Core.Instance.BaseCK3Library.FileMap[file];

                            foreach (var mapValue in ff.Map.Values)
                            {
                                mapValue.Overridden = true;
                            }

                        }

                        CK3ScriptEd.Instance.UpdateAllWindows();
                    }

                }
                else
                {
                    AddToPath = null;

                }
            }
        }
    }
}
