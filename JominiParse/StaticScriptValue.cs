#region

using System.IO;

#endregion

namespace JominiParse
{
    public class StaticScriptValue : ScriptValue
    {
        public StaticScriptValue(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }

        public float Value { get; set; }

        public override string ToString()
        {
            return Name + " = " + Value;
        }
    }
}