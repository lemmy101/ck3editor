using System.Collections.Generic;

namespace JominiParse
{
    public class ScriptedModifier : ScriptObject
    {
        public List<ModifierBase> Modifiers = new List<ModifierBase>();
        public override string ToScript()
        {
            string v = GetStart(Name);
            
            foreach (var modifierBase in Modifiers)
            {
                v += modifierBase.ToScript() + "\n";
            }

            v += GetEnd();

            return TabFormat(v);
        }

        public ScriptedModifier(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}