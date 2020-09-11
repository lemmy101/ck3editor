using System;
using System.Collections.Generic;

namespace JominiParse
{
    public class FormulaScriptValue : ScriptValue
    {
        public List<ScriptObject> Operations = new List<ScriptObject>();

        public override string ToScript()
        {
            string res = GetStart(Name);

            foreach (var scriptObject in Operations)
            {
                res += scriptObject.ToScript();
                res += "\n";
            }

            res += GetEnd();

            return res;
        }

        public override string ToString()
        {
            return Name + " - " + String.Join(" ", Operations);
        }

        public FormulaScriptValue(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}