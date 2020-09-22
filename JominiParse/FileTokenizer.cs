#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#endregion

namespace JominiParse
{
    public class FileTokenizer
    {
        public static FileTokenizer Instance = new FileTokenizer();

        public ScriptFile file { get; set; }

        public string ScriptNamespace { get; set; }

        private string EnsureSpacing(string str, string test)
        {
            if (!str.Contains(test))
                return str;

            var split = str.Split(new[] {test}, StringSplitOptions.None);
            test = string.Join(" " + test + " ", split);

            return test;
        }

        private string EnsureNoSpacing(string str, string test)
        {
            if (!str.Contains(test))
                return str;

            var split2 = str.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

            str = string.Join("", split2);

            var split = str.Split(new[] {test}, StringSplitOptions.None);
            test = string.Join(test.Trim(), split);

            return test;
        }

        public List<ScriptObject> LoadDirectory(RefFilename directory, ScriptContext context, bool save, bool load)
        {
            var results = new List<ScriptObject>();
            if (!directory.Exists)
                return results;

            var directories = directory.GetDirectories();

            foreach (var s in directories) results.AddRange(LoadDirectory(s, context, save, load));

            var files = directory.GetFiles();

            foreach (var file in files)
                if (file.Extension == ".txt")
                {
                    var binFilename = file.ToRelativeFilename().Replace(".txt", ".bin");
                    binFilename = binFilename.Replace(Globals.CK3Path, "");
                    binFilename = Globals.CK3EdDataPath.Replace("\\", "/") + "CachedCK3Data/" + binFilename;
                    if (File.Exists(binFilename) && load)
                    {
                        ScriptNamespace = "";
                        results.AddRange(LoadParsableFile(binFilename));
                    }
                    else
                    {
                        results.AddRange(LoadFile(file, context, save));
                    }
                }

            return results;
        }

        public List<ScriptObject> LoadFile(RefFilename filename, ScriptContext context, bool save)
        {
            if (!filename.Exists)
                return new List<ScriptObject>();

            var text = File.ReadAllText(filename.ToFullFilename());
            return LoadText(text, filename, context, save);
        }

        public List<ScriptObject> LoadText(string text, RefFilename filename, ScriptContext context, bool save = false)
        {
            ScriptNamespace = "";
            var results = new List<ScriptObject>();


            file = Core.Instance.LoadingCK3Library.EnsureFile(filename, context);

            var lines = text.Split('\n');
            var lineNumbers = new List<int>();
            var tokens = new List<string>();
            var index = 0;
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                line = line.Trim();
                index++;

                if (line.Contains("debug_log"))
                    continue;
                if (line.Trim().StartsWith("#"))
                    continue;
                var line2 = line;
                if (line.Length == 0)
                    continue;
                if (line2 == "name = \"Abdallah\" #ibn Muhammad\"")
                {
                }

                if (line2.Contains("\"") && (line2.IndexOf("\"") < line2.IndexOf("#") || line2.IndexOf("#") == -1))
                {
                    var ll = line2;
                    var after = "";
                    while (ll.Contains("\""))
                        if (ll.Contains("\"") &&
                            (ll.IndexOf("\"") < ll.IndexOf("#") || ll.IndexOf("#") == -1))
                        {
                            var s = ll.IndexOf("\"");
                            var e = ll.IndexOf("\"", s + 1);
                            var l = e - s;
                            var before = ll.Substring(0, s);
                            var str = ll.Substring(s + 1, l - 1);
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

                    TokenizeLine(after.Trim(), tokens);


                    while (lineNumbers.Count < tokens.Count) lineNumbers.Add(index);
                }
                else
                {
                    TokenizeLine(line2, tokens);
                    while (lineNumbers.Count < tokens.Count) lineNumbers.Add(index);
                }
            }

            if (tokens.Count == 0)
                return results;

            var parsableResults = ParseTokens(tokens, lineNumbers, filename);

            if (save)
                SaveBinary(parsableResults, context, filename.ToRelativeFilename());

            ParseResults(null, parsableResults, context, results);

            return results;
        }

