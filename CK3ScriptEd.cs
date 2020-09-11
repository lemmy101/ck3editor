using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using DarkUI.Docking;
using DarkUI.Forms;
using DarkUI.Win32;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using JominiParse;

namespace CK3ScriptEditor
{
    public partial class CK3ScriptEd : DarkForm
    {
        Dictionary<string, TabPage> tabs = new Dictionary<string, TabPage>();
        Dictionary<string, ICSharpCode.TextEditor.TextEditorControl> modTextEditors = new Dictionary<string, ICSharpCode.TextEditor.TextEditorControl>();
        Dictionary<string, ICSharpCode.TextEditor.TextEditorControl> baseTextEditors = new Dictionary<string, ICSharpCode.TextEditor.TextEditorControl>();
        private Dictionary<string, ScriptWindow> openModScriptWindows = new Dictionary<string, ScriptWindow>();
        private Dictionary<string, ScriptWindow> openBaseScriptWindows = new Dictionary<string, ScriptWindow>();
        private bool fileOverviewDirty;
        public static CK3ScriptEd Instance;
        private List<DarkDockContent> _toolWindows = new List<DarkDockContent>();
        internal FileOverviewToolWindow fileOverview;
        internal ProjectExplorer projectExplorer;
        private ScriptObjectExplorer soExplorer;
        private void ToggleToolWindow(DarkToolWindow toolWindow)
        {
            if (toolWindow.DockPanel == null)
                DockPanel.AddContent(toolWindow);
            else
                DockPanel.RemoveContent(toolWindow);
        }

        public void Goto(string tagFilename, int tagLineStart, bool isBase)
        {
            var text = GetTextEditor(tagFilename, isBase);

            text.ActiveTextAreaControl.CenterViewOn(tagLineStart, 0);
            text.ActiveTextAreaControl.Caret.Line = tagLineStart;
            fileOverview.UpdateTreeSelection(tagFilename, tagLineStart);
            text.ActiveTextAreaControl.TextArea.Focus();
            fileOverview.Invalidate();
        }

        public CK3ScriptEd()
        {
            Instance = this;
            InitializeComponent();

            ScopeManager.Instance.LoadScopeDefinitions("Scopes.xml");
            ScopeManager.Instance.LoadConditionDefinitions("Conditions.xml");
            Core.Instance.Init();

            Core.Instance.LoadMod("LemmyBalanceMod");

            Application.AddMessageFilter(new ControlScrollFilter());
            // Add the dock content drag message filter to handle moving dock content around.
            Application.AddMessageFilter(DockPanel.DockContentDragFilter);

            // Add the dock panel message filter to filter through for dock panel splitter
            // input before letting events pass through to the rest of the application.
            Application.AddMessageFilter(DockPanel.DockResizeFilter);


            fileOverview = new FileOverviewToolWindow();
            soExplorer = new ScriptObjectExplorer();
            projectExplorer = new ProjectExplorer();

            //              DockPanel.AddContent(_dockHistory, _dockLayers.DockGroup);


            _toolWindows.Add(projectExplorer);
            _toolWindows.Add(soExplorer);
            _toolWindows.Add(fileOverview);

            soExplorer.UpdateScriptExplorer();

            // Add the tool window list contents to the dock panel
            foreach (var toolWindow in _toolWindows)
                DockPanel.AddContent(toolWindow);
            DockPanel.AddContent(soExplorer, projectExplorer.DockGroup);

            DockPanel.ActiveContentChanged += DockPanel_ActiveContentChanged;

            DockPanel.ContentRemoved += DockPanel_ContentRemoved;
            // Add the history panel to the layer panel group
            //     DockPanel.AddContent(_dockHistory, _dockLayers.DockGroup);




            XmlDocument d = new XmlDocument();
/*            {
           
            d.InnerXml = "<Scopes></Scopes>";

            for(int n=0;n<(int)ScopeType.max;n++)
            {
                if(!ScopeManager.Instance.Defs.ContainsKey((ScopeType)n))
                continue;

                var l = ScopeManager.Instance.Defs[(ScopeType)n].ValidConditionScopes.Values.OrderBy(a => a.text).ToList();

                foreach (var instanceKnownCharacterEffectScope in l)
                {
                    var fe = (XmlElement)d.FirstChild;

                    fe.InnerXml += "<Scope name=\"" + instanceKnownCharacterEffectScope.text + "\" from=\""+((ScopeType)n).ToString()+"\" to=\"\" type=\"condition\"/>";

                }


            }
            d.Save("Scopes2.xml");
                 
            }
*/
            /*
            d.InnerXml = "<Conditions></Conditions>";

            for (int n = 0; n < (int)ScopeType.max; n++)
            {
                if (!ScopeManager.Instance.Defs.ContainsKey((ScopeType)n))
                    continue;

                var l = ScopeManager.Instance.Defs[(ScopeType)n].ValidConditions.OrderBy(a => a).ToList();

                foreach (var instanceKnownCharacterEffectScope in l)
                {
                    var fe = (XmlElement)d.FirstChild;

                    fe.InnerXml += "<Condition name=\"" + instanceKnownCharacterEffectScope + "\" validscope=\"" + ((ScopeType)n).ToString() + "\"/>";

                }


            }
            d.Save("Conditions2.xml");
            */

            var fsmProvider = new FileSyntaxModeProvider("./");

            HighlightingManager.Manager.AddSyntaxModeFileProvider(fsmProvider); // Attach to the text editor.

            
        }

