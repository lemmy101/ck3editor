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
        internal Dictionary<string, SchemaNode> childrenMap = new Dictionary<string, SchemaNode>();
        internal List<SchemaNode> children = new List<SchemaNode>();
        public List<ScopeType> scopes = new List<ScopeType>();

        List<string> inheritsIds = new List<string>();
        public List<SchemaNode> inherits = new List<SchemaNode>();
        private List<SchemaNode> cachedChildren;

        public string namesFrom { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        private string schema { get; set; }
        public string category { get; set; }
        public ScopeType targetScope { get; set; }
        public string doc { get; set; }
        public string function { get; set; }
        public string type { get; set; }
        
        public bool rightHandOnly { get; set; }
        public bool requiresData { get; set; }
        public bool globalLink { get; set; }
        public bool wildcard { get; set; }

       
        public void copyTo(SchemaNode dest)
        {
            dest.children = new List<SchemaNode>(children);
            dest.childrenMap = new Dictionary<string, SchemaNode>(childrenMap);
            dest.inheritsIds = new List<string>(inheritsIds);
            dest.inherits = new List<SchemaNode>(inherits);
            dest.scopes = new List<ScopeType>(scopes);
         
            dest.namesFrom = namesFrom;
            dest.name = name;
            dest.category = category;
            dest.targetScope = targetScope;
            dest.doc = doc;
            dest.function = function;
            dest.type = type;
            dest.requiresData = requiresData;
            dest.globalLink = globalLink;
            dest.wildcard = wildcard;
        }

        bool loadBool(XmlNode node, string attribute)
        {
            if (node.Attributes[attribute] != null)
                return node.Attributes[attribute].InnerText=="yes";

            return false;

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
            name = loadString(node as XmlNode, "name");
            type = loadString(node as XmlNode, "type");
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
            requiresData = loadBool(node as XmlNode, "requiresData");
            globalLink = loadBool(node as XmlNode, "globalLink");
            wildcard = loadBool(node as XmlNode, "wildcard");
            rightHandOnly = loadBool(node as XmlNode, "rightHandOnly");

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
                children.Add(cn);
                childrenMap[cn.name] = cn;
                c = c.NextSibling;
            }


            if (children.Count > 0 && !childrenMap.ContainsKey("scope"))
            {
                var cc = SchemaManager.Instance.GetSchema("rhscope").children;
                children.AddRange(cc);
                foreach (var schemaNode in cc)
                {
                    childrenMap[schemaNode.name] = schemaNode;

                }
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
                    return cachedChildren;

                List<SchemaNode> nodes = new List<SchemaNode>(children);

                foreach (var schemaNode in inherits)
                {
                    nodes.AddRange(schemaNode.Children);
                }

                cachedChildren = nodes;

                return cachedChildren;
            }
        }
        public Dictionary<string, SchemaNode> ChildrenMap
        {
            get
            {
                Dictionary<string, SchemaNode> nodes = childrenMap.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                foreach (var schemaNode in inherits)
                {
                    var map = schemaNode.ChildrenMap;
                    foreach (var entry in map)
                    {
                        nodes[entry.Key] = entry.Value;
                    }
                }

                return nodes;
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

        }

        public void DetermineChildSchema(ScriptObject so)
        {
            if (so.Name == null)
                return;

            var cm = ChildrenMap;

            if (cm.ContainsKey(so.Name))
            {
                so.lhsSchema = cm[so.Name];
            }
        }

        public SchemaNode FindChild(ScriptObject obj, string name, bool allowScriptList, bool rhs, int index)
        {
            string data = null;
            if (name.Contains(":"))
            {
                data = name.Split(':')[1];
                name = name.Split(':')[0];
            }

             var c = Children;

            if (allowScriptList && (name.StartsWith("any_") || name.StartsWith("every_") || name.StartsWith("random_") || name.StartsWith("ordered_")))
                return FindScriptList(obj, name, allowScriptList, rhs, index);

           
            if (c.Any(a=>a.name == name && (!a.rightHandOnly || rhs)))
            {
                var list = c.Where(a => a.name == name && (!a.rightHandOnly || rhs));

                foreach (var schemaNode in list)
                {
                    var f = schemaNode;

                    if (f.requiresData)
                    {
                        if (data == null || index != 0)
                            continue;

                        bool success;
                        ScopeType scopeType = GetScopeChangeFromData(obj, f, name, data, rhs, out success);
                        if (success)
                        {
                            SchemaNode clone = new SchemaNode();
                            f.copyTo(clone);
                            clone.inherits.AddRange(inherits);
                            clone.inheritsIds.AddRange(inheritsIds);
                            clone.targetScope = scopeType;
                            return clone;

                        }

                        continue;

                    }
                    else if (f.function == "event_target" && data == null)
                    {
                        SchemaNode clone = new SchemaNode();
                        f.copyTo(clone);
                        clone.inherits.AddRange(inherits);
                        clone.inheritsIds.AddRange(inheritsIds);
                        return clone;
                    }
                    else if (f.function == "script_list" && data == null)
                    {
                        if (f != null && rhs && f.type == "block")
                            continue;

                        return f;
                    }

                    // don't allow blocks on rhs
                    if (f != null && rhs && f.type == "block")
                        continue;

                    if (obj.Children.Count > 0 && f.Children.Count == 0)
                        continue;

                    return f;
                }
         
            }

            return null;
        }

        private ScopeType GetScopeChangeFromData(ScriptObject obj, SchemaNode node, string name, string data,
            bool rhs, out bool success)
        {
            success = false;
            if (name == "scope")
            {
                List<ScriptObject.ScriptScope> results = new List<ScriptObject.ScriptScope>();
                obj.Topmost.GetValidScriptScopes(results, true, ScriptObject.ScopeFindType.Any);
                foreach (var objScriptScope in results)
                {
                    if (objScriptScope.Name == data)
                    {
                        success = true;
                        return objScriptScope.To;
                    }
                }
            }

            return obj.GetScopeType();
        }

        private SchemaNode FindScriptList(ScriptObject obj, string s, bool allowScriptList, bool rhs, int index)
        {
            SchemaNode clone = new SchemaNode();
            var schema = SchemaManager.Instance.GetSchema("scriptlist");

            if (s.StartsWith("any_"))
                s = s.Substring(4);
            if (s.StartsWith("random_"))
                s = s.Substring(7);
            if (s.StartsWith("every_"))
                s = s.Substring(6);
            if (s.StartsWith("ordered_"))
                s = s.Substring(8);

            var f = schema.FindChild(obj, s, allowScriptList, rhs, index);
            if (f == null)
                return null;

            f.copyTo(clone);
            clone.inherits.AddRange(inherits);
            clone.inheritsIds.AddRange(inheritsIds);
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

            var cmap = SchemaMap["option"].ChildrenMap;
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