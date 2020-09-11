using System;

namespace JominiParse
{
    public class ScriptedTrigger : ScriptObject
    {
        public ConditionBlock Condition { get; set; }

        public override string ToScript()
        {
            string str = GetStart("scripted_trigger " + Name);

            str += Condition?.ToScriptInterior() + "\n";

            str += GetEnd();

            return TabFormat(str);
        }

        public ScriptedTrigger(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {

        }
    }
}