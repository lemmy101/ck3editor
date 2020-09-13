namespace JominiParse
{
    public class trigger_event : ScriptObject
    {
        public ScriptValue id { get; set; }
        public int days { get; set; }
        public ScriptObject Topmost { get; set; }

        public trigger_event(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}