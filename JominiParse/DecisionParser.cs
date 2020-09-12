namespace JominiParse
{
    public class DecisionParser : ParserBase
    {
        public static DecisionParser Instance = new DecisionParser();

        public ScriptDecision ParseDecision(ScriptObject parent, ScriptParsedSegment segment)
        {
            ScriptDecision d = new ScriptDecision(parent, segment);
            d.Name = segment.name;
       
          
            return d;
        }
    }
}