#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DarkUI.Controls;
using DarkUI.Docking;
using JominiParse;

#endregion

namespace CK3ScriptEditor
{
    public partial class ProjectExplorer : DarkToolWindow
    {
        private readonly List<string> allowedDirs = new List<string>
        {
            "common",
            "events",
            "gui",
            "gfx",
            "history",
            "map_data",
            "localization"
        };


        private readonly ContextMenu menu = new ContextMenu();

        public ProjectExplorer()
        {
            DockArea = DarkDockArea.Left;
            AutoScaleMode = AutoScaleMode.Inherit;

            InitializeComponent();
            FillProjectView();
        }

        public string AddToPath { get; set; }

        private string CurrentSelectedPath
        {
            get
            {
                if (projectTree.SelectedNodes.Count > 0)
                {
                    var sel = projectTree.SelectedNodes[0];

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

            var nodes = projectTree.Nodes;

            foreach (var s in p)
            foreach (var node in nodes)
                if (node.Text == s)
                {
                    node.Expanded = true;
                    projectTree.SelectNode(node);
                    nodes = node.Nodes;
                    break;
                }
        }

        public void FillProjectView()
        {
            if (Core.Instance.ModCK3Library == null)
                return;

            var path = CurrentSelectedPath;


            switch (EditorGlobals.FontSize)
            {
                case FontSize.Normal:
                    projectTree.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    break;
                case FontSize.Large:
                    projectTree.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    break;
            }


            projectTree.Nodes.Clear();
            projectTree.SuspendNodeEvents();
            var ck3 = new DarkTreeNode("base");
            var mod = new DarkTreeNode(Core.Instance.ModCK3Library.Name);

            projectTree.Nodes.Add(ck3);
            projectTree.Nodes.Add(mod);
            var startDir = Globals.CK3Path; //"D:/SteamLibrary/steamapps/common/Crusader Kings III/";
            var startModDir = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name;
            Fill(ck3, new RefFilename(startDir), false);
            Fill(mod, new RefFilename(startModDir), true);
            //   mod.Tag = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/";
            projectTree.ResumeNodeEvents();

            SelectPath(path);

            AddToPath = null;
        }

        private void Fill(DarkTreeNode node, RefFilename startDir, bool mod)
        {
            var dirs = startDir.GetDirectories();

            for (var index = 0; index < dirs.Length; index++)
            {
                var dir = dirs[index];
                var s = dir.Name;
                if (allowedDirs.Contains(s)) FillDir(node, dir, mod);
            }
        }

        private void FillDir(DarkTreeNode node, RefFilename dir, bool mod)
        {
            var d = dir.Name;

            var sub = new DarkTreeNode(d);
            sub.Tag = dir;

            var dirShort = dir.ToRelativeFilename();


            var dirs = dir.GetDirectories();
            node.Nodes.Add(sub);
            foreach (var sd in dirs) FillDir(sub, sd, mod);

            var files = dir.GetFiles();

            foreach (var file in files)
            {
                var ff = file.Name;

                var f = new DarkTreeNode(ff);
                f.Tag = file;

                sub.Nodes.Add(f);
            }
        }

        private void projectTree_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (projectTree.SelectedNodes.Count > 0)
            {
                var sel = projectTree.SelectedNodes[0];

                if (!(sel.Tag is RefFilename))
                    return;

                var tag = (RefFilename) sel.Tag;

                if (tag.IsFile)
                    CK3ScriptEd.Instance.LoadCK3File(tag);
            }
        }

        private static bool IsFromBase(DarkTreeNode sel)
        {
            var fromBase = false;

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
                var newFile = new MenuItem("&New file");
                var newFolder = new MenuItem("&New folder");
                var delete = new MenuItem("&Delete");
                var overrideFolder = new MenuItem("&Override folder");
                var overrideFile = new MenuItem("&Override file");
                // Define the MenuItem object to display for the PictureBox.

                // Clear all previously added MenuItems.
                menu.MenuItems.Clear();

                if (projectTree.SelectedNodes.Count > 0 && projectTree.SelectedNodes[0].Tag != null)
                {
                    var selected = projectTree.SelectedNodes[0].Tag as RefFilename;
                    if (selected == null)
                        return;

                    var p = projectTree.SelectedNodes[0];
                    while (p.ParentNode != null) p = p.ParentNode;

                    var isBase = false;
                    if (p.Text == "base")
                        isBase = true;


                    newFile.Tag = selected;
                    newFolder.Tag = selected;
                    delete.Tag = selected;
                    overrideFolder.Tag = selected;
                    overrideFile.Tag = selected;


                    var directory = selected.IsDirectory;

                    if (directory)
                    {
                        if (!isBase)
                        {
                            menu.MenuItems.Add(newFolder);
                            menu.MenuItems.Add(newFile);
                            menu.MenuItems.Add(delete);
                        }
                        else
                        {
                            menu.MenuItems.Add(overrideFolder);
                        }
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

            var tag = menuItem.Tag as RefFilename;

            OverrideFolder(tag);

            CK3ScriptEd.Instance.UpdateAllWindows();
        }

        private void OverrideFolder(RefFilename folder)
        {
            var dirs = folder.GetDirectories();

            foreach (var dir in dirs) OverrideFolder(dir);

            var files = folder.GetFiles();

            for (var index = 0; index < files.Length; index++)
            {
                var file = files[index];

                CreateFile(file);
            }
        }

        private void OverrideFileOnClick(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;

            var tag = menuItem.Tag as RefFilename;

            CreateFile(tag);
            CK3ScriptEd.Instance.UpdateAllWindows();
        }

        private void CreateFile(RefFilename filename)
        {
            filename = filename.ToMod();
            var dir = filename.ToFullDirectoryOfFile();

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);


            if (!filename.Exists)
            {
                using (var fs = new FileStream(filename.ToFullWindowsFilename(), FileMode.Create))
                {
                    using (TextWriter writeFile = new StreamWriter(fs, Encoding.UTF8))
                    {
                    }
                }

                if (!Core.Instance.BaseCK3Library.HasFile(filename.ToBase()))
                    return;

                Core.Instance.ModCK3Library.AddFile(filename);
                Core.Instance.LoadCK3File(filename, false, true);

                var f = Core.Instance.BaseCK3Library.GetFile(filename);

                foreach (var mapValue in f.Map.Values) mapValue.Overridden = true;
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;

            var tag = menuItem.Tag.ToString();
        }

        private void NewFolder_Click(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;

            var tag = menuItem.Tag as RefFilename;

            var f = new NewFileFolder();

            f.Dir = tag;
            f.Text = "Create new folder...";
            if (f.ShowDialog(CK3ScriptEd.Instance) == DialogResult.OK)
            {
                var t = f.NewText.Text;

                var dir = tag.SubDir(t);
                dir = dir.ToMod();

                if (!dir.Exists)
                    Directory.CreateDirectory(dir.ToFullWindowsFilename());
                CK3ScriptEd.Instance.UpdateAllWindows();
            }
        }

        private void NewFile_Click(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;

            var tag = menuItem.Tag as RefFilename;

            var f = new NewFileFolder();

            f.Dir = tag;
            f.Text = "Create new file...";

            if (f.ShowDialog(CK3ScriptEd.Instance) == DialogResult.OK)
            {
                var t = f.NewText.Text;

                var file = t;
                if (!file.EndsWith(".txt"))
                    file = file + ".txt";

                var fileRef = tag.AsFileInDirectory(file);

                fileRef = fileRef.ToMod();
                if (!fileRef.Exists)
                {
                    if (Core.Instance.ModCK3Library.AddFile(fileRef))
                    {
                        using (var fs = new FileStream(fileRef.ToFullWindowsFilename(), FileMode.Create))
                        {
                            using (TextWriter writeFile = new StreamWriter(fs, Encoding.UTF8))
                            {
                            }
                        }

                        Core.Instance.LoadCK3File(fileRef, false, true);


                        if (Core.Instance.BaseCK3Library.HasFile(fileRef))
                        {
                            var ff = Core.Instance.BaseCK3Library.GetFile(fileRef);

                            foreach (var mapValue in ff.Map.Values) mapValue.Overridden = true;
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