using System;
using System.IO;

namespace JominiParse
{
    public class ScriptedEffect : ScriptObject
    {
        
        public override string ToScript()
        {
            return "";
        }
        public override void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
            base.Read(reader, file, parent);
        

        }
        public ScriptedEffect(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {

        }
    }
}