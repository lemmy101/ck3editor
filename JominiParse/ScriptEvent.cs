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
        }
        public override void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
            base.Read(reader, file, parent);


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

        public override void OnPostInitializeChild(ScriptObject child)
        {
            base.OnPostInitializeChild(child);

            if(child.Name == "type")
                if ((child as ScriptValue).GetStringValue() == "character_event")
                {
                    SetScopeType(ScopeType.character);
                }
        }

        public ScriptEvent(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }

        private ScopeType eventScopeType = ScopeType.character;
        public override ScopeType GetScopeType()
        {
            return eventScopeType;
        }

        public override void SetScopeType(ScopeType type)
        {
            eventScopeType = type;
        }

   
    }
}