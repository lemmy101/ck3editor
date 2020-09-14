using System;
using System.Collections.Generic;
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

            }
            if (inside.BehaviourData.ExpectEffects)
            {
                results.AddRange(ScopeManager.Instance.Defs[scope].ValidEffectMap.Keys);
                results.AddRange(ScopeManager.Instance.Defs[scope].ValidEffectScopes.Keys);

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


            results.RemoveAll(a => !a.ToLower().StartsWith(match.ToLower()));

            results = results.OrderBy(a => a).Distinct().ToList();

            return results;
        }
        public List<string> GetValidTokensEqual(ScriptObject inside, string child, string sofar)
        {
            List<string> results = new List<string>();
            if (inside == null)
            {

                return results;
            }

            var expected = inside.BehaviourData.TypeExpected;

            if (expected != null)
            {
                results.AddRange(EnumManager.Instance.GetEnums(expected));
            }

            results = results.OrderBy(a => a).Distinct().ToList();
            if(sofar != null)
                results.RemoveAll(a => !a.ToLower().StartsWith(sofar.ToLower()));


            if(results.Count==1 && results[0] == sofar)
                results.Clear();

            if (inside.BehaviourData.IsScope && string.IsNullOrEmpty(sofar))
            {
                results.Insert(0, "{ }");
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
