﻿#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using DarkUI.Docking;
using DarkUI.Forms;
using DarkUI.Win32;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using JominiParse;

#endregion

namespace CK3ScriptEditor
{
    public partial class CK3ScriptEd : DarkForm
    {
        public static CK3ScriptEd Instance;
        private readonly List<DarkDockContent> _toolWindows = new List<DarkDockContent>();

        private readonly Dictionary<RefFilename, TextEditorControl> baseTextEditors =
            new Dictionary<RefFilename, TextEditorControl>();

        internal ObjectDetailsExplorer detailsExplorer;
        internal FileOverviewToolWindow fileOverview;
        private bool fileOverviewDirty;

        private ScriptWindow lastActive;

        private readonly Dictionary<RefFilename, TextEditorControl> modTextEditors =
            new Dictionary<RefFilename, TextEditorControl>();

        private readonly Dictionary<RefFilename, ScriptWindow> openBaseScriptWindows =
            new Dictionary<RefFilename, ScriptWindow>();

        private readonly Dictionary<RefFilename, ScriptWindow> openModScriptWindows =
            new Dictionary<RefFilename, ScriptWindow>();

        public List<ScriptWindow> OpenScriptWindows = new List<ScriptWindow>();
        internal ProjectExplorer projectExplorer;
        internal SearchResultsWindow searchResults;
        internal SmartFindOptionWindow smartFind;
        internal ScriptObjectExplorer soExplorer;

        private Dictionary<string, TabPage> tabs = new Dictionary<string, TabPage>();

        public CK3ScriptEd()
        {
            Instance = this;

            InitializeComponent();
            AutoSave.Interval = BackupManager.Instance.TickTimeMS;
            //  ScopeManager.Instance.LoadScopeDefinitions("ScopeDefs/scopes.xml");
            //  ScopeManager.Instance.LoadTriggerDefinitions("ScopeDefs/triggers.xml");
            //  ScopeManager.Instance.LoadEffectDefinitions("ScopeDefs/effects.xml");

            Application.AddMessageFilter(new ControlScrollFilter());
            // Add the dock content drag message filter to handle moving dock content around.
            Application.AddMessageFilter(DockPanel.DockContentDragFilter);

            // Add the dock panel message filter to filter through for dock panel splitter
            // input before letting events pass through to the rest of the application.
            Application.AddMessageFilter(DockPanel.DockResizeFilter);


            searchResults = new SearchResultsWindow();
            fileOverview = new FileOverviewToolWindow();
            soExplorer = new ScriptObjectExplorer();
            detailsExplorer = new ObjectDetailsExplorer();
            projectExplorer = new ProjectExplorer();
            smartFind = new SmartFindOptionWindow();
            //    eventPreview = new EventRepresentationPanel();
            //              DockPanel.AddContent(_dockHistory, _dockLayers.DockGroup);


            _toolWindows.Add(detailsExplorer);
            _toolWindows.Add(projectExplorer);
            _toolWindows.Add(soExplorer);
            _toolWindows.Add(fileOverview);
            _toolWindows.Add(smartFind);
            _toolWindows.Add(searchResults);

            //  _toolWindows.Add(eventPreview);

            soExplorer.UpdateScriptExplorer();

            // Add the tool window list contents to the dock panel
            foreach (var toolWindow in _toolWindows)
                DockPanel.AddContent(toolWindow);

//            DockPanel.AddContent(detailsExplorer, smartFind.DockGroup);
            DockPanel.AddContent(detailsExplorer, smartFind.DockGroup);

            DockPanel.AddContent(soExplorer, projectExplorer.DockGroup);

            DockPanel.ActiveContentChanged += DockPanel_ActiveContentChanged;

            DockPanel.ContentRemoved += DockPanel_ContentRemoved;
            // Add the history panel to the layer panel group
            //     DockPanel.AddContent(_dockHistory, _dockLayers.DockGroup);

            var fsmProvider = new FileSyntaxModeProvider("./");

            HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider); // Attach to the text editor.

            BackupManager.Instance.UpdateTick();

            CK3EditorPreferencesManager.Instance.Load();
        }

        public bool AllowUpdateFile { get; set; }

        public IHighlightingStrategy HighlightStrategy { get; set; }

        public RefFilename CurrentFile { get; set; }
        public BasicFind Find { get; set; }
        public TabOpenWindowsDlg TabOpenDlg { get; set; }

        // internal EventRepresentationPanel eventPreview;
        private void ToggleToolWindow(DarkToolWindow toolWindow)
        {
            if (toolWindow.DockPanel == null)
                DockPanel.AddContent(toolWindow);
            else
                DockPanel.RemoveContent(toolWindow);
        }

