namespace JominiParse
{
    public class trigger_event
    {
        public string on_action { get; set; }
        public string id { get; set; }
        public string days_from { get; set; }
        public string days_to { get; set; }
        public ScriptObject Topmost { get; set; }
        public ScriptObject Function { get; set; }
    }
}