namespace JominiParse
{
    public class StaticScriptValue : ScriptValue
    {
        public float Value { get; set; }
        public override string ToScript()
        {
            return Name + " = " + Value;
        }

        public override string ToString()
        {
            return Name + " = " + Value.ToString();
        }

        public StaticScriptValue(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}