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
            "#",
            "/",
            "{",
            "}",
            "[",
            "]",
            ":",
            ";",

        };

        private Color FunctionColor = Color.White;// Color.FromArgb(173, 139, 236);
        private Color ScopeColor = Color.FromArgb(139, 229, 236);
        private Color OverriddenColor = Color.FromArgb(128, 96, 96);
        private Color StringColor = Color.FromArgb(241, 250, 140);
        private Color NumberColor = Color.FromArgb(189, 147, 249);

        private Color BaseCommandColor = Color.FromArgb(228, 112, 198);
        private Color ReferencedObjectColor = Color.FromArgb(208, 176, 113);
        private Color LocalizedStringColor = Color.FromArgb(133, 113, 208);

        ScriptObjectSchema AssumedSchema = new ScriptObjectSchema();

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
            switch (data.Type)
            {
                case ScriptObjectBehaviourType.FunctionNamedFromParameterBlock:
                case ScriptObjectBehaviourType.FunctionNamedFromParameterSingleLine:
                    col = FunctionColor;
                    if (scriptObject.BehaviourData.NameTypeExpected == "num")
                        col = NumberColor;
                    bold = true;

                    break;
                case ScriptObjectBehaviourType.FunctionParameter:
                     col = FunctionColor;
                    break;
                case ScriptObjectBehaviourType.FunctionMultiline:
                case ScriptObjectBehaviourType.FunctionSingleLine:
                    col = FunctionColor;
                    bold = true;
                     break;
                case ScriptObjectBehaviourType.InherentScopeBlock:
                case ScriptObjectBehaviourType.InherentScopeToProperty:
                case ScriptObjectBehaviourType.SavedScopeBlock:
                case ScriptObjectBehaviourType.SavedScopeToProperty:
                    col = ScopeColor;
                    bold = true;
                    break;
                case ScriptObjectBehaviourType.RootObject:
                    col = ReferencedObjectColor;
                    bold = true;
                    break;
                case ScriptObjectBehaviourType.LogicalOperand:
                case ScriptObjectBehaviourType.GeneralBlock:
                case ScriptObjectBehaviourType.RootObjectProperty:
                case ScriptObjectBehaviourType.RootObjectPropertyBlock:
                case ScriptObjectBehaviourType.Limit:
                case ScriptObjectBehaviourType.If:
                case ScriptObjectBehaviourType.TriggerIf:
                case ScriptObjectBehaviourType.Else:
                case ScriptObjectBehaviourType.TriggerElse:
                case ScriptObjectBehaviourType.Trigger:
                    col = BaseCommandColor;
                    bold = true;
                    break;
                  
            }

            if (baseCommands.Contains(name))
                col = BaseCommandColor;
      


            ColourName(name, doc, lineSeg, col, backgroundColor, bold, italic);


            if (scriptObject is ScriptValue && !(scriptObject is StaticScriptValue))
            {
                col = Color.Red;

                var strVal = (scriptObject as ScriptValue).GetStringValue();

                if (data.ReferencedObject != null)
                {
                    col = ReferencedObjectColor;
                    italic = true;
                    bold = true;
                    ColourName(strVal, doc, lineSeg, col, backgroundColor, bold, italic);
                }
                else
                {
                    if (data.ValueFound)
                    {
                        if (data.ValueIsScope)
                        {
                            col = ScopeColor;
                            bold = true;
                        }
                        else if (data.TypeExpected == "localized")
                        {
                            col = LocalizedStringColor;
                            italic = true;
                            bold = true;
                        }
                        else
                            col = FunctionColor;
                    }
                    else if (data.TypeExpected == "string" && strVal != null)
                    {
                        col = StringColor;
                        italic = true;
                        bold = true;
                    }
                    else if (data.TypeExpected == "any" && strVal != null)
                    {
                        col = FunctionColor;
                        italic = true;
                    }
                    else if (data.TypeExpected == "value" && strVal.StartsWith("flag:"))
                    {
                        col = ReferencedObjectColor;
                        italic = true;
                    }

                    ColourName(strVal, doc,  lineSeg, col, backgroundColor, bold, italic);

                }
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
