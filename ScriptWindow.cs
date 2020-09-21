using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Docking;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using JominiParse;
using ContextMenu = ICSharpCode.TextEditor.ContextMenu;

namespace CK3ScriptEditor
{
    public partial class ScriptWindow : DarkToolWindow
    {
        public bool IsBaseFile = false;
        public string FullFilename;
        public bool IgnoredFirstDirty = false;

        private LocalizationEditor LocalEditor;
        public TextEditorControl Editor
        {
            get { return textEditorControl1; }
        }

        public void CloseLocalization()
        {
            LocalEditor.Close();
            LocalEditor = null;
        }
        public ScriptWindow(bool fromBase)
        {
            IsBaseFile = fromBase;
            DockArea = DarkDockArea.Document;

         
            InitializeComponent();
            textEditorControl1.ShowVRuler = false;
            textEditorControl1.Font = new System.Drawing.Font(
                "Courier New",
                12F,
                System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point,
                ((byte)(0)));

           

            textEditorControl1.ActiveTextAreaControl.Caret.PositionChanged += CaretOnPositionChanged;
            textEditorControl1.Document.DocumentChanged += ActiveTextAreaControlOnTextChanged;
            textEditorControl1.Document.TextContentChanged += ActiveTextAreaControlOnTextChanged;
            textEditorControl1.ActiveTextAreaControl.TextArea.KeyEventHandler += TextArea_KeyEventHandler;
            textEditorControl1.ActiveTextAreaControl.TextArea.DoProcessDialogKey += TextArea_DoProcessDialogKey;
            textEditorControl1.ActiveTextAreaControl.TextArea.KeyPress +=TextAreaOnKeyPress;

            textEditorControl1.ActiveTextAreaControl.TextArea.MouseDoubleClick += TextAreaOnMouseDoubleClick;
            textEditorControl1.ActiveTextAreaControl.TextArea.KeyDown += TextAreaOnKeyDown;

            var cm = ((ContextMenu) textEditorControl1.ActiveTextAreaControl.ContextMenuStrip);

            cm.ContextMenuShown += CmOnContextMenuShown;

        }

        private void TextAreaOnMouseDoubleClick(object sender, MouseEventArgs e)
        {
            int l = textEditorControl1.ActiveTextAreaControl.Caret.Line;
            int c = textEditorControl1.ActiveTextAreaControl.Caret.Column;

            var inside = GetInside();


            if (inside == null)
                return;

            if (inside.lhsSchema != null)
            {
                if (inside.lhsSchema.TypeList.Contains("localized"))
                {
                    if (LocalEditor == null)
                    {
                        LocalEditor = new LocalizationEditor();
                        LocalEditor.TopMost = true;
                        LocalEditor.Location = textEditorControl1.ActiveTextAreaControl.Caret.ScreenPosition;
                        Point location = textEditorControl1.PointToScreen(new Point(0, LocalEditor.Location.Y));
                        Point location2 = CK3ScriptEd.Instance.PointToScreen(new Point(0, 0));
                        location.X = location2.X;
                        location.Y += textEditorControl1.ActiveTextAreaControl.TextArea.TextView.FontHeight + 4;
                        location.X -= CK3ScriptEd.Instance.Location.X;
                        location.Y -= CK3ScriptEd.Instance.Location.Y;
                        LocalEditor.Location = location;//new Point(location.X + location.X, location.Y + location.Y);

                        LocalEditor.Width = CK3ScriptEd.Instance.Width-20;
                        var str = inside.GetStringValue();

                        if (str != null)
                        {
                            string localized = Core.Instance.GetLocalizedText(str);
                            LocalEditor.Set(this, str, localized);
                        }

                        if (LocalEditor.ShowDialog(CK3ScriptEd.Instance) == DialogResult.OK)
                        {
                            if (!Core.Instance.ModCK3Library.HasLocalizedText(LocalEditor.LocalizedTag, false))
                            {
                                // we don't have this to edit in the mod, so pop up the dialog to create a new localization file...
                                NewLocalizationFileObjectDialog dlg = new NewLocalizationFileObjectDialog();

                                dlg.Init();
                                if (dlg.ShowDialog(CK3ScriptEd.Instance) == DialogResult.OK)
                                {
                                    CreateLocalizationFile(dlg.ChosenFilename, LocalEditor.LocalizedTag, LocalEditor.localTextEdit.Document.TextContent.Replace("\n", "\\n"));
                                }
                            }
                            else
                            {
                                EditLocalizationFile(LocalEditor.LocalizedTag, LocalEditor.localTextEdit.Document.TextContent.Replace("\n", "\\n"));
                            }


                 
                        }
                        LocalEditor = null;
                    }
                }
            }
        }

