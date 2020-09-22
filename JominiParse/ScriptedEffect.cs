#region

using System.IO;

#endregion

namespace JominiParse
{
    public class ScriptedEffect : ScriptObject
    {
        public ScriptedEffect(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }

        public override string ToScript()
        {
            return "";
        }

        public override void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
            base.Read(reader, file, parent);
        }
    }
}