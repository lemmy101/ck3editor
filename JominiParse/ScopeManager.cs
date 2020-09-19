using System;   
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;

namespace JominiParse
{
    public enum ScopeType
    {
        none,
        value,
        @bool,
        flag,
        color,
        character,
        landed_title,
        activity,
        secret,
        province,
        scheme,
        combat,
        combat_side,
        title_and_vassal_change,
        faith,
        great_holy_war,
        religion,
        war,
        story_cycle,
        casus_belli,
        dynasty,
        dynasty_house,
        faction,
        culture,
        army,
        holy_order,
        council_task,
        mercenary_company,
        culture_group,
        inheritparent,
        any,
        @string,
        max,

    }

    public class ScopeChangeDefinition
    {
        public ScopeType toType;
        public string text;
        public bool singular { get; set; }
    }
    public class FunctionDef
    {
    
        public string name { get; set; }
        public string type { get; set; }
        public ScopeType validscope { get; set; }
        public string specialrules { get; set; }
        public bool treatAsScope { get; set; }

        public List<FunctionProperty> Properties = new List<FunctionProperty>();
    }

    public class FunctionProperty
    {
        public string name { get; set; }
        public string type { get; set; }
    }


    public class ScopeTypeDef
    {
        public ScopeType ScopeType { get; set; }

        public Dictionary<string, ScopeChangeDefinition> ValidTriggerScopes = new Dictionary<string, ScopeChangeDefinition>();
        public Dictionary<string, ScopeChangeDefinition> ValidEffectScopes = new Dictionary<string, ScopeChangeDefinition>();
         public HashSet<FunctionDef> ValidEffects = new HashSet<FunctionDef>();
        public HashSet<FunctionDef> ValidTriggers = new HashSet<FunctionDef>();
        public Dictionary<string, FunctionDef> ValidTriggerMap = new Dictionary<string, FunctionDef>();
        public Dictionary<string, FunctionDef> ValidEffectMap = new Dictionary<string, FunctionDef>();
    }

