#region

using System;
using System.Drawing;
using System.Windows.Forms;
using DarkUI.Forms;
using ICSharpCode.TextEditor.Document;

#endregion

namespace CK3ScriptEditor
{
    public partial class LocalizationEditor : DarkForm
    {
        private Color backgroundColor;

        public LocalizationEditor()
        {
            InitializeComponent();

            localTextEdit.ActiveTextAreaControl.TextArea.DoProcessDialogKey += TextAreaOnDoProcessDialogKey;


            switch (EditorGlobals.FontSize)
            {
                case FontSize.Normal:
                    localTextEdit.Font = new Font(
                        "Courier New",
                        12F,
                        FontStyle.Regular,
                        GraphicsUnit.Point,
                        0); break;
                case FontSize.Large:
                    localTextEdit.Font = new Font(
                        "Courier New",
                        14F,
                        FontStyle.Regular,
                        GraphicsUnit.Point,
                        0);
                    break;
            }

        }

        public string LocalizedTag { get; set; }
        public ScriptWindow ParentWindow { get; set; }

        private bool TextAreaOnDoProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return true;
            }

            return false;
        }

        public void Set(ScriptWindow parent, string tag, string localized)
        {
            LocalizedTag = tag;
            var lines = localized.Split(new[] {"\\n"}, StringSplitOptions.None);

            ParentWindow = parent;

            localTextEdit.Document.TextContent = string.Join("\n", lines);
            var c = Color.FromArgb(255, 42, 54, 40);

            localTextEdit.Document.ReadOnly = false;

            var d = localTextEdit.Document.HighlightingStrategy as DefaultHighlightingStrategy;
            {
                var s = Color.FromArgb(255, 73, 92, 70);
                var cl = Color.FromArgb(255, 43, 62, 40);

                d.environmentColors["Default"].BackgroundColor = c;
                d.environmentColors["FoldLine"].BackgroundColor = c;
                d.environmentColors["VRuler"].BackgroundColor = c;

                d.environmentColors["Selection"].BackgroundColor = s;
                d.environmentColors["CaretLine"].BackgroundColor = cl;
                //  d.environmentColors["VRuler"].BackgroundColor = c;
                d.DigitColor.BackgroundColor = c;
                d.DefaultTextColor.BackgroundColor = c;

                backgroundColor = c;


                foreach (var lineSegment in localTextEdit.Document.LineSegmentCollection)
                foreach (var lineSegmentWord in lineSegment.Words)
                    if (lineSegmentWord.SyntaxColor != null)
                    {
                        lineSegmentWord.SyntaxColor = new HighlightColor(lineSegmentWord.SyntaxColor.Color,
                            lineSegmentWord.SyntaxColor.BackgroundColor, lineSegmentWord.SyntaxColor.Bold,
                            lineSegmentWord.SyntaxColor.Italic);
                        lineSegmentWord.SyntaxColor.BackgroundColor = c;
                    }
            }
        }

        private void done_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}