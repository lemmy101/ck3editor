using System.IO;
using System.Linq;

namespace JominiParse
{
    public class ScriptActivity : ScriptObject
    {
        public override string ToScript()
        {
            return "";
        }


        public ScriptActivity(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }

        public override void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
            base.Read(reader, file, parent);

        }

    }

    public class ScriptActivityParser : ParserBase
    {
        public static ScriptActivityParser Instance = new ScriptActivityParser();

        public ScriptObject ParseActivity(ScriptObject parent, ScriptParsedSegment segment)
        {
            ScriptActivity d = new ScriptActivity(parent, segment);
            d.Name = segment.name;
        

            string f = d.ToScript();

            return d;
        }
    }
}