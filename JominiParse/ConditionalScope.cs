namespace JominiParse
{
    public class ConditionalScope : EffectBlock
    {
        public Limit Limit { get; set; }
     
        public override string ToScript()
        {
            string str = GetStart(Name);

            str += Limit.ToScript() + "\n";

            str += ToScriptInterior();

            str += GetEnd();

            return str;
        }

        public override string ToString()
        {
            return Name;
        }

        public override void Parse(ScriptParsedSegment value, ScriptContext context)
        {
            Name = value.name;

            Limit = new Limit(this, value);
            Limit.Parse(value.children[0], ScriptContext.EffectBlock);

            var c = value.children.GetRange(1, value.children.Count - 1);
            ParseChildren(c, context);

        }

        public ConditionalScope(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}