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
using ICSharpCode.TextEditor.Document;
using JominiParse;

namespace CK3ScriptEditor
{
    public partial class LocalizationEditor : DarkForm
    {
        private Color backgroundColor;

        public LocalizationEditor()
        {
            InitializeComponent();
            
        }

        public void Set(ScriptWindow parent, string tag, string localized)
        {
            this.LocalizedTag = tag;
            string[] lines = localized.Split(new string[] {"\\n"}, StringSplitOptions.None);

            this.ParentWindow = parent;
            
            localTextEdit.Document.TextContent = String.Join("\n", lines);
            var c = Color.FromArgb(255, 42, 54, 40);

            localTextEdit.Document.ReadOnly = false;
            
            var d = (localTextEdit.Document.HighlightingStrategy as DefaultHighlightingStrategy);
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
                {
                    foreach (var lineSegmentWord in lineSegment.Words)
                    {
                        if (lineSegmentWord.SyntaxColor != null)
                        {
                            lineSegmentWord.SyntaxColor = new HighlightColor(lineSegmentWord.SyntaxColor.Color,
                                lineSegmentWord.SyntaxColor.BackgroundColor, lineSegmentWord.SyntaxColor.Bold,
                                lineSegmentWord.SyntaxColor.Italic);
                            lineSegmentWord.SyntaxColor.BackgroundColor = c;
                        }
                    }
                }
            }
        }

        public string LocalizedTag { get; set; }

        public ScriptWindow ParentWindow { get; set; }

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