        public List<ScriptObject> LoadParsableFile(string s)
        {
            using (var reader = new BinaryReader(File.Open(s, FileMode.Open)))
            {
                var context = (ScriptContext) reader.ReadInt32();
                var filename = s.Substring(s.IndexOf("/CachedCK3Data/") + 15).Replace("\\", "/");

                var count = reader.ReadInt32();

                var parsables = new List<ScriptParsedSegment>();

                for (var x = 0; x < count; x++)
                {
                    var parsable = LoadParsable(reader, filename);
                    parsables.Add(parsable);
                }

                var results = new List<ScriptObject>();
                ParseResults(null, parsables, context, results);
                return results;
            }
        }

        private ScriptParsedSegment LoadParsable(BinaryReader reader, string filename)
        {
            var name = reader.ReadString();

            var lineStart = reader.ReadInt32();
            var lineEnd = reader.ReadInt32();
            var isBlock = reader.ReadBoolean();
            string op = null;
            if (reader.ReadBoolean())
                op = reader.ReadString();
            var valCount = reader.ReadByte();
            var values = new List<string>();
            for (var x = 0; x < valCount; x++) values.Add(reader.ReadString());

            var numChildren = reader.ReadInt32();

            var p = new ScriptParsedSegment();

            p.filename = new RefFilename(filename.Replace(".bin", ".txt"), true);
            p.name = name;
            p.op = op;
            p.value = values;
            p.lineNumbers.Add(lineStart);
            p.lineNumbers.Add(lineEnd);
            p.isBlock = isBlock;
            for (var x = 0; x < numChildren; x++) p.children.Add(LoadParsable(reader, filename));

            return p;
        }

        private void SaveBinary(List<ScriptParsedSegment> parsableResults, ScriptContext context, string filename)
        {
            var binFilename = filename.Replace(".txt", ".bin");

            binFilename = Globals.CK3EdDataPath.Replace("\\", "/") + "CachedCK3Data/" + binFilename;
            var binDir = binFilename.Substring(0, binFilename.LastIndexOf("/"));
            if (!Directory.Exists(binDir))
                Directory.CreateDirectory(binDir);

            using (var writer = new BinaryWriter(File.Open(binFilename, FileMode.Create)))
            {
                writer.Write((int) context);
                writer.Write(parsableResults.Count);

                foreach (var scriptParsedSegment in parsableResults) SaveParsable(writer, scriptParsedSegment);
            }
        }

        private void SaveParsable(BinaryWriter writer, ScriptParsedSegment parsableResults)
        {
            writer.Write(parsableResults.name);

            writer.Write(parsableResults.lineNumbers[0]);
            writer.Write(parsableResults.lineNumbers[parsableResults.lineNumbers.Count - 1]);

            writer.Write(parsableResults.isBlock);

            writer.Write(parsableResults.op != null);
            if (parsableResults.op != null)
                writer.Write(parsableResults.op);
            writer.Write((byte) parsableResults.value.Count);

            foreach (var s in parsableResults.value) writer.Write(s);

            writer.Write(parsableResults.children.Count);
            foreach (var parsableResultsChild in parsableResults.children) SaveParsable(writer, parsableResultsChild);
        }

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
                var linetokens = line2.Split(new[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);

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

                var r = ParseResult(parent, scriptParsedSegment, context);
                if (r != null) results.Add(r);
            }
        }

        private ScriptObject ParseResult(ScriptObject parent, ScriptParsedSegment parsableResult, ScriptContext context)
        {
            if (ScriptNamespace != null && context == ScriptContext.Activities)
            {
            }

            return ScriptObjectFactory.Instance.CreateScriptObject(context, parsableResult, parent, ScriptNamespace);
        }

