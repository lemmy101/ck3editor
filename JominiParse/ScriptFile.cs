using System.Collections.Generic;
using System.Linq;

namespace JominiParse
{
    public class ScriptFile
    {
        public string Filename { get; set; }
        public bool IsBase { get; set; }
        public bool Overridden { get; set; }
        public ScriptContext Context { get; set; }
        public string Namespace { get; set; }

        public Dictionary<string, ScriptObject> Map = new Dictionary<string, ScriptObject>();

        Dictionary<string, ScriptObject> LocalVars = new Dictionary<string, ScriptObject>();

        public void AddLocalVars(List<ScriptObject> vars)
        {
            foreach (var scriptObject in vars)
                LocalVars[scriptObject.Name] = scriptObject;


        }

        public ScriptObject.ScopeVarType GetVarType(string name)
        {
            if (!LocalVars.ContainsKey(name))
                return ScriptObject.ScopeVarType.None;

            return GetVarType(LocalVars[name]);
        }
        public ScriptObject.ScopeVarType GetVarType(ScriptObject v)
        {
            return v.GetVarType();

        }

        public List<string> LocalVarNamelist()
        {
            return LocalVars.Keys.ToList();
        }
        public List<string> LocalVarNamelist(ScriptObject.ScopeVarType type)
        {
            var l = LocalVars.Keys.ToList();

            return l.Where(a => GetVarType(a) == type).ToList();

        }

        public void DoSmartFind(SmartFindOptions options, List<SmartFindResults> results)
        {
            foreach (var mapValue in Map.Values)
            {
                mapValue.DoSmartFind(options, results);
            }
        }
    }
}