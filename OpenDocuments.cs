using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkUI.Controls;
using DarkUI.Docking;
using ICSharpCode.TextEditor;
using JominiParse;

namespace CK3ScriptEditor
{
    public class OpenDocuments
    {
        public RefFilename CurrentFile { get; set; }


        private readonly Dictionary<RefFilename, TextEditorControl> baseTextEditors =
            new Dictionary<RefFilename, TextEditorControl>();

        private readonly Dictionary<RefFilename, TextEditorControl> modTextEditors =
            new Dictionary<RefFilename, TextEditorControl>();

        private readonly Dictionary<RefFilename, ScriptWindow> openBaseScriptWindows =
            new Dictionary<RefFilename, ScriptWindow>();

        private readonly Dictionary<RefFilename, ScriptWindow> openModScriptWindows =
            new Dictionary<RefFilename, ScriptWindow>();

        private List<ScriptWindow> openScriptWindows = new List<ScriptWindow>();
        
        private DarkDockPanel DockPanel;

        public OpenDocuments(DarkDockPanel dockPanel)
        {
            this.DockPanel = dockPanel;
        }

        public bool CloseAllDocuments()
        {
            for (var index = 0; index < openScriptWindows.Count; index++)
            {
                var openScriptWindow = openScriptWindows[index];
                if (!openScriptWindow.CheckSave())
                    return false;
            }

            for (var index = 0; index < openScriptWindows.Count; index++)
            {
                var openScriptWindow = openScriptWindows[index];
                openScriptWindow.Close();
            }
            for (var index = 0; index < openScriptWindows.Count; index++)
            {
                var openScriptWindow = openScriptWindows[index];
                openScriptWindow.Close();
            }
            for (var index = 0; index < openScriptWindows.Count; index++)
            {
                var openScriptWindow = openScriptWindows[index];
                openScriptWindow.Close();
            }

            return true;
        }

        public void Remove(ScriptWindow sw)
        {
            modTextEditors.Remove(sw.Filename);
            openBaseScriptWindows.Remove(sw.Filename);
            openModScriptWindows.Remove(sw.Filename);
            openScriptWindows.Remove(sw);
            baseTextEditors.Remove(sw.Filename);
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

            return null;
        }

        public bool Contains(ScriptWindow window)
        {
            return openScriptWindows.Contains(window);
        }

        public void Add(ScriptWindow window)
        {
            openScriptWindows.Add(window);
        }

        public ScriptWindow AddTab(RefFilename filename)
        {
            bool fromBase = filename.IsBase;
            var textEditors = fromBase ? baseTextEditors : modTextEditors;
            var openScriptWindows = fromBase ? openBaseScriptWindows : openModScriptWindows;

            var window = new ScriptWindow(fromBase);
            window.ScriptFile = Core.Instance.GetFile(filename, fromBase);

            openScriptWindows[filename] = window;
                
            window.Filename = filename;
            textEditors[filename] = window.LoadFile(filename);
            return window;
        }

        public bool CheckSave()
        {
            foreach (var openScriptWindow in openScriptWindows)
            {
                if (!openScriptWindow.CheckSave())
                {
                    return false;
                }
            }

            return true;
        }

        public void SaveAll()
        {
            foreach (var openScriptWindow in openScriptWindows) 
                openScriptWindow.Save();
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

        public void CloseDocument(bool mod, RefFilename path)
        {
            var textEditors = !mod ? baseTextEditors : modTextEditors;
            var openScriptWindows = !mod ? openBaseScriptWindows : openModScriptWindows;

            if (openScriptWindows.ContainsKey(path)) DockPanel.RemoveContent(openScriptWindows[path]);
        }

        public int NumDocumentsOpen()
        {
            return openScriptWindows.Count;
        }

        public void MoveToStartOfDocumentList(ScriptWindow scriptWindow)
        {
            openScriptWindows.Remove(scriptWindow);
            openScriptWindows.Insert(0, scriptWindow);
        }

        public void CreateOpenTabWindowList(DarkListView openFileList)
        {
            foreach (var instanceOpenScriptWindow in openScriptWindows)
            {
                var s = instanceOpenScriptWindow.Filename;

                var ss = s.ToRelativeFilename().Substring(s.ToRelativeFilename().LastIndexOf("/") + 1);

                var col = 50;

                while (ss.Length < col)
                    ss += " ";

                if (instanceOpenScriptWindow.ScriptFile.IsBase)
                    ss += "Base: ";
                else
                    ss += "Mod:  ";

                ss += s.ToRelativeFilename().Substring(0, s.ToRelativeFilename().LastIndexOf("/"));

                var i = new DarkListItem(ss);
                i.Tag = instanceOpenScriptWindow;
                openFileList.Items.Add(i);
            }
        }
    }
}
