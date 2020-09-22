namespace JominiParse
{
    internal class ScriptValueParser
    {
        public static ScriptValueParser Instance = new ScriptValueParser();


        public ScriptValue ParseScriptValue(ScriptObject parent, ScriptParsedSegment scriptParsedSegment)
        {
            // simple
            if (scriptParsedSegment.children.Count == 0)
            {
                float val = 0;
                if (scriptParsedSegment.value.Count > 0)
                {
                    if (float.TryParse(scriptParsedSegment.value[0], out val))
                    {
                        var value = new StaticScriptValue(parent, scriptParsedSegment);
                        value.Value = val;
                        value.Name = scriptParsedSegment.name;
                        return value;
                    }
                    else
                    {
                        var value = new ReferenceScriptValue(parent, scriptParsedSegment);
                        value.Name = scriptParsedSegment.name;
                        value.Value = scriptParsedSegment.value[0];
                        return value;
                    }
                }

                {
                    var value = new NullScriptValue(parent, scriptParsedSegment);

                    value.Name = scriptParsedSegment.name;
                    return value;
                }
            }

            {
                var value = new FormulaScriptValue(parent, scriptParsedSegment);

                value.Name = scriptParsedSegment.name;

                for (var index = 0; index < scriptParsedSegment.children.Count; index++)
                {
                    var parsedSegment = scriptParsedSegment.children[index];

                    {
                        value.Operations.Add(FileTokenizer.Instance.ParseUnknown(value, parsedSegment,
                            ScriptContext.ScriptObjectValue));
                    }
                }

                if (value.Operations.Count > 4)
                {
                    var gfd = 0;
                }

                return value;
            }

            return null;
        }

        public ScriptValue CreateScriptValue(ScriptObject parent, ScriptParsedSegment scriptParsedSegment, string name,
            string value)
        {
            float valFloat = 0;

            if (float.TryParse(value, out valFloat))
            {
                var v = new StaticScriptValue(parent, scriptParsedSegment);
                v.Name = name;
                v.Value = valFloat;
                return v;
            }

            {
                var v = new ReferenceScriptValue(parent, scriptParsedSegment);
                v.Name = name;
                v.Value = value;
                return v;
            }

            return null;
        }
    }
}