using System;

namespace JominiParse
{
    public class ScriptedTrigger : ScriptObject
    {
    
        public override string ToScript()
        {
            return "";
        }

        public ScriptedTrigger(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {

        }
    }
}