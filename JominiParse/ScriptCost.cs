using System.Collections.Generic;
using System.Linq;

namespace JominiParse
{
    public class ScriptCost : ScriptObject
    {
        public override string ToScript()
        {
            string str = GetStart(Name);

            foreach (var scriptValue in costs)
            {
                str += scriptValue.ToScript() + "\n";
            }

            str += GetEnd();

            return str;
        }

        List<ScriptValue> costs = new List<ScriptValue>();
        public void Parse(string name, List<ScriptParsedSegment> values)
        {
            Name = name;
            foreach (var scriptParsedSegment in values)
            {
                costs.Add(ParseCost(this, scriptParsedSegment));
            }

            LineStart = values.First().lineNumbers.First();
            LineEnd = values.Last().lineNumbers.Last();
        }

      
        private ScriptValue ParseCost(ScriptObject parent, ScriptParsedSegment segment)
        {
            return ScriptValueParser.Instance.ParseScriptValue(parent, segment);
        }

        public ScriptCost(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}