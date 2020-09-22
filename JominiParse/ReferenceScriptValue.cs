#region

using System.IO;

#endregion

namespace JominiParse
{
    public class ReferenceScriptValue : ScriptValue
    {
        public ReferenceScriptValue(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }

        public string Value { get; set; }

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

            if (Value != null)
            {
                writer.Write(true);
                writer.Write(Value);
            }
            else
            {
                writer.Write(false);
            }
        }

        public override void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
            base.Read(reader, file, parent);

            if (reader.ReadBoolean())
                Value = reader.ReadString();
        }
    }
}