        private void EditLocalizationFile(string tag, string str)
        {
            var f = Core.Instance.ModCK3Library.LocalizationParser.LocalizationFiles.Where(a => a.Value.Any(b=>b.tag==tag)).ToList();

            if(f.Any())
            {
                string file = f.First().Key;
                string dir = file;

                dir = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/";
                dir += "localization/english/";
                dir += file;
                string text = System.IO.File.ReadAllText(dir);
                text = text.Replace("\r", "");

                var lines = text.Split(new char[] { '\n' }).ToList();

                for (var index = 0; index < lines.Count; index++)
                {
                    var line = lines[index];
                    if (line.StartsWith(tag))
                    {
                        line = tag + ":0 \"" + str + "\"";
                        lines[index] = line;
                    }
                }

                using (FileStream fs = new FileStream(dir, FileMode.Create))
                {
                    // create a new file....
                    using (StreamWriter outputFile = new StreamWriter(fs, Encoding.UTF8))
                    {

                        foreach (string line in lines)
                            if (line.Trim().Length > 0)
                                outputFile.WriteLine(line.Replace("\r", ""));
                    }
                }
                string startDir = Core.Instance.ModCK3Library.Path; //"D:/SteamLibrary/steamapps/common/Crusader Kings III/";
                Core.Instance.ModCK3Library.LoadLocalizations(startDir + "localization/english");
                UpdateDatabase();
                CK3ScriptEd.Instance.UpdateAllWindows();
            }
        }

        private void CreateLocalizationFile(string filename, string tag, string str)
        {
            string dir = filename;
         
            dir = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/";
            dir += "localization/english/";
           
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            filename = dir + filename;
            if (!File.Exists(filename))
            {
                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    using (System.IO.TextWriter writeFile = new StreamWriter(fs, Encoding.UTF8))
                    {
                        writeFile.Write("l_english:\n");
                        writeFile.Write(tag+":0 \"" + str + "\"\n");
                    }
                }

                filename = filename.Replace(Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/", "");
                
                string startDir = Core.Instance.ModCK3Library.Path; //"D:/SteamLibrary/steamapps/common/Crusader Kings III/";

                Core.Instance.ModCK3Library.LoadLocalizations(startDir + "localization/english");
                UpdateDatabase();

            }
            else
            {
                string text = System.IO.File.ReadAllText(filename);
                text = text.Replace("\r", "");
                var lines = text.Split(new char[] { '\n' }).ToList();

                {
                    lines.Add(tag + ":0 \"" + str + "\"\n");

                }

                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    // create a new file....
                    using (StreamWriter outputFile = new StreamWriter(fs, Encoding.UTF8))
                    {

                        foreach (string line in lines)
                        {
                            if(line.Trim().Length > 0)
                                outputFile.WriteLine(line);
                        }
                    }
                }
                string startDir = Core.Instance.ModCK3Library.Path; //"D:/SteamLibrary/steamapps/common/Crusader Kings III/";
                Core.Instance.ModCK3Library.LoadLocalizations(startDir + "localization/english");
                UpdateDatabase();
                CK3ScriptEd.Instance.UpdateAllWindows();
            }
        }

