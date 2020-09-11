namespace JominiParse
{
    public class DecisionParser : ParserBase
    {
        public static DecisionParser Instance = new DecisionParser();

        public ScriptDecision ParseDecision(ScriptObject parent, ScriptParsedSegment segment)
        {
            ScriptDecision d = new ScriptDecision(parent, segment);
            d.Name = segment.name;
            ReadParsedChildSegment(d, segment, d, "picture");
            ReadParsedChildSegment(d, segment, d, "desc");
            ReadParsedChildSegment(d, segment, d, "major");
            ReadParsedChildSegment(d, segment, d, "selection_tooltip");
            ReadParsedChildSegment(d, segment, d, "ai_check_interval");
            ReadParsedChildSegment(d, segment, d, "cooldown");
            ReadParsedChildSegment(d, segment, d, "is_shown");
            ReadParsedChildSegment(d, segment, d, "is_valid_showing_failures_only");
            ReadParsedChildSegment(d, segment, d, "is_valid");
            ReadParsedChildSegment(d, segment, d, "ai_will_do");
            ReadParsedChildSegment(d, segment, d, "ai_potential");
            ReadParsedChildSegment(d, segment, d, "cost");
            ReadParsedChildSegment(d, segment, d, "effect");


            string toScript = d.ToScript();

            return d;
        }
    }
}