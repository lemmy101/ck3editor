using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace JominiParse
{
    public class SchemaChild
    {
        public BlockType blockType { get; set; }
        public ScopeType scopeType { get; set; }
        public string Name { get; set; }
        public string NamesFrom { get; set; }
        public string Type { get; set; }
        public bool IsBlock { get; set; }
    }

    public class SchemaEnum : SchemaChild
    {
        public List<string> choices = new List<string>();
    }

    public class ScriptObjectSchema
    {
        public Dictionary<string, SchemaChild> children = new Dictionary<string, SchemaChild>();
        public bool Soft { get; set; }
        public ScopeType scope = ScopeType.none;
        public string scopeChildId { get; set; }
        public BlockType blockType { get; set; }
        public string simpleType { get; set; }
       
        public bool AvoidRed { get; set; }

        public void Load(string filename)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(filename);

            var docEl = doc.DocumentElement;

            XmlNode el = docEl.FirstChild;

            if (docEl.Attributes["simpleType"] != null)
            {
                simpleType = docEl.Attributes["simpleType"].InnerText;
            }
            if (docEl.Attributes["AvoidRed"] != null)
            {
                simpleType = docEl.Attributes["AvoidRed"].InnerText;
            }
            
            while (el != null)
            {
                string name = el.Attributes["name"].InnerText;
                string type = el.Attributes["type"].InnerText;

                bool isBlock = (el.Attributes["isBlock"]==null) ? false : el.Attributes["isBlock"].InnerText=="yes";
                SchemaChild c = new SchemaChild();


                if (type == "enum")
                {
                    var e = new SchemaEnum();
                    var fc = el.FirstChild;
                    List<string> options = new List<string>();
                    while (fc != null)
                    {
                        e.choices.Add(fc.InnerText);
                        fc = fc.NextSibling;
                    }

                    c = e;
                }

                string namesFrom = null;

                if (el.Attributes["namesFrom"] != null)
                {
                    namesFrom = el.Attributes["namesFrom"].InnerText;
                }
                c.Name = name;
                c.NamesFrom = namesFrom;
                c.Type = type;
                c.IsBlock = isBlock;

                if (el.Attributes["blockType"] != null)
                {
                    BlockType sc = BlockType.none;
                    Enum.TryParse(el.Attributes["blockType"].InnerText, out sc);
                    c.blockType = sc;
                }
                if (el.Attributes["scopeType"] != null)
                {
                    ScopeType sc = ScopeType.none;
                    Enum.TryParse(el.Attributes["scopeType"].InnerText, out sc);
                    c.scopeType = sc;
                }
                if (el.Attributes["scopeChildId"] != null)
                {
                    scopeChildId = el.Attributes["scopeChildId"].InnerText;
                }

                children[name] = c;
                el = el.NextSibling;
            }

            if (doc.DocumentElement.Attributes["blockType"] != null)
            {
                BlockType sc = BlockType.none;
                Enum.TryParse(doc.DocumentElement.Attributes["blockType"].InnerText, out sc);
                blockType = sc;
            }

            if (doc.DocumentElement.Attributes["scopeType"]!=null)
            {
                Enum.TryParse(doc.DocumentElement.Attributes["scopeType"].InnerText, out scope);
            }

        }

        public string GetScopeChildIdentifier()
        {
            return scopeChildId;
        }

        public void AddChildrenToList(List<string> results)
        {
            var l = children.Values.OrderBy(a => a.Name).ToList();

            foreach (var schemaChild in l)
            {
             
                results.Add(schemaChild.Name);
            }
        }

        public List<string> GetChoices(string child)
        {
            if(!children.ContainsKey(child))
                return new List<string>();

            var c = children[child];

            if (c.Type == "enum")
            {
                return (c as SchemaEnum).choices;
            }
            if (c.Type == "bool")
            {
                return new List<string>() {"yes", "no"};
            }

            return new List<string>();
        }

        public string GetChildType(string child)
        {
            if (!children.ContainsKey(child))
                return null;

            return children[child].Type;

        }

        public bool GetChildIsBlock(string child, out bool succeeded)
        {
            succeeded = false;
            if (!children.ContainsKey(child))
                return false;

            succeeded = true;
            return children[child].IsBlock;
        }

        public SchemaChild GetChild(string child)
        {
            if (!children.ContainsKey(child))
                return null;

            return children[child];

        }

        public ScopeType GetScope()
        {
            return scope;
        }

        public bool SupportsConditions()
        {
            return children.Any(a => a.Key == "scopeconditions");
        }
        public bool SupportsEffects()
        {
            return children.Any(a => a.Key == "scopeeffects");
        }

        public bool HasChild(string name)
        {
            return children.ContainsKey(name);
        }
    }

    public class SchemaManager
    {
        public static SchemaManager Instance = new SchemaManager();
        List<string> schemaFilenames = new List<string>();
        Dictionary<String, ScriptObjectSchema> SchemaMap = new Dictionary<String, ScriptObjectSchema>();
        private ScriptObjectSchema DefaultCScope = null;
        private ScriptObjectSchema DefaultEScope = null;

        public SchemaManager()
        {
            var files = Directory.GetFiles(".\\Schemas");

            foreach (var file in files)
            {
                string l = file.Substring(file.LastIndexOf("\\") + 1).Replace(".xml", "");


                ScriptObjectSchema s = new ScriptObjectSchema();

                s.Load("Schemas/" + l + ".xml");
                s.Soft = true;

                SchemaMap[l] = s;
            }
        }

        public ScriptObjectSchema GetDefaultConditionScopeSchema()
        {
            if (DefaultCScope == null)
            {
                DefaultCScope = new ScriptObjectSchema();
     //           DefaultCScope.children["scopeconditions"] = (new SchemaChild() { blockType = BlockType.condition_block, Name = "scopeconditions", IsBlock = true });
            }

            return DefaultCScope;
        }
        public ScriptObjectSchema GetDefaultEffectScopeSchema()
        {
            if (DefaultEScope == null)
            {
                DefaultEScope = new ScriptObjectSchema();
            //    DefaultEScope.children["scopeeffects"] = (new SchemaChild() { blockType = BlockType.condition_block, Name = "scopeeffects", IsBlock = true });
            }

            return DefaultEScope;
        }


        public void CreateScopeFunction(ScopeType key, FunctionDef def, BlockType bt)
        {
            if (GetSchema(def.name) != null)
                return;
            ScriptObjectSchema schema = new ScriptObjectSchema();


            schema.Soft = true;
            schema.blockType = bt;
            schema.scope = key;

            foreach (var conditionProperty in def.Properties)
            {
                var a = new SchemaChild();
                string name = conditionProperty.name;
                string type = conditionProperty.type;

                a.Name = name;
                a.Type = type;
                schema.children[name] = a;
            }

            SchemaMap[def.name] = schema;
        }
    
        public void CreateScopeSchema(ScopeType fromScope, ScopeChangeDefinition scopeDef, BlockType blockType)
        {
       
            if (GetSchema(scopeDef.text) != null)
                return;

            ScriptObjectSchema schema = new ScriptObjectSchema();

            schema.Soft = true;
            schema.blockType = BlockType.inheritparent;
            schema.scope = scopeDef.toType;

            if (blockType == BlockType.condition_scope_change)
            {
            
            }

            if (blockType == BlockType.effect_scope_change)
            {
             
            }


            SchemaMap[scopeDef.text] = schema;
        }

        public ScriptObjectSchema GetSchema(string type)
        {
            
            if (SchemaMap.ContainsKey(type))
                return SchemaMap[type];
  

            return null;
        }
        ScriptObjectSchema defaultSchema = new ScriptObjectSchema();
        public ScriptObjectSchema GetDefaultSchema()
        {
            defaultSchema.AvoidRed = true;
            return defaultSchema;
        }
    }
}