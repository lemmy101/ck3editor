namespace JominiParse
{
    public class ScriptActivity : ScriptObject
    {
        public override string ToScript()
        {
            string str = GetStart(Name);

            str += expiration_days?.ToScript() + "\n";
            str += solo?.ToScript() + "\n";
            str += frame?.ToScript() + "\n";
            str += color?.ToScript() + "\n";
            str += on_spawn?.ToScript() + "\n";
            str += on_activate?.ToScript() + "\n";
            str += on_complete?.ToScript() + "\n";
            str += on_expire?.ToScript() + "\n";

            str += GetEnd();

            return TabFormat(str);
        }
        public ScriptValue expiration_days { get; set; }
        public ScriptValue solo { get; set; }
        public ScriptValue frame { get; set; }
        public ScriptValue color { get; set; }
        public EffectBlock on_spawn { get; set; }
        public EffectBlock on_activate { get; set; }
        public EffectBlock on_complete { get; set; }
        public EffectBlock on_expire { get; set; }

        public ScriptActivity(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }

    public class ScriptActivityParser : ParserBase
    {
        public static ScriptActivityParser Instance = new ScriptActivityParser();

        public ScriptObject ParseActivity(ScriptObject parent, ScriptParsedSegment segment)
        {
            ScriptActivity d = new ScriptActivity(parent, segment);
            d.Name = segment.name;
            ReadParsedChildSegment(d, segment, d, "expiration_days");
            ReadParsedChildSegment(d, segment, d, "solo");
            ReadParsedChildSegment(d, segment, d, "frame");
            ReadParsedChildSegment(d, segment, d, "color");
            ReadParsedChildSegment(d, segment, d, "on_spawn");
            ReadParsedChildSegment(d, segment, d, "on_activate");
            ReadParsedChildSegment(d, segment, d, "on_complete");
            ReadParsedChildSegment(d, segment, d, "on_expire");

            string f = d.ToScript();

            return d;
        }
    }
}