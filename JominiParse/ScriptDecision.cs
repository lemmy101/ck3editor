using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JominiParse
{
    public class ScriptDecision : ScriptObject
    {
        public static List<String> validTokens = new List<string>();

        public override string ToScript()
        {
            return "";


        }

        public override void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
            base.Read(reader, file, parent);

        }

      
        public ScriptDecision(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}
