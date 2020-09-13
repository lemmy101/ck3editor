using System;
using System.Collections.Generic;
using System.Text;

namespace JominiParse
{
    public class ScriptObjectFactory
    {
        public static ScriptObjectFactory Instance = new ScriptObjectFactory();

        public ScriptObject CreateScriptObject(ScriptContext context, ScriptParsedSegment segment, ScriptObject parent)
        {
            ScriptObject obj = null;
            switch (context)
            {
                case ScriptContext.ScriptValues:
                    obj = ScriptValueParser.Instance.ParseScriptValue(parent, segment);
                    break;
                  default:
                    obj = new ScriptObject(parent, segment);
                    break;
            }

            InitializeObject(obj, context);

            AddScriptScope("root", obj, obj.GetScopeType(), false);
            AddScriptScope("this", obj, obj.GetScopeType(), false);

            return obj;

        }

        private void AddScriptScope(string name, ScriptObject obj, ScopeType to, bool requiresScopeTag)
        {
            obj.AddScriptScope(name, obj, to, requiresScopeTag);
        }

        private void InitializeObject(ScriptObject scriptObject, ScriptContext context)
        {
            if (context == ScriptContext.Events)
            {
                InitEvent(scriptObject);
            }
            if (context == ScriptContext.Activities)
            {
                InitActivity(scriptObject);
            }
            if (context == ScriptContext.Schemes)
            {
                InitScheme(scriptObject);
            }
        }

        private void InitScheme(ScriptObject scriptObject)
        {
            AddScriptScope("target", scriptObject, ScopeType.character, true);
            AddScriptScope("owner", scriptObject, ScopeType.character, true);

        }

        private void InitActivity(ScriptObject scriptObject)
        {
            
        }

        private void InitEvent(ScriptObject scriptObject)
        {
            foreach (var child in scriptObject.Children)
            {
                if (child.Name == "type")
                    if ((child as ScriptValue).GetStringValue() == "character_event")
                    {
                        scriptObject.SetScopeType(ScopeType.character);
                    }
            }
        }
    }
}
