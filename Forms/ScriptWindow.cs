#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DarkUI.Docking;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using JominiParse;
using ContextMenu = ICSharpCode.TextEditor.ContextMenu;

#endregion

namespace CK3ScriptEditor
{
    public partial class ScriptWindow : DarkToolWindow
    {
        private bool _dirty;
        private Color backgroundColor;


        private bool FlaggedCloseIntellisense;
        public string FullFilename;
        public bool IgnoredFirstDirty;
        public bool IsBaseFile;

        public int lineForIntellisense;

        private LocalizationEditor LocalEditor;

        public ScriptWindow(bool fromBase)
        {
            AutoScaleMode = AutoScaleMode.Inherit;
            IsBaseFile = fromBase;
            DockArea = DarkDockArea.Document;


            InitializeComponent();
            Editor.ShowVRuler = false;
          

            SetFont();

            Editor.ActiveTextAreaControl.Caret.PositionChanged += CaretOnPositionChanged;
            Editor.Document.DocumentChanged += ActiveTextAreaControlOnTextChanged;
            Editor.Document.TextContentChanged += ActiveTextAreaControlOnTextChanged;
            Editor.ActiveTextAreaControl.TextArea.KeyEventHandler += TextArea_KeyEventHandler;
            Editor.ActiveTextAreaControl.TextArea.DoProcessDialogKey += TextArea_DoProcessDialogKey;
            Editor.ActiveTextAreaControl.TextArea.KeyPress += TextAreaOnKeyPress;

            Editor.ActiveTextAreaControl.TextArea.MouseDoubleClick += TextAreaOnMouseDoubleClick;
            Editor.ActiveTextAreaControl.TextArea.KeyDown += TextAreaOnKeyDown;

            var cm = (ContextMenu) Editor.ActiveTextAreaControl.ContextMenuStrip;

            cm.ContextMenuShown += CmOnContextMenuShown;
        }
        public TextEditorControl Editor { get; private set; }

        public bool Dirty
        {
            get => _dirty;
            set
            {
                if (value && !_dirty) DockText = "* " + DockText;
                if (value == false && _dirty)
                    if (DockText.StartsWith("*"))
                        DockText = DockText.Substring(2);

                _dirty = value;
            }
        }
        public IntellisenseDlg CurrentIntellisense { get; set; }
        public RefFilename Filename { get; set; }
        public ScriptFile ScriptFile { get; set; }

        public bool IsLocalization { get; set; }

        public void CloseLocalization()
        {
            LocalEditor.Close();
            LocalEditor = null;
        }

        private void TextAreaOnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            var l = Editor.ActiveTextAreaControl.Caret.Line;
            var c = Editor.ActiveTextAreaControl.Caret.Column;

            var inside = GetInside();


            if (inside == null)
                return;

            if (inside.lhsSchema != null)
                if (inside.lhsSchema.TypeList.Contains("localized"))
                    if (LocalEditor == null)
                    {
                        LocalEditor = new LocalizationEditor();
                        LocalEditor.TopMost = true;
                        LocalEditor.Location = Editor.ActiveTextAreaControl.Caret.ScreenPosition;
                        var location = Editor.PointToScreen(new Point(0, LocalEditor.Location.Y));
                        var location2 = CK3ScriptEd.Instance.PointToScreen(new Point(0, 0));
                        location.X = location2.X;
                        location.Y += Editor.ActiveTextAreaControl.TextArea.TextView.FontHeight + 4;
                        location.X -= CK3ScriptEd.Instance.Location.X;
                        location.Y -= CK3ScriptEd.Instance.Location.Y;
                        LocalEditor.Location = location; //new Point(location.X + location.X, location.Y + location.Y);

                        LocalEditor.Width = CK3ScriptEd.Instance.Width - 20;
                        var str = inside.GetStringValue();

                        if (str != null)
                        {
                            var localized = Core.Instance.GetLocalizedText(str);
                            LocalEditor.Set(this, str, localized);
                        }

                        if (LocalEditor.ShowDialog(CK3ScriptEd.Instance) == DialogResult.OK)
                        {
                            if (!Core.Instance.ModCK3Library.HasLocalizedText(LocalEditor.LocalizedTag, false))
                            {
                                // we don't have this to edit in the mod, so pop up the dialog to create a new localization file...
                                var dlg = new NewLocalizationFileObjectDialog();

                                dlg.Init();
                                if (dlg.ShowDialog(CK3ScriptEd.Instance) == DialogResult.OK)
                                    CreateLocalizationFile(dlg.ChosenFilename, LocalEditor.LocalizedTag,
                                        LocalEditor.localTextEdit.Document.TextContent.Replace("\n", "\\n"));
                            }
                            else
                            {
                                EditLocalizationFile(LocalEditor.LocalizedTag,
                                    LocalEditor.localTextEdit.Document.TextContent.Replace("\n", "\\n"));
                            }
                        }

                        LocalEditor = null;
                    }
        }

