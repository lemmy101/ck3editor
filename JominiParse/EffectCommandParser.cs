using System;
using System.Collections.Generic;
using System.Text;
using JominiParse.EffectCommands;

namespace JominiParse
{
    namespace EffectCommands
    {
        public class EffectCommand : EffectBase
        {
            public override string ToString()
            {
                return Name;
            }

            public EffectCommand(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
            {
            }
        }
        public class trigger_event : EffectCommand
        {
            public override string ToString()
            {
                if(id != null)
                    return "trigger_event: " + id.GetStringValue();
                else
                    return "trigger_event: " + on_action.GetStringValue();
            }

            public ScriptValue id { get; set; }
            public ScriptValue on_action { get; set; }
            public ScriptValue days { get; set; }
            
            public trigger_event(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
            {
            }
        }
    }
  

    public class EffectCommandParser : ParserBase
    {
        public static EffectCommandParser Instance = new EffectCommandParser();

        public EffectBase Create_trigger_event(ScriptObject parent, ScriptParsedSegment seg, ScriptContext context)
        {
            trigger_event e = new trigger_event(parent, seg);

            if (seg.children.Count > 0)
            {
                ReadParsedChildSegment(e, seg, e, "id");
                ReadParsedChildSegment(e, seg, e, "days");
                ReadParsedChildSegment(e, seg, e, "on_action");

            }
            else
            {
                e.id = ScriptValueParser.Instance.ParseScriptValue(parent, seg);
                
            }

            if(e.id != null)
                Core.Instance.LoadingCK3Library.RegisterTrigger(e);

            return e;
        }

        public bool Parse(ScriptObject parent, ScriptParsedSegment ch, ScriptContext context,
            out EffectBase effectBase)
        {
            effectBase = null;

            if (ch.name == "trigger_event")
            {
                effectBase = Create_trigger_event(parent, ch, context);
                return true;
            }

            return false;
        }
     
    }
}
