using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JominiParse
{
    public class ScriptObjectFactory
    {
        public static ScriptObjectFactory Instance = new ScriptObjectFactory();

        private Dictionary<string, List<KeyValuePair<string, ScopeType>>> OnActionScopeInitializations = new Dictionary<string, List<KeyValuePair<string, ScopeType>>>()
        {
            {"on_birth_mother", new List<KeyValuePair<string, ScopeType>>()
            {
                new KeyValuePair<string, ScopeType>("mother", ScopeType.character), //"mother", "real_father", "father"
                new KeyValuePair<string, ScopeType>("child", ScopeType.character),
                new KeyValuePair<string, ScopeType>("real_father", ScopeType.character),
                new KeyValuePair<string, ScopeType>("father", ScopeType.character),
            }}
        };

        public ScriptObject CreateScriptObject(ScriptContext context, ScriptParsedSegment segment, ScriptObject parent,
            string scriptNamespace)
        {
            ScriptObject obj = null;
            switch (context)
            {
                  case ScriptContext.ScriptValues:
                    obj = ScriptValueParser.Instance.ParseScriptValue(parent, segment);
                    break;
                case ScriptContext.Events:
                    if(segment.name.StartsWith("scripted_trigger "))
                        obj = new ScriptObject(parent, segment, SchemaManager.Instance.GetSchema("trigger"));
                    else
                        obj = new ScriptObject(parent, segment, SchemaManager.Instance.GetSchema("event"));

                    break;
                case ScriptContext.OnActions:
                    obj = new ScriptObject(parent, segment, SchemaManager.Instance.GetSchema("on_action"));
                    break;
                  default:
                    obj = new ScriptObject(parent, segment);
                    break;
            }

            InitializeObject(obj, context);

            AddScriptScope("root", obj, obj.GetScopeType(), true, false);
            AddScriptScope("this", obj, obj.GetScopeType(), true, false);


            obj.Namespace = scriptNamespace;

            return obj;

        }

        private void AddScriptScope(string name, ScriptObject obj, ScopeType to, bool temp, bool requiresScopeTag)
        {
            obj.AddScriptScope(name, obj, to, temp, requiresScopeTag);
        }

        private void InitializeObject(ScriptObject scriptObject, ScriptContext context)
        {
            if (context == ScriptContext.OnActions)
            {
                InitOnAction(scriptObject);
                scriptObject.SetScopeType(ScopeType.character);
            }
            if (context == ScriptContext.CharacterInteractions)
            {
                InitCharacterInteraction(scriptObject);
                scriptObject.SetScopeType(ScopeType.character);
            }
            if (context == ScriptContext.Events)
            {
                InitEvent(scriptObject);
                scriptObject.SetScopeType(ScopeType.character);
            }
            if (context == ScriptContext.Activities)
            {
                InitActivity(scriptObject);
                scriptObject.SetScopeType(ScopeType.character);
            }
            if (context == ScriptContext.Schemes)
            {
                InitScheme(scriptObject);
                scriptObject.SetScopeType(ScopeType.character);
            }
        }

        private void InitCharacterInteraction(ScriptObject scriptObject)
        {
            AddScriptScope("actor", scriptObject, ScopeType.character, false, true);
            AddScriptScope("target", scriptObject, ScopeType.character, false, true);
            AddScriptScope("recipient", scriptObject, ScopeType.character, false, true);
            AddScriptScope(scriptObject.Name, scriptObject, ScopeType.any, false, true);

        }

        private void InitOnAction(ScriptObject scriptObject)
        {
            if (OnActionScopeInitializations.ContainsKey(scriptObject.Name))
            {
                var m = OnActionScopeInitializations[scriptObject.Name];
                foreach (var s in m)
                {
                    AddScriptScope(s.Key, scriptObject, s.Value, false, true);
                }
            }
        }

        private void InitScheme(ScriptObject scriptObject)
        {
            AddScriptScope("target", scriptObject, ScopeType.character, false, true);
            AddScriptScope("scheme", scriptObject, ScopeType.scheme, false, true);
            AddScriptScope("owner", scriptObject, ScopeType.character, false, true);

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
