using System.Linq;

namespace JominiParse
{
    public class Modifier : ModifierBase
    {
        public ScriptObject Factor { get; set; }
        public ConditionBlock ConditionBlock { get; set; }


        public override string ToScript()
        {
            string v = GetStart("modifier");

            v += Factor.ToScript() + "\n";

            v += ConditionBlock.ToScriptInterior();

            v += GetEnd();

            return v;
        }

        public void Parse(ScriptParsedSegment value, ScriptContext context)
        {
            var b = value.children.First();

            Factor = ScriptValueParser.Instance.ParseScriptValue(this, b);


            ConditionBlock c = new ConditionBlock(this, value);

            c.Parse(value.name, value.children.GetRange(1, value.children.Count - 1), ScriptContext.ConditionBlock);

            ConditionBlock = c;
        }

        public Modifier(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}