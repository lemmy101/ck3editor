using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace JominiParse
{
    public enum ScopeType
    {
        none,
        inheritparent,
        character,
        realm,
        dynasty,
        culture,
        county,
        province,
        house,
        war,
        title,
        claim,
        secret,
        scheme,
        holy_order,
        story,
        faction,
        faith,
        activity,
        combat_side,
        army,
        ghw,
        any,
        max,
      
    }

    public class ScopeChangeDefinition
    {
        public ScopeType toType;
        public string text;
    }
    public class ConditionDef
    {
        public class ConditionProperty
        {
            public string name { get; set; }
            public string type { get; set; }
        }
        public string name { get; set; }
        public string type { get; set; }
        public ScopeType validscope { get; set; }
        public string specialrules { get; set; }

        public List<ConditionProperty> Properties = new List<ConditionProperty>();
    }
    public class EffectDef
    {
        public class EffectProperty
        {
            public string name { get; set; }
            public string type { get; set; }
        }
        public string name { get; set; }
        public string type { get; set; }
        public ScopeType validscope { get; set; }
        public string specialrules { get; set; }

        public override string ToString()
        {
            return name;
        }

        public List<EffectProperty> Properties = new List<EffectProperty>();
    }

    public class ScopeTypeDef
    {
        public ScopeType ScopeType { get; set; }

        public Dictionary<string, ScopeChangeDefinition> ValidConditionScopes = new Dictionary<string, ScopeChangeDefinition>();
        public Dictionary<string, ScopeChangeDefinition> ValidEffectScopes = new Dictionary<string, ScopeChangeDefinition>();
         public HashSet<EffectDef> ValidEffects = new HashSet<EffectDef>();
        public HashSet<ConditionDef> ValidConditions = new HashSet<ConditionDef>();
        public Dictionary<string, ConditionDef> ValidConditionMap = new Dictionary<string, ConditionDef>();
        public Dictionary<string, EffectDef> ValidEffectMap = new Dictionary<string, EffectDef>();
    }

    public class ScopeManager
    {
        public static ScopeManager Instance = new ScopeManager();

        public Dictionary<ScopeType, ScopeTypeDef> Defs = new Dictionary<ScopeType, ScopeTypeDef>();
        public void LoadConditionDefinitions(string filename)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(filename);

            for (int n = 0; n < (int)ScopeType.max; n++)
            {
                if (Defs[(ScopeType)n] == null)
                    Defs[(ScopeType)n] = new ScopeTypeDef() { ScopeType = (ScopeType)n };
            }

            var el = doc.DocumentElement.FirstChild as XmlNode;

            while (el != null)
            {
                LoadCondition(el);
                el = el.NextSibling;

            }

            foreach (var scopeTypeDef in Defs)
            {
 
                foreach (var valueValidConditionScope in scopeTypeDef.Value.ValidConditions)
                {
                    if (valueValidConditionScope.Properties.Count > 0)
                        SchemaManager.Instance.CreateScopeFunction(scopeTypeDef.Key, valueValidConditionScope, BlockType.function_block);
                }

       
            }
        
        }
        public void LoadEffectDefinitions(string filename)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(filename);

            for (int n = 0; n < (int)ScopeType.max; n++)
            {
                if (Defs[(ScopeType)n] == null)
                    Defs[(ScopeType)n] = new ScopeTypeDef() { ScopeType = (ScopeType)n };
            }

            var el = doc.DocumentElement.FirstChild as XmlNode;

            while (el != null)
            {
                LoadEffect(el);
                el = el.NextSibling;

            }

            foreach (var scopeTypeDef in Defs)
            {
            
                foreach (var valueValidEffectScope in scopeTypeDef.Value.ValidEffects)
                {
                    if (valueValidEffectScope.Properties.Count > 0)
                        SchemaManager.Instance.CreateScopeFunction(scopeTypeDef.Key, valueValidEffectScope, BlockType.function_block);
                }
            }
        }

        public void LoadScopeDefinitions(string filename)
        {
            XmlDocument doc  = new XmlDocument();

            doc.Load(filename);

            for (int n = 0; n < (int) ScopeType.max; n++)
            {
                if (!Defs.ContainsKey((ScopeType)n))
                    Defs[(ScopeType)n] = new ScopeTypeDef() {ScopeType = (ScopeType)n};
            }

            var el = doc.DocumentElement.FirstChild as XmlNode;

            while (el != null)
            {
                LoadScope(el);
                el = el.NextSibling;

            }

            foreach (var scopeTypeDef in Defs)
            {
                foreach (var valueValidConditionScope in scopeTypeDef.Value.ValidConditionScopes)
                {
                    CreateScopeSchema(scopeTypeDef.Key, valueValidConditionScope.Value, BlockType.condition_scope_change);
                }

                foreach (var valueValidEffectScope in scopeTypeDef.Value.ValidEffectScopes)
                {
                    CreateScopeSchema(scopeTypeDef.Key, valueValidEffectScope.Value, BlockType.effect_scope_change);
                }
            
            }
        }

        private void CreateScopeSchema(ScopeType fromScope, ScopeChangeDefinition scopeDef, BlockType blockType)
        {
            SchemaManager.Instance.CreateScopeSchema(fromScope, scopeDef, blockType);
        }

        public EffectDef GetEffect(ScopeType scope, string child)
        {
            if (Defs[scope].ValidEffectMap.ContainsKey(child))
                return Defs[scope].ValidEffectMap[child];

            if (scope != ScopeType.any)
                return GetEffect(ScopeType.any, child);


            return null;

        }
        public ConditionDef GetCondition(ScopeType scope, string child)
        {
            if(Defs[scope].ValidConditionMap.ContainsKey(child))
                return Defs[scope].ValidConditionMap[child];

            if (scope != ScopeType.any)
                return GetCondition(ScopeType.any, child);

            return null;
        }
        private void LoadScope(XmlNode el)
        {
            string name = el.Attributes["name"].InnerText;
            string from = el.Attributes["from"].InnerText;
            string to = el.Attributes["to"].InnerText;
            string type = el.Attributes["type"].InnerText;

            ScopeType f = ScopeType.none;
            ScopeType t = ScopeType.none;
            if (!Enum.TryParse(from, out f))
            {
                throw new Exception();
            }

            if (!Enum.TryParse(to, out t))
            {
                throw new Exception();
            }

            CreateScopeChange(f, name, t, type);
        }
        private void LoadCondition(XmlNode el)
        {
            string name = el.Attributes["name"].InnerText;
            string validscope = el.Attributes["validscope"].InnerText;
            var cd = new ConditionDef();

            if (el.FirstChild != null)
            {
                ScopeType f = ScopeType.none;
                if (!Enum.TryParse(validscope, out f))
                {
                    throw new Exception();
                }

                cd.validscope = f;
                cd.name = name;

                if (el.Attributes["specialrules"] != null)
                {
                    cd.specialrules = el.Attributes["specialrules"].InnerText;
                }

                var c = el.FirstChild;

                while (c != null)
                {
                    string pn = c.Attributes["name"].InnerText;
                    string pt = c.Attributes["type"].InnerText;

                    cd.Properties.Add(new ConditionDef.ConditionProperty() { name = pn, type = pt });

                    c = c.NextSibling;
                }

                Defs[f].ValidConditions.Add(cd);
                Defs[f].ValidConditionMap[cd.name] = cd;
            }
            else
            {
                cd.type = el.Attributes["type"] != null ? el.Attributes["type"].InnerText : null;
                cd.name = name;

                ScopeType f = ScopeType.none;
                if (!Enum.TryParse(validscope, out f))
                {
                    throw new Exception();
                }

                if (el.Attributes["specialrules"] != null)
                {
                    cd.specialrules = el.Attributes["specialrules"].InnerText;
                }

                cd.validscope = f;
                Defs[f].ValidConditions.Add(cd);
                Defs[f].ValidConditionMap[cd.name] = cd;

            }
        }
        private void LoadEffect(XmlNode el)
        {
            string name = el.Attributes["name"].InnerText;
            string validscope = el.Attributes["validscope"].InnerText;
            var cd = new EffectDef();

            if (el.FirstChild != null)
            {
                ScopeType f = ScopeType.none;
                if (!Enum.TryParse(validscope, out f))
                {
                    throw new Exception();
                }

                cd.validscope = f;
                cd.name = name;

                if (el.Attributes["specialrules"] != null)
                {
                    cd.specialrules = el.Attributes["specialrules"].InnerText;
                }

                var c = el.FirstChild;

                while (c != null)
                {
                    string pn = c.Attributes["name"].InnerText;
                    string pt = c.Attributes["type"].InnerText;

                    cd.Properties.Add(new EffectDef.EffectProperty() { name = pn, type = pt });

                    c = c.NextSibling;
                }

                Defs[f].ValidEffects.Add(cd);
                Defs[f].ValidEffectMap[cd.name] = cd;
            }
            else
            {
                cd.type = el.Attributes["type"] != null ? el.Attributes["type"].InnerText : null;
                cd.name = name;

                ScopeType f = ScopeType.none;
                if (!Enum.TryParse(validscope, out f))
                {
                    throw new Exception();
                }

                if (el.Attributes["specialrules"] != null)
                {
                    cd.specialrules = el.Attributes["specialrules"].InnerText;
                }

                cd.validscope = f;
                Defs[f].ValidEffects.Add(cd);
                Defs[f].ValidEffectMap[cd.name] = cd;

            }
        }

        private void CreateScopeChange(ScopeType from, string name, ScopeType to, string type)
        {
            ScopeTypeDef def = null;
            if (!Defs.ContainsKey(from))
            {
                def = new ScopeTypeDef() { ScopeType = from };
                Defs[from] = def;
            }
            else
            {
                def = Defs[from];
            }

            if (type == "effect")
            {
                def.ValidEffectScopes[name] = new ScopeChangeDefinition() { text = name, toType = to };
            }
            else if(type=="condition")
            {
                def.ValidConditionScopes[name] = new ScopeChangeDefinition() { text = name, toType = to };

            }
            else
            {
                def.ValidEffectScopes[name] = new ScopeChangeDefinition() { text = name, toType = to };
                def.ValidConditionScopes[name] = new ScopeChangeDefinition() { text = name, toType = to };
            }
        }

        internal void AddScopeFunction(ScopeType from, string name)
        {
            return;

            if (name.StartsWith("scope:"))
                return;
      
            ScopeTypeDef def = null;
            if (!Defs.ContainsKey(from))
            {
                def = new ScopeTypeDef() { ScopeType = from };
                Defs[from] = def;
            }
            else
            {
                def = Defs[from];
            }

            

            if (!def.ValidEffectScopes.ContainsKey(name))
            {
                def.ValidEffects.Add(new EffectDef() {name=name});
                def.ValidEffectMap[name] = def.ValidEffects.Last();
            }

        }
        private void AddScopeCondition(ScopeType from, string name)
        {
            ScopeTypeDef def = null;
            if (!Defs.ContainsKey(from))
            {
                def = new ScopeTypeDef() { ScopeType = from };
                Defs[from] = def;
            }
            else
            {
                def = Defs[from];
            }


        }
        HashSet<string> found = new HashSet<string>();
        public void Add(EffectBase effect)
        {
            ScopeType type = effect.GetScopeType();

            if (effect.Parent is ScriptEvent)
            {
       
                var ev = effect.Parent as ScriptEvent;

                var str = ev.type != null ? ev.type.GetStringValue() : "character_event";

                if (str == "character_event")
                {
                    type = ScopeType.character;
                    effect.SetScopeType(type);

                }

                str = ev.scope != null ? ev.scope.GetStringValue() : "";

                if(Enum.TryParse(str, out type))
                {
                    effect.SetScopeType(type);
                }
                
            }


            else if (effect.Parent is EffectBlock)
            {
                if (effect.Name.StartsWith("scope:") || effect.Name.StartsWith("cp:"))
                {
                    effect.SetScopeType(ScopeType.none);

                    return;

                }

                type = effect.Parent.GetScopeType();
                if (type == ScopeType.none)
                    return;
                if (isEffectScope(type, effect.Name))
                {
                    if (effect.Name.Contains("."))
                    {
                        string[] split = effect.Name.Split('.');

                        foreach (var s in split)
                        {
                            bool success;
                            type = ChangeScope(type, s, out success);

                            if (!success)
                            {
                                found.Add(type + "->" + s);
                                type = ScopeType.none;
                                effect.SetScopeType(type);
                                return;
                            }
                            else
                            {
                                effect.SetScopeType(type);

                            }
                        }
                    }
                    else
                    {

                        bool success;
                        type = ChangeScope(type, effect.Name, out success);

                        if (!success)
                        {
                            found.Add(type + "->" + effect.Name);
                            type = ScopeType.none;
                            effect.SetScopeType(type);
                            return;
                        }
                        else
                        {
                            effect.SetScopeType(type);

                        }
                    }

                }
                else
                {
                    AddScopeFunction(type, effect.Name);
                    effect.SetScopeType(type);
                }
            }

        }

        static List<string> ignoreList = new List<string>() {

            "modifier",
        };
        public void Add(ConditionBase effect)
        {
        
            ScopeType type = effect.GetScopeType();
            if (effect.Parent is ScriptEvent)
            {
               
                var ev = effect.Parent as ScriptEvent;

                var str = ev.type != null ? ev.type.GetStringValue() : "character_event";

                if (str == "character_event")
                {
                    type = ScopeType.character;
                    effect.SetScopeType(type);

                }

                str = ev.scope != null ? ev.scope.GetStringValue() : "";

                if (Enum.TryParse(str, out type))
                {
                    effect.SetScopeType(type);
                }
            }
            else if (effect.Parent is ConditionBase)
            {
                if (effect.Name == null)
                    return;
                if (effect.Name == "faith")
                {

                }
                if (effect.Name.StartsWith("scope:") || effect.Name.StartsWith("cp:") || ignoreList.Contains(effect.Name) || effect.Name.EndsWith("_trigger"))
                {
                    effect.SetScopeType(ScopeType.none);

                    return;

                }
                type = effect.Parent.GetScopeType();
                if (type == ScopeType.none)
                    return;


                if (isConditionScope(type, effect.Name))
                {
                    if (effect.Name.Contains("."))
                    {
                        string[] split = effect.Name.Split('.');

                        foreach (var s in split)
                        {
                            bool success;
                            type = ChangeConditionScope(type, s, out success);

                            if (!success)
                            {
                                found.Add(type + "->" + s);
                                type = ScopeType.none;
                                effect.SetScopeType(type);
                                return;
                            }
                            else
                            {
                                effect.SetScopeType(type);

                            }
                        }
                    }
                    else
                    {
                        bool success;
                        type = ChangeConditionScope(type, effect.Name, out success);

                        if (!success)
                        {
                            found.Add(type + "->" + effect.Name);
                            type = ScopeType.none;
                            effect.SetScopeType(type);
                            return;
                        }
                        else
                        {
                            effect.SetScopeType(type);

                        }
                    }

                }
                else
                {
        //            AddScopeCondition(type, effect.Name);
                    effect.SetScopeType(type);
                }
            }

        }

        public ScopeType ChangeScope(ScopeType from, string name, out bool success, ScriptObject parent = null)
        {
            success = false;

            if (Defs.ContainsKey(from))
            {
                if (Defs[ScopeType.any].ValidEffectScopes.ContainsKey(name))
                {
                    success = true;
                    return Defs[ScopeType.any].ValidEffectScopes[name].toType;
                }
                else if (Defs[from].ValidEffectScopes.ContainsKey(name))
                {
                    success = true;
                    return Defs[from].ValidEffectScopes[name].toType;
                }
              
            }
           

            {
                if (parent != null && isEffectScopeInside(from, name, parent))
                {
                    success = true;
                    return getEffectScopeInside(from, name, parent);
                }

             }
            return from;
        }

        public ScopeType ChangeConditionScope(ScopeType from, string name, out bool success, ScriptObject parent=null)
        {
            success = false;

            if (Defs.ContainsKey(from))
            {
                if (Defs[ScopeType.any].ValidConditionScopes.ContainsKey(name))
                {
                    success = true;
                    return Defs[ScopeType.any].ValidConditionScopes[name].toType;
                }
                else if (Defs[from].ValidConditionScopes.ContainsKey(name))
                {
                    success = true;
                    return Defs[from].ValidConditionScopes[name].toType;
                }
               
            }
           
            {

                if (parent != null && isConditionScopeInside(from, name, parent))
                {
                    success = true;
                    return getConditionScopeInside(from, name, parent);
                }

              
            }
            return from;
        }

        public Dictionary<string, HashSet<string>> otherScopes = new Dictionary<string, HashSet<string>>();

        public bool isEffectScope(ScopeType current, string name)
        {
            if (name == null)
                return false;

            if (name.StartsWith("scope:"))
                return false;

            if (Defs.ContainsKey(current) && Defs[current].ValidEffectScopes.ContainsKey(name))
                return true;

            if (Defs.ContainsKey(current) && Defs[ScopeType.any].ValidEffectScopes.ContainsKey(name))
                return true;

            if (name.Contains("."))
            {
                string[] split = name.Split('.');
                if (Defs.ContainsKey(current) && Defs[current].ValidEffectScopes.ContainsKey(split[0].Trim()))
                    return true;
                if (Defs.ContainsKey(current) && Defs[ScopeType.any].ValidEffectScopes.ContainsKey(split[0].Trim()))
                    return true;

            }

            return false;
        }
        public bool isCondition(ScopeType scope, string name)
        {
            if (Defs.ContainsKey(scope) && Defs[scope].ValidConditionMap.ContainsKey(name.Trim()))
                return true;
            if (scope != ScopeType.any)
                return isEffect(ScopeType.any, name);

            return false;
        }

        public bool isEffect(ScopeType scope, string name)
        {
            if (Defs.ContainsKey(scope) && Defs[scope].ValidEffectMap.ContainsKey(name.Trim()))
                return true;

            if (scope != ScopeType.any)
                return isEffect(ScopeType.any, name);
            return false;

        }

        public bool isConditionScope(ScopeType current, string name)
        {
            if (name == null)
                return false;

            if (name.StartsWith("scope:"))
                return false;

            if (Defs.ContainsKey(current) && Defs[current].ValidConditionScopes.ContainsKey(name))
                return true;
            if (name.StartsWith("any_"))
                return true;
            if (Defs.ContainsKey(current) && Defs[ScopeType.any].ValidConditionScopes.ContainsKey(name))
                return true;

            if (name.Contains("."))
            {
                string[] split = name.Split('.');
                if (Defs.ContainsKey(current) && Defs[current].ValidConditionScopes.ContainsKey(split[0].Trim()))
                    return true;
                if (Defs.ContainsKey(current) && Defs[ScopeType.any].ValidConditionScopes.ContainsKey(split[0].Trim()))
                    return true;

            }

            return false;
        }

        public void AddCompleteScopeConditionResults(ScopeType scope, List<string> results)
        {
            foreach (var scopeChangeDefinition in Defs[scope].ValidConditionScopes.Values)
                results.Add(scopeChangeDefinition.text);
            foreach (var scopeChangeDefinition in Defs[scope].ValidConditionMap.Values)
                results.Add(scopeChangeDefinition.name);

            foreach (var scopeChangeDefinition in Defs[ScopeType.any].ValidConditionScopes.Values)
                results.Add(scopeChangeDefinition.text);
            foreach (var scopeChangeDefinition in Defs[ScopeType.any].ValidConditionMap.Values)
                results.Add(scopeChangeDefinition.name);

            results.Add("NOT");
            results.Add("AND");
            results.Add("OR");
            results.Add("NOR");
        }
        public void AddCompleteScopeEffectResults(ScopeType scope, List<string> results)
        {
            foreach (var scopeChangeDefinition in Defs[scope].ValidEffectScopes.Values)
                results.Add(scopeChangeDefinition.text);
            foreach (var scopeChangeDefinition in Defs[scope].ValidEffectMap.Values)
                results.Add(scopeChangeDefinition.name);

            foreach (var scopeChangeDefinition in Defs[ScopeType.any].ValidEffectScopes.Values)
                results.Add(scopeChangeDefinition.text);
            foreach (var scopeChangeDefinition in Defs[ScopeType.any].ValidEffectMap.Values)
                results.Add(scopeChangeDefinition.name);

            results.Add("hidden_effect");
        }

        public bool isEffectScopeInside(ScopeType scope, string name, ScriptObject scriptObjectParent)
        {
            if (!name.StartsWith("scope:"))
                return false;

            string scopeName = name.Substring(name.IndexOf(":") + 1);

            if (scopeName.Contains("."))
            {
                var stu = scope;
                string[] split = scopeName.Split('.');

                for (var index = 0; index < split.Length; index++)
                {
                    var s = split[index];
                    if (index == 0)
                    {
                        bool success;
                        var newScope = ChangeScope(stu, "scope:"+s, out success, scriptObjectParent);
                        if (!success)
                            return false;
                        stu = newScope;
                    }
                    else
                    {
                        bool success;
                        var newScope = ChangeScope(stu, s, out success, scriptObjectParent);
                        if (!success)
                            return false;
                        stu = newScope;
                    }

                }

                return true;
            }

            List<ScriptObject.ScriptScope> results = new List<ScriptObject.ScriptScope>();
            scriptObjectParent.GetValidScriptScopes(results, true);

            if (results.Any(a => a.Name == scopeName))
                return true;
           
            if (scope != ScopeType.any)
                return isEffectScopeInside(ScopeType.any, name, scriptObjectParent);

            return false;
        }

        public bool isConditionScopeInside(ScopeType scope, string name, ScriptObject scriptObjectParent)
        {
            if (!name.StartsWith("scope:"))
                return false;

            string scopeName = name.Substring(name.IndexOf(":") + 1);

            if (scopeName.Contains("."))
            {
                var stu = scope;
                string[] split = scopeName.Split('.');

                for (var index = 0; index < split.Length; index++)
                {
                    var s = split[index];
                    if (index == 0)
                    {
                        bool success;
                        var newScope = ChangeConditionScope(stu, "scope:"+s, out success, scriptObjectParent);
                        if (!success)
                            return false;
                        stu = newScope;
                    }
                    else
                    {
                        bool success;
                        var newScope = ChangeConditionScope(stu, s, out success);
                        if (!success)
                            return false;
                        stu = newScope;
                    }

                }

                return true;
            }

            List<ScriptObject.ScriptScope> results = new List<ScriptObject.ScriptScope>();
            scriptObjectParent.GetValidScriptScopes(results, true);

            if (results.Any(a => a.Name == scopeName))
                return true;
           
            if (scope != ScopeType.any)
                return isConditionScopeInside(ScopeType.any, name, scriptObjectParent);

            return false;
        }
        public ScopeType getEffectScopeInside(ScopeType scope, string name, ScriptObject scriptObjectParent)
        {
            if (!name.StartsWith("scope:"))
                return scope;

            string scopeName = name.Substring(name.IndexOf(":") + 1);

            if (scopeName.Contains("."))
            {
                var stu = scope;
                string[] split = scopeName.Split('.');

                for (var index = 0; index < split.Length; index++)
                {
                    var s = split[index];
                    if (index == 0)
                    {
                        bool success;
                        var newScope = ChangeScope(stu, "scope:" + s, out success, scriptObjectParent);
                        if (!success)
                            return scope;
                        stu = newScope;
                    }
                    else
                    {
                        bool success;
                        var newScope = ChangeScope(stu, s, out success);
                        if (!success)
                            return scope;
                        stu = newScope;
                    }

                }

                return stu;
            }

            List<ScriptObject.ScriptScope> results = new List<ScriptObject.ScriptScope>();
            scriptObjectParent.GetValidScriptScopes(results, true);


            if (results.Any(a => a.Name == scopeName))
            {

                return results.First().To;
            }

            if (scope != ScopeType.any)
                return getEffectScopeInside(ScopeType.any, name, scriptObjectParent);

            return scope;
        }

        public ScopeType getConditionScopeInside(ScopeType scope, string name, ScriptObject scriptObjectParent)
        {
            if (!name.StartsWith("scope:"))
                return scope;

            string scopeName = name.Substring(name.IndexOf(":") + 1);

            if (scopeName.Contains("."))
            {
                var stu = scope;
                string[] split = scopeName.Split('.');

                for (var index = 0; index < split.Length; index++)
                {
                    var s = split[index];
                    if (index == 0)
                    {
                        bool success;
                        var newScope = ChangeConditionScope(stu, "scope:" + s, out success, scriptObjectParent);
                        if (!success)
                            return scope;
                        stu = newScope;
                    }
                    else
                    {
                        bool success;
                        var newScope = ChangeConditionScope(stu, s, out success);
                        if (!success)
                            return scope;
                        stu = newScope;
                    }

                }

                return stu;
            }

            List<ScriptObject.ScriptScope> results = new List<ScriptObject.ScriptScope>();
            scriptObjectParent.GetValidScriptScopes(results, true);

            if (results.Any(a => a.Name == scopeName))
            {

                return results.First().To;
            }

            if (scope != ScopeType.any)
                return getConditionScopeInside(ScopeType.any, name, scriptObjectParent);

            return scope;
        }
    }
}
