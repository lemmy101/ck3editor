using System;
using System.Collections.Generic;
using System.IO;

namespace JominiParse
{
    public class ScriptEvent : ScriptObject
    {
      
        public override string ToScript()
        {
            return "";
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
        public override void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
            base.Read(reader, file, parent);

            type = (ScriptValue)FindChild("type");
            scope = (ScriptValue)FindChild("scope");
            title = (LocalizedString)FindChild("title");
            desc = (LocalizedString)FindChild("desc");
            theme = (ScriptValue)FindChild("theme");
            trigger = (ConditionBlock)FindChild("trigger");
            weight_multiplier = (ValueModifier)FindChild("weight_multiplier");
            immediate = (EffectBlock)FindChild("immediate");
            after = (EffectBlock)FindChild("after");

            /*
            ReadParsedChildSegment(d, segment, d, "type");
            ReadParsedChildSegment(d, segment, d, "scope");
            ReadParsedChildSegment(d, segment, d, "title");
            ReadParsedChildSegment(d, segment, d, "desc");
            ReadParsedChildSegment(d, segment, d, "theme");
            ReadParsedChildSegment(d, segment, d, "trigger");
            ReadParsedChildSegment(d, segment, d, "weight_multiplier");
            ReadParsedChildSegment(d, segment, d, "immediate");
            */

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