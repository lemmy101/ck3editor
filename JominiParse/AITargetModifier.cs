using System.Linq;

namespace JominiParse
{
    public class AITargetModifier : ModifierBase
    {
        public ScriptObject CHARACTER { get; set; }
        public ScriptObject AI_VALUE { get; set; }
        public ScriptObject VALUE { get; set; }


        public override string ToScript()
        {
            string v = GetStart("ai_target_modifier");

            v += CHARACTER.ToScript() + "\n";
            v += AI_VALUE.ToScript() + "\n";
            v += VALUE.ToScript() + "\n";

            v += GetEnd();

            return v;
        }

        public void Parse(ScriptParsedSegment value, ScriptContext context)
        {
          
            var chr = value.children.Where(a => a.name == "CHARACTER").First();
            var aiv = value.children.Where(a => a.name == "AI_VALUE").First();
            var v = value.children.Where(a => a.name == "VALUE").First();
            CHARACTER = ScriptValueParser.Instance.ParseScriptValue(this, chr);
            AI_VALUE = ScriptValueParser.Instance.ParseScriptValue(this, aiv);
            VALUE = ScriptValueParser.Instance.ParseScriptValue(this, v);
        }

        public AITargetModifier(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}