        public void Goto(RefFilename tagFilename, int tagLineStart, bool isBase)
        {
            var text = GetTextEditor(tagFilename);

            text.ActiveTextAreaControl.CenterViewOn(tagLineStart, 0);
            text.ActiveTextAreaControl.Caret.Line = tagLineStart;
            fileOverview.UpdateTreeSelection(tagFilename, tagLineStart);
            text.ActiveTextAreaControl.TextArea.Focus();
            fileOverview.Invalidate();
        }

        public void Load(string modName)
        {
            for (var index = 0; index < OpenScriptWindows.Count; index++)
            {
                var openScriptWindow = OpenScriptWindows[index];
                if (!openScriptWindow.CheckSave())
                    return;
            }

            for (var index = 0; index < OpenScriptWindows.Count; index++)
            {
                var openScriptWindow = OpenScriptWindows[index];
                openScriptWindow.Close();
            }

            Core.Instance = new Core();

            UpdateAllWindows();
            Core.Instance.Init();

            if (Directory.Exists(Globals.CK3ModPath + modName))
            {
                Core.Instance.CreateOrLoadMod(modName);
                CK3EditorPreferencesManager.Instance.LoadWindows();
            }

            CK3EditorPreferencesManager.Instance.Save();

            UpdateAllWindows();
            BackupManager.Instance.MinutesSinceLastSave = 1000000000;
            BackupManager.Instance.UpdateTick();
        }

        private void DockPanel_ContentRemoved(object sender, DockContentEventArgs e)
        {
            if (e.Content is ScriptWindow)
            {
                var sw = e.Content as ScriptWindow;


                if (!sw.CheckSave())
                {
                    AllowUpdateFile = false;
                    sw.IgnoredFirstDirty = true;
                    sw.DockArea = DarkDockArea.Document;
                    DockPanel.AddContent(sw);
                    DockPanel.ActiveContent = sw;
                    AllowUpdateFile = true;
                    sw.IgnoredFirstDirty = false;
                }
                else
                {
                    sw.RemoveEventHandlers();
                    baseTextEditors.Remove(sw.Filename);
                    modTextEditors.Remove(sw.Filename);
                    openBaseScriptWindows.Remove(sw.Filename);
                    openModScriptWindows.Remove(sw.Filename);
                    OpenScriptWindows.Remove(e.Content as ScriptWindow);
                    CK3EditorPreferencesManager.Instance.Save();
                }
            }
        }

        private void DockPanel_ActiveContentChanged(object sender, DockContentEventArgs e)
        {
            if (DockPanel.ActiveContent is ScriptWindow && lastActive != DockPanel.ActiveContent)
            {
                (DockPanel.ActiveContent as ScriptWindow).Activate();
                lastActive = DockPanel.ActiveContent as ScriptWindow;
            }
        }


        public void LoadCK3File(RefFilename filename)
        {
            var fromBase = Core.Instance.LoadCK3File(filename);

            CurrentFile = filename;

            var scriptTextFile = GetTextEditor(filename);

            fileOverview.UpdateTree(filename, scriptTextFile.ActiveTextAreaControl.Caret.Line, fromBase);
        }

        public TextEditorControl GetTextEditor(RefFilename filename)
        {
            var fromBase = filename.IsBase;

            if (!filename.Exists)
                fromBase = !fromBase;

            var textEditors = fromBase ? baseTextEditors : modTextEditors;
            var openScriptWindows = fromBase ? openBaseScriptWindows : openModScriptWindows;

            if (textEditors.ContainsKey(filename))
            {
                DockPanel.ActiveContent = openScriptWindows[filename];

                openScriptWindows[filename].GetInside();

                CurrentFile = filename;
                return textEditors[filename];
            }

            CurrentFile = filename;
            AddTab(filename, fromBase);
            //   fileOverview.UpdateTree(filename, textEditors[filename].ActiveTextAreaControl.Caret.Line);
            return textEditors[filename];
        }

        public void AddTab(RefFilename filename, bool fromBase)
        {
            var textEditors = fromBase ? baseTextEditors : modTextEditors;
            var openScriptWindows = fromBase ? openBaseScriptWindows : openModScriptWindows;

            var window = new ScriptWindow(fromBase);

            openScriptWindows[filename] = window;

            var startDir =
                fromBase
                    ? Globals.CK3Path
                    : Core.Instance.ModCK3Library.Path; //"D:/SteamLibrary/steamapps/common/Crusader Kings III/";
            window.Filename = filename;
            AllowUpdateFile = false;
            window.ScriptFile = Core.Instance.GetFile(filename, fromBase);
            window.IsBaseFile = fromBase;
            textEditors[filename] = window.LoadFile(filename);
            DockPanel.AddContent(window);
            DockPanel.ActiveContent = window;
            window.UpdateLocalizations();
            window.Name = window.Text = filename.ToRelativeFilename();
            window.FullFilename = startDir + filename;
            AllowUpdateFile = true;
            fileOverview.UpdateTree(filename, textEditors[filename].ActiveTextAreaControl.Caret.Line, fromBase);
            if (!OpenScriptWindows.Contains(window))
                OpenScriptWindows.Add(window);
            CK3EditorPreferencesManager.Instance.Save();
        }

