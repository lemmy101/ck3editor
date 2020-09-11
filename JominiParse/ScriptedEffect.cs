using System;

namespace JominiParse
{
    public class ScriptedEffect : ScriptObject
    {
        public EffectBlock Effect { get; set; }

        public override string ToScript()
        {
            string str = GetStart("scripted_effect " + Name);

            str += Effect?.ToScriptInterior() + "\n";

            str += GetEnd();

            return TabFormat(str);
        }

        public ScriptedEffect(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {

        }
    }
}