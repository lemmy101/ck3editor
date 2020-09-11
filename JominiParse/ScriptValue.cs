namespace JominiParse
{
    public abstract class ScriptValue : ScriptObject
    {
       public LocalizedString desc { get; set; }
     
       public override string ToScript()
        {
            return GetStart(Name) + @"
            " + desc?.ToScript() + @"
            " + GetEnd();
        }

        public int GetIntValue()
        {
            if (this is StaticScriptValue)
            {
                return (int)(this as StaticScriptValue).Value;
            }

            return 0;
        }
        public float GetFloatValue()
        {
            if (this is StaticScriptValue)
            {
                return (this as StaticScriptValue).Value;
            }

            return 0;
        }
        public string GetStringValue()
        {
            if (this is StaticScriptValue)
            {
                return (this as StaticScriptValue).Value.ToString();
            }
            if (this is ReferenceScriptValue)
            {
                return (this as ReferenceScriptValue).Value.ToString();
            }

            return null;
        }

        protected ScriptValue(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}