        private void windowToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        public void CloseDocument(bool mod, RefFilename path)
        {
            var textEditors = !mod ? baseTextEditors : modTextEditors;
            var openScriptWindows = !mod ? openBaseScriptWindows : openModScriptWindows;

            if (openScriptWindows.ContainsKey(path)) DockPanel.RemoveContent(openScriptWindows[path]);
        }

        private void fixOverriddenFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                "This process will take every object in overridden mod files that have changed (files that share the filename with CK3 base files) and move them to a new file with object specific overrides. This will improve mod compatibility for the mod.\n\nWARNING: This may not be what you want to do for all mods, if you are purposefully overriding content from the base files to remove them intentionally, for the purposes of Total Conversions etc then this may cause damage to your mod.\n\nPLEASE BACK UP BEFORE USING THIS!",
                "Warning", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                /*     List<string> toDelete = new List<string>();
                     foreach (var fileMapValue in Core.Instance.ModCK3Library.FileMap.Values)
                     {
                         var baseFile = Core.Instance.BaseCK3Library.GetFile(fileMapValue.Filename);
                         if (baseFile != null)
                         {
                             string filename = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/" + fileMapValue.Filename;
     
                             toDelete.Add(filename);
                             List<ScriptObject> changedObjects = new List<ScriptObject>();
                             List<ScriptObject> unchangedObjects = new List<ScriptObject>();
                             foreach (var scriptObject in fileMapValue.Map.Values)
                             {
                                 if (scriptObject.Name == "release_from_prison_interaction")
                                 {
     
                                 }
                                 var lines = RawScriptHelper.GetScriptFromFile(scriptObject);
                                 var baseLines = RawScriptHelper.GetScriptFromFileFromBase(scriptObject);
     
                                 bool matches = RawScriptHelper.DoScriptsMatchIgnoreWhitespace(baseLines, lines);
     
                                 if (!matches)
                                 {
                                     changedObjects.Add(scriptObject);
                                 }
                                 else
                                 {
                                     unchangedObjects.Add(scriptObject);
                                 }
                             }
     
                             // now transplant to a new file...
                             if(changedObjects.Count==0)
                                 continue;
                             ;
                             string fileEnd = fileMapValue.Filename.ToRelativeFilename().Substring(fileMapValue.Filename.ToRelativeFilename().LastIndexOf("/") + 1);
                             bool add00 = false;
                             int numOfFile = 1;
                             var under = fileEnd.IndexOf("_");
                                 String num = "";
     
                             if (under!=-1)
                                 num = fileEnd.Substring(0, under);
                             
     
                             if (Int32.TryParse(num, out numOfFile))
                             {
                                 numOfFile++;
                                 if (numOfFile == 100)
                                     numOfFile = 999;
                                 fileEnd = fileEnd.Substring(fileEnd.IndexOf("_")+1);
                                 add00 = true;
                                 fileEnd = Core.Instance.ModCK3Library.Name.ToLower() + "_" + fileEnd;
     
     
                                 string numStr = numOfFile.ToString();
     
                                 if (numStr.Length < 2)
                                     numStr = "0" + numStr;
     
                                 fileEnd = numStr + "_" + fileEnd;
     
                             }
                             else
                             {
                                 numOfFile = 1;
                                 fileEnd = "zz_" + fileEnd;
                             }
     
     
                        
                             string newFilename = fileMapValue.Filename.ToRelativeFilename().Substring(0, fileMapValue.Filename.ToRelativeFilename().LastIndexOf("/") + 1) + fileEnd;
     
                             RawScriptHelper.SaveScriptObjectsToFile(fileMapValue, changedObjects, newFilename);
                         }
                     }
     
                     foreach (var file in toDelete)
                     {
                         File.Delete(file);
                     }
     
                     Core.Instance = new Core();
                     Core.Instance.Init();
                     Core.Instance.LoadMod(Core.Instance.ModCK3Library.Name);
                     CK3ScriptEd.Instance.CloseAllModFileWindows();
                     CK3ScriptEd.Instance.UpdateAllWindows();
                     CK3EditorPreferencesManager.Instance.Save();*/
            }
        }

        public void UpdateAllWindows()
        {
            projectExplorer.FillProjectView();
            soExplorer.UpdateScriptExplorer();
            detailsExplorer.SetObject(null);
            if (DockPanel.ActiveContent is ScriptWindow)
                fileOverview.UpdateTree((DockPanel.ActiveContent as ScriptWindow).Filename,
                    (DockPanel.ActiveContent as ScriptWindow).Editor.ActiveTextAreaControl.Caret.Line,
                    (DockPanel.ActiveContent as ScriptWindow).ScriptFile.IsBase);
        }

        private void CloseAllModFileWindows()
        {
            foreach (var w in openModScriptWindows) DockPanel.RemoveContent(w.Value);
        }

        private void AutoSave_Tick(object sender, EventArgs e)
        {
            BackupManager.Instance.UpdateTick();
        }

        public List<RefFilename> GetOpenModWindowsFilenameList()
        {
            var files = new List<RefFilename>();

            foreach (var openModScriptWindow in openModScriptWindows) files.Add(openModScriptWindow.Value.Filename);

            return files;
        }

        public int GetOpenWindowsFilenameListIndex()
        {
            var files = new List<ScriptWindow>();

            foreach (var openModScriptWindow in openModScriptWindows) files.Add(openModScriptWindow.Value);

            foreach (var openModScriptWindow in openBaseScriptWindows) files.Add(openModScriptWindow.Value);

            files = files.OrderBy(a => DockPanel.Controls[0].Controls[0].Controls.IndexOf(a)).ToList();

            var results = new List<string>();

            for (var index = 0; index < files.Count; index++)
            {
                var scriptWindow = files[index];
                if (scriptWindow == DockPanel.ActiveContent)
                    return index;
            }

            return -1;
        }

        public List<string> GetOpenWindowsFilenameList()
        {
            var files = new List<ScriptWindow>();

            foreach (var openModScriptWindow in openModScriptWindows) files.Add(openModScriptWindow.Value);

            foreach (var openModScriptWindow in openBaseScriptWindows) files.Add(openModScriptWindow.Value);

            files = files.OrderBy(a => DockPanel.Controls[0].Controls[0].Controls.IndexOf(a)).ToList();

            var results = new List<string>();

            foreach (var scriptWindow in files)
            {
                if (scriptWindow.Filename == null)
                    return null;
                if (scriptWindow.ScriptFile == null)
                    return null;
                results.Add(scriptWindow.ScriptFile.IsBase
                    ? "base:" + scriptWindow.Filename
                    : "mod:" + scriptWindow.Filename);
            }

            return results;
        }

        public List<RefFilename> GetOpenBaseWindowsFilenameList()
        {
            var files = new List<RefFilename>();

            foreach (var openModScriptWindow in openBaseScriptWindows) files.Add(openModScriptWindow.Value.Filename);

            return files;
        }

        public void SetActiveEditor(TextEditorControl c)
        {
            DockPanel.ActiveContent = c.Parent as ScriptWindow;
        }

        public void ActivateWindow(ScriptWindow scriptWindow)
        {
            DockPanel.ActiveContent = scriptWindow;
        }

        private void loadModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Globals.CK3ModPath.Replace("/", "\\");
                openFileDialog.Filter = "descriptor.mod file (descriptor.mod)|descriptor.mod";
                openFileDialog.FilterIndex = 0;
                openFileDialog.RestoreDirectory = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    var filePath = openFileDialog.FileName.Replace("\\", "/");
                    filePath = filePath.Substring(0, filePath.LastIndexOf("/"));

                    Load(filePath.Substring(filePath.LastIndexOf("/") + 1));
                }
            }
        }

        private void CK3ScriptEd_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var openScriptWindow in OpenScriptWindows)
                if (!openScriptWindow.CheckSave())
                {
                    e.Cancel = true;
                    return;
                }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var sw = DockPanel.ActiveContent as ScriptWindow;

            if (sw.ScriptFile == null)
                return;

            if (sw != null && !sw.ScriptFile.IsBase) sw.Save();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new About();

            about.ShowDialog(this);
        }

        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (var index = 0; index < OpenScriptWindows.Count; index++)
            {
                var openScriptWindow = OpenScriptWindows[index];
                openScriptWindow.Close();
            }

            for (var index = 0; index < OpenScriptWindows.Count; index++)
            {
                var openScriptWindow = OpenScriptWindows[index];
                openScriptWindow.Close();
            }

            for (var index = 0; index < OpenScriptWindows.Count; index++)
            {
                var openScriptWindow = OpenScriptWindows[index];
                openScriptWindow.Close();
            }

            for (var index = 0; index < OpenScriptWindows.Count; index++)
            {
                var openScriptWindow = OpenScriptWindows[index];
                openScriptWindow.Close();
            }
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var openScriptWindow in OpenScriptWindows) openScriptWindow.Save();
        }
    }
}