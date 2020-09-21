using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.TextEditor.Document;
using JominiParse;

namespace CK3ScriptEditor
{
    public class SyntaxHighlightingManager
    {
        public static SyntaxHighlightingManager Instance = new SyntaxHighlightingManager();

        public void DoDocument(IDocument doc, Color backgroundColor, ScriptFile file)
        {

            foreach (var scriptObject in file.Map.Values)
            {
                //  ColorEntireObject(Color.White, backgroundColor, doc, scriptObject.LineStart - 1, scriptObject.LineEnd);
                DoDelimitersForObject(Color.White, backgroundColor, doc, scriptObject.LineStart - 1, scriptObject.LineEnd);

                DoScriptObject(scriptObject, backgroundColor, doc);
            }

        }

        private List<string> baseCommands = new List<string>()
        {
            "if",
            "else_if",
            "else",
            "trigger_if",
            "trigger_else_if",
            "trigger_else",
            "limit",
            "alternative_limit",
            "NOT",
            "NOR",
            "OR",
            "AND",
            "NAND",

        };
        private List<string> delimiters = new List<string>()
        {
            ">",
            "<",
            "=",
            "~",
            "!",
            "%",
            "^",
            "*",
            "(",
            ")",
            "-",
            "+",
            "=",
            "|",
            "\\",
            "/",
            "{",
            "}",
            "[",
            "]",
            ":",
            ";",

        };
            
        private Color VarColor = Color.FromArgb(203, 236, 139);
        private Color FunctionColor = Color.White;// Color.FromArgb(173, 139, 236);
        private Color ScopeColor = Color.FromArgb(139, 229, 236);
        private Color OverriddenColor = Color.FromArgb(128, 96, 96);
        private Color StringColor = Color.FromArgb(241, 250, 140);
        private Color NumberColor = Color.FromArgb(189, 147, 249);

        private Color BaseCommandColor = Color.FromArgb(228, 112, 198);
        private Color ReferencedObjectColor = Color.FromArgb(208, 176, 113);
        private Color LocalizedStringColor = Color.FromArgb(133, 113, 208);

