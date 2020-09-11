using System.Collections.Generic;
using System.IO;

namespace JominiParse
{
    public class If : ConditionBase
    {
        public override string ToScript()
        {
            string str = GetStart(Name);
            str += Limit?.ToScript() + "\n";
            foreach (var scriptObject in Then)
            {
                str += scriptObject.ToScript() + "\n";
            }

            str += GetEnd() + "\n";

            if (Else != null)
            {
                str += Else.ToScript() + "\n";
            }

            return str + "\n";

        }

        public override void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
            base.Read(reader, file, parent);

            int nChildren = reader.ReadInt32();

            for (int x = 0; x < nChildren; x++)
            {
                int i = reader.ReadInt32();
                Then.Add(Children[i]);
            }

            if (reader.ReadBoolean())
            {
                int limitIndex = reader.ReadInt32();
                Limit = (Limit)Children[limitIndex];
            }
            if (reader.ReadBoolean())
            {
                this.ElseIndex = reader.ReadInt32();
           }
        }

        public override void PostRead()
        {
            base.PostRead();

            if(ElseIndex != -1)
                Else = (If)Parent.Children[ElseIndex];


        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(Then.Count);

            for (int x = 0; x < Then.Count; x++)
                writer.Write(Children.IndexOf(Then[x]));

            if (Limit != null)
            {
                writer.Write(true);
                int limitIndex = Children.IndexOf(Limit);
                writer.Write(limitIndex);
            }
            else
            {
                writer.Write(false);
            }

            if (Else != null)
            {
                writer.Write(true);
                writer.Write(Parent.Children.IndexOf(Else));
            }
            else
            {
                writer.Write(false);
            }
        }

        public Limit Limit { get; set; }
        public List<ScriptObject> Then = new List<ScriptObject>();
        private int ElseIndex = -1;
        public ScriptObject Else { get; set; }

        public If(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
            if(seg != null)
                Name = seg.name;
        }
    }
}