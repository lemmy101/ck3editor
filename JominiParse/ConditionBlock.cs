using System;
using System.Collections.Generic;
using System.Linq;

namespace JominiParse
{
    public class TriggerBlock : ConditionBlock
    {
        public TriggerBlock(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
    public class ConditionBlock : ConditionBase
    {
    
        public List<ConditionBase> conditions = new List<ConditionBase>();
        public override string ToScript()
        {
            string str = GetStart(Name);


            str += ToScriptInterior() + "\n";

            str += GetEnd();

            return str;
        }


        public string ToScriptInterior()
        {
            String str = "";
       
            foreach (var condition in conditions)
            {
                str += condition.ToScript() + "\n";
            }
          
            return str;
        }

        public override string ToString()
        {
            return Name;
        }

        public void Parse(string name, List<ScriptParsedSegment> segments, ScriptContext context)
        {
            Name = name;

            for (var index = 0; index < segments.Count; index++)
            {
                var scriptParsedSegment = segments[index];
                if (scriptParsedSegment.value.Count > 0)
                {
                    Condition c = new Condition(this, scriptParsedSegment);
                    c.test = scriptParsedSegment.name;
                    c.op = scriptParsedSegment.op;
                    c.val = scriptParsedSegment.value[0];
                    ScopeManager.Instance.Add(c);
                    conditions.Add(c);
                }
                else
                {
                 
                    {
                        ConditionBlock b = new ConditionBlock(this, scriptParsedSegment);

                        b.Parse(scriptParsedSegment, context);
                        conditions.Add(b);
                    }
                }
            }
        }

        public void Parse(ScriptParsedSegment segment, ScriptContext context)
        {
            Name = segment.name;
            if(Name == "NOT" && segment.children.Count==0)
            {

            }
            ScopeManager.Instance.Add(this);
            var children = segment.children;
            for (var index = 0; index < segment.children.Count; index++)
            {
                var scriptParsedSegment = segment.children[index];
                if (scriptParsedSegment.value.Count > 0)
                {
                    Condition c = new Condition(this, scriptParsedSegment);
                    c.test = scriptParsedSegment.name;
                    c.op = scriptParsedSegment.op;
                    if (scriptParsedSegment.value == null || scriptParsedSegment.value.Count == 0)
                        continue;
                    c.val = scriptParsedSegment.value[0];
                    ScopeManager.Instance.Add(c);
                    conditions.Add(c);
                }
                else 
                {

          
                    if (scriptParsedSegment.name == "trigger_if")
                    {
                        if (scriptParsedSegment.filename.Contains("breakin_"))
                        {

                        }
                        if (index + 1 == children.Count)
                        {
                            If i = null;
                            i = IfParser.Instance.ParseIf(this, children.GetRange(index, 1), context);
                            conditions.Add(i);

                            continue;
                        }
                        for (int i2 = index + 1; i2 < children.Count; i2++)
                        {
                            var cht = children[i2];
                            if (cht.name != "trigger_else" && cht.name != "trigger_elseif" && cht.name != "trigger_else_if")
                            {
                                If i = null;
                                i = IfParser.Instance.ParseIf(this, children.GetRange(index, i2 - index), context);
                                conditions.Add(i);
                                index = i2 - 1;
                                break;
                            }

                            if (i2 == children.Count - 1)
                            {
                                If i = null;
                                i = IfParser.Instance.ParseIf(this, children.GetRange(index, i2 - index), context);
                                conditions.Add(i);
                                index = i2 - 1;
                                return;
                            }
                        }
                    }
                    else
                    {
                        ConditionBlock b = new ConditionBlock(this, scriptParsedSegment);

                        b.Parse(scriptParsedSegment, context);
                        conditions.Add(b);

                    }
                }
            }
        }

        public ConditionBlock(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }

        public static ConditionBase Instantiate(ScriptObject parent, ScriptContext context, ScriptParsedSegment scriptParsedSegment)
        {
            ConditionBase b = null;

            if (scriptParsedSegment.name == "NOT" && scriptParsedSegment.children.Count == 0)
            {

            }

            if (scriptParsedSegment.value.Count > 0)
            {
                var c = new Condition(parent, scriptParsedSegment);

                c.test = scriptParsedSegment.name;
                c.op = scriptParsedSegment.op;
                if (scriptParsedSegment.value == null || scriptParsedSegment.value.Count == 0)
                    return b;
                c.val = scriptParsedSegment.value[0];
                ScopeManager.Instance.Add(c);
            
                 b = c;
            }
            else
            {
                ConditionBlock c = new ConditionBlock(parent, scriptParsedSegment);

                c.Parse(scriptParsedSegment, context);

                b = c;
            }

            return b;
        }
    }
}