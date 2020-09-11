namespace JominiParse
{
    public class Condition : ConditionBase
    {
        public string test { get; set; }
        public string op { get; set; }
        public string val { get; set; }

        public override string Name
        {
            get { return test; }
        }

        public override string ToScript()
        {
            return test + " " + op + " " + val;
        }

        public override string ToString()
        {
            return test + " " + op + " " + val;
        }

        public Condition(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}