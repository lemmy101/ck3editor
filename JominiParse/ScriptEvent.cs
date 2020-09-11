using System;
using System.Collections.Generic;

namespace JominiParse
{
    public class ScriptEvent : ScriptObject
    {
      
        public override string ToScript()
        {
            string str = GetStart(Name);

            str += type?.ToScript() + "\n";
            str += title?.ToScript() + "\n";
            str += desc?.ToScript() + "\n";
            str += theme?.ToScript() + "\n";
            str += trigger?.ToScript() + "\n";
            str += weight_multiplier?.ToScript() + "\n";
            str += immediate?.ToScript() + "\n";

            foreach (var eventOption in options)
            {
                str += eventOption.ToScript() + "\n";

            }

            str += after?.ToScript() + "\n";

            str += GetEnd();

            return TabFormat(str);
        }

        public ScriptEvent(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }

   
        public List<EventOption> options = new List<EventOption>();

        public LocalizedString title { get; set; }
        public ConditionBlock trigger { get; set; }
        public LocalizedString desc { get; set; }

        public ValueModifier weight_multiplier { get; set; }
        public EffectBlock immediate { get; set; }
        public EffectBlock after { get; set; }
        public ScriptValue type { get; set; }
        public ScriptValue scope { get; set; }
        public ScriptValue theme { get; set; }
   
    }
}