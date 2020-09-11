namespace JominiParse
{
    public class ModifierReference : ModifierBase
    {
        public override string ToScript()
        {
            return Modifier.ToScript();
        }

        public ScriptValue Modifier { get; set; }
        public ModifierReference(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
        public void Parse(ScriptParsedSegment value, ScriptContext context)
        {
            Modifier = new ReferenceScriptValue(Parent, value);
            ScriptValueParser.Instance.ParseScriptValue(Parent, value);
        }


    }
}