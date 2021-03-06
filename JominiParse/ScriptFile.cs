﻿#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace JominiParse
{
    public class ScriptFile
    {
        private readonly Dictionary<string, ScriptObject> LocalVars = new Dictionary<string, ScriptObject>();

        public Dictionary<string, ScriptObject> Map = new Dictionary<string, ScriptObject>();
        public RefFilename Filename { get; set; }

        public bool IsBase => Filename.IsBase;

        public bool Overridden { get; set; }
        public ScriptContext Context { get; set; }
        public string Namespace { get; set; }

        public void AddLocalVars(List<ScriptObject> vars)
        {
            foreach (var scriptObject in vars)
                LocalVars[scriptObject.Name] = scriptObject;
        }

        public ScopeType GetVarType(string name)
        {
            if (!LocalVars.ContainsKey(name))
                return ScopeType.none;

            return GetVarType(LocalVars[name]);
        }

        public ScopeType GetVarType(ScriptObject v)
        {
            return v.GetVarType();
        }

        public List<string> LocalVarNamelist()
        {
            return LocalVars.Keys.ToList();
        }

        public List<string> LocalVarNamelist(ScopeType type)
        {
            var l = LocalVars.Keys.ToList();

            return l.Where(a => GetVarType(a) == type).ToList();
        }

        public void DoSmartFind(SmartFindOptions options, List<SmartFindResults> results)
        {
            foreach (var mapValue in Map.Values) mapValue.DoSmartFind(options, results);
        }
    }
}