        public void Save()
        {
            if (!IsBaseFile)
            {
                textEditorControl1.SaveFile(FullFilename);
                Dirty = false;

            }

        }
        private void TextAreaOnKeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.S)
            {
                if (e.Control)
                {
                    textEditorControl1.SaveFile(FullFilename);
                    Dirty = false;
                }
            
                // intellisense...
            }
            if (e.KeyCode == Keys.Tab && e.Control)
            {
                if (CK3ScriptEd.Instance.OpenScriptWindows.Count > 1)
                {

                    DoCloseIntellisense();
            
                    if (CK3ScriptEd.Instance.TabOpenDlg == null || CK3ScriptEd.Instance.TabOpenDlg.IsDisposed)
                        CK3ScriptEd.Instance.TabOpenDlg = new TabOpenWindowsDlg();

                    if (!CK3ScriptEd.Instance.TabOpenDlg.Visible)
                    {
                        CK3ScriptEd.Instance.TabOpenDlg.Show(CK3ScriptEd.Instance);
                        CK3ScriptEd.Instance.TabOpenDlg.Fill();
                        CK3ScriptEd.Instance.TabOpenDlg.Focus();

                        Point location = PointToScreen(Location);

                        location.X += textEditorControl1.Size.Width / 2;
                        location.Y += textEditorControl1.Size.Height / 2;

                        location.X -= CK3ScriptEd.Instance.TabOpenDlg.Size.Width / 2;
                        location.Y -= CK3ScriptEd.Instance.TabOpenDlg.Size.Height / 2;

                        CK3ScriptEd.Instance.TabOpenDlg.Location = new Point(location.X, location.Y);

                    }
                }
             
            }
        }

        private void TextAreaOnKeyPress(object sender, KeyPressEventArgs e)
        {
            if ((Control.ModifierKeys & Keys.Control) != 0)
            {
                if (e.KeyChar == '\u0006')
                {
                    if(CK3ScriptEd.Instance.Find == null || CK3ScriptEd.Instance.Find.IsDisposed)
                        CK3ScriptEd.Instance.Find = new BasicFind(this);

                    if (!CK3ScriptEd.Instance.Find.Visible)
                    {
                        CK3ScriptEd.Instance.Find.Show(CK3ScriptEd.Instance);

                        Point location = CK3ScriptEd.Instance.PointToScreen(Location);

                        CK3ScriptEd.Instance.Find.Location = new Point(location.X + (textEditorControl1.Width - (CK3ScriptEd.Instance.Find.Size.Width)), location.Y);

                    }

                    if (textEditorControl1.ActiveTextAreaControl.SelectionManager.SelectedText != null)
                        CK3ScriptEd.Instance.Find.FindText.Text = textEditorControl1.ActiveTextAreaControl.SelectionManager.SelectedText;

                       // intellisense...
                }
            }
        }

        public void RemoveEventHandlers()
        {
            var cm = ((ContextMenu)textEditorControl1.ActiveTextAreaControl.ContextMenuStrip);
            cm.ContextMenuShown -= CmOnContextMenuShown;
         }

        private void CmOnContextMenuShown(object sender, ContextMenu.ContextMenuCreateEventArgs e)
        {
            var inside = GetInside();

            if (inside != null)
            {
                while (inside.Parent != null)
                {
                    inside = inside.Parent;
                }
                if (Core.Instance.BaseCK3Library.Has(inside) && !inside.ScriptFile.IsBase)
                {
                    // already have it in mod... go to...
                    ToolStripMenuItem m = new ToolStripMenuItem("Goto base data definition of: \"" + inside.Name + "\"");
                    m.Tag = new List<object>() { "gotoBase", inside };
                    e.contextMenu.Items.Insert(0, m);
                    e.contextMenu.ItemClicked += ContextMenu_ItemClicked;
                }
                else
                if (Core.Instance.ModCK3Library.Has(inside) && inside.ScriptFile.IsBase)
                {
                    // already have it in mod... go to...
                    ToolStripMenuItem m = new ToolStripMenuItem("Goto \"" + Core.Instance.ModCK3Library.Name + "\" definition of: \"" + inside.Name + "\"");
                    m.Tag = new List<object>() { "gotoMod", inside };
                    e.contextMenu.Items.Insert(0, m);
                    e.contextMenu.ItemClicked += ContextMenu_ItemClicked;
                }
                else if(inside.ScriptFile.IsBase)
                {
                    ToolStripMenuItem m = new ToolStripMenuItem("Clone \"" + inside.Name + "\" ("+inside.Context+ ") to \"" + Core.Instance.ModCK3Library.Name + "\"");
                    m.Tag = new List<object>() { "clone", inside };
                    e.contextMenu.Items.Insert(0, m);
                    e.contextMenu.ItemClicked += ContextMenu_ItemClicked;
                }
            }
            //ToolStripMenuItem m = new ToolStripMenuItem("Test");
            //e.contextMenu.Items.Insert(0, m);
        }

        private void ContextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (!(e.ClickedItem.Tag is List<object>))
                return;

            List<object> prms = (List<object>) (e.ClickedItem.Tag);

            string command = prms[0].ToString();

            if (command == "clone")
            {
                ScriptObject toClone = prms[1] as ScriptObject;

                ScriptCloningManager.Instance.Clone(toClone);
            }

            if (command == "gotoBase")
            {
                ScriptObject toClone = prms[1] as ScriptObject;

                var got = Core.Instance.Get(toClone.Context, toClone.Name,  true);

                CK3ScriptEd.Instance.Goto(got.Filename, got.LineStart-1, true);
            }

            if (command == "gotoMod")
            {
                ScriptObject toClone = prms[1] as ScriptObject;
                var got = Core.Instance.Get(toClone.Context, toClone.Name, false);
                CK3ScriptEd.Instance.Goto(got.Filename, got.LineStart - 1, false);
            }
            var cm = ((ContextMenu)textEditorControl1.ActiveTextAreaControl.ContextMenuStrip);
            cm.ItemClicked -= ContextMenu_ItemClicked;
        }

        private void ActiveTextAreaControlOnTextChanged(object sender, EventArgs e)
        {
           UpdateDatabase();
            //DoIntellisense();
            if(!IgnoredFirstDirty)
                Dirty = true;

        }

        public bool Dirty
        {
            get => _dirty;
            set
            {
                if (value == true && !_dirty)
                {
                    DockText = "* " + DockText;
                }
                if (value == false && _dirty)
                {
                    if(DockText.StartsWith("*"))
                        DockText = DockText.Substring(2);
                }

                _dirty = value;

                

            }
        }

        private bool TextArea_DoProcessDialogKey(Keys keyData)
        {
            if (CurrentIntellisense != null && (keyData == Keys.Escape))
            {
                DoCloseIntellisense();
                return true;
            }
            if (CurrentIntellisense != null && (keyData == Keys.Return || (keyData == Keys.Tab)))
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
                {
                    for(int x=0;x<10;x++) CurrentIntellisense.SelectUp();
                }
                else
                {
                    for (int x = 0; x < 10; x++) CurrentIntellisense.SelectDown();

                }
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

            if ((Control.ModifierKeys & Keys.Control) != 0)
            {
                if (ch == 's')
                {
                    textEditorControl1.SaveFile(FullFilename);
                    Dirty = false;
                    // intellisense...
                    return true;
                }
            }
          
            if ((Control.ModifierKeys & Keys.Control) != 0)
            {
                if (ch == ' ')
                {
                    ForceDoIntellisense();
                    // intellisense...
                    return true;
                }
            }



            return false;
        }


        private bool FlaggedCloseIntellisense;
        public void DoCloseIntellisense()
        {
            if(FlaggedCloseIntellisense)
            {
                if (CurrentIntellisense != null)
                {
                    CurrentIntellisense.Close();
                    CurrentIntellisense.Dispose();
                    CurrentIntellisense = null;
                //    textEditorControl1.ActiveTextAreaControl.TextArea.Select();
                    textEditorControl1.ActiveTextAreaControl.TextArea.Focus();
                }

            }
            FlaggedCloseIntellisense = false;
        }
        public void CloseIntellisense()
        {
            FlaggedCloseIntellisense = true;
        }

        private void ForceDoIntellisense()
        {     //  if(this.CurrentIntellisense != null)

            DoIntellisense(true);
        }

        public void UpdateDatabase()
        {
            if (Filename == null)
                return;
            if (CK3ScriptEd.Instance.AllowUpdateFile)
            {
                Core.Instance.UpdateFile(Filename, this.textEditorControl1.Document.TextContent);
                SyntaxHighlightingManager.Instance.DoDocument(this.textEditorControl1.Document, backgroundColor, Core.Instance.GetFile(Filename));

                CK3ScriptEd.Instance.fileOverview.UpdateTree(Filename, textEditorControl1.ActiveTextAreaControl.Caret.Line, IsBaseFile);


            }
        }
        private void DoIntellisense(bool force=false, bool onlyUpdate=false)
        {
        
            if(CurrentIntellisense != null)
            {
                CurrentIntellisense.Force = true;
                ScriptObject inside = GetInside();
                var i = CurrentIntellisense;
                if (i.FillIntellisense(this, textEditorControl1,
                    textEditorControl1.Document.GetText(
                        textEditorControl1.Document.GetLineSegment(textEditorControl1.ActiveTextAreaControl.Caret.Line)),
                    textEditorControl1.ActiveTextAreaControl.Caret.Column, inside))
                {
                    //textEditorControl1.ActiveTextAreaControl.TextArea.Select();
                    textEditorControl1.ActiveTextAreaControl.TextArea.Focus();

                }
                else
                {
                    DoCloseIntellisense();
                }

                return;
            }
            if (onlyUpdate)
                return;

            {
                CloseIntellisense();

                IntellisenseDlg i = new IntellisenseDlg();

                this.CurrentIntellisense = i;
                i.Force = force;
                i.TopMost = true;
                i.KeyPress += IntellisenseKeyPress;
                i.suggestionListbox.KeyPress += IntellisenseKeyPress;
                //  i.Location = new Point(100, 100);
                //CK3ScriptEd.Instance.Controls.Add(CurrentIntellisense);
                //   CurrentIntellisense.Show();
                lineForIntellisense = textEditorControl1.ActiveTextAreaControl.Caret.Line;

                ScriptObject inside = GetInside();

                if (i.FillIntellisense(this, textEditorControl1,
                    textEditorControl1.Document.GetText(
                        textEditorControl1.Document.GetLineSegment(textEditorControl1.ActiveTextAreaControl.Caret.Line)),
                    textEditorControl1.ActiveTextAreaControl.Caret.Column, inside))
                {
                    i.Show(textEditorControl1.ActiveTextAreaControl);
                    i.Location = textEditorControl1.ActiveTextAreaControl.Caret.ScreenPosition;
                    Point location = textEditorControl1.PointToScreen(i.Location);
                    location.Y += textEditorControl1.ActiveTextAreaControl.TextArea.TextView.FontHeight + 4;
                    i.Location = location;//new Point(location.X + location.X, location.Y + location.Y);

                   // textEditorControl1.ActiveTextAreaControl.TextArea.Select();
                    textEditorControl1.ActiveTextAreaControl.TextArea.Focus();

                }
                else
                {
                    DoCloseIntellisense();
                }
            }
        }

        public ScriptObject GetInside()
        {
            var line = textEditorControl1.ActiveTextAreaControl.Caret.Line + 1;

            var file = Core.Instance.GetFile(Filename.Replace("\\", "/"));
            ScriptObject best = null;
            foreach (var scriptObject in file.Map.Values)
            {
                if (scriptObject.LineStart <= line && scriptObject.LineEnd >= line)
                {
                    var res = GetInside(scriptObject);

                    if(res != null)
                        CK3ScriptEd.Instance.detailsExplorer.SetObject(res);

                    return res;
                }
            }

            return null;
        }
        private ScriptObject GetInside(ScriptObject parent)
        {
            var line = textEditorControl1.ActiveTextAreaControl.Caret.Line+1;

         
            foreach (var scriptObject in parent.Children)
            {
                if (scriptObject.LineStart <= line && scriptObject.LineEnd >= line)
                {
                    var res = GetInside(scriptObject);

                    if (res != scriptObject)
                        return res;

                    return scriptObject;
                }
            }

            return parent;
        }


        private void IntellisenseKeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        public int lineForIntellisense;
        private Color backgroundColor;
        private bool _dirty;
        public IntellisenseDlg CurrentIntellisense { get; set; }

        private void CaretOnPositionChanged(object sender, EventArgs e)
        {
            if (IsLocalization)
                return;

            if (Filename != null)
            {
                CK3ScriptEd.Instance.fileOverview.UpdateTreeSelection(Filename, textEditorControl1.ActiveTextAreaControl.Caret.Line);

                try
                {
                    DoIntellisense(false, false);
                }
                catch (Exception exception)
                {
                    DoCloseIntellisense();
                }
                

                if (lineForIntellisense != textEditorControl1.ActiveTextAreaControl.Caret.Line)
                {
                    if (CurrentIntellisense != null)
                    {
                        CloseIntellisense();
                    }
                }
            }

        }

        public void UpdateLocalizations()
        {
            textEditorControl1.Document.LocalizationMap.Clear();

            var file = ScriptFile;
            /*
            foreach (var scriptObject in file.Map)
            {
                if(scriptObject.Value.Schema != null)
                {
                    var l = scriptObject.Value.Schema.children.Where(a => a.Value.type == "localized").ToList();

                    foreach (var keyValuePair in l)
                    {
                        var res = scriptObject.Value.Children.Where(a => a.Name == keyValuePair.Key).ToList();

                        foreach (var re in res)
                        {
                            //
                            if (re is LocalizedString)
                            {

                            }
                            if(re is ScriptValue)
                            {
                                string val = (re as ScriptValue).GetStringValue();

                                if(!string.IsNullOrEmpty(val))
                                {
                                    textEditorControl1.Document.LocalizationMap[re.LineStart-1] = Core.Instance.GetLocalizedText(val);
                                }
                            }
                        }
                    }
                }
            }*/
            textEditorControl1.Invalidate();
            textEditorControl1.ActiveTextAreaControl.Invalidate();
            textEditorControl1.ActiveTextAreaControl.TextArea.Invalidate();
        }

        public override string ToString()
        {
            return DockText;
        }

        public string Filename { get; set; }
        public ScriptFile ScriptFile { get; set; }

        public TextEditorControl LoadFile(string filename)
        {
            if (filename.EndsWith(".yml"))
            {
                IsLocalization = true;
          }

            IgnoredFirstDirty = true;
            if(File.Exists(filename))
            {
                textEditorControl1.LoadFile(filename);

                if (IsBaseFile)
                {
                    Name = Text = DockText = "Base: " + filename.Substring(filename.LastIndexOf("/") + 1) + " (Read-Only)";
                    textEditorControl1.Document.ReadOnly = true;
                    textEditorControl1.Document.HighlightingStrategy = new DefaultHighlightingStrategy("Default", true);//textEditorControl1.Document.HighlightingStrategy
                    var d = (textEditorControl1.Document.HighlightingStrategy as DefaultHighlightingStrategy);
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


                        foreach (var lineSegment in textEditorControl1.Document.LineSegmentCollection)
                        {
                            foreach (var lineSegmentWord in lineSegment.Words)
                            {
                                if (lineSegmentWord.SyntaxColor != null)
                                {
                                    lineSegmentWord.SyntaxColor = new HighlightColor(lineSegmentWord.SyntaxColor.Color, lineSegmentWord.SyntaxColor.BackgroundColor, lineSegmentWord.SyntaxColor.Bold, lineSegmentWord.SyntaxColor.Italic);
                                    lineSegmentWord.SyntaxColor.BackgroundColor = c;
                                }
                            }
                        }

                        if (!IsLocalization)
                            SyntaxHighlightingManager.Instance.DoDocument(textEditorControl1.Document, backgroundColor, ScriptFile);

                    }

                }
                else
                {
                    Name = Text = DockText = "Mod: " + filename.Substring(filename.LastIndexOf("/") + 1);

                    textEditorControl1.Document.ReadOnly = false;
                    // textEditorControl1.Document.HighlightingStrategy = new DefaultHighlightingStrategy("Default", false);//textEditorControl1.Document.HighlightingStrategy
                    var d = (textEditorControl1.Document.HighlightingStrategy as DefaultHighlightingStrategy);
                    {
                        var c = Color.FromArgb(255, 40, 42, 54);
                        var s = Color.FromArgb(255, 70, 73, 92);
                        var cl = Color.FromArgb(255, 40, 43, 62);

                        backgroundColor = c;

                        foreach (var lineSegment in textEditorControl1.Document.LineSegmentCollection)
                        {
                            foreach (var lineSegmentWord in lineSegment.Words)
                            {
                                if (lineSegmentWord.SyntaxColor != null)
                                {
                                    lineSegmentWord.SyntaxColor = new HighlightColor(lineSegmentWord.SyntaxColor.Color, lineSegmentWord.SyntaxColor.BackgroundColor, lineSegmentWord.SyntaxColor.Bold, lineSegmentWord.SyntaxColor.Italic);
                                    lineSegmentWord.SyntaxColor.BackgroundColor = c;
                                }
                            }
                        }

                    }
                    if (!IsLocalization)
                        SyntaxHighlightingManager.Instance.DoDocument(textEditorControl1.Document, backgroundColor, ScriptFile);
                }
            }
          //  Filename = filename.Substring(filename.LastIndexOf("game/") + 5);
        
            IgnoredFirstDirty = false;
            return textEditorControl1;
        }

        public bool IsLocalization { get; set; }


        public void Activate()
        {

            if (CK3ScriptEd.Instance.Find != null)
                CK3ScriptEd.Instance.Find.Window = this;

            if (Filename != null)
            {
                CK3ScriptEd.Instance.fileOverview.UpdateTree(Filename, textEditorControl1.ActiveTextAreaControl.Caret.Line, IsBaseFile);

                if (ScriptFile!=null)
                    UpdateLocalizations();
            }

            CK3EditorPreferencesManager.Instance.Save();

            CK3ScriptEd.Instance.OpenScriptWindows.Remove(this);
            CK3ScriptEd.Instance.OpenScriptWindows.Insert(0, this);


        }

        private void textEditorControl1_Click(object sender, EventArgs e)
        {
            if (Filename != null)
                CK3ScriptEd.Instance.fileOverview.UpdateTree(Filename, textEditorControl1.ActiveTextAreaControl.Caret.Line, IsBaseFile);

        }

        private void textEditorControl1_KeyUp(object sender, KeyEventArgs e)
        {
            if (Filename != null)
                CK3ScriptEd.Instance.fileOverview.UpdateTree(Filename, textEditorControl1.ActiveTextAreaControl.Caret.Line, IsBaseFile);

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
                textEditorControl1.SaveFile(FullFilename);
                Dirty = false;
                return true;
            }

            if (res == DialogResult.Cancel)
            {
                return false;
            }

            if (res == DialogResult.No)
            {
                return true;
            }

            return false;
        }

    }
}
