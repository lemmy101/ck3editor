#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

#endregion

namespace JominiParse
{
    public class SchemaNode
    {
        public static Dictionary<long, SchemaNode> CloneHashMap = new Dictionary<long, SchemaNode>();
        public static Dictionary<long, SchemaNode> NodeHashMap = new Dictionary<long, SchemaNode>();


        private static readonly List<string> nonScriptedListTags = new List<string>
        {
            "random",
            "random_list",
            "random_in_list",
            "random_valid",
            "random_events",
            "random_on_action",
            "every_in_list",
            "random_skills"
        };

        private HashSet<SchemaNode> cachedChildren;
        internal HashSet<SchemaNode> children = new HashSet<SchemaNode>();

        private long hashCode;
        public HashSet<SchemaNode> inherits = new HashSet<SchemaNode>();
        public HashSet<SchemaNode> inheritsCached = new HashSet<SchemaNode>();

        internal HashSet<string> inheritsIds = new HashSet<string>();
        public List<ScopeType> scopes = new List<ScopeType>();
        public string namesFrom { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        private string schema { get; set; }
        public string category { get; set; }
        public ScopeType targetScope { get; set; }
        public string doc { get; set; }
        public string function { get; set; }
        private string typeList { get; set; }

        public bool rightHandOnly { get; set; }
        public bool avoidRed { get; set; }
        public bool requiresData { get; set; }
        public bool globalLink { get; set; }
        public bool wildcard { get; set; }
        public bool allowScopes { get; set; } = true;

        public HashSet<SchemaNode> Children
        {
            get
            {
                if (cachedChildren != null)
                    if (inheritsCached != null)
                    {
                        var match = inherits.SequenceEqual(inheritsCached);

                        if (match)
                            return cachedChildren;
                    }

                var nodes = new HashSet<SchemaNode>(children);

                foreach (var schemaNode in inherits) nodes.UnionWith(schemaNode.Children);

                cachedChildren = nodes;
                inheritsCached = inherits;
                return cachedChildren;
            }
        }

        public string[] TypeList
        {
            get
            {
                if (typeList == null)
                    return new string[1] {null};
                var s = typeList.Split(',');
                return s;
            }
        }


        public void copyTo(SchemaNode dest)
        {
            dest.children = new HashSet<SchemaNode>(children);
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

        public override string ToString()
        {
            return name + " - " + string.Join(",", TypeList) + " - " + children.Count;
        }

        public void Read(BinaryReader reader)
        {
            hashCode = reader.ReadInt64();
            var numInherits = reader.ReadInt32();
            for (var x = 0; x < numInherits; x++)
            {
                var s = reader.ReadString();
                inheritsIds.Add(s);
                inherits.Add(SchemaManager.Instance.GetSchema(s));
            }

            name = reader.ReadString();
            if (reader.ReadBoolean())
                category = reader.ReadString();
            if (reader.ReadBoolean())
                namesFrom = reader.ReadString();
            targetScope = (ScopeType) reader.ReadInt32();
            if (reader.ReadBoolean())
                function = reader.ReadString();
            if (reader.ReadBoolean())
                typeList = reader.ReadString();

            requiresData = reader.ReadBoolean();
            globalLink = reader.ReadBoolean();
            wildcard = reader.ReadBoolean();
            allowScopes = reader.ReadBoolean();
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(inheritsIds.Count);

            foreach (var inheritsId in inheritsIds)
                writer.Write(inheritsId);

            writer.Write(name);
            writer.Write(category != null);
            if (category != null) writer.Write(category);
            writer.Write(namesFrom != null);
            if (namesFrom != null) writer.Write(namesFrom);
            writer.Write((int) targetScope);

            writer.Write(function != null);
            if (function != null) writer.Write(function);

            writer.Write(typeList != null);
            if (typeList != null) writer.Write(typeList);

            writer.Write(requiresData);
            writer.Write(globalLink);
            writer.Write(wildcard);
            writer.Write(allowScopes);
        }

        private bool loadBool(XmlNode node, string attribute, bool def)
        {
            if (node.Attributes[attribute] != null)
                return node.Attributes[attribute].InnerText == "yes";

            return def;
        }

        private string loadString(XmlNode node, string attribute)
        {
            if (node.Attributes[attribute] != null)
                return node.Attributes[attribute].InnerText;

            return null;
        }

        public void Load(string file)
        {
            var d = new XmlDocument();

            d.Load(file);

            var node = d.DocumentElement as XmlNode;

            LoadNode(node);
        }

        private void Serialize(bool isClone = true)
        {
            var hashCode = GetHashCodeL();
            this.hashCode = hashCode;

            //if (name == "root")
            {
                if (isClone)
                    CloneHashMap[hashCode] = this;
                else
                    NodeHashMap[hashCode] = this;
            }
        }

        public long GetHashCodeL()
        {
            if (hashCode != 0)
                return hashCode;

            long id = name.GetHashCode();
            if (namesFrom != null)
                id += namesFrom.GetHashCode() << 4;

            if (category != null)
                id += category.GetHashCode() << 4;

            id += (int) targetScope << 4;

            foreach (var scopeType in scopes)
                id += (int) scopeType << 5;

            foreach (var s in TypeList)
                id += s.GetHashCode() << 8;

            foreach (var inherit in inherits) id += inherit.GetHashCode() << 1;

            foreach (var child in children) id += child.GetHashCodeL() << 16;

            return id;
        }

        public void LoadNode(XmlNode node, SchemaNode parent = null)
        {
            id = loadString(node, "id");

            name = loadString(node, "name");
            typeList = loadString(node, "type");
            namesFrom = loadString(node, "namesFrom");
            category = loadString(node, "category");
            schema = loadString(node, "schema");

            var scopes = loadString(node, "scope");
            var inherits = loadString(node, "inherits");

            if (node.Name == "Localized")
            {
                typeList = "localized";
            }
            else if (node.Name == "Effect")
            {
                inherits = "effect";
                typeList = inherits;
            }
            else if (node.Name == "Trigger")
            {
                inherits = "trigger";
                typeList = inherits;
            }
            else if (node.Name == "ModifierStack")
            {
                inherits = "modifierStack";
                typeList = inherits;
            }
            else if (node.Name == "Bool")
            {
                typeList = "bool";
            }
            else if (node.Name == "Value")
            {
                typeList = "value";
            }
            else if (node.Name != "Child")
            {
                typeList = node.Name.ToLower();
            }

            if (name == null)
            {
                if (typeList == "effect_group")
                {
                }

                name = typeList;
            }

            if (typeList == null)
                typeList = "none";

            if (inherits != null)
            {
                var i = inherits.Split(',');

                foreach (var s in i) inheritsIds.Add(s.Trim());
            }

            if (scopes != null)
            {
                var scps = scopes.Split(',');

                foreach (var scp in scps)
                {
                    ScopeType scopeEnum;
                    if (Enum.TryParse(scp, out scopeEnum)) this.scopes.Add(scopeEnum);
                }
            }
            else
            {
                this.scopes.Add(ScopeType.inheritparent);
            }


            var targetScope = loadString(node, "targetScope");
            ScopeType tsv;
            Enum.TryParse(targetScope, out tsv);

            this.targetScope = tsv;

            function = loadString(node, "function");

            requiresData = loadBool(node, "requiresData", false);
            globalLink = loadBool(node, "globalLink", false);
            wildcard = loadBool(node, "wildcard", false);
            allowScopes = loadBool(node, "allowScopes", true);
            rightHandOnly = loadBool(node, "rightHandOnly", false);
            avoidRed = loadBool(node, "avoidRed", false);

            if (typeList.Contains("value")) inheritsIds.Add("value");
            var c = node.FirstChild;

            while (c != null)
            {
                if (c is XmlComment)
                {
                    c = c.NextSibling;
                    continue;
                }

                var cn = new SchemaNode();
                cn.LoadNode(c, this);


                if (cn.function == "script_list")
                {
                    var any = new SchemaNode();
                    cn.copyTo(any);
                    any.name = "any_" + cn.name;
                    children.Add(any);

                    any.inheritsIds.Add("scriptlistTriggerInherits");
                    any.inheritsIds.Add("trigger");

                    var random = new SchemaNode();
                    cn.copyTo(random);
                    random.name = "random_" + cn.name;
                    children.Add(random);

                    random.inheritsIds.Add("scriptlistEffectInherits");
                    random.inheritsIds.Add("effect");

                    var every = new SchemaNode();
                    cn.copyTo(every);
                    every.name = "every_" + cn.name;
                    children.Add(every);

                    every.inheritsIds.Add("scriptlistEffectInherits");
                    every.inheritsIds.Add("effect");

                    var ordered = new SchemaNode();
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


            if (children.Count > 0 && !children.Any(a => a.name == "scope"))
            {
                var cc = SchemaManager.Instance.GetSchema("rhscope").children;
                children.UnionWith(cc);
            }

            if (id != null) SchemaManager.Instance.AddSchema(id, this);

            if (node.Name == "Localized" && parent != null)
            {
                var clone = new SchemaNode();
                copyTo(clone);
                clone.inheritsIds.Add("localized");
                parent.children.Add(clone);
                clone.Serialize();
            }

            Serialize(false);
        }

        public void Init()
        {
            foreach (var schemaNode in children) schemaNode.Init();

            if (schema != null) SchemaManager.Instance.GetSchema(schema).copyTo(this);

            foreach (var inheritsId in inheritsIds) inherits.Add(SchemaManager.Instance.GetSchema(inheritsId));

            //inherits = inherits;
        }

        public SchemaNode FindChild(ScriptObject obj, string name, bool allowScriptList, bool rhs, ScopeType scope,
            int index, List<SchemaNode> candidates)
        {
            string data = null;
            if (name.Contains(":"))
            {
                data = name.Split(':')[1];
                name = name.Split(':')[0];
            }

            var c = Children;

            if (allowScriptList && !IsNonScriptedListTag(name) &&
                (name.StartsWith("any_") || name.StartsWith("every_") || name.StartsWith("random_") ||
                 name.StartsWith("ordered_")))
            {
                var v = FindScriptList(obj, name, allowScriptList, rhs, scope, index, candidates);

                if (v != null)
                    return v;
            }

            SchemaNode found = null;

            var list = c.Where(a => a.name == name && (!a.rightHandOnly || rhs)).ToList();

            if (list.Count > 0)
                foreach (var schemaNode in list)
                {
                    var f = schemaNode;

                    if (f.requiresData)
                    {
                        if (data == null)
                            continue;

                        bool success;
                        var scopeType = GetScopeChangeFromData(obj, f, name, data, rhs, scope, out success);
                        if (success)
                        {
                            var clone = new SchemaNode();
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
                                    clone.inherits.UnionWith(obj.Parent.lhsSchema.inherits);
                                    clone.inheritsIds.UnionWith(obj.Parent.lhsSchema.inheritsIds);
                                }
                            }

//                            clone.inherits = clone.inherits.Distinct().ToList();
                            clone.targetScope = scopeType;
                            found = clone; //return clone;

                            clone.Serialize();

                            if (candidates != null)
                                candidates.Add(clone);
                        }

                        continue;
                    }

                    if (f.function == "event_target" && data == null)
                    {
                        var clone = new SchemaNode();
                        f.copyTo(clone);
                        clone.inherits.UnionWith(inherits);
                        //   clone.inherits = clone.inherits.Distinct().ToList();
                        clone.inheritsIds.UnionWith(inheritsIds);
                        if (f.targetScope == ScopeType.none)
                            clone.targetScope = scope;

                        if (found == null)
                            found = clone;
                        //    if (candidates != null)
                        //        candidates.Add(clone);
                        clone.Serialize();
                    }
                    else if (f.function == "script_list" && data == null)
                    {
                        if (f != null && rhs && f.TypeList.Contains("block"))
                            continue;

                        if (found == null)
                            found = f;
                        //     if (candidates != null)
                        //       candidates.Add(f);
                    }

                    // don't allow blocks on rhs
                    if (f != null && rhs && f.TypeList.Contains("block"))
                        continue;

                    if (obj.Children.Count > 0 && f.Children.Count == 0)
                        if (!HandleUnexpectedChildren(schemaNode, obj, name, rhs, index))
                            continue;

                    if (!obj.IsBlock && f.Children.Count > 0 && !f.TypeList.Contains("value"))
                    {
                        if (candidates != null)
                            candidates.Add(f);
                        continue;
                    }

                    if (found == null)
                        found = f;
                    if (candidates != null)
                        candidates.Add(f);
                }

            if (found != null)
                return found;

            if (rhs && obj.lhsSchema != null && obj.lhsSchema.TypeList.Contains("value"))
            {
                var sv = Core.Instance.Get(ScriptContext.ScriptedValues, name);
                if (sv != null)
                    return new SchemaNode
                        {typeList = "value", name = sv.Name, targetScope = ScopeType.value, function = "effect"};
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

            var namesFrom = c.Where(a => a.namesFrom != null).ToList();

            if (namesFrom.Count > 0)
                foreach (var schemaNode in namesFrom)
                    if (schemaNode.namesFrom == "value")
                    {
                        int num;

                        if (int.TryParse(name, out num)) return schemaNode;
                    }
                    else
                    {
                        var e = EnumManager.Instance.GetEnums(schemaNode.namesFrom);
                        if (e.Count > 0)
                            if (e.Contains(name))
                            {
                                obj.BehaviourData.lhsNamedFrom = schemaNode.namesFrom;
                                return schemaNode;
                            }
                    }

            return found;
        }

        private bool IsNonScriptedListTag(string s)
        {
            return nonScriptedListTags.Contains(s);
        }

        private bool HandleUnexpectedChildren(SchemaNode schemaNode, ScriptObject scriptObject, string s, bool rhs,
            int index)
        {
            if (schemaNode.TypeList.Contains("value")) return true;

            return false;
        }

        private ScopeType GetScopeChangeFromData(ScriptObject obj, SchemaNode node, string name, string data,
            bool rhs, ScopeType scope, out bool success)
        {
            success = false;
            if (name == "scope")
            {
                var results = new List<ScriptObject.ScriptScope>();
                obj.Topmost.GetValidScriptScopesInit(results, true, ScriptObject.ScopeFindType.Any);
                foreach (var objScriptScope in results)
                    if (objScriptScope.Name == data)
                    {
                        success = true;
                        return objScriptScope.To;
                    }
            }

            if (name == "var")
                if (VariableStore.Instance.unsortedScopedVariables.ContainsKey(data))
                {
                    var results = VariableStore.Instance.unsortedScopedVariables[data]
                        .Where(a => a.InsideScope == scope);

                    if (results.Any())
                    {
                        success = true;
                        return results.First().VarType;
                    }
                }

            if (name == "cp")
                if (EnumManager.Instance.GetEnums("council_position", true).Contains(data))
                {
                    success = true;
                    return ScopeType.character;
                }

            if (name == "culture_group")
                if (EnumManager.Instance.GetEnums("culture_group", true).Contains(data))
                {
                    success = true;
                    return ScopeType.culture_group;
                }

            if (name == "culture")
                if (EnumManager.Instance.GetEnums("culture", true).Contains(data))
                {
                    success = true;
                    return ScopeType.culture;
                }

            if (name == "global_var")
                if (VariableStore.Instance.globalVariables.ContainsKey(data))
                {
                    var results = VariableStore.Instance.globalVariables[data];

                    {
                        success = true;
                        return results.VarType;
                    }
                }

            if (name == "local_var")
            {
                var results = new List<Variable>();
                obj.Topmost.GetValidLocalVariablesInit(results);
                foreach (var objScriptScope in results)
                    if (objScriptScope.Name == data)
                    {
                        success = true;
                        return objScriptScope.VarType;
                    }
            }

            return obj.GetScopeType();
        }

        private SchemaNode FindScriptList(ScriptObject obj, string s, bool allowScriptList, bool rhs, ScopeType scope,
            int index, List<SchemaNode> candidates)
        {
            var clone = new SchemaNode();
            var schema = SchemaManager.Instance.GetSchema("scriptlist");

            var f = schema.FindChild(obj, s, false, rhs, scope, index, candidates);
            if (f == null)
                return null;

            f.copyTo(clone);
            clone.inherits.UnionWith(inherits);
            clone.inheritsIds.UnionWith(inheritsIds);
            return clone;
        }

        public bool IsEffect()
        {
            if (inheritsIds.Contains("effect"))
                return true;

            foreach (var schemaNode in inherits)
                if (schemaNode.IsEffect())
                    return true;

            return false;
        }

        public bool IsTrigger()
        {
            if (inheritsIds.Contains("trigger"))
                return true;

            foreach (var schemaNode in inherits)
                if (schemaNode.IsTrigger())
                    return true;

            return false;
        }
    }

    public class SchemaManager
    {
        public static SchemaManager Instance = new SchemaManager();
        private List<string> schemaFilenames = new List<string>();
        private readonly Dictionary<string, SchemaNode> SchemaMap = new Dictionary<string, SchemaNode>();

        public void Init()
        {
            var files = Directory.GetFiles(".\\Schemas");
            {
                var s = new SchemaNode();

                s.Load("Schemas/rhscope.xml");
            }

            foreach (var file in files)
            {
                var l = file.Substring(file.LastIndexOf("\\") + 1).Replace(".xml", "");


                var s = new SchemaNode();
                if (!SchemaMap.ContainsKey(l))
                    s.Load("Schemas/" + l + ".xml");
            }

            foreach (var schemaMapValue in SchemaMap.Values) schemaMapValue.Init();

            LoadBinary();
        }

        public void LoadBinary()
        {
            var binFilename = "schemaNodes.bin";
            binFilename = Globals.CK3EdDataPath.Replace("\\", "/") + binFilename;

            if (!File.Exists(binFilename))
                return;

            using (var reader = new BinaryReader(File.Open(binFilename, FileMode.Open)))
            {
                ReadSchemas(reader);
            }
        }

        private void ReadSchemas(BinaryReader reader)
        {
            var num = reader.ReadInt32();

            for (var x = 0; x < num; x++)
            {
                var node = new SchemaNode();

                node.Read(reader);
                SchemaNode.CloneHashMap[node.GetHashCodeL()] = node;
            }
        }

        public void SaveBinary()
        {
            var binFilename = "schemaNodes.bin";
            binFilename = Globals.CK3EdDataPath.Replace("\\", "/") + binFilename;

            using (var writer = new BinaryWriter(File.Open(binFilename, FileMode.Create)))
            {
                WriteSchemas(writer);
            }
        }

        public void WriteSchemas(BinaryWriter writer)
        {
            writer.Write(SchemaNode.CloneHashMap.Count);

            foreach (var schemaNode in SchemaNode.CloneHashMap)
            {
                writer.Write(schemaNode.Key);
                schemaNode.Value.Write(writer);
            }
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

        public SchemaNode GetSchema(long node)
        {
            if (SchemaNode.NodeHashMap.ContainsKey(node))
                return SchemaNode.NodeHashMap[node];

            if (SchemaNode.CloneHashMap.ContainsKey(node))
                return SchemaNode.CloneHashMap[node];

            return null;
        }
    }
}