        public ScriptObject ParseUnknown(ScriptObject parent, ScriptParsedSegment segment, ScriptContext context)
        {
            return ParseResult(parent, segment, context);
        }


        public List<ScriptParsedSegment> ParseTokens(List<string> tokens, List<int> lineNumbers, RefFilename filename)
        {
            var parsables = new List<ScriptParsedSegment>();
            var lineNumbersForObject = new List<int>();
            var tokensForObject = new List<string>();
            var token = 0;
            while (tokens.Count > 0)
            {
                if (tokens.Count == 1)
                {
                    tokensForObject.Clear();
                    tokensForObject.AddRange(tokens);
                    var lineNumChild = new List<int>();
                    lineNumChild.AddRange(lineNumbers);
                    var parsable = new ScriptParsedSegment();
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
                    var lineNumChild = new List<int>();
                    lineNumChild.Add(lineNumbers[0]);
                    var parsable = new ScriptParsedSegment();
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

                var first = tokens[0];
                var second = tokens[1];
                var third = tokens[2];

                var extraTokens = 0;
                if (first == "NOT" && lineNumbers[0] == 431)
                {
                }

                if (third == "hsv")
                {
                    third = string.Join("", tokens.GetRange(2, tokens.Count - 2));
                    lineNumbers.RemoveRange(2, tokens.Count - 3);
                    tokens.RemoveAt(2);
                    tokens.Insert(2, third);
                    tokens.RemoveRange(3, tokens.Count - 3);
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

                if (!(second == "=" || second == ">" || second == "<" || second == ">=" || second == "<=" ||
                      second == "!=" || second == "==") && second != "{")
                {
                    tokensForObject.Clear();
                    tokensForObject.Add(tokens[0]);
                    var lineNumChild = new List<int>();
                    lineNumChild.Add(lineNumbers[0]);
                    var parsable = new ScriptParsedSegment();
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

                if (second == "=" || second == ">" || second == "<" || second == ">=" || second == "<=" ||
                    second == "!=" || second == "==")
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

                var isBlock = false;
                if (second == "=" || second == ">" || second == "<" || second == ">=" || second == "<=" ||
                    second == "!=" || second == "==")
                {
                    if (third == "{")
                    {
                        // this is a complex bracketed object... find the equivelent closing one...

                        var range = GetRangeOfClosedBracket(tokens, extraTokens);

                        if (range == -1)
                        {
                            tokens.Add("}");
                            lineNumbers.Add(lineNumbers.Last() + 1);
                            range = tokens.Count - 1;
                        }

                        isBlock = true;

                        tokensForObject = tokens.GetRange(0, range + 1);

                        lineNumbersForObject = lineNumbers.GetRange(0, range + 1);
                        lineNumbers.RemoveRange(0, range + 1);
                        tokens.RemoveRange(0, range + 1);
                    }
                    else if (third.StartsWith("@["))
                    {
                        // weird inliney calculations in scriptvalues
                        var index = tokens.IndexOf("]");
                        tokensForObject = tokens.GetRange(0, index + 1);

                        lineNumbersForObject = lineNumbers.GetRange(0, index + 1);
                        lineNumbers.RemoveRange(0, index + 1);


                        tokens.RemoveRange(0, index + 1);
                        token += index + 1;
                    }
                    else
                    {
                        var n = 3 + extraTokens;

                        tokensForObject = tokens.GetRange(0, n);

                        lineNumbersForObject = lineNumbers.GetRange(0, n);
                        lineNumbers.RemoveRange(0, n);

                        tokens.RemoveRange(0, n);
                        token += n;
                    }
                }

                {
                    var parsable = new ScriptParsedSegment();
                    parsable.lineNumbers = lineNumbersForObject;
                    parsable.filename = filename;
                    parsable.isBlock = isBlock;
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
            var depth = 0;
            for (var n = 2 + extraTokens; n < tokens.Count; n++)
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
            file.Namespace = segmentValue;
        }
    }
}