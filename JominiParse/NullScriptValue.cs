namespace JominiParse
{
    public class NullScriptValue : ScriptValue
    {
        public NullScriptValue(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }

        public override string ToString()
        {
            return "";
        }
    }
}