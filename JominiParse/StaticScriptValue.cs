#region

using System.IO;

#endregion

namespace JominiParse
{
    public class StaticScriptValue : ScriptValue
    {
        public StaticScriptValue(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }

        public float Value { get; set; }

        public override string ToScript()
        {
            return Name + " = " + Value;
        }

        public override string ToString()
        {
            return Name + " = " + Value;
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);

            writer.Write(Value);
        }

        public override void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
            base.Read(reader, file, parent);

            Value = reader.ReadSingle();
        }
    }
}