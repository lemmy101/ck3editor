using System.Collections.Generic;
using System.Linq;

namespace JominiParse
{
    public class ValueModifier : ScriptObject
    {
        public ScriptObject Base { get; set; }
        public List<ModifierBase> Modifiers = new List<ModifierBase>();
        public override string ToScript()
        {
            string v = GetStart(Name);

            v += Base.ToScript() + "\n";

            foreach (var modifier in Modifiers)
            {
                v += modifier.ToScript() + "\n";
            }

            v += GetEnd();

            return v;
        }

        public override string ToString()
        {
            return Name;
        }

        public void Parse(ScriptParsedSegment value, ScriptContext context)
        {
            Name = value.name;
           var b = value.children.First();

           Base = ScriptValueParser.Instance.ParseScriptValue(this, b);

           for (int i = 1; i < value.children.Count; i++)
           {
               if (value.name == "modifier")
               {
                   Modifier m = new Modifier(this, value.children[i]);

                   m.Parse(value.children[i], ScriptContext.Modifier);

                   Modifiers.Add(m);
               }
               if (value.name == "ai_target_modifier")
               {
                   AITargetModifier m = new AITargetModifier(this, value.children[i]);

                   m.Parse(value.children[i], ScriptContext.Modifier);

                   Modifiers.Add(m);
                }
                else {
                   ModifierReference m = new ModifierReference(this, value.children[i]);

                   m.Parse(value.children[i], ScriptContext.Modifier);

                   Modifiers.Add(m);
               }

            }
        }


        public ValueModifier(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}