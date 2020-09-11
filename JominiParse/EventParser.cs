using System.Linq;
using System.Text;

namespace JominiParse
{
    public class EventParser : ParserBase
    {
        public static EventParser Instance = new EventParser();

        public EventOption ParseOption(ScriptObject parent, ScriptParsedSegment segment)
        {
            EventOption option = new EventOption(parent, segment);

            ReadParsedChildSegment(option, segment, option, "name");
            ReadParsedChildSegment(option, segment, option, "trigger");
            ReadParsedChildSegment(option, segment, option, "ai_chance");

            segment.children.RemoveAll(a => a.name == "trigger" || a.name == "name" || a.name == "ai_chance");


            EffectBlock b = new EffectBlock(parent, segment);
            b.Parse(segment, ScriptContext.EffectBlock);
            option.Effect = b;

            return option;
        }

        public ScriptObject ParseEvent(ScriptObject parent, ScriptParsedSegment segment)
        {
            if (segment.name == "feast_default.0001")
            {

            }
            if (segment.name.StartsWith("scripted_trigger"))
            {
                return ParseScriptedTrigger(parent, segment);
            }

            if (segment.name.StartsWith("scripted_effect"))
            {
                return ParseScriptedEffect(parent, segment);
            }


            ScriptEvent d = new ScriptEvent(parent, segment);
            d.Name = segment.name;
         
            d.Namespace = FileTokenizer.Instance.ScriptNamespace;
            ReadParsedChildSegment(d, segment, d, "type");
            ReadParsedChildSegment(d, segment, d, "scope");
            ReadParsedChildSegment(d, segment, d, "title");
            ReadParsedChildSegment(d, segment, d, "desc");
            ReadParsedChildSegment(d, segment, d, "theme");
            ReadParsedChildSegment(d, segment, d, "trigger");
            ReadParsedChildSegment(d, segment, d, "weight_multiplier");
            ReadParsedChildSegment(d, segment, d, "immediate");

            ReadParsedChildSegment(d, segment, d, "after");

            var list = segment.children.Where(a => a.name == "option").ToList();

            foreach (var scriptParsedSegment in list)
            {
                d.options.Add(ParseOption(d, scriptParsedSegment));
            }

            string toScript = d.ToScript();

            return d;
        }

        private ScriptObject ParseScriptedEffect(ScriptObject parent, ScriptParsedSegment segment)
        {
            ScriptedEffect effect = new ScriptedEffect(parent, segment);

            effect.Name = segment.name.Split(' ')[1];
            if (effect.Name == "on_trigger_fail_full_notifications_effect")
            {

            }
            effect.Effect = new EffectBlock(effect, segment);
            effect.Effect.Parse(segment, ScriptContext.EffectBlock);
            string toScript = effect.ToScript();
            return effect;
        }

        private ScriptObject ParseScriptedTrigger(ScriptObject parent, ScriptParsedSegment segment)
        {
            ScriptedTrigger trigger = new ScriptedTrigger(parent, segment);

            trigger.Name = segment.name.Split(' ')[1];
            trigger.Condition = new ConditionBlock(trigger, segment);
            trigger.Condition.Parse(segment, ScriptContext.ConditionBlock);
            string toScript = trigger.ToScript();
            return trigger;
        }
    }
}
