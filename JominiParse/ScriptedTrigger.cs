namespace JominiParse
{
    public class ScriptedTrigger : ScriptObject
    {
        public ScriptedTrigger(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }

        public override string ToScript()
        {
            return "";
        }
    }
}