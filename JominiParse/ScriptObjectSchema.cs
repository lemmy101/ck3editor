using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace JominiParse
{
    public class SchemaChild
    {
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
        Dictionary<string, SchemaChild> children = new Dictionary<string, SchemaChild>();
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
                children[name] = c;
                el = el.NextSibling;
            }

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
    }

    public class SchemaManager
    {
        public static SchemaManager Instance = new SchemaManager();
        List<string> schemaFilenames = new List<string>();
        Dictionary<Type, ScriptObjectSchema> SchemaMap = new Dictionary<Type, ScriptObjectSchema>();
        Dictionary<String, ScriptObjectSchema> SoftSchemaMap = new Dictionary<String, ScriptObjectSchema>();
        public ScriptObjectSchema GetSchema(Type type)
        {
            if (SchemaMap.ContainsKey(type))
                return SchemaMap[type];

            ScriptObjectSchema s = new ScriptObjectSchema();
            if (!File.Exists("Schemas/" + type.Name + ".xml"))
                return null;

            s.Load("Schemas/"+type.Name+".xml");

            SchemaMap[type] = s;

            return s;
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

            SoftSchemaMap[type] = s;

            return s;
        }
    }
}