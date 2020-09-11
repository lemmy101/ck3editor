using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JominiParse
{
    public class ValueModifier : ScriptObject
    {
        public ScriptObject Base { get; set; }
        public List<ModifierBase> Modifiers = new List<ModifierBase>();
        public override string ToScript()
        {
            return "";
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

        public override void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
            base.Read(reader, file, parent);
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
        }

        public void Parse(ScriptParsedSegment value, ScriptContext context)
        {
            Name = value.name;
           var b = value.children.First();
            /*
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

            }*/
        }


        public ValueModifier(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}