        private void DockPanel_ContentRemoved(object sender, DockContentEventArgs e)
        {
            if (e.Content is ScriptWindow)
            {
                var sw = e.Content as ScriptWindow;
                this.baseTextEditors.Remove(sw.Filename);
                this.modTextEditors.Remove(sw.Filename);
                this.openBaseScriptWindows.Remove(sw.Filename);
                this.openModScriptWindows.Remove(sw.Filename);
            }
        }

        private void DockPanel_ActiveContentChanged(object sender, DockContentEventArgs e)
        {
            if (DockPanel.ActiveContent is ScriptWindow)
            {
                (DockPanel.ActiveContent as ScriptWindow).Activate();
            }
        }

        public void LoadCK3File(string filename)
        {
            bool fromBase = JominiParse.Core.Instance.LoadCK3File(filename);

            CurrentFile = filename;

            ICSharpCode.TextEditor.TextEditorControl scriptTextFile = GetTextEditor(filename, fromBase);

            fileOverview.UpdateTree(filename, scriptTextFile.ActiveTextAreaControl.Caret.Line, fromBase);

        }
        public void LoadCK3File(string filename, bool forceBase)
        {
            bool fromBase = JominiParse.Core.Instance.LoadCK3File(filename, forceBase);

            CurrentFile = filename;

            ICSharpCode.TextEditor.TextEditorControl scriptTextFile = GetTextEditor(filename, fromBase);

            fileOverview.UpdateTree(filename, scriptTextFile.ActiveTextAreaControl.Caret.Line, fromBase);

        }

        private TextEditorControl GetTextEditor(string filename, bool fromBase = true)
        {
            var textEditors = fromBase ? baseTextEditors : modTextEditors;
            var openScriptWindows = fromBase ? openBaseScriptWindows : openModScriptWindows;

            if (textEditors.ContainsKey(filename))
            {
                DockPanel.ActiveContent = openScriptWindows[filename];

                CurrentFile = filename;
                return textEditors[filename];
            }

            CurrentFile = filename;
            AddTab(filename, fromBase);
         //   fileOverview.UpdateTree(filename, textEditors[filename].ActiveTextAreaControl.Caret.Line);
            return textEditors[filename];
        }

        public void AddTab(string filename, bool fromBase)
        {
            var textEditors = fromBase ? baseTextEditors : modTextEditors;
            var openScriptWindows = fromBase ? openBaseScriptWindows : openModScriptWindows;

            ScriptWindow window = new ScriptWindow(fromBase);
            
            openScriptWindows[filename] = window;
            DockPanel.AddContent(window);
            string startDir = fromBase ? Globals.CK3Path : Core.Instance.ModCK3Library.Path;//"D:/SteamLibrary/steamapps/common/Crusader Kings III/";
            window.Filename = filename;
            AllowUpdateFile = false;
            textEditors[filename] = window.LoadFile(startDir+filename);
            AllowUpdateFile = true;
            fileOverview.UpdateTree(filename, textEditors[filename].ActiveTextAreaControl.Caret.Line, fromBase);


        }

        public bool AllowUpdateFile { get; set; }

        public IHighlightingStrategy HighlightStrategy { get; set; }

        public string CurrentFile { get; set; }

        private void windowToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
