#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Controls;
using ICSharpCode.TextEditor;
using JominiParse;

#endregion

namespace CK3ScriptEditor
{
    public partial class IntellisenseDlg : Form
    {
        private bool autoCompletePostEquals;

        public IntellisenseDlg()
        {
            InitializeComponent();
        }
        public string TabSpacing { get; set; }
        public ScriptObject Inside { get; set; }
        public ScriptWindow ScriptWindow { get; set; }
        public string StartText { get; set; }
        public TextEditorControl Control { get; set; }
        public bool Force { get; set; }

        public bool FillIntellisense(ScriptWindow sw, TextEditorControl control, string lineSegment, int caretColumn,
            ScriptObject inside)
        {
            ScriptWindow = sw;
            Control = control;
            var before = lineSegment.Substring(0, caretColumn);
            var after = lineSegment.Substring(caretColumn);

            if (after.Trim().Length > 0 && (after[0] != ' ' || after[0] != '\t'))
                return false;

            Size = new Size(600, 400);
            var startOfLastword = before.IndexOf(before.Trim());
            TabSpacing = before.Substring(0, startOfLastword);
            if (before.Trim().Length == 0)
                TabSpacing = before;
            var endOfLastWord = before.IndexOf(before.Trim()) + before.Trim().Length;
            Inside = inside;
            var beforeOriginal = before;
            before = before.Trim();
            var choices = new List<string>();
            if (before.Length == 0 && !Force)
                return false;

            if (before.Contains("=") || before.Contains("<=") || before.Contains(">=") || before.Contains("!=") ||
                before.Contains("=="))
            {
                beforeOriginal = beforeOriginal.Replace("==", "*");
                beforeOriginal = beforeOriginal.Replace("<=", "*");
                beforeOriginal = beforeOriginal.Replace(">=", "*");
                beforeOriginal = beforeOriginal.Replace("!=", "*");
                beforeOriginal = beforeOriginal.Replace("==", "*");
                beforeOriginal = beforeOriginal.Replace("=", "*");
                var afterOpindex = beforeOriginal.IndexOf("*") + 1;
                before = before.Replace("==", "*");
                before = before.Replace("<=", "*");
                before = before.Replace(">=", "*");
                before = before.Replace("!=", "*");
                before = before.Replace("==", "*");
                before = before.Replace("=", "*");
                var beforeReplaced = before;
                var sofar = before.Substring(before.LastIndexOf("*") + 1).Trim();
                before = before.Substring(0, before.LastIndexOf("*")).Trim();
                var dif = beforeOriginal.Length - beforeOriginal.TrimEnd(' ').Length;

                if (!beforeReplaced.Trim().EndsWith("*"))
                    dif = 1;

                if (dif >= 1)
                {
                    // find appropriate choices for type before =
                    choices = CoreIntellisenseHandler.Instance.GetValidTokensEqual(inside, before, sofar);

                    var index = lineSegment.IndexOf(before);

                    StartText = lineSegment.Substring(0, afterOpindex);
                    autoCompletePostEquals = true;
                }
            }
            else
            {
                if (endOfLastWord < caretColumn && before.Length > 0)
                    return false;

                choices = CoreIntellisenseHandler.Instance.GetValidTokens(inside, before);

                var index = lineSegment.IndexOf(before);

                StartText = lineSegment.Substring(0, index);
                if (before.Length == 0)
                    StartText = lineSegment.Substring(0, caretColumn);

                autoCompletePostEquals = false;
            }

            suggestionListbox.Items.Clear();
            if (choices.Count == 0)
                return false;

            suggestionListbox.SuspendEvents(true);

            foreach (var choice in choices)
            {
                var l = new DarkListItem(choice);
                suggestionListbox.Items.Add(l);
            }

            suggestionListbox.SuspendEvents(false);

            suggestionListbox.SelectItem(0);
            Size = new Size(Size.Width,
                Math.Min(choices.Count * suggestionListbox.ItemHeight + 5, 10 * suggestionListbox.ItemHeight));

            return true;
        }

        private void suggestionListbox_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void suggestionListbox_SelectedIndicesChanged(object sender, EventArgs e)
        {
        }

        private void suggestionListbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return) DoComplete();
        }

        public void DoComplete()
        {
            Control.ActiveTextAreaControl.TextArea.Focus();
            var ls = Control.Document.GetLineSegment(Control.ActiveTextAreaControl.Caret.Line);
            var t = suggestionListbox.Items[suggestionListbox.SelectedIndices[0]].Text;
            var tabs = TabSpacing;

            var needBraces = false; //CoreIntellisenseHandler.Instance.GetNeedBraces(Inside, t);
            if (!autoCompletePostEquals)
            {
                t += " = ";
            }
            else
            {
                StartText = StartText.TrimEnd(' ');
                if (t == "{ }")
                {
                    t = "";
                    t += " {\n" + tabs + "\t\n" + tabs + "}";
                    needBraces = true;
                }
                else
                {
                    t = " " + t;
                }
            }


            Control.Document.Replace(ls.Offset, ls.Length,
                StartText + t);

            if (!needBraces)
            {
                Control.ActiveTextAreaControl.Caret.Column = (StartText + t).Length;
            }
            else
            {
                Control.ActiveTextAreaControl.Caret.Line++;
                Control.ActiveTextAreaControl.Caret.Column = tabs.Length + 1;
            }

            ScriptWindow.UpdateDatabase();
        }

        public void SelectUp()
        {
            if (suggestionListbox.SelectedIndices.Count > 0)
                if (suggestionListbox.SelectedIndices[0] > 0)
                    suggestionListbox.SelectItem(suggestionListbox.SelectedIndices[0] - 1);
            suggestionListbox.EnsureVisible();
            suggestionListbox.Invalidate();
        }

        public void SelectDown()
        {
            if (suggestionListbox.SelectedIndices.Count > 0)
                if (suggestionListbox.SelectedIndices[0] < suggestionListbox.Items.Count - 1)
                    suggestionListbox.SelectItem(suggestionListbox.SelectedIndices[0] + 1);

            suggestionListbox.EnsureVisible();
            suggestionListbox.Invalidate();
        }

        private void suggestionListbox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DoComplete();
        }
    }
}