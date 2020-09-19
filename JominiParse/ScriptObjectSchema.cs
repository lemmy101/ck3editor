using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using NotImplementedException = System.NotImplementedException;

namespace JominiParse
{

    public class SchemaNode
    {
        internal List<SchemaNode> children = new List<SchemaNode>();
        public List<ScopeType> scopes = new List<ScopeType>();

        HashSet<string> inheritsIds = new HashSet<string>();
        public HashSet<SchemaNode> inherits = new HashSet<SchemaNode>();
        public HashSet<SchemaNode> inheritsCached = new HashSet<SchemaNode>();
        private List<SchemaNode> cachedChildren;

        public string namesFrom { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        private string schema { get; set; }
        public string category { get; set; }
        public ScopeType targetScope { get; set; }
        public string doc { get; set; }
        public string function { get; set; }
        string typeList { get; set; }

        public bool rightHandOnly { get; set; }
        public bool avoidRed { get; set; }
        public bool requiresData { get; set; }
        public bool globalLink { get; set; }
        public bool wildcard { get; set; }
        public bool allowScopes { get; set; } = true;

        
        public void copyTo(SchemaNode dest)
        {
            dest.children = new List<SchemaNode>(children);
            dest.inheritsIds = new HashSet<string>();
            dest.inheritsIds.UnionWith(inheritsIds);
            dest.inherits = new HashSet<SchemaNode>(inherits);
            dest.scopes = new List<ScopeType>(scopes);
         
            dest.namesFrom = namesFrom;
            dest.name = name;
            dest.category = category;
            dest.targetScope = targetScope;
            dest.doc = doc;
            dest.function = function;
            dest.typeList = typeList;
            dest.requiresData = requiresData;
            dest.globalLink = globalLink;
            dest.wildcard = wildcard;
            dest.allowScopes = allowScopes;
        }

        bool loadBool(XmlNode node, string attribute, bool def)
        {
            if (node.Attributes[attribute] != null)
                return node.Attributes[attribute].InnerText=="yes";

            return def;

        }
        string loadString(XmlNode node, string attribute)
        {
            if (node.Attributes[attribute] != null)
                return node.Attributes[attribute].InnerText;

            return null;
        }
        public void Load(string file)
        {
            XmlDocument d = new XmlDocument();

            d.Load(file);

            var node = d.DocumentElement as XmlNode;

            LoadNode(node);

            
        }

        public void LoadNode(XmlNode node)
        {

            id = loadString(node as XmlNode, "id");

            if(id == "bookmark_character")
            {

            }
            
            name = loadString(node as XmlNode, "name");
            typeList = loadString(node as XmlNode, "type");
            namesFrom = loadString(node as XmlNode, "namesFrom");
            category = loadString(node as XmlNode, "category");
            schema = loadString(node as XmlNode, "schema");

            string scopes = loadString(node as XmlNode, "scope");
            string inherits = loadString(node as XmlNode, "inherits");

            if(inherits!= null)
            {
                var i = inherits.Split(',');

                foreach (var s in i)
                {
                    this.inheritsIds.Add(s.Trim());
                }
            }
         
            if (scopes != null)
            {
                var scps = scopes.Split(',');

                foreach (var scp in scps)
                {
                    ScopeType scopeEnum;
                    if (Enum.TryParse(scp, out scopeEnum))
                    {
                        this.scopes.Add(scopeEnum);
                    }
                }
            }
            else
            {
                this.scopes.Add(ScopeType.inheritparent);
            }
          

            string targetScope = loadString(node as XmlNode, "targetScope");
            ScopeType tsv;
            Enum.TryParse(targetScope, out tsv);

            this.targetScope = tsv;

            function = loadString(node as XmlNode, "function");

            requiresData = loadBool(node as XmlNode, "requiresData", false);
            globalLink = loadBool(node as XmlNode, "globalLink", false);
            wildcard = loadBool(node as XmlNode, "wildcard", false);
            allowScopes = loadBool(node as XmlNode, "allowScopes", true);
            rightHandOnly = loadBool(node as XmlNode, "rightHandOnly", false);
            avoidRed = loadBool(node as XmlNode, "avoidRed", false);

            var c = node.FirstChild;

            while(c != null)
            {
                if (c is XmlComment)
                {
                    c = c.NextSibling;
                    continue;
                }
                SchemaNode cn = new SchemaNode();
                cn.LoadNode(c);


                if (cn.function == "script_list")
                {

                    SchemaNode any = new SchemaNode();
                    cn.copyTo(any);
                    any.name = "any_" + cn.name;
                    children.Add(any);
                 
                    any.inheritsIds.Add("scriptlistTriggerInherits");
                    any.inheritsIds.Add("trigger");

                    SchemaNode random = new SchemaNode();
                    cn.copyTo(random);
                    random.name = "random_" + cn.name;
                    children.Add(random);
                 
                    random.inheritsIds.Add("scriptlistEffectInherits");
                    random.inheritsIds.Add("effect");

                    SchemaNode every = new SchemaNode();
                    cn.copyTo(every);
                    every.name = "every_" + cn.name;
                    children.Add(every);
                
                    every.inheritsIds.Add("scriptlistEffectInherits");
                    every.inheritsIds.Add("effect");

                    SchemaNode ordered = new SchemaNode();
                    cn.copyTo(ordered);
                    ordered.name = "ordered_" + cn.name;
                    children.Add(ordered);
                
                    ordered.inheritsIds.Add("scriptlistEffectInherits");
                    ordered.inheritsIds.Add("effect");

                }
                else
                {
                    children.Add(cn);
                 

                }

                c = c.NextSibling;
            }


            if (children.Count > 0 && !children.Any(a=>a.name == "scope"))
            {
                var cc = SchemaManager.Instance.GetSchema("rhscope").children;
                children.AddRange(cc);
            }

            if (id != null)
            {
                SchemaManager.Instance.AddSchema(id, this);
            }
        }
        public List<SchemaNode> Children
        {
            get
            {
                if (cachedChildren != null)
                {
                    if(inheritsCached != null)
                    {
                        bool match = inherits.SequenceEqual(inheritsCached);

                        if(match)
                            return cachedChildren;
                    }
                    
                }

                List<SchemaNode> nodes = new List<SchemaNode>(children);

                foreach (var schemaNode in inherits)
                {
                    nodes.AddRange(schemaNode.Children);
                }

                cachedChildren = nodes;

                return cachedChildren;
            }
        }
     
        public string[] TypeList
        {
            get
            {

                if(typeList == null)
                    return new string[1] { null };
                var s = typeList.Split(',');
                return s;
            }
        }

        public void Init()
        {
         
            foreach (var schemaNode in children)
            {
                schemaNode.Init();
            }

            if (schema != null)
            {
                SchemaManager.Instance.GetSchema(schema).copyTo(this);
            }
            
            foreach (var inheritsId in inheritsIds)
            {
                inherits.Add(SchemaManager.Instance.GetSchema(inheritsId));
            }

            //inherits = inherits;
        }

        public SchemaNode FindChild(ScriptObject obj, string name, bool allowScriptList, bool rhs, ScopeType scope, int index)
        {
            string data = null;
            if (name.Contains(":"))
            {
                data = name.Split(':')[1];
                name = name.Split(':')[0];
            }

             var c = Children;

            if (allowScriptList && !IsNonScriptedListTag(name) && (name.StartsWith("any_") || name.StartsWith("every_") || (name.StartsWith("random_")) || name.StartsWith("ordered_")))
                return FindScriptList(obj, name, allowScriptList, rhs, scope, index);

           
            if (c.Any(a=>a.name == name && (!a.rightHandOnly || rhs)))
            {
                var list = c.Where(a => a.name == name && (!a.rightHandOnly || rhs));

                foreach (var schemaNode in list)
                {
                    var f = schemaNode;

                    if (f.requiresData)
                    {
                        if (data == null)
                            continue;

                        bool success;
                        ScopeType scopeType = GetScopeChangeFromData(obj, f, name, data, rhs, scope, out success);
                        if (success)
                        {
                            
                            SchemaNode clone = new SchemaNode();
                            f.copyTo(clone);

                            if (rhs)
                            {
                                clone.inheritsIds.Add("trigger");
                                clone.inherits.Add(SchemaManager.Instance.GetSchema("trigger"));
                            }
                            else
                            {
                                clone.inherits.UnionWith(inherits);
                                clone.inheritsIds.UnionWith(inheritsIds);
                                if (obj.Parent.lhsSchema != null)
                                {
                                    clone.children.AddRange(obj.Parent.lhsSchema.children);
                                }

                            }
//                            clone.inherits = clone.inherits.Distinct().ToList();
                            clone.targetScope = scopeType;
                            return clone;

                        }

                        continue;

                    }
                    else if (f.function == "event_target" && data == null)
                    {
                        SchemaNode clone = new SchemaNode();
                        f.copyTo(clone);
                        clone.inherits.UnionWith(inherits);
                     //   clone.inherits = clone.inherits.Distinct().ToList();
                        clone.inheritsIds.UnionWith(inheritsIds);
                        if (f.targetScope == ScopeType.none)
                            clone.targetScope = scope;
                        return clone;
                    }
                    else if (f.function == "script_list" && data == null)
                    {
                        if (f != null && rhs && f.TypeList.Contains("block"))
                            continue;

                        return f;
                    }

                    // don't allow blocks on rhs
                    if (f != null && rhs && f.TypeList.Contains("block"))
                        continue;

                    if (obj.Children.Count > 0 && f.Children.Count == 0)
                    {
                        if (!HandleUnexpectedChildren(schemaNode, obj, name, rhs, index))
                            continue;
                    }

                    if (!obj.IsBlock && f.Children.Count > 0 && !f.TypeList.Contains("value"))
                    {
                        continue;
                    }

                    return f;
                }

            }
            if(rhs && obj.lhsSchema != null && obj.lhsSchema.TypeList.Contains("value"))
            {
                var sv = Core.Instance.Get(ScriptContext.ScriptedValues, name, false);
                if(sv != null)
                {
                    return new SchemaNode() {typeList = "value", name = sv.Name, targetScope = ScopeType.value, function = "effect"};
                }
            }
          /*  if(rhs && !inheritsIds.Contains("trigger") && id!="trigger")
            {
                // we get to try a trigger...
                SchemaNode clone = new SchemaNode();
                copyTo(clone);
                clone.children.Clear();
                clone.inheritsIds.Add("trigger");
                clone.inherits.Add(SchemaManager.Instance.GetSchema("trigger"));
                return clone.FindChild(obj, name, allowScriptList, rhs, scope, index);
            }*/
            if (c.Any(a => a.namesFrom != null && !rhs))
            {
                var namesFrom = c.Where(a => a.namesFrom != null).ToList();
                foreach (var schemaNode in namesFrom)
                {
                    if (schemaNode.namesFrom == "value")
                    {
                        int num;

                        if (Int32.TryParse(name, out num))
                        {
                            return schemaNode;
                        }
                    }
                    else
                    {
                        var e = EnumManager.Instance.GetEnums(schemaNode.namesFrom, false);
                        if (e.Count > 0)
                        {
                            if (e.Contains(name))
                            {
                                obj.BehaviourData.lhsNamedFrom = schemaNode.namesFrom;
                                return schemaNode;
                            }
                        }

                    }
                }
            }

                return null;
        }

        private static List<string> nonScriptedListTags = new List<string>()
        {
            "random",
            "random_list",
            "random_in_list",
            "random_valid",
            "random_events",
            "random_on_action",
            "every_in_list",
        };
        private bool IsNonScriptedListTag(string s)
        {
            return nonScriptedListTags.Contains(s);
        }

        private bool HandleUnexpectedChildren(SchemaNode schemaNode, ScriptObject scriptObject, string s, bool rhs, int index)
        {
            if(schemaNode.TypeList.Contains("value"))
            {
                return true;
            }

            return false;
        }

        private ScopeType GetScopeChangeFromData(ScriptObject obj, SchemaNode node, string name, string data,
            bool rhs, ScopeType scope, out bool success)
        {
            success = false;
            if (name == "scope")
            {
                List<ScriptObject.ScriptScope> results = new List<ScriptObject.ScriptScope>();
                obj.Topmost.GetValidScriptScopesInit(results, true, ScriptObject.ScopeFindType.Any);
                foreach (var objScriptScope in results)
                {
                    if (objScriptScope.Name == data)
                    {
                        success = true;
                        return objScriptScope.To;
                    }
                }
            }
            if (name == "var")
            {
                if (VariableStore.Instance.unsortedScopedVariables.ContainsKey(data))
                {
                    var results = VariableStore.Instance.unsortedScopedVariables[data].Where(a => a.InsideScope == scope);

                    if (results.Any())
                    {
                        success = true;
                        return results.First().VarType;

                    }


                }
            }
            if (name == "cp")
            {
                if (EnumManager.Instance.GetEnums("council_position", true).Contains(data))
                {
                    success = true;
                    return ScopeType.character;

                }
            }
            if (name == "culture_group")
            {
                if (EnumManager.Instance.GetEnums("culture_group", true).Contains(data))
                {
                    success = true;
                    return ScopeType.culture_group;

                }
            }
            if (name == "culture")
            {
                if (EnumManager.Instance.GetEnums("culture", true).Contains(data))
                {
                    success = true;
                    return ScopeType.culture;

                }
            }
            if (name == "global_var")
            {
                if (VariableStore.Instance.globalVariables.ContainsKey(data))
                {
                    var results = VariableStore.Instance.globalVariables[data];

                    {
                        success = true;
                        return results.VarType;

                    }


                }
            }
            if (name == "local_var")
            {
                List<Variable> results = new List<Variable>();
                obj.Topmost.GetValidLocalVariablesInit(results);
                foreach (var objScriptScope in results)
                {
                    if (objScriptScope.Name == data)
                    {
                        success = true;
                        return objScriptScope.VarType;
                    }
                }
            }

            return obj.GetScopeType();
        }

        private SchemaNode FindScriptList(ScriptObject obj, string s, bool allowScriptList, bool rhs, ScopeType scope, int index)
        {
            SchemaNode clone = new SchemaNode();
            var schema = SchemaManager.Instance.GetSchema("scriptlist");

            var f = schema.FindChild(obj, s, false, rhs, scope, index);
            if (f == null)
                return null;

            f.copyTo(clone);
            clone.inherits.UnionWith(inherits);
            clone.inheritsIds.UnionWith(inheritsIds);
            return clone;
        }
    }

    public class SchemaManager
    {
        public static SchemaManager Instance = new SchemaManager();
        List<string> schemaFilenames = new List<string>();
        Dictionary<String, SchemaNode> SchemaMap = new Dictionary<String, SchemaNode>();
   
        public void Init()
        {
            var files = Directory.GetFiles(".\\Schemas");
            {
                 SchemaNode s = new SchemaNode();

                s.Load("Schemas/rhscope.xml");
            }

            foreach (var file in files)
            {
                string l = file.Substring(file.LastIndexOf("\\") + 1).Replace(".xml", "");


                SchemaNode s = new SchemaNode();
                if(!SchemaMap.ContainsKey(l))
                    s.Load("Schemas/" + l + ".xml");
            }

            foreach (var schemaMapValue in SchemaMap.Values)
            {

                schemaMapValue.Init();
                
            }

        }
        public SchemaManager()
        {
    
        }

      
        public SchemaNode GetSchema(string type)
        {
            
            if (SchemaMap.ContainsKey(type))
                return SchemaMap[type];
  

            return null;
        }

        public void AddSchema(string id, SchemaNode schemaNode)
        {
            SchemaMap[id] = schemaNode;
        }
    }
}