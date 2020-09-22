#region


#endregion

namespace JominiParse
{
    public class ReferenceScriptValue : ScriptValue
    {
        public ReferenceScriptValue(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }

        public string Value { get; set; }

        public override string ToString()
        {
            return Name + " = " + Value;
        }
    }
}