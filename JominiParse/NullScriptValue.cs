namespace JominiParse
{
    public class NullScriptValue : ScriptValue
    {
        public override string ToString()
        {
            return "";
        }

        public NullScriptValue(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}