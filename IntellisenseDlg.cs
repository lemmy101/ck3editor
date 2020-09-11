using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Controls;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;
using JominiParse;

namespace CK3ScriptEditor
{
    public partial class IntellisenseDlg : Form
    {
        
        public IntellisenseDlg()
        {
            InitializeComponent();
        }

        private bool autoCompletePostEquals = false;

        public bool FillIntellisense(ScriptWindow sw, ICSharpCode.TextEditor.TextEditorControl control, string lineSegment, int caretColumn, ScriptObject inside)
        {
            this.ScriptWindow = sw;
            this.Control = control;
            string before = lineSegment.Substring(0, caretColumn);
            string after = lineSegment.Substring(caretColumn);

            if (after.Trim().Length > 0 && (after[0] != ' ' || after[0] != '\t'))
                return false;

            Size = new Size(400, 200);
            int startOfLastword = before.IndexOf(before.Trim());
            this.TabSpacing = before.Substring(0, startOfLastword);
            if (before.Trim().Length == 0)
                TabSpacing = before;
            int endOfLastWord = before.IndexOf(before.Trim()) + before.Trim().Length;
            this.Inside = inside;
            string beforeOriginal = before;
            before = before.Trim();
            List<String> choices = new List<string>();
            if (before.Length == 0 && !Force)
                return false;

            if (before.EndsWith("=") || before.EndsWith("<=") || before.EndsWith(">=") || before.EndsWith("!=") ||
                before.EndsWith("=="))
            {
                before = before.Replace("==", "*");
                before = before.Replace("<=", "*");
                before = before.Replace(">=", "*");
                before = before.Replace("!=", "*");
                before = before.Replace("==", "*");
                before = before.Replace("=", "*");
                
                before = before.Substring(0, before.LastIndexOf("*")).Trim();
                int dif = beforeOriginal.Length - beforeOriginal.TrimEnd(new char[] { ' ' }).Length;

                if (dif == 1)
                {
                    // find appropriate choices for type before =
                    choices = CoreIntellisenseHandler.Instance.GetValidTokensEqual(inside, before);

                    var index = lineSegment.IndexOf(before);

                    StartText = lineSegment.Substring(0, caretColumn);
                    autoCompletePostEquals = true;

                }
            }
            else
            {
                if (endOfLastWord < caretColumn && before.Length > 0)
                    return false;

                choices = CoreIntellisenseHandler.Instance.GetValidTokens(inside, before);

                var index = lineSegment.IndexOf(before);
                
                this.StartText = lineSegment.Substring(0, index);
                if (before.Length == 0)
                    StartText = lineSegment.Substring(0, caretColumn);

                autoCompletePostEquals = false;
            }

            this.suggestionListbox.Items.Clear();
            if (choices.Count == 0)
                return false;

            foreach (var choice in choices)
            {
                var l = new DarkListItem(choice);
                suggestionListbox.Items.Add(l);
            }

            suggestionListbox.SelectItem(0);
            Size = new Size(Size.Width, Math.Min((choices.Count*suggestionListbox.ItemHeight)+5, 10 * suggestionListbox.ItemHeight));

            return true;
        }

        public string TabSpacing { get; set; }

        public ScriptObject Inside { get; set; }

        public ScriptWindow ScriptWindow { get; set; }

        public string StartText { get; set; }

        public TextEditorControl Control { get; set; }
        public bool Force { get; set; }

        private void suggestionListbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void suggestionListbox_SelectedIndicesChanged(object sender, EventArgs e)
        {

        }

        private void suggestionListbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                DoComplete();
            }
            
        }

        public void DoComplete()
        {
            Control.ActiveTextAreaControl.TextArea.Focus();
            var ls = Control.Document.GetLineSegment(Control.ActiveTextAreaControl.Caret.Line);
            var t = suggestionListbox.Items[suggestionListbox.SelectedIndices[0]].Text;
            string tabs = TabSpacing;
            
            var needBraces = CoreIntellisenseHandler.Instance.GetNeedBraces(Inside, t);
            if (!autoCompletePostEquals)
            {
                if (needBraces)
                {
                    t += " = {\n"+ tabs + "\t\n"+ tabs + "}";
                }
                else
                {
                    t += " ";

                }
            }
            else
            {
                StartText = StartText.TrimEnd(new char[] {' '});
                t = " " + t;
            }

            

            Control.Document.Replace(ls.Offset, ls.Length,
                StartText + (t));

            if (!needBraces)
            {
                Control.ActiveTextAreaControl.Caret.Column = (StartText + t).Length;
            }
            else
            {
                Control.ActiveTextAreaControl.Caret.Line++;
                Control.ActiveTextAreaControl.Caret.Column = (tabs.Length)+1;

            }

            this.ScriptWindow.UpdateDatabase();
        }

        public void SelectUp()
        {
            if(suggestionListbox.SelectedIndices.Count > 0)
            {
                if(suggestionListbox.SelectedIndices[0] > 0)
                    suggestionListbox.SelectItem(suggestionListbox.SelectedIndices[0]-1);
            }
            suggestionListbox.EnsureVisible();
            suggestionListbox.Invalidate();
        }

        public void SelectDown()
        {
            if (suggestionListbox.SelectedIndices.Count > 0)
            {
                if (suggestionListbox.SelectedIndices[0] < suggestionListbox.Items.Count-1)
                    suggestionListbox.SelectItem(suggestionListbox.SelectedIndices[0] + 1);
            }

            suggestionListbox.EnsureVisible();
            suggestionListbox.Invalidate();

        }
    }
}
