using System.Collections.Generic;
using System.Linq;

namespace JominiParse
{
    public class VariableStore
    {
        public static VariableStore Instance = new VariableStore();
        public Dictionary<string, List<Variable>> unsortedScopedVariables = new Dictionary<string, List<Variable>>();
        public Dictionary<RefFilename, List<Variable>> unsortedScopedVariablesByFilename = new Dictionary<RefFilename, List<Variable>>();

        public Dictionary<string, Dictionary<ScopeType, Variable>> scopedVariables = new Dictionary<string, Dictionary<ScopeType, Variable>>();

        public Dictionary<string, Variable> globalVariables = new Dictionary<string, Variable>();
        public Dictionary<RefFilename, List<Variable>> globalVariablesByFilename = new Dictionary<RefFilename, List<Variable>>();
        public void RegisterSetLocalVariable(ScriptObject node)
        {
            var w = node.Children.Where(a => a.Name == "value");
            ScopeType type = ScopeType.@bool;

            if (w.Count() > 0)
            {
                var f = w.First();

                string s = f.GetStringValue();

                if (s != null)
                {
                    if (!(s == "yes" || s == "no"))
                    {
                        if (s.StartsWith("flag"))
                            type = ScopeType.flag;
                        else
                            type = ScopeType.value;
                    }
                }
            }
            var name = node.Children.Where(a => a.Name == "name");
            var target = node.Children.Where(a => a.Name == "value");

            var n = name.First().GetStringValue();
            var t = target.First().GetStringValue();

            node.Topmost.localVariables[n] = new Variable() { Declared = node, IsValue = true, Name = n, RequiresVarTag = true, ToObj = node.Topmost, VarType = type, VariableType = VariableType.Local };
        }

        public void RegisterSetGlobalVariable(ScriptObject node)
        {
            var w = node.Children.Where(a => a.Name == "value");
            ScopeType type = ScopeType.@bool;

            if (w.Count() > 0)
            {
                var f = w.First();

                string s = f.GetStringValue();

                if (s != null)
                {
                    if (!(s == "yes" || s == "no"))
                    {
                        if (s.StartsWith("flag"))
                            type = ScopeType.flag;
                        else
                            type = ScopeType.value;
                    }
                }
             


            }
            var name = node.Children.Where(a => a.Name == "name");
            var target = node.Children.Where(a => a.Name == "value");

            var n = name.First().GetStringValue();
            var t = target.First().GetStringValue();
            if (!globalVariablesByFilename.ContainsKey(node.Topmost.Filename))
            {
                globalVariablesByFilename[node.Topmost.Filename] = new List<Variable>();
            }

            var v = new Variable()
            {
                Declared = node,
                IsValue = true,
                Name = n,
                RequiresVarTag = true,
                ToObj = node.Topmost,
                VarType = type,
                VariableType = VariableType.Local
            };

            globalVariables[n] = v;
            globalVariablesByFilename[node.Topmost.Filename].Add(v);
        }

        public void CompleteScopedVar(ScriptObject scriptObject, ScriptObjectBehaviourData data, ScopeType scopeType)
        {
            var name = scriptObject.GetChildStringValue("name");

            if (!unsortedScopedVariables.ContainsKey(name))
                return;

            foreach (var variable in unsortedScopedVariables[name])
            {
                if (variable.Declared == scriptObject)
                {
                    variable.InsideScope = scopeType;
                }
            }
        }
        public bool HasScopedVariableComplete(string name, ScopeType scopeType)
        {
            if (name.Contains(':'))
                name = name.Substring(name.IndexOf(':')+1);
            if (!unsortedScopedVariables.ContainsKey(name))
                return false;

            var list = unsortedScopedVariables[name];
            {
                foreach (var variable in list)
                {
                    if (variable.InsideScope == scopeType)
                        return true;
                }
            }

            return false;
        }

        public void RegisterSetScopedVariable(ScriptObject node)
        {
            var w = node.Children.Where(a => a.Name == "value");
            ScopeType type = ScopeType.@bool;

            if (w.Count() > 0)
            {
                var f = w.First();

                string s = f.GetStringValue();

                if (s != null)
                {
                    if (!(s == "yes" || s == "no"))
                    {
                        if (s.StartsWith("flag"))
                            type = ScopeType.flag;
                        else
                            type = ScopeType.value;
                    }

                }


            }
            var name = node.Children.Where(a => a.Name == "name");
            var target = node.Children.Where(a => a.Name == "value");

            var n = name.First().GetStringValue();
          //  var t = target.First().GetStringValue();
            /*
            if (!scopedVariables.ContainsKey(n))
            {
                scopedVariables[n] = new Dictionary<ScopeType, Variable>();
            }*/
            if (!unsortedScopedVariables.ContainsKey(n))
            {
                unsortedScopedVariables[n] = new List<Variable>();
            }
            if (!unsortedScopedVariablesByFilename.ContainsKey(node.Topmost.Filename))
            {
                unsortedScopedVariablesByFilename[node.Topmost.Filename] = new List<Variable>();
            }

            var v = new Variable()
            {
                Declared = node, IsValue = true, Name = n, RequiresVarTag = true, ToObj = node.Topmost, VarType = type,
                VariableType = VariableType.Scoped, InsideScope = ScopeType.none
            };
            unsortedScopedVariables[n].Add(v);

            unsortedScopedVariablesByFilename[node.Topmost.Filename].Add(v);
        }

        public void RemoveAllVariablesFromFile(RefFilename filename)
        {
            if (unsortedScopedVariablesByFilename.ContainsKey(filename))
            {
                var variables = unsortedScopedVariablesByFilename[filename];

                for (var index = 0; index < variables.Count; index++)
                {
                    var variable = variables[index];
                    unsortedScopedVariables[variable.Name].Remove(variable);
                    variables.Remove(variable);
                    index--;
                }
            }
            if (globalVariablesByFilename.ContainsKey(filename))
            {
                var variables = globalVariablesByFilename[filename];

                for (var index = 0; index < variables.Count; index++)
                {
                    var variable = variables[index];
                    globalVariables.Remove(variable.Name);
                    variables.Remove(variable);
                    index--;
                }
            }


        }

    }
}