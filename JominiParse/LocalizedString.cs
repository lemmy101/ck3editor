using System.ComponentModel.Design;
using System.IO;

namespace JominiParse
{
    public class LocalizedString : ScriptObject
    {
        public string key;

        public override string ToScript()
        {
            return Name + " = " + key;
        }

        public override string ToString()
        {
            return Name + " = " + key + " (" + Core.Instance.GetLocalizedText(key) + ")";
        }

        public override void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
            base.Read(reader, file, parent);
            key = reader.ReadString();
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(key);
        }

        public LocalizedString(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }

        public string Localized()
        {
            return Core.Instance.GetLocalizedText(key);
        }
    }
}