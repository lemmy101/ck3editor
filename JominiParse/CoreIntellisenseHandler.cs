using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JominiParse
{
    public class CoreIntellisenseHandler
    {
        public static CoreIntellisenseHandler Instance = new CoreIntellisenseHandler();


        public List<string> GetValidTokens(ScriptObject inside, string match)
        {
            List<string> results = new List<string>();
            if (inside == null)
            {

                return results;
            }


            if (inside.Parent != null && inside.Parent.Schema != null && inside.Name != null)
            {
                List<string> results2 = new List<string>();
                inside.Parent.Schema.AddChildrenToList(results2);
                if (results2.Contains("scopeconditions"))
                {
                    results2.Remove("scopeconditions");
                    // can use condition commands...

                    var condition = ScopeManager.Instance.GetCondition(inside.Parent.GetScopeType(), inside.Name);

                    if (condition != null)
                    {
                        foreach (var c in condition.Properties)
                        {
                            results.Add(c.name);
                        }
                        results.RemoveAll(a => !a.ToLower().StartsWith(match.ToLower()));

                        results = results.OrderBy(a => a).ToList();

                        return results;
                    }

                }
                if (results2.Contains("scopeeffects"))
                {
                    results2.Remove("scopeeffects");
                    // can use condition commands...

                    var condition = ScopeManager.Instance.GetEffect(inside.Parent.GetScopeType(), inside.Name);

                    if (condition != null)
                    {
                        foreach (var c in condition.Properties)
                        {
                            results.Add(c.name);
                        }
                        results.RemoveAll(a => !a.ToLower().StartsWith(match.ToLower()));

                        results = results.OrderBy(a => a).ToList();

                        return results;
                    }

                }
            }



            if (inside.Schema != null)
            {
                inside.Schema.AddChildrenToList(results);
                if (results.Contains("scopeconditions"))
                {
                    results.Remove("scopeconditions");
                    var scope = inside.GetScopeType();
                    ScopeManager.Instance.AddCompleteScopeConditionResults(scope, results);
                }
                if (results.Contains("scopeeffects"))
                {
                    results.Remove("scopeeffects");
                    var scope = inside.GetScopeType();
                    ScopeManager.Instance.AddCompleteScopeEffectResults(scope, results);

                }
            }
            // is a child of an object with a schema so we can ascertain things from it...
            else if (inside.SchemaChild != null)
            {

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
            
            if (inside != null && inside.Parent != null && inside.Parent.Schema != null)
            {
                // add enum options
                results.AddRange(inside.Parent.Schema.GetChoices(child));

                string type = inside.Parent.Schema.GetChildType(child);

                if (type == null)
                {
                    List<string> results2 = new List<string>();
                    inside.Parent.Schema.AddChildrenToList(results2);
                    if (results2.Contains("scopeconditions"))
                    {
                        results2.Remove("scopeconditions");
                        var scope = inside.Parent.GetScopeType();
                        {
                            var condition = ScopeManager.Instance.GetCondition(scope, child);
                            if (condition != null)
                            {
                                type = condition.type;
                            }
                        }
                    }
                    if (results2.Contains("scopeeffects"))
                    {
                        results2.Remove("scopeeffects");
                        var scope = inside.Parent.GetScopeType();
                        {
                            var eff = ScopeManager.Instance.GetEffect(scope, child);
                            if (eff != null)
                            {
                                type = eff.type;
                            }
                        }

                    }
                    results.AddRange(results2);
                }

            
                if (type != null)
                {
                    switch (type)
                    {
                        case "bool":
                            results.Add("yes");
                            results.Add("no");
                            break;
                        default:
                            results.AddRange(Core.Instance.GetNameSetFromEnumType(type));
                            break;
                    }
                }
               
            }
            results = results.OrderBy(a => a).Distinct().ToList();
            if(sofar != null)
                results.RemoveAll(a => !a.ToLower().StartsWith(sofar.ToLower()));

            if(results.Count==1 && results[0] == sofar)
                results.Clear();

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
