using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
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
        bool IsBaseFile = false;
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
            textEditorControl1.Document.LineCountChanged += ActiveTextAreaControlOnTextChanged;
            textEditorControl1.Document.TextContentChanged += ActiveTextAreaControlOnTextChanged;
            textEditorControl1.ActiveTextAreaControl.TextArea.KeyEventHandler += TextArea_KeyEventHandler;
            textEditorControl1.ActiveTextAreaControl.TextArea.DoProcessDialogKey += TextArea_DoProcessDialogKey;

            var cm = ((ContextMenu) textEditorControl1.ActiveTextAreaControl.ContextMenuStrip);

            cm.ContextMenuShown += CmOnContextMenuShown;

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
                if (Core.Instance.ModCK3Library.Has(inside) && !inside.ScriptFile.IsBase)
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
                else
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

                var got = Core.Instance.Get(toClone.Name, toClone.Context, true);

                CK3ScriptEd.Instance.Goto(got.Filename, got.LineStart-1, true);
            }

            if (command == "gotoMod")
            {
                ScriptObject toClone = prms[1] as ScriptObject;
                var got = Core.Instance.Get(toClone.Name, toClone.Context, false);
                CK3ScriptEd.Instance.Goto(got.Filename, got.LineStart - 1, false);
            }
            var cm = ((ContextMenu)textEditorControl1.ActiveTextAreaControl.ContextMenuStrip);
            cm.ItemClicked -= ContextMenu_ItemClicked;
        }

        private void ActiveTextAreaControlOnTextChanged(object sender, EventArgs e)
        {
           UpdateDatabase();
            //DoIntellisense();
        }

        private bool TextArea_DoProcessDialogKey(Keys keyData)
        {
            if (CurrentIntellisense != null && (keyData == Keys.Escape))
            {
                DoCloseIntellisense();
                return true;
            }
            if (CurrentIntellisense != null && (keyData == Keys.Return || (keyData == Keys.Tab) || (keyData == Keys.Space)))
            {
                CurrentIntellisense.DoComplete();
                DoCloseIntellisense();
                UpdateDatabase();
                ForceDoIntellisense();
                return true;
            }
          
            if (CurrentIntellisense != null && (keyData == Keys.Up || keyData == Keys.Down))
            {
                if(keyData == Keys.Up)
                    CurrentIntellisense.SelectUp();
                else
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
            if ((Control.ModifierKeys & Keys.Control) != 0)
            {
                if(ch == ' ')
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
            if(CK3ScriptEd.Instance.AllowUpdateFile)
                Core.Instance.UpdateFile(Filename, this.textEditorControl1.Document.TextContent);
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

        private ScriptObject GetInside()
        {
            var line = textEditorControl1.ActiveTextAreaControl.Caret.Line + 1;

            var file = Core.Instance.GetFile(Filename);
            ScriptObject best = null;
            foreach (var scriptObject in file.Map.Values)
            {
                if (scriptObject.LineStart <= line && scriptObject.LineEnd >= line)
                {
                    var res = GetInside(scriptObject);

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
        public IntellisenseDlg CurrentIntellisense { get; set; }

        private void CaretOnPositionChanged(object sender, EventArgs e)
        {
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


        public string Filename { get; set; }

        public TextEditorControl LoadFile(string filename)
        {
          //  Filename = filename.Substring(filename.LastIndexOf("game/") + 5);
            textEditorControl1.LoadFile(filename);
            
            
            if (IsBaseFile)
            {
                DockText = "Base:" + filename.Substring(filename.LastIndexOf("/") + 1) + " (Read-Only)";
                textEditorControl1.Document.ReadOnly = true;
                textEditorControl1.Document.HighlightingStrategy = new DefaultHighlightingStrategy("Default", true);//textEditorControl1.Document.HighlightingStrategy
                var d = (textEditorControl1.Document.HighlightingStrategy as DefaultHighlightingStrategy);
                {
                    var c= Color.FromArgb(255, 54, 42, 40);
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
                    
                    foreach (var lineSegment in textEditorControl1.Document.LineSegmentCollection)
                    {
                        foreach (var lineSegmentWord in lineSegment.Words)
                        {
                            if(lineSegmentWord.SyntaxColor != null)
                                lineSegmentWord.SyntaxColor.BackgroundColor = c;
                        }
                    }
                    

                }

            }
            else
            {
                DockText = "Mod:" + filename.Substring(filename.LastIndexOf("/") + 1);
                textEditorControl1.Document.ReadOnly = false;
               // textEditorControl1.Document.HighlightingStrategy = new DefaultHighlightingStrategy("Default", false);//textEditorControl1.Document.HighlightingStrategy
                var d = (textEditorControl1.Document.HighlightingStrategy as DefaultHighlightingStrategy);
                {
                    var c = Color.FromArgb(255, 40, 42, 54);
                    var s = Color.FromArgb(255, 70, 73, 92);
                    var cl = Color.FromArgb(255, 40, 43, 62);

                    
                    foreach (var lineSegment in textEditorControl1.Document.LineSegmentCollection)
                    {
                        foreach (var lineSegmentWord in lineSegment.Words)
                        {
                            if (lineSegmentWord.SyntaxColor != null)
                                lineSegmentWord.SyntaxColor.BackgroundColor = c;
                        }
                    }
                    
                }
            }
            return textEditorControl1;
        }


        public void Activate()
        {
            if (Filename != null)
            {
                CK3ScriptEd.Instance.fileOverview.UpdateTree(Filename, textEditorControl1.ActiveTextAreaControl.Caret.Line, IsBaseFile);

                if (IsBaseFile)
                {
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
                        
                        foreach (var lineSegment in textEditorControl1.Document.LineSegmentCollection)
                        {
                            if (lineSegment.Words != null)
                            foreach (var lineSegmentWord in lineSegment.Words)
                            {
                                if (lineSegmentWord.SyntaxColor != null)
                                    lineSegmentWord.SyntaxColor.BackgroundColor = c;
                            }
                        }
                        
                    }

                }
                else
                {
                    textEditorControl1.Document.ReadOnly = false;
                  //  textEditorControl1.Document.HighlightingStrategy = new DefaultHighlightingStrategy("Default", false);//textEditorControl1.Document.HighlightingStrategy
                    var d = (textEditorControl1.Document.HighlightingStrategy as DefaultHighlightingStrategy);
                    {
                        var c = Color.FromArgb(255, 40, 42, 54);
                        var s = Color.FromArgb(255, 70, 73, 92);
                        var cl = Color.FromArgb(255, 40, 43, 62);

                        
                        foreach (var lineSegment in textEditorControl1.Document.LineSegmentCollection)
                        {
                            if(lineSegment.Words != null)
                            foreach (var lineSegmentWord in lineSegment.Words)
                            {
                                if (lineSegmentWord.SyntaxColor != null)
                                    lineSegmentWord.SyntaxColor.BackgroundColor = c;
                            }
                        }

                    }
                }
            }

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

    }
}
