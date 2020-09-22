#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace JominiParse
{
    public class CoreIntellisenseHandler
    {
        public static CoreIntellisenseHandler Instance = new CoreIntellisenseHandler();

        private readonly List<ScriptObject> visited = new List<ScriptObject>();

        private void GetScriptScopesFromReferences(ScriptObject eventConnectionFrom,
            HashSet<ScriptScope> scopes)
        {
            if (visited.Contains(eventConnectionFrom))
                return;

            visited.Add(eventConnectionFrom);

            foreach (var keyValuePair in eventConnectionFrom.scriptScopes)
                if (!keyValuePair.Value.Temporary)
                    scopes.Add(keyValuePair.Value);


            var ConnectionsIn = ReferenceManager.Instance.GetConnectionsTo(eventConnectionFrom.Name).Distinct();

            foreach (var eventConnection in ConnectionsIn) GetScriptScopesFromReferences(eventConnection.From, scopes);
        }

        public List<string> GetValidTokens(ScriptObject inside, string match)
        {
            if (inside == null || inside.BehaviourData == null)
                return new List<string>();

            var results = new List<string>();
            var results2 = new List<string>();
            if (inside == null) return results;

            if (!string.IsNullOrEmpty(match))
                inside = inside.Parent;
            if (inside == null)
                return results;

            var scope = inside.GetScopeType();

            if (inside.lhsSchema != null)
            {
                var ch = inside.lhsSchema.Children.Where(a =>
                    !a.rightHandOnly && (a.scopes.Contains(scope) || a.scopes.Contains(ScopeType.none) ||
                                         a.scopes.Contains(ScopeType.inheritparent)));

                foreach (var schemaNode in ch)
                    if (schemaNode.namesFrom != null)
                    {
                    }
                    else
                    {
                        results.Add(schemaNode.name);
                    }

                if (inside.lhsSchema.IsEffect())
                {
                    var sl = SchemaManager.Instance.GetSchema("scriptlist");

                    var options = sl.Children.Where(a => a.scopes.Contains(scope) && (a.name.StartsWith("every_") ||
                        a.name.StartsWith("random_") || a.name.StartsWith("ordered_"))).ToList();

                    foreach (var schemaNode in options) results.Add(schemaNode.name);
                }

                if (inside.lhsSchema.IsTrigger())
                {
                    var sl = SchemaManager.Instance.GetSchema("scriptlist");

                    var options = sl.Children.Where(a => a.scopes.Contains(scope) && a.name.StartsWith("any_"))
                        .ToList();

                    foreach (var schemaNode in options) results.Add(schemaNode.name);
                }
            }

            if (match != null)
                results.RemoveAll(a => !a.ToLower().Contains(match.ToLower()));

            results = results.OrderBy(a => a).Distinct().ToList();


            if (inside.lhsSchema != null)
            {
                var ch = inside.lhsSchema.Children.Where(a =>
                    !a.rightHandOnly && (a.scopes.Contains(scope) || a.scopes.Contains(ScopeType.none) ||
                                         a.scopes.Contains(ScopeType.inheritparent)));

                foreach (var schemaNode in ch)
                    if (schemaNode.namesFrom != null)
                        results2.AddRange(EnumManager.Instance.GetEnums(schemaNode.namesFrom));
            }


            if (match != null)
                results2.RemoveAll(a => !a.ToLower().Contains(match.ToLower()));

            results2 = results2.OrderBy(a => a).Distinct().ToList();

            results.AddRange(results2);


            if (match != null)
                results = results.OrderBy(a => !a.ToLower().StartsWith(match.ToLower())).ToList();


            return results;
        }

        public List<string> GetValidTokensEqual(ScriptObject inside, string child, string sofar)
        {
            var results = new List<string>();

            if (inside == null) return results;

            if (inside.BehaviourData == null)
                return new List<string>();

            var expectedType = new List<string>();

            var scope = inside.GetScopeType();

            if (inside.lhsSchema != null)
            {
                expectedType.AddRange(inside.lhsSchema.TypeList);

                var ch = inside.lhsSchema.Children.Where(a =>
                    a.rightHandOnly && (a.scopes.Contains(scope) || a.scopes.Contains(ScopeType.none) ||
                                        a.scopes.Contains(ScopeType.inheritparent)));

                //var l = ch.Where(a => a.TypeList.Any(b => inside.lhsSchema.TypeList.Contains(b))).ToList();

                foreach (var schemaNode in ch) results.Add(schemaNode.name);

                foreach (var s in inside.lhsSchema.TypeList)
                {
                    var enums = EnumManager.Instance.GetEnums(s);

                    results.AddRange(enums);
                }
            }

            results = results.OrderBy(a => a).Distinct().ToList();
            if (sofar != null)
                results.RemoveAll(a => !a.ToLower().Contains(sofar.ToLower()));

            if (sofar != null)
                results = results.OrderBy(a => !a.ToLower().StartsWith(sofar.ToLower())).ToList();

            if (string.IsNullOrWhiteSpace(sofar))
            {
                if (expectedType.Contains("value"))
                    results.Insert(0, "{ }");
                else if (inside.BehaviourData.candidates != null)
                    foreach (var behaviourDataCandidate in inside.BehaviourData.candidates)
                        if (behaviourDataCandidate.Children.Count > 0)
                        {
                            results.Insert(0, "{ }");
                            break;
                        }
            }

            if (results.Count == 1 && results[0] == sofar)
                results.Clear();
            if (sofar != null && sofar.Trim().Length > 0)
            {
                int test;
                if (int.TryParse(sofar, out test)) results.Clear();
            }

            return results;

            /*
                  if (inside.BehaviourData == null)
                      return new List<string>();

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
                      inside.BehaviourData.ExpectedTriggerFunction != null) ||
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
                  }*/
        }
    }
}