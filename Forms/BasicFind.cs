using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Forms;
using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Document;

namespace CK3ScriptEditor
{
    public partial class BasicFind : DarkForm
    {
        private ScriptWindow _window;

        public BasicFind(ScriptWindow window)
        {
            InitializeComponent();
            this.Window = window;
        }

        public ScriptWindow Window
        {
            get => _window;
            set
            {
                _window = value;
                replaceNext.Enabled = !_window.Filename.IsBase;
                ReplaceText.Enabled = !_window.Filename.IsBase;
            }
        }

        private bool findSuccess = false;
        private void findNext_Click(object sender, EventArgs e)
        {
            findSuccess = false;
            var line = Window.Editor.ActiveTextAreaControl.Caret.Line;
            var column = Window.Editor.ActiveTextAreaControl.Caret.Column+1;
            
            int startLine = line;
            var lineSegments = Window.Editor.Document.LineSegmentCollection;

            string find = FindText.Text;
            if (!caseSensitive.Checked)
                find = find.ToLower();

            for (var index = line; index < lineSegments.Count; index++)
            {
                var lineSegment = lineSegments[index];

                var str = lineSegment.LineToString();
                if (!caseSensitive.Checked)
                    str = str.ToLower();
                if (column > str.Length)
                    column = str.Length;

                if (str.Contains(find))
                {
                    
                    var i = str.IndexOf(find, column);
                    if ( i > column)
                    {
                        if (wholeWordOnly.Checked)
                        {
                            bool success = false;
                            if (i == 0 || isWhitespaceOrOperator(str[i - 1].ToString()))
                            {
                                if(i + find.Length > str.Length)
                                    continue;
                                
                                if (i + find.Length == str.Length || (i + find.Length < str.Length && isWhitespaceOrOperator(str[i + find.Length].ToString())))
                                {
                                    success = true;
                                }
                            }

                            if (!success)
                                continue;
                        }

                        Window.Editor.ActiveTextAreaControl.Caret.Line = index;
                        Window.Editor.ActiveTextAreaControl.Caret.Column = i;
                        Window.Editor.ActiveTextAreaControl.SelectionManager.SetSelection(new TextLocation(i, index), new TextLocation(i+find.Length, index));
                        findSuccess = true;
                        return;
                    }
                }

                column = 0;
                line++;
            }

            line = 0;
            column = 0;
            for (var index = line; index < startLine; index++)
            {
                var lineSegment = lineSegments[index];

                var str = lineSegment.LineToString();
                if (!caseSensitive.Checked)
                    str = str.ToLower();
                if (str.Contains(find))
                {
                    var i = str.IndexOf(find, column);
                    if (i > column)
                    {
                        if (wholeWordOnly.Checked)
                        {
                            bool success = false;
                            if (i == 0 || isWhitespaceOrOperator(str[i - 1].ToString()))
                            {
                                if (i + find.Length > str.Length)
                                    continue;

                                if (i + find.Length == str.Length || (i + find.Length < str.Length && isWhitespaceOrOperator(str[i + find.Length].ToString())))
                                {
                                    success = true;
                                }
                            }

                            if (!success)
                                continue;
                        }

                        Window.Editor.ActiveTextAreaControl.Caret.Line = index;
                        Window.Editor.ActiveTextAreaControl.Caret.Column = i;
                        Window.Editor.ActiveTextAreaControl.SelectionManager.SetSelection(new TextLocation(i, index), new TextLocation(i + find.Length, index));
                        findSuccess = true;
                        return;
                    }
                }

                column = 0;
                line++;
            }
            
        }

        private List<string> operators = new List<string>()
        {
            ":",
            ".",
            "<",
            ">",
            "=",
        };
        private bool isWhitespaceOrOperator(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return true;

            if(operators.Contains(str))
                return true;

            return false;
        }
        private void FindText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                findNext_Click(null, new EventArgs());
                FindText.Focus();
            }

            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }

        }

        private void replaceNext_Click(object sender, EventArgs e)
        {
            var selectedText = Window.Editor.ActiveTextAreaControl.SelectionManager.SelectedText;

            if (!caseSensitive.Checked)
                selectedText = selectedText.ToLower();

            string test = FindText.Text;
            if (!caseSensitive.Checked)
                test = test.ToLower();

            if (test == selectedText)
            {
                var ls = Window.Editor.Document.GetLineSegment(Window.Editor.ActiveTextAreaControl.Caret.Line);
                //       Window.Editor.ActiveTextAreaControl.SelectionManager.RemoveSelectedText();
                Window.Editor.Document.Replace(ls.Offset + Window.Editor.ActiveTextAreaControl.Caret.Column, selectedText.Length,
                    ReplaceText.Text);

                Window.Editor.ActiveTextAreaControl.SelectionManager.SetSelection(new TextLocation(Window.Editor.ActiveTextAreaControl.Caret.Column, Window.Editor.ActiveTextAreaControl.Caret.Line), new TextLocation(Window.Editor.ActiveTextAreaControl.Caret.Column+ ReplaceText.Text.Length, Window.Editor.ActiveTextAreaControl.Caret.Line));
            }

            findNext_Click(this, new EventArgs());
        }

        private void ReplaceAll_Click(object sender, EventArgs e)
        {
            findSuccess = false;

            do
            {
                replaceNext_Click(null, new EventArgs());

            } while (findSuccess);
        }
    }
}
