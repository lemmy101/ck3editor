using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace JominiParse
{
    public class FileTokenizer
    {
        public static FileTokenizer Instance = new FileTokenizer();

        string EnsureSpacing(string str, string test)
        {
            if (!str.Contains(test))
                return str;

            string[] split = str.Split(new string[] { test }, StringSplitOptions.None);
            test = String.Join(" " + test + " ", split);

            return test;
        }
        string EnsureNoSpacing(string str, string test)
        {
            if (!str.Contains(test))
                return str;

            string[] split2 = str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            str = String.Join("", split2);
            
            string[] split = str.Split(new string[] { test }, StringSplitOptions.None);
            test = String.Join(test.Trim(), split);

            return test;
        }

        public List<ScriptObject> LoadDirectory(string directory, string basePath, ScriptContext context)
        {
            List<ScriptObject> results = new List<ScriptObject>();
            if (!Directory.Exists(directory))
                return results;

            var directories = Directory.GetDirectories(directory);

            foreach (var s in directories)
            {
                results.AddRange(LoadDirectory(s, basePath, context));
            }

            var files = Directory.GetFiles(directory);

            foreach (var file in files)
            {
                if (file.EndsWith(".txt"))
                {
                    results.AddRange(LoadFile(file.Replace("\\", "/"), basePath, context));
                }
            }

            return results;
        }
        public List<ScriptObject> LoadFile(string filename, string basePath, ScriptContext context)
        {
            string text = System.IO.File.ReadAllText(filename);
            return LoadText(text, filename, basePath, context);
        }
        public List<ScriptObject> LoadText(string text, string filename, string basePath, ScriptContext context)
        {
            ScriptNamespace = "";
            List<ScriptObject> results = new List<ScriptObject>();

       
            filename = filename.Replace(basePath, "");

            this.File = Core.Instance.LoadingCK3Library.EnsureFile(filename, context);

            string[] lines = text.Split(new char[] {'\n'});
            List<int> lineNumbers = new List<int>();
            List<string> tokens = new List<string>();
            int index = 0;
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                line = line.Trim();
                index++;
                if (line.Contains("soundparameter"))
                {

                }
                if (line.Contains("debug_log"))
                    continue;
                if (line.Trim().StartsWith("#"))
                    continue;
                string line2 = line;
                if (line.Length == 0)
                    continue;
                if(line2 == "name = \"Abdallah\" #ibn Muhammad\"")
                {

                }
                if (line2.Contains("\"") && ((line2.IndexOf("\"") < line2.IndexOf("#") || line2.IndexOf("#") == -1)))
                {
                    string ll = line2;
                    string after = "";
                    while (ll.Contains("\""))
                    {
                        if (ll.Contains("\"") &&
                            ((ll.IndexOf("\"") < ll.IndexOf("#") || ll.IndexOf("#") == -1)))
                        {
                            var s = ll.IndexOf("\"");
                            var e = ll.IndexOf("\"", s + 1);
                            var l = e - s;
                            string before = ll.Substring(0, s);
                            string str = ll.Substring((s + 1), l - 1);
                            after = ll.Substring(e + 1);

                            TokenizeLine(before.Trim(), tokens);
                            tokens.Add("\"" + str + "\"");
                            lineNumbers.Add(index);
                            ll = after;
                        }
                        else
                        {
                            ll = ll.Substring(0, ll.IndexOf("#")).Trim();
                        }
                      
                        

                    }
                    TokenizeLine(after.Trim(), tokens);



                    while (lineNumbers.Count < tokens.Count)
                    {
                        lineNumbers.Add(index);
                    }
                }
                else
                {
                    TokenizeLine(line2, tokens);
                    while (lineNumbers.Count < tokens.Count)
                    {
                        lineNumbers.Add(index);
                    }
                }
            }

            if (tokens.Count == 0)
                return results;

            var parsableResults = ParseTokens(tokens, lineNumbers, filename);



            ParseResults(null, parsableResults, context, results);

            return results;
        }

        public ScriptFile File { get; set; }

        private void TokenizeLine(string line2, List<string> tokens)
        {
            if (line2.Length == 0)
                return;

            line2 = line2.Split('#')[0].Trim();

            line2 = EnsureSpacing(line2, "=");
            line2 = EnsureSpacing(line2, "<");
            line2 = EnsureSpacing(line2, ">");
            
            line2 = EnsureSpacing(line2, "= =");
            line2 = EnsureSpacing(line2, "! =");

            line2 = EnsureSpacing(line2, "< =");
            line2 = EnsureSpacing(line2, "> =");
            line2 = EnsureSpacing(line2, "{");
            line2 = EnsureSpacing(line2, "}");
            line2 = EnsureSpacing(line2, "@[");
            line2 = EnsureSpacing(line2, "]");

            //

            
            if (line2.Contains("<  =") || line2.Contains(">  ="))
            {
                line2 = line2.Replace("<  =", "<=");
                line2 = line2.Replace(">  =", ">=");
            }
            if (line2.Contains("= =") || line2.Contains("! ="))
            {
                line2 = line2.Replace("= =", "==");
                line2 = line2.Replace("! =", "!=");
            }


            {
                string[] linetokens = line2.Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

                tokens.AddRange(linetokens);
            }
        }

        public void ParseResults(ScriptObject parent, List<ScriptParsedSegment> parsableResults, ScriptContext context,
            List<ScriptObject> results)
        {
            for (var index = 0; index < parsableResults.Count; index++)
            {
                var scriptParsedSegment = parsableResults[index];
                if (scriptParsedSegment.name == "namespace")
                {
                    SetNamespace(scriptParsedSegment.value[0]);
                    continue;
                }

                if (scriptParsedSegment.name == "if" || scriptParsedSegment.name == "trigger_if")
                {
                    int range;
                    results.Add(ParseIf(parent, parsableResults, index, context, out range));
                    index += range - 1;
                    continue;
                }

                var r = ParseResult(parent, scriptParsedSegment, context);
                if(r != null)
                    results.Add(r);
                else
                {

                }
            }
        }
        private ScriptObject ParseResult(ScriptObject parent, ScriptParsedSegment parsableResult, ScriptContext context)
        {
            switch (context)
            {
                case ScriptContext.ScriptValues:
                    return ScriptValueParser.Instance.ParseScriptValue(parent, parsableResult);
                    break;
                case ScriptContext.Activities:
                    return ScriptActivityParser.Instance.ParseActivity(parent, parsableResult);
                    break;
                case ScriptContext.Decisions:
                    return DecisionParser.Instance.ParseDecision(parent, parsableResult);
                    break;
                case ScriptContext.Events:
                    return EventParser.Instance.ParseEvent(parent, parsableResult);
                    break;
                case ScriptContext.ConditionBlock:
                    return EffectParser.Instance.ParseCondition(parent, parsableResult);
                    break;
                case ScriptContext.EffectBlock:
                    return EffectParser.Instance.ParseEffect(parent, parsableResult);
                    break;
                case ScriptContext.ScriptedModifiers:
                    return ScriptedModifierParser.Instance.ParseModifier(parent, parsableResult);
                    break;
            }

            return new ScriptObject(parent, parsableResult);
        }

        public ScriptObject ParseUnknown(ScriptObject parent, ScriptParsedSegment segment, ScriptContext context)
        {
            return ParseResult(parent, segment, context);
        }


        public If ParseIf(ScriptObject parent, List<ScriptParsedSegment> results, int index, ScriptContext context, out int range)
        {
            for (int i = index+1; i < results.Count; i++)
            {
                if (results[i].name != "elseif" && results[i].name != "else_if" && results[i].name != "else")
                {
                    range = i - index;
                    return IfParser.Instance.ParseIf(parent, results.GetRange(index, i - index), context);
                }

                if (i == results.Count - 1)
                {
                    range = (i - index) + 1;
                    return IfParser.Instance.ParseIf(parent, results.GetRange(index, (i - index) + 1), context);

                }
            }

            if (results.Count==index+1)
            {
                range = 1;
                return IfParser.Instance.ParseIf(parent, results.GetRange(0, 1), context);

            }

            range = 1;
            return null;
        }

        public List<ScriptParsedSegment> ParseTokens(List<string> tokens, List<int> lineNumbers, string filename)
        {
            List<ScriptParsedSegment> parsables = new List<ScriptParsedSegment>();
            List<int> lineNumbersForObject = new List<int>();
            List<string> tokensForObject = new List<string>();
            int token = 0;
            while (tokens.Count > 0)
            {
                if (tokens.Count == 1)
                {
                    tokensForObject.Clear();
                    tokensForObject.AddRange(tokens);
                    List<int> lineNumChild = new List<int>();
                    lineNumChild.AddRange(lineNumbers);
                    ScriptParsedSegment parsable = new ScriptParsedSegment();
                    parsable.lineNumbers = lineNumChild;
                    parsable.filename = filename;
                    parsable.Parse(tokensForObject);
                    parsables.Add(parsable);
                    tokens.Clear();
                   // lineNumChild.Clear();
                    token += 1;
                    continue;
                }
                if (tokens.Count == 2)
                {
                    tokensForObject.Clear();
                    tokensForObject.Add(tokens[0]);
                    List<int> lineNumChild = new List<int>();
                    lineNumChild.Add(lineNumbers[0]);
                    ScriptParsedSegment parsable = new ScriptParsedSegment();
                    parsable.lineNumbers = lineNumChild;
                    parsable.filename = filename;
                    parsable.Parse(tokensForObject);
                    parsables.Add(parsable);
                    tokens.RemoveAt(0);
                  //  lineNumChild.Clear();
                    lineNumbers.RemoveAt(0);
                    token += 1;
                    continue;
                }
                string first = tokens[0];
                string second = tokens[1];
                string third = tokens[2];

                int extraTokens =0;
                if (first == "NOT" && lineNumbers[0] == 431)
                {

                }
                if(third == "hsv")
                {
                    third = String.Join("", tokens.GetRange(2, tokens.Count - 2));
                    lineNumbers.RemoveRange(2, tokens.Count - 3);
                    tokens.RemoveAt(2);
                    tokens.Insert(2, third);
                    tokens.RemoveRange(3, tokens.Count-3);
                    token += 3;
                }
                if (first == "scripted_trigger" || first == "scripted_effect")
                {
                    first = first + " " + second;
                    second = third;
                    third = tokens[3];
                    tokens.RemoveAt(1);
                    lineNumbers.RemoveAt(1);
                    tokens[0] = first;
             
                }

                // could be a list...
                if (first == "{")
                {
                    first = "listEntry";
                    second = "=";
                    third = tokens[0];
                    tokens.Insert(0, "listEntry");
                    tokens.Insert(1, "=");
                    lineNumbers.Insert(0, lineNumbers[0]);
                    lineNumbers.Insert(1, lineNumbers[0]);


                }

                if ((!(second == "=" || second == ">" || second == "<" || second == ">=" || second == "<=" || second == "!=" || second == "==")) && second != "{")
                {
                    tokensForObject.Clear();
                    tokensForObject.Add(tokens[0]);
                    List<int> lineNumChild = new List<int>();
                    lineNumChild.Add(lineNumbers[0]);
                    ScriptParsedSegment parsable = new ScriptParsedSegment();
                    parsable.lineNumbers = lineNumChild;
                    parsable.filename = filename;
                    parsable.Parse(tokensForObject);
                    parsables.Add(parsable);
                    tokens.RemoveAt(0);
                    //  lineNumChild.Clear();
                    lineNumbers.RemoveAt(0);
                    token += 1;
                    continue;
                }

                else if (second == "=" || second == ">" || second == "<" || second == ">=" || second == "<=" || second == "!=" || second == "==")
                {
                }
                else
                {
                    // something weird going on - insert an = sign
            
                    {
                        // whoopsie, missed =...
                        tokens.Insert(1, "=");
                        first = tokens[0];
                        second = tokens[1];
                        third = tokens[2];
                        lineNumbers.Insert(1, lineNumbers[0]);
                    }

                }

                if (second == "=" || second == ">" || second == "<" || second == ">=" || second == "<=" || second == "!=" || second == "==")
                {
                    if (third == "{")
                    {
                        // this is a complex bracketed object... find the equivelent closing one...

                        int range = GetRangeOfClosedBracket(tokens, extraTokens);

                        if (range == -1)
                        {
                            tokens.Add("}");
                            lineNumbers.Add(lineNumbers.Last()+1);
                            range = tokens.Count-1;
                        }

                        tokensForObject = tokens.GetRange(0, range+1);

                        lineNumbersForObject = lineNumbers.GetRange(0, range + 1);
                        lineNumbers.RemoveRange(0, range + 1);
                        tokens.RemoveRange(0, range+1);
                    } else if (third.StartsWith("@["))
                    {
                        // weird inliney calculations in scriptvalues
                        int index = tokens.IndexOf("]");
                        tokensForObject = tokens.GetRange(0, index+1);

                        lineNumbersForObject = lineNumbers.GetRange(0, index + 1);
                        lineNumbers.RemoveRange(0, index + 1);


                        tokens.RemoveRange(0, index + 1);
                        token += index + 1;
                    }
                    else
                    {
                        int n = 3 + extraTokens;
                        
                        tokensForObject = tokens.GetRange(0, n);

                        lineNumbersForObject = lineNumbers.GetRange(0, n);
                        lineNumbers.RemoveRange(0, n);

                        tokens.RemoveRange(0, n);
                        token += n;
                    }
                }
                else
                {
                    // err.
                }

                {
                    ScriptParsedSegment parsable = new ScriptParsedSegment();
                    parsable.lineNumbers = lineNumbersForObject;
                    parsable.filename = filename;

                    if (parsable.lineNumbers[0] == 125 && filename.Equals("common/on_action/childhood_on_actions.txt"))
                    {

                    }
                        
                    parsable.Parse(tokensForObject);
                    parsables.Add(parsable);
                }

            }

            return parsables;
        }

        private int GetRangeOfClosedBracket(List<string> tokens, int extraTokens)
        {
            int depth = 0;
            for (int n = 2+extraTokens; n < tokens.Count; n++)
            {
                if (tokens[n] == "{")
                    depth++;

                if (tokens[n] == "}")
                {
                    depth--;
                    if (depth == 0)
                        return n;
                }

            }

            // error...
            return -1;
        }

        public void SetNamespace(string segmentValue)
        {
            ScriptNamespace = segmentValue;
            File.Namespace = segmentValue;
        }

        public string ScriptNamespace { get; set; }

      
    }
}