        private void EditLocalizationFile(string tag, string str)
        {
            var f = Core.Instance.ModCK3Library.LocalizationParser.LocalizationFiles
                .Where(a => a.Value.Any(b => b.tag == tag)).ToList();

            if (f.Any())
            {
                var file = f.First().Key;
                var dir = file;

                dir = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/";
                dir += "localization/english/";
                dir += file;
                var text = File.ReadAllText(dir);
                text = text.Replace("\r", "");

                var lines = text.Split('\n').ToList();

                for (var index = 0; index < lines.Count; index++)
                {
                    var line = lines[index];
                    if (line.StartsWith(tag))
                    {
                        line = tag + ":0 \"" + str + "\"";
                        lines[index] = line;
                    }
                }

                using (var fs = new FileStream(dir, FileMode.Create))
                {
                    // create a new file....
                    using (var outputFile = new StreamWriter(fs, Encoding.UTF8))
                    {
                        foreach (var line in lines)
                            if (line.Trim().Length > 0)
                                outputFile.WriteLine(line.Replace("\r", ""));
                    }
                }

                var startDir =
                    Core.Instance.ModCK3Library.Path; 
                Core.Instance.ModCK3Library.LoadLocalizations(startDir + "localization/english");
                if (!Filename.IsBase)
                    UpdateDatabase();
                CK3ScriptEd.Instance.UpdateAllWindows();
            }
        }

        private void CreateLocalizationFile(string filename, string tag, string str)
        {
            var dir = filename;

            dir = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/";
            dir += "localization/english/";

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            filename = dir + filename;
            if (!File.Exists(filename))
            {
                using (var fs = new FileStream(filename, FileMode.Create))
                {
                    using (TextWriter writeFile = new StreamWriter(fs, Encoding.UTF8))
                    {
                        writeFile.Write("l_english:\n");
                        writeFile.Write(tag + ":0 \"" + str + "\"\n");
                    }
                }

                filename = filename.Replace(Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/", "");

                var startDir =
                    Core.Instance.ModCK3Library.Path; 

                Core.Instance.ModCK3Library.LoadLocalizations(startDir + "localization/english");
                UpdateDatabase();
            }
            else
            {
                var text = File.ReadAllText(filename);
                text = text.Replace("\r", "");
                var lines = text.Split('\n').ToList();

                {
                    lines.Add(tag + ":0 \"" + str + "\"\n");
                }

                using (var fs = new FileStream(filename, FileMode.Create))
                {
                    // create a new file....
                    using (var outputFile = new StreamWriter(fs, Encoding.UTF8))
                    {
                        foreach (var line in lines)
                            if (line.Trim().Length > 0)
                                outputFile.WriteLine(line);
                    }
                }

                var startDir =
                    Core.Instance.ModCK3Library.Path; 
                Core.Instance.ModCK3Library.LoadLocalizations(startDir + "localization/english");
                UpdateDatabase();
                CK3ScriptEd.Instance.UpdateAllWindows();
            }
        }

        public void Save()
        {
            if (!IsBaseFile)
            {
                Editor.SaveFile(FullFilename);
                Dirty = false;
            }
        }

        private void TextAreaOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S)
                if (e.Control)
                {
                    Editor.SaveFile(FullFilename);
                    Dirty = false;
                }

            // intellisense...

