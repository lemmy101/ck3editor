using System.Collections.Generic;

namespace JominiParse
{
    public class If : ConditionBase
    {
        public override string ToScript()
        {
            string str = GetStart(Name);
            str += Limit?.ToScript() + "\n";
            foreach (var scriptObject in Then)
            {
                str += scriptObject.ToScript() + "\n";
            }

            str += GetEnd() + "\n";

            if (Else != null)
            {
                str += Else.ToScript() + "\n";
            }

            return str + "\n";

        }

        public Limit Limit { get; set; }
        public List<ScriptObject> Then = new List<ScriptObject>();
        public ScriptObject Else { get; set; }

        public If(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
            Name = seg.name;
        }
    }
}