        private void DoScriptObject(ScriptObject scriptObject, Color backgroundColor, IDocument doc)
        {
            string name = scriptObject.Name;
            int line = scriptObject.LineStart - 1;
            var lineSeg = doc.LineSegmentCollection[line];

            if (scriptObject.LineStart == ScriptObjectBehaviourManager.BreakpointLine &&
                scriptObject.Filename.Contains(ScriptObjectBehaviourManager.BreakpointFile))
            {

            }
            var col = Color.Red;

            bool bold = false;
            bool italic = false;
            if (scriptObject == scriptObject.Topmost && scriptObject.Overridden)
            {
                ColorEntireObject(OverriddenColor, backgroundColor, doc, scriptObject.LineStart - 1, scriptObject.LineEnd);
                return;
            }

            if (scriptObject.Topmost.Overridden)
            {
                return;
            }

            var data = scriptObject.BehaviourData;
            if (data == null)
                return;

            bool avoidRed = false;

            if (scriptObject.Parent != null && scriptObject.Parent.lhsSchema != null && scriptObject.Parent.lhsSchema.avoidRed)
            {
                avoidRed = true;
                col = FunctionColor;
            }
            if (scriptObject.Topmost != null && scriptObject.Topmost.lhsSchema != null && scriptObject.Topmost.lhsSchema.avoidRed)
            {
                avoidRed = true;
                col = FunctionColor;
            }
            if (data.lhsError == null)
            {
                col = FunctionColor;
                if (baseCommands.Contains(data.lhs) || (scriptObject.Parent != null && scriptObject.Parent.Parent == null))
                    col = BaseCommandColor;
                if (data.NameIsReference == true)
                {
                    col = ReferencedObjectColor;
                    bold = true;
                    italic = true;
                }
            }

            if (scriptObject.lhsSchema?.namesFrom == "value")
            {
                float val;

                if (Single.TryParse(data.lhs, out val))
                {
                    col = NumberColor;
                }
            }

            if (data.ParentData != null && data.ParentData.ChildrenAreValueRange)
            {
                col = NumberColor;
            }

            if (data.lhsScopeTextColorLength > 0)
            {
                ColourName(name.Substring(0, Math.Min(data.lhsScopeTextColorLength, name.Length)), doc, lineSeg, ScopeColor, backgroundColor, true, italic);
                if(data.lhsScopeTextColorLength < name.Length)
                    ColourName(name.Substring(data.lhsScopeTextColorLength), doc, lineSeg, col, backgroundColor, bold, italic);
            }
            else ColourName(name, doc, lineSeg, col, backgroundColor, bold, italic);




            if (scriptObject is ScriptValue)
            {
                if(avoidRed)
                    col = FunctionColor;
                else
                    col = Color.Red;

                var strVal = (scriptObject as ScriptValue).GetStringValue();

                if (data.rhsError == null)
                {
                    col = FunctionColor;
                    if (data.ReferencedObject != null)
                        col = ReferencedObjectColor;
                    // scope from save_scope_as etc
                    if (scriptObject.lhsSchema != null)
                    {
                        if (scriptObject.lhsSchema.TypeList.Contains("scope"))
                            col = ScopeColor;
                        if (scriptObject.lhsSchema.TypeList.Contains("var") || scriptObject.lhsSchema.TypeList.Contains("global_var") ||
                            scriptObject.lhsSchema.TypeList.Contains("local_var"))
                            col = VarColor;
                        if (scriptObject.lhsSchema.TypeList.Contains("bool"))
                        {
                            if (strVal == "yes" || strVal == "no")
                                col = BaseCommandColor;
                        }

                        if (scriptObject.lhsSchema.TypeList.Contains("localized"))
                        {
                            col = LocalizedStringColor;
                        }
                        if (scriptObject.lhsSchema.TypeList.Contains("trigger_localization"))
                        {
                            col = LocalizedStringColor;
                        }

                        if (scriptObject.lhsSchema.TypeList.Contains("string"))
                        {
                            italic = true;
                            col = StringColor;
                        }

                        if (scriptObject.lhsSchema.TypeList.Contains("value"))
                        {
                            float val;

                            if (Single.TryParse(data.rhs, out val))
                            {
                                col = NumberColor;
                            }
                        }

                        if (scriptObject.lhsSchema.TypeList.Contains("any"))
                        {
                            float val;

                            if (Single.TryParse(data.rhs, out val))
                            {
                                col = NumberColor;
                            }
                        }
                    }


                    if (data.ReferenceValid)
                    {
                        italic = true;
                        bold = true;
                    }
                }

                if (data.rhsScopeTextColorLength > 0)
                {
                    ColourName(strVal.Substring(0, Math.Min(data.rhsScopeTextColorLength, strVal.Length)), doc, lineSeg, ScopeColor, backgroundColor, true, italic);
                    if (data.rhsScopeTextColorLength < strVal.Length)
                        ColourName(strVal.Substring(data.rhsScopeTextColorLength), doc, lineSeg, col, backgroundColor, bold, italic);
                }
                else ColourName(strVal, doc, lineSeg, col, backgroundColor, bold, italic);
            }

            foreach (var scriptObjectChild in scriptObject.Children)
            {
                DoScriptObject(scriptObjectChild, backgroundColor, doc);
            }
        }


        private void DoDelimitersForObject(Color col, Color backgroundColor, IDocument doc, int f, int t)
        {
            for (int x = f; x < t; x++)
            {
                int xx = Math.Min(doc.LineSegmentCollection.Count - 1, x);
                ColorDelimiters(col, doc, backgroundColor, doc.LineSegmentCollection[xx]);
            }

        }

        private void ColorEntireObject(Color col, Color backgroundColor, IDocument doc, int f, int t)
        {
            for (int x = f; x < t; x++)
            {
                int xx = Math.Min(doc.LineSegmentCollection.Count - 1, x);
                ColorEntireLine(col, doc, backgroundColor, doc.LineSegmentCollection[xx]);
            }
        }

        private void ColorDelimiters(Color col, IDocument doc, Color backgroundColor, LineSegment line)
        {
            foreach (var lineWord in line.Words)
            {
                if (delimiters.Contains(lineWord.Word))
                {
                    if (lineWord.SyntaxColor != null)
                        lineWord.SyntaxColor = new HighlightColor(col, backgroundColor, false, false);

                }
            }
        }

        private void ColorEntireLine(Color col, IDocument doc, Color backgroundColor, LineSegment line)
        {
            foreach (var lineWord in line.Words)
            {
                if (lineWord.SyntaxColor != null)
                    lineWord.SyntaxColor = new HighlightColor(col, backgroundColor, false, false);
            }
        }
        private void ColourName(string name, IDocument doc, LineSegment line, Color color, Color backgroundColor, bool bold, bool italic)
        {
            if (!line.ColorWord(name, color, bold, italic, backgroundColor))
            {

            }
        }
    }
}
