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

            if (parent == null)
            {
                if (Core.Instance.LoadingCK3Library == Core.Instance.ModCK3Library)
                {

                    ReferenceManager.Instance.ClearConnectionsFrom(segment.name);
                }

            }

            if (parent == null && segment.name.StartsWith("@"))
            {
                obj = ScriptValueParser.Instance.ParseScriptValue(parent, segment);
                InitializeObject(obj, context);

                obj.Namespace = scriptNamespace;
                if (string.IsNullOrEmpty(scriptNamespace))
                    obj.Namespace = null;

                return obj;
            }

            switch (context)
            {
                case ScriptContext.Buildings:
                    obj = new ScriptObject(parent, segment, SchemaManager.Instance.GetSchema("building"));
                    break;
                case ScriptContext.Activities:
                    obj = new ScriptObject(parent, segment, SchemaManager.Instance.GetSchema("activity"));
                    break;
                case ScriptContext.Decisions:
                    obj = new ScriptObject(parent, segment, SchemaManager.Instance.GetSchema("decision"));
                    break;
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

            if (string.IsNullOrEmpty(scriptNamespace))
                obj.Namespace = null;

            return obj;

        }

        private void AddScriptScope(string name, ScriptObject obj, ScopeType to, bool temp, bool requiresScopeTag)
        {
            var r = obj.AddScriptScope(name, obj, to, temp, requiresScopeTag);
            if (r.Declared == null)
                r.Declared = obj;
        }

        private void InitializeObject(ScriptObject scriptObject, ScriptContext context)
        {
            if (context == ScriptContext.Buildings)
            {
                InitBuilding(scriptObject);
                scriptObject.SetScopeType(ScopeType.province);
            }
            if (context == ScriptContext.Activities)
            {
                InitActivity(scriptObject);
                scriptObject.SetScopeType(ScopeType.activity);
            }
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
           
            if (context == ScriptContext.Schemes)
            {
                InitScheme(scriptObject);
                scriptObject.SetScopeType(ScopeType.character);
            }
        }

        private void InitBuilding(ScriptObject scriptObject)
        {
            AddScriptScope("holder", scriptObject, ScopeType.character, false, true);

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
            AddScriptScope("activity_owner", scriptObject, ScopeType.character, false, true);
            AddScriptScope("activity", scriptObject, ScopeType.activity, false, true);
       
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
