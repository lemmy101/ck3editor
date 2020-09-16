using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace JominiParse
{

    public class SchemaNode
    {
        public Dictionary<string, SchemaNode> childrenMap = new Dictionary<string, SchemaNode>();
        public List<SchemaNode> children = new List<SchemaNode>();
        public List<ScopeType> scopes = new List<ScopeType>();

        public string namesFrom { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public ScopeType targetScope { get; set; }
        public string doc { get; set; }
        public string function { get; set; }
        public string type { get; set; }

        public bool requiresData { get; set; }
        public bool globalLink { get; set; }
        public bool wildcard { get; set; }

        public SchemaNode GetChild(string name)
        {
            return null;
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
            namesFrom = loadString(node as XmlNode, "namesFrom");
            category = loadString(node as XmlNode, "category");

            string scopes = loadString(node as XmlNode, "scope");

            if(scopes != null)
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
                this.scopes.Add(ScopeType.none);
            }
          

            string targetScope = loadString(node as XmlNode, "scope");
            ScopeType tsv;
            Enum.TryParse(targetScope, out tsv);

            this.targetScope = tsv;

            function = loadString(node as XmlNode, "function");
            requiresData = loadBool(node as XmlNode, "requiresData");
            globalLink = loadBool(node as XmlNode, "globalLink");
            wildcard = loadBool(node as XmlNode, "wildcard");

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

            if (id != null)
            {
                SchemaManager.Instance.AddSchema(id, this);
            }
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

            foreach (var file in files)
            {
                string l = file.Substring(file.LastIndexOf("\\") + 1).Replace(".xml", "");


                SchemaNode s = new SchemaNode();

                s.Load("Schemas/" + l + ".xml");
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