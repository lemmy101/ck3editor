namespace JominiParse
{
    public class EffectParser : ParserBase
    {
        public static EffectParser Instance = new EffectParser();


        public ScriptObject ParseEffect(ScriptObject parent, ScriptParsedSegment parsableResult)
        {
            EffectBase e = null;

            e = EffectBlock.Instantiate(parent, ScriptContext.EffectBlock, parsableResult);

            return e;
        }

        public ScriptObject ParseCondition(ScriptObject parent, ScriptParsedSegment parsableResult)
        {
            ConditionBase e = null;

            e = ConditionBlock.Instantiate(parent, ScriptContext.ConditionBlock, parsableResult);

            return e;
        }
    }
}