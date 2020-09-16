using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JominiParse
{
    public class CoreIntellisenseHandler
    {
        public static CoreIntellisenseHandler Instance = new CoreIntellisenseHandler();

        List<ScriptObject> visited = new List<ScriptObject>();
        private void GetScriptScopesFromReferences(ScriptObject eventConnectionFrom, HashSet<ScriptObject.ScriptScope> scopes)
        {
            if (visited.Contains(eventConnectionFrom))
                return;

            visited.Add(eventConnectionFrom);

            foreach (var keyValuePair in eventConnectionFrom.scriptScopes)
            {
                if (!keyValuePair.Value.Temporary)
                    scopes.Add(keyValuePair.Value);
            }


            var ConnectionsIn = ReferenceManager.Instance.GetConnectionsTo(eventConnectionFrom.Name).Distinct();

            foreach (var eventConnection in ConnectionsIn)
            {
                GetScriptScopesFromReferences(eventConnection.From, scopes);
            }


        }
        public List<string> GetValidTokens(ScriptObject inside, string match)
        {
            if (inside == null || inside.BehaviourData == null)
                return new List<string>();

            List<string> results = new List<string>();
            if (inside == null)
            {

                return results;
            }

            if (!string.IsNullOrEmpty(match))
                inside = inside.Parent;
            if (inside == null)
                return results;

            var scope = inside.GetScopeType();

            if (inside.BehaviourData.ExpectConditions)
            {
                results.AddRange(ScopeManager.Instance.Defs[scope].ValidConditionMap.Keys);
                results.AddRange(ScopeManager.Instance.Defs[scope].ValidConditionScopes.Keys);
                results.AddRange(ScopeManager.Instance.Defs[ScopeType.any].ValidConditionMap.Keys);
                results.AddRange(ScopeManager.Instance.Defs[ScopeType.any].ValidConditionScopes.Keys);
                if (inside.BehaviourData.IsScopedBlock)
                {
                    results.Add("limit");
                    results.Add("trigger_if");
                }

            }
            if (inside.BehaviourData.ExpectEffects)
            {
                results.AddRange(ScopeManager.Instance.Defs[scope].ValidEffectMap.Keys);
                results.AddRange(ScopeManager.Instance.Defs[scope].ValidEffectScopes.Keys);
                results.AddRange(ScopeManager.Instance.Defs[ScopeType.any].ValidEffectMap.Keys);
                results.AddRange(ScopeManager.Instance.Defs[ScopeType.any].ValidEffectScopes.Keys);
                results.Add("if");

            }
            
            if (inside.BehaviourData.Type == ScriptObjectBehaviourType.FunctionMultiline && inside.Schema != null)
            {
                foreach (var keyValuePair in inside.Schema.children)
                {
                    if (keyValuePair.Value.NamesFrom == null)
                        results.Add(keyValuePair.Key);
                    else if (keyValuePair.Value.NamesFrom != "num")
                        results.AddRange(EnumManager.Instance.GetEnums(keyValuePair.Value.NamesFrom));
                }
            }
            else if (inside.BehaviourData.Type == ScriptObjectBehaviourType.RootObject && inside.Schema != null)
            {
                foreach (var keyValuePair in inside.Schema.children)
                {
                    if (keyValuePair.Value.NamesFrom == null)
                        results.Add(keyValuePair.Key);
                    else if (keyValuePair.Value.NamesFrom != "num")
                        results.AddRange(EnumManager.Instance.GetEnums(keyValuePair.Value.NamesFrom));
                }
            }
            else if (inside.BehaviourData.Type == ScriptObjectBehaviourType.RootObjectPropertyBlock && inside.Schema != null)
            {
                foreach (var keyValuePair in inside.Schema.children)
                {
                    if (keyValuePair.Value.NamesFrom == null)
                        results.Add(keyValuePair.Key);
                    else if (keyValuePair.Value.NamesFrom != "num")
                        results.AddRange(EnumManager.Instance.GetEnums(keyValuePair.Value.NamesFrom));
                }
            }
            else if (inside.BehaviourData.Type == ScriptObjectBehaviourType.FunctionNamedFromParameterBlock && inside.Schema != null)
            {
                foreach (var keyValuePair in inside.Schema.children)
                {
                    if (keyValuePair.Value.NamesFrom == null)
                        results.Add(keyValuePair.Key);
                    else if (keyValuePair.Value.NamesFrom != "num")
                        results.AddRange(EnumManager.Instance.GetEnums(keyValuePair.Value.NamesFrom));
                }
            }

            if (inside.BehaviourData.ExpectConditions || inside.BehaviourData.ExpectEffects)
            {
                var ConnectionsIn = ReferenceManager.Instance.GetConnectionsTo(inside.Topmost.Name);
                HashSet<ScriptObject.ScriptScope> Scopes = new HashSet<ScriptObject.ScriptScope>();
                visited.Clear();
                foreach (var eventConnection in ConnectionsIn)
                {
                    GetScriptScopesFromReferences(eventConnection.From, Scopes);
                }

                foreach (var scriptScope in inside.scriptScopes.Values)
                {
                    if (scriptScope.RequiresScopeTag)
                        results.Add("scope:" + scriptScope.Name);
                    else
                        results.Add(scriptScope.Name);
                }
                foreach (var scriptScope in Scopes)
                {
                    if (scriptScope.RequiresScopeTag)
                        results.Add("scope:" + scriptScope.Name);
                    else
                        results.Add(scriptScope.Name);
                }

            }


            if (match != null)
                results.RemoveAll(a => !a.ToLower().Contains(match.ToLower()));

            results = results.OrderBy(a => a).Distinct().ToList();
            if(match != null)
                results = results.OrderBy(a => !a.ToLower().StartsWith(match.ToLower())).ToList();

            return results;
        }
        public List<string> GetValidTokensEqual(ScriptObject inside, string child, string sofar)
        {
            if (inside.BehaviourData == null)
                return new List<string>();

            List<string> results = new List<string>();
            if (inside == null)
            {

                return results;
            }

            var expected = inside.BehaviourData.TypeExpected;

         
            if (expected != null)
            {
                results.AddRange(EnumManager.Instance.GetEnums(expected, false));
                //results.AddRange();
                List<ScriptObject.ScriptScope> scoperesults = new List<ScriptObject.ScriptScope>();
           
                var ConnectionsIn = ReferenceManager.Instance.GetConnectionsTo(inside.Topmost.Name);
                HashSet<ScriptObject.ScriptScope> Scopes = new HashSet<ScriptObject.ScriptScope>();
                visited.Clear();
                foreach (var eventConnection in ConnectionsIn)
                {
                    GetScriptScopesFromReferences(eventConnection.From, Scopes);
                }
                inside.Topmost.GetValidScriptScopes(scoperesults, true, ScriptObject.ScopeFindType.Any);
                scoperesults.AddRange(Scopes);
                var scopesval = scoperesults.Where(a => a.IsValue && a.VarType.ToString().ToLower() == expected).ToList();
                var scopes = scoperesults.Where(a => !a.IsValue && a.To.ToString().ToLower() == expected).ToList();
                scopes = scopes.Union(scopesval).ToList();
                foreach (var scriptScope in scopes)
                {
                    if (scriptScope.RequiresScopeTag)
                        results.Add("scope:"+scriptScope.Name);
                    else
                        results.Add(scriptScope.Name);
                }
            }
            List<string> addToTop = new List<string>();

            if (expected != null)
            {
                var l = Core.Instance.BaseCK3Library.ContextData.Where(a => a.Value.Type == expected).ToList();

                if (l.Any())
                {
                    foreach (var keyValuePair in l)
                    {
                        if (keyValuePair.Value.Prepend != null)
                        {
                            if (string.IsNullOrEmpty(sofar) || ((keyValuePair.Value.Prepend + ":").Contains(sofar) && (keyValuePair.Value.Prepend + ":") != sofar))
                            {
                                addToTop.Add(keyValuePair.Value.Prepend + ":");
                            }
                            else if (sofar.StartsWith(keyValuePair.Value.Prepend + ":"))
                            {
                                var list = Core.Instance.GetNameSet(keyValuePair.Key, false).Select(a=>string.Concat(keyValuePair.Value.Prepend, ":", a)).ToList();
                                results.AddRange(list);
                            }
                        }
                    }
                }
            }

            results = results.OrderBy(a => a).Distinct().ToList();
            if(sofar != null)
                results.RemoveAll(a => !a.ToLower().Contains(sofar.ToLower()));


            foreach (var v in addToTop)
            {
                results.Insert(0, v);

            }

            if (sofar != null)
                results = results.OrderBy(a => !a.ToLower().StartsWith(sofar.ToLower())).ToList();

            if (results.Count==1 && results[0] == sofar)
                results.Clear();

            if (child == "limit" && string.IsNullOrEmpty(sofar) ||
                (string.IsNullOrEmpty(sofar) && expected == null &&
                 inside.BehaviourData.ExpectedEffectFunction != null) ||
                (string.IsNullOrEmpty(sofar) && expected == null &&
                inside.BehaviourData.ExpectedConditionFunction != null) ||
                (inside.BehaviourData.CanBeScope && string.IsNullOrEmpty(sofar))
                )
            {
                results.Insert(0, "{ }");
            }

            if (sofar != null && sofar.Trim().Length > 0)
            {
                int test;
                if (Int32.TryParse(sofar, out test))
                {
                    results.Clear();
                }
            }
            return results;
        }

        public bool GetNeedBraces(ScriptObject inside, string child)
        {
            List<string> results = new List<string>();

            if (child == "AND" || child == "OR" || child == "NOT" || child == "NOR")
                return true;

            if (inside.Schema != null)
            {
                // add enum options
                bool success;
                var res = inside.Schema.GetChildIsBlock(child, out success);

                if (success)
                    return res;

                // need to figure out if we need braces from other means...
                List<string> results2 = new List<string>();
                inside.Schema.AddChildrenToList(results2);
                var scope = inside.GetScopeType();
                if (results2.Contains("scopeconditions"))
                {
                    results2.Remove("scopeconditions");
                    {
                        var condition = ScopeManager.Instance.GetCondition(scope, child);

                        if (condition != null)
                            return condition.Properties.Count > 0;
                    }

                    var schema = SchemaManager.Instance.GetSchema(child);

                    if (schema != null)
                    {
                        return true;
                    }
                }
                if (results2.Contains("scopeeffects"))
                {
                    results2.Remove("scopeeffects");
                    {
                        var eff = ScopeManager.Instance.GetEffect(scope, child);

                        if (eff != null)
                            return eff.Properties.Count > 0;
                    }

                    var schema = SchemaManager.Instance.GetSchema(child);

                    if (schema != null)
                    {
                        return true;
                    }
                }

                if (inside.Parent != null && ScopeManager.Instance.isConditionScope(inside.Parent.GetScopeType(), child))
                {
                    return true;
                }

            }

            return false;
        }
    }
}
