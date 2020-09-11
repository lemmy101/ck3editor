using System.IO;

namespace JominiParse
{
    public class ModifierReference : ModifierBase
    {
        public override string ToScript()
        {
            return "";//Modifier.ToScript();
        }

        public override void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
            base.Read(reader, file, parent);
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
        }

        public ScriptValue Modifier { get; set; }
        public ModifierReference(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
        public void Parse(ScriptParsedSegment value, ScriptContext context)
        {
         //   Modifier = new ReferenceScriptValue(Parent, value);
            ScriptValueParser.Instance.ParseScriptValue(Parent, value);
        }


    }
}