    public class ScopeManager
    {
  /*      public static ScopeManager Instance = new ScopeManager();

        public Dictionary<ScopeType, ScopeTypeDef> Defs = new Dictionary<ScopeType, ScopeTypeDef>();
        public void LoadTriggerDefinitions(string filename)
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
                LoadTrigger(el);
                el = el.NextSibling;

            }

            foreach (var scopeTypeDef in Defs)
            {
 
                foreach (var valueValidTriggerScope in scopeTypeDef.Value.ValidTriggers)
                {
                    if (valueValidTriggerScope.Properties.Count > 0)
                        SchemaManager.Instance.CreateScopeFunction(scopeTypeDef.Key, valueValidTriggerScope, BlockType.function_block);
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
                foreach (var valueValidTriggerScope in scopeTypeDef.Value.ValidTriggerScopes)
                {
                    CreateScopeSchema(scopeTypeDef.Key, valueValidTriggerScope.Value, BlockType.Trigger_scope_change);
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

        public FunctionDef GetEffect(ScopeType scope, string child)
        {
            if (Defs[scope].ValidEffectMap.ContainsKey(child))
                return Defs[scope].ValidEffectMap[child];

            if (scope != ScopeType.any)
                return GetEffect(ScopeType.any, child);


            return null;

        }
        public FunctionDef GetTrigger(ScopeType scope, string child)
        {
            if(Defs[scope].ValidTriggerMap.ContainsKey(child))
                return Defs[scope].ValidTriggerMap[child];

            if (scope != ScopeType.any)
                return GetTrigger(ScopeType.any, child);

            return null;
        }
        private void LoadScope(XmlNode el)
        {
            string name = el.Attributes["name"].InnerText;
            string from = el.Attributes["from"].InnerText;
            string to = el.Attributes["to"].InnerText;
            string type = el.Attributes["type"].InnerText;
            bool singular = false;
            if(el.Attributes["singular"] != null)
            {
                singular = el.Attributes["singular"].InnerText == "yes";
            }
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

            CreateScopeChange(f, name, t, type, singular);
        }
        private void LoadTrigger(XmlNode el)
        {
            string name = el.Attributes["name"].InnerText;
            string validscope = el.Attributes["validscope"].InnerText;
            var cd = new FunctionDef();

            if (el.FirstChild != null)
            {
                ScopeType f = ScopeType.none;
                if (!Enum.TryParse(validscope, out f))
                {
                    throw new Exception();
                }

                cd.type = el.Attributes["type"] != null ? el.Attributes["type"].InnerText : null;
                cd.validscope = f;
                cd.name = name;

                if (el.Attributes["specialrules"] != null)
                {
                    cd.specialrules = el.Attributes["specialrules"].InnerText;
                }

                if (el.Attributes["treatAsScope"] != null)
                {
                    cd.treatAsScope = el.Attributes["treatAsScope"].InnerText=="yes";
                }

                var c = el.FirstChild;

                while (c != null)
                {
                    string pn = c.Attributes["name"].InnerText;
                    string pt = c.Attributes["type"].InnerText;

                    cd.Properties.Add(new FunctionProperty() { name = pn, type = pt });

                    c = c.NextSibling;
                }

                Defs[f].ValidTriggers.Add(cd);
                Defs[f].ValidTriggerMap[cd.name] = cd;
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

                if (el.Attributes["treatAsScope"] != null)
                {
                    cd.treatAsScope = el.Attributes["treatAsScope"].InnerText == "yes";
                }
                cd.validscope = f;
                Defs[f].ValidTriggers.Add(cd);
                Defs[f].ValidTriggerMap[cd.name] = cd;

            }
        }
        private void LoadEffect(XmlNode el)
        {
            string name = el.Attributes["name"].InnerText;
            string validscope = el.Attributes["validscope"].InnerText;
            var cd = new FunctionDef();

            if (el.FirstChild != null)
            {
                ScopeType f = ScopeType.none;
                if (!Enum.TryParse(validscope, out f))
                {
                    throw new Exception();
                }

                cd.type = el.Attributes["type"] != null ? el.Attributes["type"].InnerText : null;
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

                    cd.Properties.Add(new FunctionProperty() { name = pn, type = pt });

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

        private void CreateScopeChange(ScopeType from, string name, ScopeType to, string type, bool singular=false)
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
                def.ValidEffectScopes[name] = new ScopeChangeDefinition() { text = name, toType = to, singular=singular };
            }
            else if(type=="Trigger")
            {
                def.ValidTriggerScopes[name] = new ScopeChangeDefinition() { text = name, toType = to, singular = singular };

            }
            else
            {
                def.ValidEffectScopes[name] = new ScopeChangeDefinition() { text = name, toType = to, singular = singular };
                def.ValidTriggerScopes[name] = new ScopeChangeDefinition() { text = name, toType = to, singular = singular };
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
                def.ValidEffects.Add(new FunctionDef() {name=name});
                def.ValidEffectMap[name] = def.ValidEffects.Last();
            }

        }
        private void AddScopeTrigger(ScopeType from, string name)
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
      
        static List<string> ignoreList = new List<string>() {

            "modifier",
        };
    
        public ScopeType ChangeScope(ScopeType from, string name, out bool success, ScriptObject parent = null, ScriptObject.ScopeFindType findType = ScriptObject.ScopeFindType.Object)
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

            string scopeName = name;
            ScopeType scope = from;
            if (scopeName.Contains("."))
            {
                var stu = scope;
                string[] split = scopeName.Split('.');

                for (var index = 0; index < split.Length; index++)
                {
                    var s = split[index];

                    {

                        var newScope = ChangeScope(stu, s, out success, parent, findType);
                        if (!success)
                            break;
                        stu = newScope;
                    }

                }
                if(success)
                    return stu;
            }

            {
                if (parent != null && isEffectScopeInside(from, name, parent, findType))
                {
                    success = true;
                    return getEffectScopeInside(from, name, parent);
                }

             }
            return from;
        }

        public ScopeType ChangeTriggerScope(ScopeType from, string name, out bool success, ScriptObject parent = null, ScriptObject.ScopeFindType findType = ScriptObject.ScopeFindType.Object)
        {
            success = false;

            if (name == "root")
            {
                success = true;
                if (parent == null)
                    return from;
                return parent.GetRootScopeType();
            }

            if (name == "prev")
            {
                success = true;
                if (parent == null)
                    return from;
                return parent.GetPrevScopeType();
            }

            if (name == "this")
            {
                success = true;
                return from;
            }


            if (Defs.ContainsKey(from))
            {
                if (Defs[ScopeType.any].ValidTriggerScopes.ContainsKey(name))
                {
                    success = true;
                    return Defs[ScopeType.any].ValidTriggerScopes[name].toType;
                }
                else if (Defs[from].ValidTriggerScopes.ContainsKey(name))
                {
                    success = true;
                    return Defs[from].ValidTriggerScopes[name].toType;
                }

            }

            string scopeName = name;
            ScopeType scope = from;
            if (scopeName.Contains("."))
            {
                var stu = scope;
                string[] split = scopeName.Split('.');

                for (var index = 0; index < split.Length; index++)
                {
                    var s = split[index];

                    {

                        var newScope = ChangeTriggerScope(stu, s, out success, parent, findType);
                        if (!success)
                            break;
                        stu = newScope;
                    }

                }
                if(success)
                    return stu;
            }

            {

                if (parent != null && isTriggerScopeInside(from, name, parent, findType))
                {
                    success = true;
                    return getTriggerScopeInside(from, name, parent, findType);
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

                foreach (var s in split)
                {
                    if (!((Defs.ContainsKey(current) && Defs[current].ValidEffectScopes.ContainsKey(s.Trim()) &&
                           Defs[current].ValidEffectScopes[s.Trim()].singular) ||
                          (Defs.ContainsKey(ScopeType.any) &&
                           Defs[ScopeType.any].ValidEffectScopes.ContainsKey(s.Trim()) &&
                           Defs[ScopeType.any].ValidEffectScopes[s.Trim()].singular)))
                    {
                        return false;
                    }
                    else
                    {
                        if (Defs[current].ValidEffectScopes.ContainsKey(s.Trim()))
                            current = Defs[current].ValidEffectScopes[s.Trim()].toType;
                        else if (Defs[ScopeType.any].ValidEffectScopes.ContainsKey(s.Trim()))
                            current = Defs[ScopeType.any].ValidEffectScopes[s.Trim()].toType;
                    }


                }
            }

            return false;
        }
        public bool isEffectScopeToParam(ScopeType current, string name)
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

                for (var index = 0; index < split.Length-1; index++)
                {
                    var s = split[index];
                    if (!((Defs.ContainsKey(current) && Defs[current].ValidEffectScopes.ContainsKey(s.Trim()) &&
                           Defs[current].ValidEffectScopes[s.Trim()].singular) ||
                          (Defs.ContainsKey(ScopeType.any) &&
                           Defs[ScopeType.any].ValidEffectScopes.ContainsKey(s.Trim()) &&
                           Defs[ScopeType.any].ValidEffectScopes[s.Trim()].singular)))
                    {
                        return false;
                    }
                    else
                    {
                        if (Defs[current].ValidEffectScopes.ContainsKey(s.Trim()))
                            current = Defs[current].ValidEffectScopes[s.Trim()].toType;
                        else if (Defs[ScopeType.any].ValidEffectScopes.ContainsKey(s.Trim()))
                            current = Defs[ScopeType.any].ValidEffectScopes[s.Trim()].toType;
                    }
                }

                if (isEffect(current, split[split.Length - 1]))
                    return true;
            }

            return false;
        }
        public bool isTrigger(ScopeType scope, string name)
        {
            if (Defs.ContainsKey(scope) && Defs[scope].ValidTriggerMap.ContainsKey(name.Trim()))
                return true;
            if (scope != ScopeType.any)
                return isTrigger(ScopeType.any, name);

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

        public bool isSingularTriggerScope(ScopeType current, string name, out ScopeType resScope, ScriptObject parent = null)
        {
            resScope = current;
            bool inside = isTriggerScopeInside(current, name, parent);
            bool Trigger = isTriggerScope(current, name);

            if (!Trigger && !inside)
                return false;

            if (inside)
            {
                if(name.Contains("."))
                {
                    string test = name.Substring(0, name.LastIndexOf("."));
                    var secondLast = getTriggerScopeInside(current, test, parent);
                    string final = name.Substring(name.LastIndexOf(".")+1);
                    
                    if (Defs[secondLast].ValidTriggerScopes.ContainsKey(final) && Defs[secondLast].ValidTriggerScopes[final].singular)
                    {
                        resScope = Defs[secondLast].ValidTriggerScopes[final].toType;
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }

            if (Defs[current].ValidTriggerScopes.ContainsKey(name) && Defs[current].ValidTriggerScopes[name].singular)
            {
                resScope = Defs[current].ValidTriggerScopes[name].toType;
                return true;
            }

            if (name.Contains("."))
            {
                string[] split = name.Split('.');

                foreach (var s in split)
                {
                    if (!((Defs.ContainsKey(current) && Defs[current].ValidTriggerScopes.ContainsKey(s.Trim()) &&
                         Defs[current].ValidTriggerScopes[s.Trim()].singular) ||
                        (Defs.ContainsKey(ScopeType.any) && Defs[ScopeType.any].ValidTriggerScopes.ContainsKey(s.Trim()) &&
                        Defs[ScopeType.any].ValidTriggerScopes[s.Trim()].singular)))
                    {
                        return false;
                    }
                    else
                    {
                        if (Defs[current].ValidTriggerScopes.ContainsKey(s.Trim()))
                            current = Defs[current].ValidTriggerScopes[s.Trim()].toType;
                        else
                        if (Defs[ScopeType.any].ValidTriggerScopes.ContainsKey(s.Trim()))
                            current = Defs[ScopeType.any].ValidTriggerScopes[s.Trim()].toType;
                    }


                }


                return true;

            }

            return false;
        }
        public bool isTriggerScope(ScopeType current, string name)
        {
            if (name == null)
                return false;

            if (name.StartsWith("scope:"))
                return false;

            if (Defs.ContainsKey(current) && Defs[current].ValidTriggerScopes.ContainsKey(name))
                return true;
            if (Defs.ContainsKey(current) && Defs[ScopeType.any].ValidTriggerScopes.ContainsKey(name))
                return true;


            if (name.Contains("."))
            {
                string[] split = name.Split('.');

                foreach (var s in split)
                {
                    if (!((Defs.ContainsKey(current) && Defs[current].ValidTriggerScopes.ContainsKey(s.Trim()) &&
                           Defs[current].ValidTriggerScopes[s.Trim()].singular) ||
                          (Defs.ContainsKey(ScopeType.any) && Defs[ScopeType.any].ValidTriggerScopes.ContainsKey(s.Trim()) &&
                           Defs[ScopeType.any].ValidTriggerScopes[s.Trim()].singular)))
                    {
                        return false;
                    }
                    else
                    {
                        if (Defs[current].ValidTriggerScopes.ContainsKey(s.Trim()))
                            current = Defs[current].ValidTriggerScopes[s.Trim()].toType;
                        else
                        if (Defs[ScopeType.any].ValidTriggerScopes.ContainsKey(s.Trim()))
                            current = Defs[ScopeType.any].ValidTriggerScopes[s.Trim()].toType;
                    }


                }


                return true;

            }

            return false;
        }
        public bool isTriggerScopeToParam(ScopeType current, string name)
        {
            if (name == null)
                return false;

            if (name.StartsWith("scope:"))
                return false;

            if (Defs.ContainsKey(current) && Defs[current].ValidTriggerScopes.ContainsKey(name))
                return true;
            if (Defs.ContainsKey(current) && Defs[ScopeType.any].ValidTriggerScopes.ContainsKey(name))
                return true;


            if (name.Contains("."))
            {
                string[] split = name.Split('.');

                for (var index = 0; index < split.Length - 1; index++)
                {
                    var s = split[index];
                    if (!((Defs.ContainsKey(current) && Defs[current].ValidTriggerScopes.ContainsKey(s.Trim()) &&
                           Defs[current].ValidTriggerScopes[s.Trim()].singular) ||
                          (Defs.ContainsKey(ScopeType.any) &&
                           Defs[ScopeType.any].ValidTriggerScopes.ContainsKey(s.Trim()) &&
                           Defs[ScopeType.any].ValidTriggerScopes[s.Trim()].singular)))
                    {
                        return false;
                    }
                    else
                    {
                        if (Defs[current].ValidTriggerScopes.ContainsKey(s.Trim()))
                            current = Defs[current].ValidTriggerScopes[s.Trim()].toType;
                        else if (Defs[ScopeType.any].ValidTriggerScopes.ContainsKey(s.Trim()))
                            current = Defs[ScopeType.any].ValidTriggerScopes[s.Trim()].toType;
                    }
                }

                if (isTrigger(current, split[split.Length - 1]))
                    return true;


            }

            return false;
        }

        public void AddCompleteScopeTriggerResults(ScopeType scope, List<string> results)
        {
            foreach (var scopeChangeDefinition in Defs[scope].ValidTriggerScopes.Values)
                results.Add(scopeChangeDefinition.text);
            foreach (var scopeChangeDefinition in Defs[scope].ValidTriggerMap.Values)
                results.Add(scopeChangeDefinition.name);

            foreach (var scopeChangeDefinition in Defs[ScopeType.any].ValidTriggerScopes.Values)
                results.Add(scopeChangeDefinition.text);
            foreach (var scopeChangeDefinition in Defs[ScopeType.any].ValidTriggerMap.Values)
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

        public bool isEffectScopeInside(ScopeType scope, string name, ScriptObject scriptObjectParent, ScriptObject.ScopeFindType findType= ScriptObject.ScopeFindType.Object)
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
                        var newScope = ChangeScope(stu, "scope:"+s, out success, scriptObjectParent, findType);
                        if (!success)
                            return false;
                        stu = newScope;
                    }
                    else
                    {
                        bool success;
                        var newScope = ChangeScope(stu, s, out success, scriptObjectParent, findType);
                        if (!success)
                            return false;
                        stu = newScope;
                    }

                }

                return true;
            }

            List<ScriptObject.ScriptScope> results = new List<ScriptObject.ScriptScope>();
            scriptObjectParent.GetValidScriptScopes(results, true, findType);

            if (results.Any(a => a.Name == scopeName && (a.IsValue == (findType != ScriptObject.ScopeFindType.Object))))
                return true;
           
            if (scope != ScopeType.any)
                return isEffectScopeInside(ScopeType.any, name, scriptObjectParent, findType);

            return false;
        }

        public bool isTriggerScopeInside(ScopeType scope, string name, ScriptObject scriptObjectParent, ScriptObject.ScopeFindType findType = ScriptObject.ScopeFindType.Object)
        {
            if (!name.StartsWith("scope:"))
                name = "scope:" + name;

            if (scriptObjectParent == null)
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
                        var newScope = ChangeTriggerScope(stu, "scope:" + s, out success, scriptObjectParent, findType);
                        if (!success)
                            return false;
                        stu = newScope;
                    }
                    else
                    {
                        bool success;
                        var newScope = ChangeTriggerScope(stu, s, out success, null, findType);
                        if (!success)
                            return false;
                        stu = newScope;
                    }

                }

                return true;
            }

            List<ScriptObject.ScriptScope> results = new List<ScriptObject.ScriptScope>();
            scriptObjectParent.GetValidScriptScopes(results, true, findType);

            if (results.Any(a => a.Name == scopeName && (a.IsValue==(findType != ScriptObject.ScopeFindType.Object))))
                return true;

            if (scope != ScopeType.any)
                return isTriggerScopeInside(ScopeType.any, name, scriptObjectParent, findType);

            return false;
        }
        public bool isTriggerScopeEndParamInside(ScopeType scope, string name, ScriptObject scriptObjectParent)
        {
            if (!name.StartsWith("scope:"))
                return false;

            if (scriptObjectParent == null)
                return false;

            string scopeName = name.Substring(name.IndexOf(":") + 1);

            if (scopeName.Contains("."))
            {
                var stu = scope;
                string[] split = scopeName.Split('.');

                for (var index = 0; index < split.Length-1; index++)
                {
                    var s = split[index];
                    if (index == 0)
                    {
                        bool success;
                        var newScope = ChangeTriggerScope(stu, "scope:" + s, out success, scriptObjectParent);
                        if (!success)
                            return false;
                        stu = newScope;
                    }
                    else
                    {
                        bool success;
                        var newScope = ChangeTriggerScope(stu, s, out success);
                        if (!success)
                            return false;
                        stu = newScope;
                    }

                }

                if (isTrigger(stu, split[split.Length - 1]))
                    return true;

                return false;
            }

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

                return results.First(a => a.Name == scopeName).To;
            }

            if (scope != ScopeType.any)
                return getEffectScopeInside(ScopeType.any, name, scriptObjectParent);

            return scope;
        }

        public ScopeType getTriggerScopeInside(ScopeType scope, string name, ScriptObject scriptObjectParent, ScriptObject.ScopeFindType findType = ScriptObject.ScopeFindType.Object)
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
                        var newScope = ChangeTriggerScope(stu, "scope:" + s, out success, scriptObjectParent, findType);
                        if (!success)
                            return scope;
                        stu = newScope;
                    }
                    else
                    {
                        bool success;
                        var newScope = ChangeTriggerScope(stu, s, out success, null, findType);
                        if (!success)
                            return scope;
                        stu = newScope;
                    }

                }

                return stu;
            }

            List<ScriptObject.ScriptScope> results = new List<ScriptObject.ScriptScope>();
            scriptObjectParent.GetValidScriptScopes(results, true, findType);

            if (results.Any(a => a.Name == scopeName))
            {

                return results.First(a => a.Name == scopeName && (a.IsValue == (findType != ScriptObject.ScopeFindType.Object))).To;
            }

            if (scope != ScopeType.any)
                return getTriggerScopeInside(ScopeType.any, name, scriptObjectParent, findType);

            return scope;
        }

        public ScriptObject.ScriptScope GetSavedScope(ScopeType scope, string name, ScriptObject scriptObjectParent, ScriptObject.ScopeFindType findType = ScriptObject.ScopeFindType.Object)
        {
            if (!name.StartsWith("scope:"))
                return null;

            string scopeName = name.Substring(name.IndexOf(":") + 1);

         
            List<ScriptObject.ScriptScope> results = new List<ScriptObject.ScriptScope>();
            scriptObjectParent.GetValidScriptScopes(results, true, findType);

            if (results.Any(a => a.Name == scopeName))
            {

                return results.First(a => a.Name == scopeName && (a.IsValue == (findType != ScriptObject.ScopeFindType.Object)));
            }

            if (scope != ScopeType.any)
                return GetSavedScope(ScopeType.any, name, scriptObjectParent, findType);

            return null;
        }
  */

    }
}
