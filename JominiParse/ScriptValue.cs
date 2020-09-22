namespace JominiParse
{
    public abstract class ScriptValue : ScriptObject
    {
        protected ScriptValue(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }

        public string GetStringValue()
        {
            if (this is StaticScriptValue)
            {
                return (this as StaticScriptValue)?.Value.ToString();
            }
            if (this is ReferenceScriptValue)
            {
                return (this as ReferenceScriptValue)?.Value.ToString();
            }

            return null;
        }

    }
}