#region

using System.Collections.Generic;

#endregion

namespace JominiParse
{
    public class ScriptParsedSegment
    {
        public List<ScriptParsedSegment> children = new List<ScriptParsedSegment>();
        public RefFilename filename;
        public bool isBlock;
        public List<int> lineNumbers = new List<int>();
        public string name;
        public string op;
        public List<string> value = new List<string>();

        public override string ToString()
        {
            if (children.Count > 0) return name + " " + op + " " + " { " + string.Join(" ", children) + "}";

            return name + " " + op + " " + string.Join(" ", value);
        }

        public void Parse(List<string> tokensForObject)
        {
            if (tokensForObject.Contains("{"))
            {
                var i = tokensForObject.IndexOf("{");
                var c = FileTokenizer.Instance.ParseTokens(
                    tokensForObject.GetRange(i + 1, tokensForObject.Count - (i + 2)),
                    lineNumbers.GetRange(i + 1, lineNumbers.Count - (i + 2)), filename);
                name = tokensForObject[0];
                op = tokensForObject[1];
                children.AddRange(c);
            }
            else if (tokensForObject.Count >= 3)
            {
                name = tokensForObject[0];
                op = tokensForObject[1];
                value = tokensForObject.GetRange(2, tokensForObject.Count - 2);
            }
            else if (tokensForObject.Count == 2)
            {
                value = tokensForObject.GetRange(0, 2);
            }
            else
            {
                name = tokensForObject[0];
            }
        }
    }
}