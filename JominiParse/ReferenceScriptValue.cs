namespace JominiParse
{
    public class ReferenceScriptValue : ScriptValue
    {
        public string Value { get; set; }

        public override string ToScript()
        {
            return Name + " = " + Value;
        }

        public override string ToString()
        {
            return Name + " = " + Value.ToString();
        }
      
        public ReferenceScriptValue(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}