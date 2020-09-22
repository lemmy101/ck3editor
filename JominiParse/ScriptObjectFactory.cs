#region

using System.Collections.Generic;

#endregion

namespace JominiParse
{
    public class ScriptObjectFactory
    {
        public static ScriptObjectFactory Instance = new ScriptObjectFactory();

        private readonly Dictionary<string, List<KeyValuePair<string, ScopeType>>> OnActionScopeInitializations =
            new Dictionary<string, List<KeyValuePair<string, ScopeType>>>
            {
                {
                    "on_birth_mother", new List<KeyValuePair<string, ScopeType>>
                    {
                        new KeyValuePair<string, ScopeType>("mother",
                            ScopeType.character), //"mother", "real_father", "father"
                        new KeyValuePair<string, ScopeType>("child", ScopeType.character),
                        new KeyValuePair<string, ScopeType>("real_father", ScopeType.character),
                        new KeyValuePair<string, ScopeType>("father", ScopeType.character)
                    }
                }
            };

        public ScriptObject CreateScriptObject(ScriptContext context, ScriptParsedSegment segment, ScriptObject parent,
            string scriptNamespace)
        {
            if (context == ScriptContext.CharacterInteractions)
            {
            }

            ScriptObject obj = null;

            if (parent == null)
                if (Core.Instance.LoadingCK3Library == Core.Instance.ModCK3Library)
                {
                    if (segment.name == "abduct_outcome_roll_effect")
                    {
                    }

                    ReferenceManager.Instance.ClearConnectionsFrom(segment.name);
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
                case ScriptContext.None:
                    obj = new ScriptObject(parent, segment);
                    break;
                case ScriptContext.ScriptObjectValue:
                    obj = ScriptValueParser.Instance.ParseScriptValue(parent, segment);
                    break;
                case ScriptContext.Event:
                    if (segment.name.StartsWith("scripted_trigger "))
                        obj = new ScriptObject(parent, segment, SchemaManager.Instance.GetSchema("trigger"));
                    else
                        obj = new ScriptObject(parent, segment, SchemaManager.Instance.GetSchema("event"));

                    break;
                default:
                    obj = new ScriptObject(parent, segment,
                        SchemaManager.Instance.GetSchema(Core.Instance.BaseCK3Library.ContextData[context].Type));
                    break;
            }

            obj.Context = context;
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
            if (context == ScriptContext.VassalContracts)
            {
                AddScriptScope("vassal", scriptObject, ScopeType.character, false, true);
                AddScriptScope("liege", scriptObject, ScopeType.character, false, true);
                AddScriptScope("opinion_of_liege", scriptObject, ScopeType.value, false, true);
                scriptObject.SetScopeType(ScopeType.character);
            }

            if (context == ScriptContext.StoryCycles)
            {
                AddScriptScope("story", scriptObject, ScopeType.story_cycle, false, true);
                scriptObject.SetScopeType(ScopeType.story_cycle);
            }

            if (context == ScriptContext.SecretTypes)
            {
                AddScriptScope("secret_owner", scriptObject, ScopeType.character, false, true);
                AddScriptScope("secret_target", scriptObject, ScopeType.character, false, true);
                AddScriptScope("discoverer", scriptObject, ScopeType.character, false, true);
                AddScriptScope("secret_exposer", scriptObject, ScopeType.character, false, true);
                scriptObject.SetScopeType(ScopeType.secret);
            }

            if (context == ScriptContext.ImportantActions)
            {
                AddScriptScope("recipient", scriptObject, ScopeType.character, false, true);
                AddScriptScope("actor", scriptObject, ScopeType.character, false, true);
                AddScriptScope("landed_title", scriptObject, ScopeType.landed_title, false, true);
                scriptObject.SetScopeType(ScopeType.character);
            }

            if (context == ScriptContext.Governments)
            {
                AddScriptScope("vassal", scriptObject, ScopeType.character, false, true);
                AddScriptScope("liege", scriptObject, ScopeType.character, false, true);
                scriptObject.SetScopeType(ScopeType.none);
            }

            if (context == ScriptContext.CasusBelliType)
            {
                AddScriptScope("attacker", scriptObject, ScopeType.character, false, true);
                AddScriptScope("defender", scriptObject, ScopeType.character, false, true);
                AddScriptScope("claimant", scriptObject, ScopeType.character, false, true);
                scriptObject.SetScopeType(ScopeType.none);
            }

            if (context == ScriptContext.CouncilTasks)
            {
                AddScriptScope("county", scriptObject, ScopeType.landed_title, false, true);
                AddScriptScope("councillor_liege", scriptObject, ScopeType.character, false, true);
                scriptObject.SetScopeType(ScopeType.none);
            }

            if (context == ScriptContext.CouncilPositions)
            {
                AddScriptScope("county", scriptObject, ScopeType.landed_title, false, true);
                AddScriptScope("councillor_liege", scriptObject, ScopeType.character, false, true);
                scriptObject.SetScopeType(ScopeType.none);
            }

            if (context == ScriptContext.Factions)
                AddScriptScope("target", scriptObject, ScopeType.character, false, true);
            if (context == ScriptContext.ScriptedTriggers) scriptObject.SetScopeType(ScopeType.none);
            if (context == ScriptContext.ScriptedEffects) scriptObject.SetScopeType(ScopeType.none);
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

            if (context == ScriptContext.Event)
            {
                InitEvent(scriptObject);
                scriptObject.SetScopeType(ScopeType.character);
            }

            if (context == ScriptContext.Schemes)
            {
                InitScheme(scriptObject);
                scriptObject.SetScopeType(ScopeType.character);
            }

            if (scriptObject.Name == "duel")
                AddScriptScope("duel_value", scriptObject.Topmost, ScopeType.value, false, true);
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
                foreach (var s in m) AddScriptScope(s.Key, scriptObject, s.Value, false, true);
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
                if (child.Name == "type")
                    if ((child as ScriptValue).GetStringValue() == "character_event")
                        scriptObject.SetScopeType(ScopeType.character);
        }
    }
}