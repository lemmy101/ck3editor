namespace JominiParse
{
    public class ScriptedModifierParser
    {
        public static ScriptedModifierParser Instance = new ScriptedModifierParser();

        public ScriptObject ParseModifier(ScriptObject Parent, ScriptParsedSegment segment)
        {
            ScriptedModifier m = new ScriptedModifier(Parent, segment);

            m.Name = segment.name;

            foreach (var scriptParsedSegment in segment.children)
            {
                if (scriptParsedSegment.name == "modifier")
                {
                    var mm = new Modifier(m, scriptParsedSegment);
                    mm.Parse(scriptParsedSegment, ScriptContext.Modifier);
                    m.Modifiers.Add(mm);

                }
                else if (scriptParsedSegment.name == "ai_target_modifier")
                {
                    var mm = new AITargetModifier(m, scriptParsedSegment);
                    mm.Parse(scriptParsedSegment, ScriptContext.Modifier);
                    m.Modifiers.Add(mm);

                }
            }

            string toString = m.ToScript();

            return m;
        }
    }
}