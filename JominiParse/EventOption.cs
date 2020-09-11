using System.IO;

namespace JominiParse
{
    public class EventOption : ScriptObject
    {
        public LocalizedString name { get; set; }
        public ConditionBlock trigger { get; set; }
        public ValueModifier ai_chance { get; set; }
        public EffectBlock Effect { get; set; }
        public override string ToString()
        {
            if (name == null)
                return "option";

            return "option: " + name.ToString();
        }

       

        public override string ToScript()
        {
            string str = GetStart("option");

            str += name?.ToScript() + "\n";

            str += trigger?.ToScript() + "\n";

            str += ai_chance?.ToScript() + "\n";

            str += Effect?.ToScriptInterior() + "\n";

            str += GetEnd();

            return TabFormat(str);
        }

        public EventOption(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {

        }
    }
}