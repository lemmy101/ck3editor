using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JominiParse
{
    public class EffectBase : ScriptObject
    {
        public EffectBase(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }

    }

    public class Effect : EffectBase
    {
        public override string Name
        {
            get { return Value?.Name; }
        }

        public ScriptValue Value { get; set; }

        public override string ToScript()
        {
            return Value.ToScript();
        }

        public override void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
            base.Read(reader, file, parent);

            Value = (ScriptValue) Children[0];
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
        }

        public override ScopeType GetScopeType()
        {
            return base.GetScopeType();
        }

        public override string ToString()
        {
            return Name + " - " + Value.ToString();
        }

        public Effect(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }

    public class IfEffectBlock : EffectBase
    {
        public If If { get; set; }

        public override string ToScript()
        {
            return If.ToScript();
        }

        public override void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
            base.Read(reader, file, parent);
            if (Parent != null)
            {
                SetScopeType(Parent.GetScopeType());
            }
        }

        public void Parse(List<ScriptParsedSegment> values, ScriptContext context)
        {
            if (Parent != null)
            {
                SetScopeType(Parent.GetScopeType());
            }
            If = IfParser.Instance.ParseIf(this, values, context);
            LineStart = values.First().lineNumbers.First();
            LineEnd = values.Last().lineNumbers.Last();
        }

        public IfEffectBlock(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }

    public class EffectBlock : EffectBase
    {
        public List<EffectBase> Effects = new List<EffectBase>();

        public override string ToScript()
        {
            string str = GetStart(Name);

            str += ToScriptInterior();
          
            str += GetEnd();

            return str;
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            
            writer.Write(Effects.Count);

            for (int x = 0; x < Effects.Count; x++)
                writer.Write(Children.IndexOf(Effects[x]));

          
        }

        public override void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
            base.Read(reader, file, parent);

            int nChildren = reader.ReadInt32();

            for (int x = 0; x < nChildren; x++)
            {
                int i = reader.ReadInt32();
                Effects.Add((EffectBase)Children[i]);
            }

        }
        public string ToScriptInterior()
        {
            String str = "";
            foreach (var effectBase in Effects)
            {
                str += effectBase.ToScript() + "\n";
            }

            return str;
        }

        public override string ToString()
        {
            return Name;
        }

        public virtual void Parse(ScriptParsedSegment value, ScriptContext context)
        {
            Name = value.name;
            if (Name == "after")
            {
               
            }

            ScopeManager.Instance.Add(this);

            ParseChildren(value.children, context);

        }

        protected void ParseChildren(List<ScriptParsedSegment> children, ScriptContext context)
        {
            for (var index = 0; index < children.Count; index++)
            {
                var ch = children[index];
                if (ch.name == "if")
                {
                    if (index + 1 == children.Count)
                    {
                        IfEffectBlock i = new IfEffectBlock(this, children[index]);
                        i.Parse(children.GetRange(index, 1), context);
                        Effects.Add(i);

                        continue;
                    }
                    for (int i2 = index + 1; i2 < children.Count; i2++)
                    {
                        var cht = children[i2];
                        if (cht.name != "else" && cht.name != "elseif" && cht.name != "else_if")
                        {
                            IfEffectBlock i = new IfEffectBlock(this, cht);
                            i.Parse(children.GetRange(index, i2-index), context);
                            Effects.Add(i);
                            index = i2-1;
                            break;
                        }

                        if (i2 == children.Count - 1)
                        {
                            IfEffectBlock i = new IfEffectBlock(this, cht);
                            i.Parse(children.GetRange(index, i2 - index), context);
                            Effects.Add(i);
                            index = i2 - 1;
                            return;
                        }
                    }
                }
                else
                    ParseChild(context, ch);
            }
        }

        public static EffectBase Instantiate(ScriptObject parent, ScriptContext context, ScriptParsedSegment ch)
        {
            EffectBase b = null;
            if (ch.children.Count > 0)
            {

                if (ch.children.Any(a => a.name == "limit"))
                {
                    b = new ConditionalScope(parent, ch);
                    (b as ConditionalScope).Parse(ch, context);
                }
                else
                {
                    if (!EffectCommandParser.Instance.Parse(parent, ch, context, out b))

                    {
                        b = new EffectBlock(parent, ch);
                        (b as EffectBlock).Parse(ch, context);
                    }
                }
            }
            else
            {

                if (!EffectCommandParser.Instance.Parse(parent, ch, context, out b))
                
                {
                    b = new Effect(parent, ch);
                    (b as Effect).Value = ScriptValueParser.Instance.ParseScriptValue(b, ch);

                }

            }

            return b;
        }
        protected void ParseChild(ScriptContext context, ScriptParsedSegment ch)
        {
            EffectBase b = null;
      
            b = (EffectBase)EffectParser.Instance.ParseEffect(this, ch);
            
           // b = Instantiate(this, context, ch);

            Effects.Add(b);

          
        }

        public EffectBlock(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }

}