            if (e.KeyCode == Keys.Tab && e.Control)
                if (CK3ScriptEd.Instance.OpenDocuments.NumDocumentsOpen() > 1)
                {
                    DoCloseIntellisense();

                    if (CK3ScriptEd.Instance.TabOpenDlg == null || CK3ScriptEd.Instance.TabOpenDlg.IsDisposed)
                        CK3ScriptEd.Instance.TabOpenDlg = new TabOpenWindowsDlg();

                    if (!CK3ScriptEd.Instance.TabOpenDlg.Visible)
                    {
                        CK3ScriptEd.Instance.TabOpenDlg.Show(CK3ScriptEd.Instance);
                        CK3ScriptEd.Instance.TabOpenDlg.Fill();
                        CK3ScriptEd.Instance.TabOpenDlg.Focus();

                        var location = PointToScreen(Location);

                        location.X += Editor.Size.Width / 2;
                        location.Y += Editor.Size.Height / 2;

                        location.X -= CK3ScriptEd.Instance.TabOpenDlg.Size.Width / 2;
                        location.Y -= CK3ScriptEd.Instance.TabOpenDlg.Size.Height / 2;

                        CK3ScriptEd.Instance.TabOpenDlg.Location = new Point(location.X, location.Y);
                    }
                }
        }

        private void TextAreaOnKeyPress(object sender, KeyPressEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) != 0)
                if (e.KeyChar == '\u0006')
                {
                    if (CK3ScriptEd.Instance.Find == null || CK3ScriptEd.Instance.Find.IsDisposed)
                        CK3ScriptEd.Instance.Find = new BasicFind(this);

                    if (!CK3ScriptEd.Instance.Find.Visible)
                    {
                        CK3ScriptEd.Instance.Find.Show(CK3ScriptEd.Instance);

                        var location = CK3ScriptEd.Instance.PointToScreen(Location);

                        CK3ScriptEd.Instance.Find.Location =
                            new Point(location.X + (Editor.Width - CK3ScriptEd.Instance.Find.Size.Width), location.Y);
                    }

                    if (Editor.ActiveTextAreaControl.SelectionManager.SelectedText != null)
                        CK3ScriptEd.Instance.Find.FindText.Text =
                            Editor.ActiveTextAreaControl.SelectionManager.SelectedText;

                    // intellisense...
                }
        }

        public void RemoveEventHandlers()
        {
            var cm = (ContextMenu) Editor.ActiveTextAreaControl.ContextMenuStrip;
            cm.ContextMenuShown -= CmOnContextMenuShown;
        }

        private void CmOnContextMenuShown(object sender, ContextMenu.ContextMenuCreateEventArgs e)
        {
            var inside = GetInside();

            if (inside != null)
            {
                while (inside.Parent != null) inside = inside.Parent;
                if (Core.Instance.BaseCK3Library.Has(inside) && !inside.ScriptFile.IsBase)
                {
                    // already have it in mod... go to...
                    var m = new ToolStripMenuItem("Goto base data definition of: \"" + inside.Name + "\"");
                    m.Tag = new List<object> {"gotoBase", inside};
                    e.contextMenu.Items.Insert(0, m);
                    e.contextMenu.ItemClicked += ContextMenu_ItemClicked;
                }
                else if (Core.Instance.ModCK3Library.Has(inside) && inside.ScriptFile.IsBase)
                {
                    // already have it in mod... go to...
                    var m = new ToolStripMenuItem("Goto \"" + Core.Instance.ModCK3Library.Name +
                                                  "\" definition of: \"" + inside.Name + "\"");
                    m.Tag = new List<object> {"gotoMod", inside};
                    e.contextMenu.Items.Insert(0, m);
                    e.contextMenu.ItemClicked += ContextMenu_ItemClicked;
                }
                else if (inside.ScriptFile.IsBase)
                {
                    var m = new ToolStripMenuItem("Clone \"" + inside.Name + "\" (" + inside.Context + ") to \"" +
                                                  Core.Instance.ModCK3Library.Name + "\"");
                    m.Tag = new List<object> {"clone", inside};
                    e.contextMenu.Items.Insert(0, m);
                    e.contextMenu.ItemClicked += ContextMenu_ItemClicked;
                }
            }

        }

        private void ContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (!(e.ClickedItem.Tag is List<object>))
                return;

            var prms = (List<object>) e.ClickedItem.Tag;

            var command = prms[0].ToString();

            if (command == "clone")
            {
                var toClone = prms[1] as ScriptObject;

                ScriptCloningManager.Instance.Clone(toClone);
            }

            if (command == "gotoBase")
            {
                var toClone = prms[1] as ScriptObject;

                var got = Core.Instance.Get(toClone.Context, toClone.Name, true);

                CK3ScriptEd.Instance.Goto(got.Filename, got.LineStart - 1, true);
            }

            if (command == "gotoMod")
            {
                var toClone = prms[1] as ScriptObject;
                var got = Core.Instance.Get(toClone.Context, toClone.Name);
                CK3ScriptEd.Instance.Goto(got.Filename, got.LineStart - 1, false);
            }

            var cm = (ContextMenu) Editor.ActiveTextAreaControl.ContextMenuStrip;
            cm.ItemClicked -= ContextMenu_ItemClicked;
        }

        private void ActiveTextAreaControlOnTextChanged(object sender, EventArgs e)
        {
            UpdateDatabase();
            //DoIntellisense();
            if (!IgnoredFirstDirty)
                Dirty = true;
        }

        private bool TextArea_DoProcessDialogKey(Keys keyData)
        {
            if (CurrentIntellisense != null && keyData == Keys.Escape)
            {
                DoCloseIntellisense();
                return true;
            }

            if (CurrentIntellisense != null && (keyData == Keys.Return || keyData == Keys.Tab))
            {
                CurrentIntellisense.DoComplete();
                DoCloseIntellisense();
                UpdateDatabase();
                ForceDoIntellisense();
                return true;
            }

            if (CurrentIntellisense != null && (keyData == Keys.Up || keyData == Keys.Down))
            {
                if (keyData == Keys.Up)
                    CurrentIntellisense.SelectUp();
                else
                    CurrentIntellisense.SelectDown();
                return true;
            }

            if (CurrentIntellisense != null && (keyData == Keys.PageUp || keyData == Keys.PageDown))
            {
                if (keyData == Keys.PageUp)
                    for (var x = 0; x < 10; x++)
                        CurrentIntellisense.SelectUp();
                else
                    for (var x = 0; x < 10; x++)
                        CurrentIntellisense.SelectDown();
                return true;
            }

            if (CurrentIntellisense != null && (keyData == Keys.Left || keyData == Keys.Right))
            {
                DoCloseIntellisense();
                return false;
            }

            return false;
        }

        private bool TextArea_KeyEventHandler(char ch)
        {
            if ((ModifierKeys & Keys.Control) != 0)
                if (ch == 's')
                {
                    Editor.SaveFile(FullFilename);
                    Dirty = false;
                    // intellisense...
                    return true;
                }

            if ((ModifierKeys & Keys.Control) != 0)
                if (ch == ' ')
                {
                    ForceDoIntellisense();
                    // intellisense...
                    return true;
                }


            return false;
        }

        public void DoCloseIntellisense()
        {
            if (FlaggedCloseIntellisense)
                if (CurrentIntellisense != null)
                {
                    CurrentIntellisense.Close();
                    CurrentIntellisense.Dispose();
                    CurrentIntellisense = null;
                    //    textEditorControl1.ActiveTextAreaControl.TextArea.Select();
                    Editor.ActiveTextAreaControl.TextArea.Focus();
                }

            FlaggedCloseIntellisense = false;
        }

        public void CloseIntellisense()
        {
            FlaggedCloseIntellisense = true;
        }

        private void ForceDoIntellisense()
        {
            //  if(this.CurrentIntellisense != null)

            DoIntellisense(true);
        }

        public void UpdateDatabase()
        {
            if (Filename == null)
                return;
            if (Filename.IsBase)
                return;

            if (CK3ScriptEd.Instance.AllowUpdateFile)
            {
                Core.Instance.UpdateFile(Filename, Editor.Document.TextContent);
                SyntaxHighlightingManager.Instance.DoDocument(Editor.Document, backgroundColor,
                    Core.Instance.GetFile(Filename));

                CK3ScriptEd.Instance.fileOverview.UpdateTree(Filename, Editor.ActiveTextAreaControl.Caret.Line,
                    IsBaseFile);
            }
        }

        private void DoIntellisense(bool force = false, bool onlyUpdate = false)
        {
            if (CurrentIntellisense != null)
            {
                CurrentIntellisense.Force = true;
                var inside = GetInside();
                var i = CurrentIntellisense;
                if (i.FillIntellisense(this, Editor,
                        Editor.Document.GetText(
                            Editor.Document.GetLineSegment(Editor.ActiveTextAreaControl.Caret.Line)),
                        Editor.ActiveTextAreaControl.Caret.Column, inside))
                    //textEditorControl1.ActiveTextAreaControl.TextArea.Select();
                    Editor.ActiveTextAreaControl.TextArea.Focus();
                else
                    DoCloseIntellisense();

                return;
            }

            if (onlyUpdate)
                return;

            {
                CloseIntellisense();

                var i = new IntellisenseDlg();

                CurrentIntellisense = i;
                i.Force = force;
                i.TopMost = true;
                i.KeyPress += IntellisenseKeyPress;
                i.suggestionListbox.KeyPress += IntellisenseKeyPress;
                //  i.Location = new Point(100, 100);
                //CK3ScriptEd.Instance.Controls.Add(CurrentIntellisense);
                //   CurrentIntellisense.Show();
                lineForIntellisense = Editor.ActiveTextAreaControl.Caret.Line;

                var inside = GetInside();

                if (i.FillIntellisense(this, Editor,
                    Editor.Document.GetText(
                        Editor.Document.GetLineSegment(Editor.ActiveTextAreaControl.Caret.Line)),
                    Editor.ActiveTextAreaControl.Caret.Column, inside))
                {
                    i.Show(Editor.ActiveTextAreaControl);
                    i.Location = Editor.ActiveTextAreaControl.Caret.ScreenPosition;
                    var location = Editor.PointToScreen(i.Location);
                    location.Y += Editor.ActiveTextAreaControl.TextArea.TextView.FontHeight + 4;
                    i.Location = location; //new Point(location.X + location.X, location.Y + location.Y);

                    // textEditorControl1.ActiveTextAreaControl.TextArea.Select();
                    Editor.ActiveTextAreaControl.TextArea.Focus();
                }
                else
                {
                    DoCloseIntellisense();
                }
            }
        }

        public ScriptObject GetInside()
        {
            var line = Editor.ActiveTextAreaControl.Caret.Line + 1;

            var file = Core.Instance.GetFile(Filename);
            ScriptObject best = null;
            foreach (var scriptObject in file.Map.Values)
                if (scriptObject.LineStart <= line && scriptObject.LineEnd >= line)
                {
                    var res = GetInside(scriptObject);

                    if (res != null)
                        CK3ScriptEd.Instance.detailsExplorer.SetObject(res);

                    return res;
                }

            return null;
        }

        private ScriptObject GetInside(ScriptObject parent)
        {
            var line = Editor.ActiveTextAreaControl.Caret.Line + 1;


            foreach (var scriptObject in parent.Children)
                if (scriptObject.LineStart <= line && scriptObject.LineEnd >= line)
                {
                    var res = GetInside(scriptObject);

                    if (res != scriptObject)
                        return res;

                    return scriptObject;
                }

            return parent;
        }


        private void IntellisenseKeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void CaretOnPositionChanged(object sender, EventArgs e)
        {
            if (IsLocalization)
                return;

            if (Filename != null)
            {
                CK3ScriptEd.Instance.fileOverview.UpdateTreeSelection(Filename,
                    Editor.ActiveTextAreaControl.Caret.Line);

                try
                {
                    DoIntellisense();
                }
                catch (Exception exception)
                {
                    DoCloseIntellisense();
                }


                if (lineForIntellisense != Editor.ActiveTextAreaControl.Caret.Line)
                    if (CurrentIntellisense != null)
                        CloseIntellisense();
            }
        }

        public void UpdateLocalizations()
        {
            Editor.Document.LocalizationMap.Clear();

            var file = ScriptFile;
      
            Editor.Invalidate();
            Editor.ActiveTextAreaControl.Invalidate();
            Editor.ActiveTextAreaControl.TextArea.Invalidate();
        }

        public override string ToString()
        {
            return DockText;
        }

        public TextEditorControl LoadFile(RefFilename filename)
        {
            if (filename.Extension == ".yml") IsLocalization = true;

            IgnoredFirstDirty = true;
            if (filename.Exists)
            {
                Editor.LoadFile(filename.ToFullWindowsFilename());

                if (IsBaseFile)
                {
                    Name = Text = DockText = "Base: " + filename.Name + " (Read-Only)";
                    Editor.Document.ReadOnly = true;
                    Editor.Document.HighlightingStrategy =
                        new DefaultHighlightingStrategy("Default",
                            true); //textEditorControl1.Document.HighlightingStrategy
                    var d = Editor.Document.HighlightingStrategy as DefaultHighlightingStrategy;
                    {
                        var c = Color.FromArgb(255, 54, 42, 40);
                        var s = Color.FromArgb(255, 92, 73, 70);
                        var cl = Color.FromArgb(255, 62, 43, 40);

                        d.environmentColors["Default"].BackgroundColor = c;
                        d.environmentColors["FoldLine"].BackgroundColor = c;
                        d.environmentColors["VRuler"].BackgroundColor = c;

                        d.environmentColors["Selection"].BackgroundColor = s;
                        d.environmentColors["CaretLine"].BackgroundColor = cl;
                        //  d.environmentColors["VRuler"].BackgroundColor = c;
                        d.DigitColor.BackgroundColor = c;
                        d.DefaultTextColor.BackgroundColor = c;

                        backgroundColor = c;


                        foreach (var lineSegment in Editor.Document.LineSegmentCollection)
                        foreach (var lineSegmentWord in lineSegment.Words)
                            if (lineSegmentWord.SyntaxColor != null)
                            {
                                lineSegmentWord.SyntaxColor = new HighlightColor(lineSegmentWord.SyntaxColor.Color,
                                    lineSegmentWord.SyntaxColor.BackgroundColor, lineSegmentWord.SyntaxColor.Bold,
                                    lineSegmentWord.SyntaxColor.Italic);
                                lineSegmentWord.SyntaxColor.BackgroundColor = c;
                            }

                        if (!IsLocalization)
                            SyntaxHighlightingManager.Instance.DoDocument(Editor.Document, backgroundColor, ScriptFile);
                    }
                }
                else
                {
                    Name = Text = DockText = "Mod: " + filename.Name;

                    Editor.Document.ReadOnly = false;
                    // textEditorControl1.Document.HighlightingStrategy = new DefaultHighlightingStrategy("Default", false);//textEditorControl1.Document.HighlightingStrategy
                    var d = Editor.Document.HighlightingStrategy as DefaultHighlightingStrategy;
                    {
                        var c = Color.FromArgb(255, 40, 42, 54);
                        var s = Color.FromArgb(255, 70, 73, 92);
                        var cl = Color.FromArgb(255, 40, 43, 62);

                        backgroundColor = c;

                        foreach (var lineSegment in Editor.Document.LineSegmentCollection)
                        foreach (var lineSegmentWord in lineSegment.Words)
                            if (lineSegmentWord.SyntaxColor != null)
                            {
                                lineSegmentWord.SyntaxColor = new HighlightColor(lineSegmentWord.SyntaxColor.Color,
                                    lineSegmentWord.SyntaxColor.BackgroundColor, lineSegmentWord.SyntaxColor.Bold,
                                    lineSegmentWord.SyntaxColor.Italic);
                                lineSegmentWord.SyntaxColor.BackgroundColor = c;
                            }
                    }
                    if (!IsLocalization)
                        SyntaxHighlightingManager.Instance.DoDocument(Editor.Document, backgroundColor, ScriptFile);
                }
            }
      
            IgnoredFirstDirty = false;
            return Editor;
        }


        public void Activate()
        {
            if (CK3ScriptEd.Instance.Find != null)
                CK3ScriptEd.Instance.Find.Window = this;

            if (Filename != null)
            {
                CK3ScriptEd.Instance.fileOverview.UpdateTree(Filename, Editor.ActiveTextAreaControl.Caret.Line,
                    IsBaseFile);

                if (ScriptFile != null)
                    UpdateLocalizations();
            }

            CK3EditorPreferencesManager.Instance.Save();

            CK3ScriptEd.Instance.OpenDocuments.MoveToStartOfDocumentList(this);
        }

        private void textEditorControl1_Click(object sender, EventArgs e)
        {
            if (Filename != null)
                CK3ScriptEd.Instance.fileOverview.UpdateTree(Filename, Editor.ActiveTextAreaControl.Caret.Line,
                    IsBaseFile);
        }

        private void textEditorControl1_KeyUp(object sender, KeyEventArgs e)
        {
            if (Filename != null)
                CK3ScriptEd.Instance.fileOverview.UpdateTree(Filename, Editor.ActiveTextAreaControl.Caret.Line,
                    IsBaseFile);
        }

        public bool CheckSave()
        {
            if (!Dirty)
                return true;
            if (IsBaseFile)
                return true;

            var res = MessageBox.Show("Save modified file: " + Filename, "Warning: Unsaved script file.",
                MessageBoxButtons.YesNoCancel);

            if (res == DialogResult.Yes)
            {
                Editor.SaveFile(FullFilename);
                Dirty = false;
                return true;
            }

            if (res == DialogResult.Cancel) return false;

            if (res == DialogResult.No) return true;

            return false;
        }

        public void SetFont()
        {
            switch (EditorGlobals.FontSize)
            {
                case FontSize.Normal:
                    Editor.Font = new Font(
                        "Courier New",
                        12F,
                        FontStyle.Regular,
                        GraphicsUnit.Point,
                        0); break;
                case FontSize.Large:
                    Editor.Font = new Font(
                        "Courier New",
                        14F,
                        FontStyle.Regular,
                        GraphicsUnit.Point,
                        0);
                    break;
            }
        }
    }
}