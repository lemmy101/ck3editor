using System.Linq;
using System.Text;

namespace JominiParse
{
    public class EventParser : ParserBase
    {
        public static EventParser Instance = new EventParser();

        
        public ScriptObject ParseEvent(ScriptObject parent, ScriptParsedSegment segment)
        {
          
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

            return d;

            /*
            ReadParsedChildSegment(d, segment, d, "type");
            ReadParsedChildSegment(d, segment, d, "scope");
            ReadParsedChildSegment(d, segment, d, "title");
            ReadParsedChildSegment(d, segment, d, "desc");
            ReadParsedChildSegment(d, segment, d, "theme");
            ReadParsedChildSegment(d, segment, d, "trigger");
            ReadParsedChildSegment(d, segment, d, "weight_multiplier");
            ReadParsedChildSegment(d, segment, d, "immediate");
            
            ReadParsedChildSegment(d, segment, d, "after");
            */
            var list = segment.children.Where(a => a.name == "option").ToList();

            foreach (var scriptParsedSegment in list)
            {
         //       d.options.Add(ParseOption(d, scriptParsedSegment));
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
           
            string toScript = effect.ToScript();
            return effect;
        }

        private ScriptObject ParseScriptedTrigger(ScriptObject parent, ScriptParsedSegment segment)
        {
            ScriptedTrigger trigger = new ScriptedTrigger(parent, segment);

            trigger.Name = segment.name.Split(' ')[1];
           
            string toScript = trigger.ToScript();
            return trigger;
        }
    }
}
