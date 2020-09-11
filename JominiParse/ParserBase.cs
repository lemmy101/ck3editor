using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace JominiParse
{
 

    public class ParserBase
    {
        public void ReadParsedSegment<T>(ScriptObject parent, ScriptParsedSegment segment, T targetObject)
        {
            ReadParsedSegment(parent, segment, segment.name, segment.value[0], targetObject, segment.name);

        }
        public void ReadParsedChildSegment<T>(ScriptObject parento, ScriptParsedSegment parent, T targetObject, string targetProperty)
        {
      
            var cc = parent.children.Where(a => a.name == targetProperty);

            if (cc.Count() > 0)
            {
                var c = cc.First();
                if (c != null)
                {

                    if (!ReadParsedSegmentBlock(parento, c.name, c, targetObject, targetProperty))
                    {

                        if (c.value.Count > 0)
                            ReadParsedSegment(parento, c, c.name, c.value[0], targetObject, targetProperty);
                        else
                        {

                            // we have children...
                            ReadParsedSegment(parento, c.name, c.children, targetObject, targetProperty);
                        }
                    }

                }

            }

        }

        bool isCompatible(Type a, Type b)
        {
            return a.IsSubclassOf(b) || a == b;
        }
        public bool ReadParsedSegmentBlock<T>(ScriptObject parent, string name, ScriptParsedSegment value, T targetObject,
            string targetProperty)
        {
            var property = targetObject.GetType().GetProperty(targetProperty);

            object val = null;

            if (isCompatible(property.PropertyType, typeof(ConditionBlock)))
            {
                val = ParseConditionBlock(parent, name, value);
            }

            if (isCompatible(property.PropertyType, typeof(ValueModifier)))
            {
                val = ParseAIValue(parent, name, value);
            }
            if (isCompatible(property.PropertyType, typeof(EffectBlock)))
            {
                val = ParseEffectBlock(parent, name, value);
            }

            if (property != null && val != null)
            {
                property.SetValue(targetObject, val);
                return true;
            }

            return false;
        }


        public void ReadParsedSegment<T>(ScriptObject parent, string name, List<ScriptParsedSegment> values, T targetObject, string targetProperty)
        {
            if (values.Count == 0)
                return;

            var property = targetObject.GetType().GetProperty(targetProperty);

            object val = null;

            if (isCompatible(property.PropertyType, typeof(ScriptPeriod)))
            {
                val = ParsePeriod(parent, name, values);
            }
            else if (isCompatible(property.PropertyType, typeof(ScriptCost)))
            {
                
                val = ParseCost(parent, name, values);
            }
          

            if (property != null)
                property.SetValue(targetObject, val);


        }

        private ScriptPeriod ParsePeriod(ScriptObject parent, string name, List<ScriptParsedSegment> values)
        {
            ScriptPeriod p = new ScriptPeriod(parent, values[0]);
            p.Name = name;
            if (values[0].name == "months")
            {
                p.value = ScriptValueParser.Instance.CreateScriptValue(p, values[0], name, values[0].value[0]);
                p.periodType = Period.months;
            }
            if (values[0].name == "years")
            {
                p.value = ScriptValueParser.Instance.CreateScriptValue(p, values[0], name, values[0].value[0]);
                p.periodType = Period.years;
            }
            if (values[0].name == "days")
            {
                p.value = ScriptValueParser.Instance.CreateScriptValue(p, values[0], name, values[0].value[0]);
                p.periodType = Period.days;
            }

            return p;
        }
        private ScriptCost ParseCost(ScriptObject parent, string name, List<ScriptParsedSegment> values)
        {

            ScriptCost p = new ScriptCost(parent, values[0]);

            p.Parse(name, values);

            return p;
        }
        private EffectBlock ParseEffectBlock(ScriptObject parent, string name, ScriptParsedSegment value)
        {
            EffectBlock p = new EffectBlock(parent, value);

            p.Parse(value, ScriptContext.EffectBlock);

            return p;
        }
        private ValueModifier ParseAIValue(ScriptObject parent, string name, ScriptParsedSegment value)
        {
            ValueModifier v = new ValueModifier(parent, value);

            v.Parse(value, ScriptContext.ConditionBlock);

            return v;
        }
        private ConditionBlock ParseConditionBlock(ScriptObject parent, string name, ScriptParsedSegment value)
        {
            ConditionBlock p = new ConditionBlock(parent, value);
            p.Name = name;
            p.Parse(value, ScriptContext.ConditionBlock);

            return p;
        }

        public void ReadParsedSegment<T>(ScriptObject parent, ScriptParsedSegment segment, string name, string value, T targetObject,
            string targetProperty)
        {
            object val = value;

            float valFloat = 0;

            if (Single.TryParse(value, out valFloat))
            {
                val = valFloat;
            }
            else
            {

            }

            var property = targetObject.GetType().GetProperty(targetProperty);

            if (property.PropertyType == typeof(ScriptValue))
            {
                val = ScriptValueParser.Instance.CreateScriptValue(parent, segment, name, value);
            }
            if (property.PropertyType == typeof(LocalizedString))
            {
                val = new LocalizedString(parent, segment) { Name = name, key = value };
            }

            if (property != null)
                property.SetValue(targetObject, val);


        }
    }
}