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


        public void Load(string filename)
        {
            XmlDocument doc = new XmlDocument();

            doc.Load(filename);

            XmlNode el = doc.DocumentElement.FirstChild;

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
                c.Name = name;
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

    }

    public class SchemaManager
    {
        public static SchemaManager Instance = new SchemaManager();
        List<string> schemaFilenames = new List<string>();
        Dictionary<Type, ScriptObjectSchema> SchemaMap = new Dictionary<Type, ScriptObjectSchema>();
        Dictionary<String, ScriptObjectSchema> SoftSchemaMap = new Dictionary<String, ScriptObjectSchema>();
        public ScriptObjectSchema GetSchema(Type type)
        {
            if (schemaFilenames.Count == 0)
            {
                string[] dir = Directory.GetFiles("Schemas/");
                foreach (var s1 in dir)
                {
                    schemaFilenames.Add(s1);
                }

            }

            if (SchemaMap.ContainsKey(type))
                return SchemaMap[type];

            if (!schemaFilenames.Contains("Schemas/" + type.Name + ".xml"))
                return null;

            ScriptObjectSchema s = new ScriptObjectSchema();
            if (!File.Exists("Schemas/" + type.Name + ".xml"))
                return null;

            s.Load("Schemas/" + type.Name + ".xml");
            s.Soft = false;
            SchemaMap[type] = s;

            return s;
        }

        public void CreateScopeFunction(ScopeType key, ConditionDef def, BlockType bt)
        {
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

            SoftSchemaMap[def.name] = schema;
        }
        public void CreateScopeFunction(ScopeType key, EffectDef def, BlockType bt)
        {
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

            SoftSchemaMap[def.name] = schema;
        }
        public void CreateScopeSchema(ScopeType fromScope, ScopeChangeDefinition scopeDef, BlockType blockType)
        {
            ScriptObjectSchema schema = new ScriptObjectSchema();

            schema.Soft = true;
            schema.blockType = BlockType.inheritparent;
            schema.scope = scopeDef.toType;

            if (blockType == BlockType.condition_scope_change)
            {
                var a = new SchemaChild();
                string name = "scopeconditions";
                string type = "scopeconditions";

                a.Name = name;
                a.Type = type;
                schema.children[name] = a;
            }

            if (blockType == BlockType.effect_scope_change)
            {
                var a = new SchemaChild();
                string name = "scopeeffects";
                string type = "scopeeffects";

                a.Name = name;
                a.Type = type;
                schema.children[name] = a;
            }


            SoftSchemaMap[scopeDef.text] = schema;
        }

        public ScriptObjectSchema GetSchema(string type)
        {
            if (schemaFilenames.Count == 0)
            {
                string[] dir = Directory.GetFiles("Schemas/");
                foreach (var s1 in dir)
                {
                    schemaFilenames.Add(s1);
                }
                
            }
            if (SoftSchemaMap.ContainsKey(type))
                return SoftSchemaMap[type];

            if (!schemaFilenames.Contains("Schemas/" + type + ".xml"))
                return null;
          
            ScriptObjectSchema s = new ScriptObjectSchema();

            s.Load("Schemas/" + type + ".xml");
            s.Soft = true;

            SoftSchemaMap[type] = s;

            return s;
        }

    }
}