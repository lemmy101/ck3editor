using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Xml.Linq;

namespace JominiParse
{
    public enum ScriptObjectBehaviourType
    {
        Unknown,
        RootObject,
        Effect,
        Trigger,
    }

    public enum ScriptObjectOperationType
    {

    }

    public class ScriptObjectBehaviourData
    {
        public string Name
        {
            get { return ScriptObject.Name; }
        }

        private ScopeType _scopeType;
        public string lhs;
        public string rhs;
        public string lhsNamedFrom;
        public bool foundRHSScope { get; set; }
        public ScopeType rhsScope { get; set; }

        public ScopeType ScopeType
        {
            get { return _scopeType; }
            set
            {
                _scopeType = value;
                ScriptObject.SetScopeType(value);
            }
        }

        public ScriptObject ScriptObject { get; set; }
        public ScriptObjectBehaviourData ParentData { get; set; }
        public bool IsBlock { get; set; }
      
        public ScriptObject ReferencedObject { get; set; }
        public bool ValueFound { get; set; }

        public string lhsError { get; set; }
        public string rhsError { get; set; }
        public bool ReferenceValid { get; set; }
        public int rhsScopeTextColorLength { get; set; }
        public int lhsScopeTextColorLength { get; set; }
        public bool ChildrenAreValueRange { get; set; }
        public bool NameIsReference { get; set; }
        public ScopeType deepestRHSScopeFound { get; set; }
        public List<SchemaNode> candidates { get; set; } = new List<SchemaNode>();

        /*public bool ValueIsScope { get; set; }
        public FunctionDef ExpectedEffectFunction { get; set; }
        public FunctionDef ExpectedTriggerFunction { get; set; }
        public bool CanBeScope { get; set; }*/
        /*
        public bool IsFunction { 
            get
            {
                return Type == ScriptObjectBehaviourType.FunctionSingleLine ||
                       Type == ScriptObjectBehaviourType.FunctionMultiline;
            }

        }*/
        public void Write(BinaryWriter writer)
        {
            
        }
    }

    public class ScriptObjectBehaviourManager
    {
        public static ScriptObjectBehaviourManager Instance = new ScriptObjectBehaviourManager();

        static HashSet<string> InherentScopeBlocks = new HashSet<string>();
        static HashSet<string> SavedScopeBlocks = new HashSet<string>();
        static HashSet<string> SavedScopeToProperty = new HashSet<string>();
        static HashSet<string> InherentScopeToProperty = new HashSet<string>();
        static HashSet<string> FunctionSingleLine = new HashSet<string>();
        static HashSet<string> FunctionMultiline = new HashSet<string>();
        static HashSet<string> UnknownProperties = new HashSet<string>();

        static HashSet<string> UnlinkedScope = new HashSet<string>();

        private List<string> Operands = new List<string>()
        {
            "OR",
            "AND",
            "NOT",
            "NOR",
            "NAND",
            "XOR"
        };

        public static int BreakpointLine = 24;
        public static string BreakpointFile = "events/test_event.txt";


        public void ProcessObject(BinaryWriter writer, BinaryReader reader, ScriptObject obj)
        {
            if (obj.Name == "root")
                obj.SetScopeType(obj.Topmost.GetScopeType());



            BreakpointLine = 24;
            //   BreakpointFile = "common/on_action/test_on_action.txt";
            //BreakpointFile = "events/test_event5.txt";
            //BreakpointFile = "common/scripted_effects/test.txt";
            //BreakpointFile = "events/test_events_8.txt";
            //BreakpointFile = "common/bookmarks/a.txt";
            BreakpointFile = "events/test_event/";


            if (obj.Filename.Contains(BreakpointFile))
            {

            }

            if (obj.LineStart == BreakpointLine &&
                obj.Filename.Contains(BreakpointFile))
            {

            }

            var data = new ScriptObjectBehaviourData();
            data.ScriptObject = obj;
            data.ScopeType = obj.GetScopeType();

            if (obj.Parent != null)
                data.ParentData = obj.Parent.BehaviourData;

            obj.BehaviourData = data;

            data.lhs = obj.Name;
            if(obj.Parent != null)
                data.ParentData = obj.Parent.BehaviourData;

            if (data.lhs == "set_variable")
            {
                VariableStore.Instance.CompleteScopedVar(obj, data, obj.Parent.GetScopeType());

            }

            data.rhs = obj.GetStringValue();
            data.IsBlock = obj.IsBlock;

            if (reader == null)
            {
                DetermineBehaviourScope(obj, data);
            }
            else
            {
                ReadBehaviour(reader, obj);
            }

            if (data.lhs.Contains("var:"))
            {
                if (!VariableStore.Instance.HasScopedVariableComplete(data.lhs, obj.GetScopeType()) && obj.DeferedCount < 1)
                {
                    obj.DeferedCount++;
                    ScriptObject.BehaviourRecalculateList.Add(obj);

                }
            }

            if (data.rhs != null && data.rhs.Contains("var:"))
            {
                if (!VariableStore.Instance.HasScopedVariableComplete(data.rhs.Substring(data.rhs.IndexOf("var:")),
                    data.deepestRHSScopeFound) && obj.DeferedCount < 1)
                {
                    obj.DeferedCount++;
                    ScriptObject.BehaviourRecalculateList.Add(obj);

                }
            }


            if (writer != null)
            {
                WriteBehaviour(writer, obj);
            }

            foreach (var scriptObject in obj.Children)
            {
                scriptObject.PostInitialize(writer, reader);
            }
          
        }

        private void WriteBehaviour(BinaryWriter writer, ScriptObject obj)
        {
            var data = obj.BehaviourData;

            writer.Write(data.ChildrenAreValueRange);
            writer.Write(data.IsBlock);
            writer.Write(data.NameIsReference);
            writer.Write(data.ReferenceValid);
            writer.Write(data.ValueFound);
            writer.Write(data.foundRHSScope);
            
            writer.Write(data.lhsNamedFrom!= null);

            if (data.lhsNamedFrom != null)
                writer.Write(data.lhsNamedFrom);

            writer.Write((int) data.ScopeType);
            writer.Write(data.candidates.Count);
            foreach (var dataCandidate in data.candidates)
            {
                writer.Write(dataCandidate.GetHashCodeL());
            }

            writer.Write((int)data.deepestRHSScopeFound);
            writer.Write(data.rhsScopeTextColorLength);
            writer.Write(data.lhsScopeTextColorLength);
            writer.Write(obj.lhsSchema != null); 
            if(obj.lhsSchema != null)
                writer.Write(obj.lhsSchema.GetHashCodeL());
            writer.Write(obj.rhsSchema != null);
            if (obj.rhsSchema != null)
                writer.Write(obj.rhsSchema.GetHashCodeL());

        }

        private void ReadBehaviour(BinaryReader reader, ScriptObject obj)
        {
            var data = obj.BehaviourData;

            data.ChildrenAreValueRange = reader.ReadBoolean();
            data.IsBlock = reader.ReadBoolean();
            data.NameIsReference = reader.ReadBoolean();
            data.ReferenceValid = reader.ReadBoolean();
            data.ValueFound = reader.ReadBoolean();
            data.foundRHSScope = reader.ReadBoolean();

            if (reader.ReadBoolean())
                data.lhsNamedFrom = reader.ReadString();

            data.ScopeType = (ScopeType) reader.ReadInt32();

            int nCand = reader.ReadInt32();

            for (int x = 0; x < nCand; x++)
            {
                var node = reader.ReadInt64();
                var n = SchemaManager.Instance.GetSchema(node);
                data.candidates.Add(n);
            }

            data.deepestRHSScopeFound = (ScopeType)reader.ReadInt32();
            data.rhsScopeTextColorLength = reader.ReadInt32();
            data.lhsScopeTextColorLength = reader.ReadInt32();

            if (reader.ReadBoolean())
            {
                var node = reader.ReadInt64();
                var n = SchemaManager.Instance.GetSchema(node);
                obj.lhsSchema = n;
            }
            if (reader.ReadBoolean())
            {
                var node = reader.ReadInt64();
                var n = SchemaManager.Instance.GetSchema(node);
                obj.rhsSchema = n;
            }
        }

        private void DetermineBehaviourScope(ScriptObject obj, ScriptObjectBehaviourData data)
        {
            if (obj.Parent != null)
            {
                var parentScope = obj.Parent.GetScopeType();

                if (data.lhs != null)
                {
                    int scopeColorLength = 0;
                    string[] scopeLine = data.lhs.Split('.');
                    ScopeType type = parentScope;
                    SchemaNode node = obj.Parent.lhsSchema;
                    if (node != null)
                    {
                        List<SchemaNode> candidates = new List<SchemaNode>();

                        if (obj.Parent.lhsSchema != null && !obj.Parent.lhsSchema.allowScopes)
                        {
                            scopeLine = new[] {data.lhs};
                        }

                        for (int i = 0; i < scopeLine.Length; i++)
                        {
                            string scopeChange = scopeLine[i];

                            node = node.FindChild(obj, scopeChange, scopeLine.Length == 1, false, type, i,
                                i == scopeLine.Length - 1 ? candidates : null);

                            if (node != null && (node.function == "event_target" || node.function == "script_list"))
                            {
                                scopeColorLength += scopeChange.Length;
                                if (i < scopeLine.Length - 1)
                                    scopeColorLength++;
                            }

                            // failed
                            if (node == null)
                                break;

                            type = node.targetScope;
                        }

                        data.candidates = candidates;
                    }



                    // no effect or trigger blocks after '.'s
                    if (node != null &&
                        (node.TypeList.Contains("block") || node.TypeList.Contains("function") ||
                         node.function == "effect") && scopeLine.Length > 1)
                        node = null;


                    obj.lhsSchema = node;
                    if (obj.lhsSchema != null)
                    {
                        if (obj.lhsSchema.TypeList.Contains("value"))
                        {
                            obj.lhsSchema.inheritsIds.Add("value");
                            obj.lhsSchema.inherits.Add(SchemaManager.Instance.GetSchema("value"));
                        }

                        data.lhsScopeTextColorLength = scopeColorLength;
                        if (obj.lhsSchema.function == "trigger")
                        {
                            ScopeType scope;
                            bool foundScope = false;
                            foreach (var s in node.TypeList)
                            {
                                if (Enum.TryParse(s, out scope))
                                {
                                    obj.SetScopeType(scope);
                                    foundScope = true;
                                }


                            }

                            if (!foundScope)
                            {
                                if (obj.lhsSchema.targetScope != ScopeType.any &&
                                    obj.lhsSchema.targetScope != ScopeType.none)
                                {
                                    obj.SetScopeType(obj.lhsSchema.targetScope);
                                }
                            }

                        }
                        else if (obj.lhsSchema.function == "effect")
                        {
                            ScopeType scope;
                            bool foundScope = false;
                            foreach (var s in node.TypeList)
                            {
                                if (Enum.TryParse(s, out scope))
                                {
                                    obj.SetScopeType(scope);
                                    foundScope = true;
                                }


                            }

                            if (!foundScope)
                            {
                                if (obj.lhsSchema.targetScope != ScopeType.any &&
                                    obj.lhsSchema.targetScope != ScopeType.none)
                                {
                                    obj.SetScopeType(obj.lhsSchema.targetScope);
                                }
                            }
                        }
                        else if (obj.lhsSchema.function == "event_target")
                        {
                            if (obj.lhsSchema.targetScope != ScopeType.any &&
                                obj.lhsSchema.targetScope != ScopeType.none)
                            {
                                obj.SetScopeType(obj.lhsSchema.targetScope);
                            }
                        }
                        else if (obj.lhsSchema.function == "script_list")
                        {
                            if (obj.lhsSchema.targetScope != ScopeType.any &&
                                obj.lhsSchema.targetScope != ScopeType.none)
                            {
                                obj.SetScopeType(obj.lhsSchema.targetScope);
                            }
                        }

                    }
                    else
                    {
                        if (data.ParentData.ChildrenAreValueRange)
                        {
                            data.ValueFound = true;
                        }
                        else data.lhsError = "Unexpected";
                    }
                }

                if (data.rhs != null && obj.lhsSchema != null)
                {
                    string[] scopeLine = data.rhs.Split('.');
                    ScopeType type = parentScope;
                    data.deepestRHSScopeFound = type;
                    SchemaNode node = obj.Parent.lhsSchema;
                    int scopeColorLength = 0;
                    data.foundRHSScope = true;
                    for (int i = 0; i < scopeLine.Length; i++)
                    {
                        string scopeChange = scopeLine[i];

                        var prev = node;
                        node = node.FindChild(obj, scopeChange, false, true, type, i, null);

                        if (node != null && (node.function == "event_target" || node.function == "script_list") &&
                            node.name != "yes" && node.name != "no")
                        {
                            scopeColorLength += scopeChange.Length;
                            if (i < scopeLine.Length - 1)
                                scopeColorLength++;
                        }


                        // failed
                        if (node == null)
                        {
                            data.foundRHSScope = false;
                            if (prev.targetScope != ScopeType.any && prev.targetScope != ScopeType.none)
                                data.deepestRHSScopeFound = prev.targetScope;
                            break;
                        }

                        type = node.targetScope;
                    }

                    obj.rhsSchema = node;
                    if (obj.rhsSchema != null)
                    {
                        data.rhsScopeTextColorLength = scopeColorLength;
                        if (obj.rhsSchema.function == "trigger")
                        {
                            ScopeType scope;
                            bool foundScope = false;
                            foreach (var s in node.TypeList)
                            {
                                if (Enum.TryParse(s, out scope))
                                {
                                    data.rhsScope = (scope);
                                    foundScope = true;
                                }


                            }

                            if (!foundScope)
                            {
                                if (obj.rhsSchema.targetScope != ScopeType.any &&
                                    obj.rhsSchema.targetScope != ScopeType.none)
                                {
                                    data.rhsScope = (obj.rhsSchema.targetScope);
                                }
                            }
                        }
                        else if (obj.lhsSchema.function == "effect")
                        {
                            ScopeType scope;
                            bool foundScope = false;
                            foreach (var s in node.TypeList)
                            {
                                if (Enum.TryParse(s, out scope))
                                {
                                    data.rhsScope = (scope);
                                    foundScope = true;
                                }


                            }

                            if (!foundScope)
                            {
                                if (obj.rhsSchema.targetScope != ScopeType.any &&
                                    obj.rhsSchema.targetScope != ScopeType.none)
                                {
                                    data.rhsScope = (obj.rhsSchema.targetScope);
                                }
                            }
                        }
                        else if (obj.rhsSchema.function == "event_target")
                        {
                            if (obj.rhsSchema.targetScope != ScopeType.any &&
                                obj.rhsSchema.targetScope != ScopeType.none)
                            {
                                data.rhsScope = (obj.rhsSchema.targetScope);
                            }
                        }
                        else if (obj.rhsSchema.function == "script_list")
                        {
                            if (obj.rhsSchema.targetScope != ScopeType.any &&
                                obj.rhsSchema.targetScope != ScopeType.none)
                            {
                                data.rhsScope = (obj.rhsSchema.targetScope);
                            }
                        }
                        else
                        {
                            if (!ValidateValue(obj, data))
                                data.rhsError = "Unexpected";
                        }
                    }
                    else
                    {
                        if (!ValidateValue(obj, data))
                            data.rhsError = "Unexpected";
                    }
                }
                else
                {
                    if (data.rhs == null && obj.Children.Count > 0 && obj.lhsSchema != null &&
                        obj.lhsSchema.TypeList.Contains("value"))
                    {
                        // look for alternative value types (scripted values / ranges)
                        if (obj.Children.Count == 2)
                        {
                            string a = obj.Children[0].Name;
                            string b = obj.Children[1].Name;

                            float range = 0;

                            if (Single.TryParse(a, out range) && Single.TryParse(b, out range))
                            {
                                data.ChildrenAreValueRange = true;
                            }
                        }
                    }
                    else if (data.rhs == null && data.lhsNamedFrom != null)
                    {
                        var nameSet =
                            EnumManager.Instance.GetEnums(data.lhsNamedFrom,
                                true); //Core.Instance.GetNameSetFromEnumType(type, true);

                        if (nameSet.Contains(data.lhs))
                        {
                            data.ReferencedObject = Core.Instance.Get(data.lhs, data.lhsNamedFrom);
                            data.ReferenceValid = true;
                            data.NameIsReference = true;
                        }

                    }
                }
            }

        }

        private bool ValidateValue(ScriptObject scriptObject, ScriptObjectBehaviourData data)
        {
            if (!CheckCompatibilityOfSides(scriptObject, data))
                return false;

            foreach (var s in scriptObject.lhsSchema.TypeList)
            {

                string type = scriptObject.rhsSchema == null ? InferTypeFromRHS(scriptObject, data, s) : s;

                string ltype = s;
                if (ltype == null)
                    ltype = scriptObject.lhsSchema.targetScope.ToString();
                if (type != ltype)
                    continue;

                // primitive or scope
                if (type == "value")
                {
                    float val;
                    if (Single.TryParse(data.rhs, out val))
                    {
                        return true;
                    }

                    var nameSet =
                        EnumManager.Instance.GetEnums("value",
                            true); //Core.Instance.GetNameSetFromEnumType(type, true);

                    var v = data.rhs;

                    if (v != null && v.Contains("."))
                        v = v.Substring(v.LastIndexOf(".") + 1);

                    if (nameSet.Contains(v))
                    {
                        data.ReferencedObject = Core.Instance.Get(v, type);
                        data.ReferenceValid = true;
                        return true;
                    }

                    var triggerSchema = SchemaManager.Instance.GetSchema("trigger");

                    var results = triggerSchema.Children.Where(a =>
                        a.name == v && (a.scopes.Contains(data.deepestRHSScopeFound) ||
                                        a.scopes.Contains(ScopeType.none)));

                    if (results.Count() > 0)
                    {
                        data.ReferenceValid = true;
                        return true;

                    }

                }

                if (type == "any" || type == "string")
                {
                    data.ReferenceValid = true;
                    return true;
                }

                if (type == "flag")
                {
                    if (data.rhs.StartsWith("flag:"))
                    {
                        data.ReferenceValid = true;
                        return true;

                    }
                }

                else if (type == "scope")
                {
                    List<ScriptObject.ScriptScope> results = new List<ScriptObject.ScriptScope>();
                    scriptObject.Topmost.GetValidScriptScopesInit(results, true, ScriptObject.ScopeFindType.Any);
                    foreach (var topmostScriptScope in results)
                    {
                        // its a scope!
                        if (topmostScriptScope.Name == data.rhs)
                        {
                            data.ReferenceValid = true;
                            return true;
                        }
                    }
                }
                else if (type == "var")
                {
                    data.ReferenceValid = true;
                    return true;
                }
                else if (type == "global_var")
                {
                    data.ReferenceValid = true;
                    return true;
                }
                else if (type == "local_var")
                {
                    data.ReferenceValid = true;
                    return true;
                }
                else if (type == "bool")
                {
                    if (data.rhs == "yes" || data.rhs == "no")
                    {
                        data.ReferenceValid = true;
                        return true;
                    }
                }
                else if (type == "localized")
                {
                    if (Core.Instance.HasLocalizedText(data.rhs.Replace("\"", "")))
                    {
                        data.ReferenceValid = true;
                        return true;
                    }

                }
                else if (type == "trigger_localization")
                {
                    if (Core.Instance.GetNameSet("trigger_localization", false).Contains(data.rhs.Replace("\"", "")))
                    {
                        data.ReferenceValid = true;
                        return true;
                    }
                }
                else if (type == scriptObject.GetScopeType().ToString())
                {
                    if (!data.foundRHSScope)
                        continue;

                    data.ReferenceValid = true;
                    return true;
                }

                {

                    var nameSet =
                        EnumManager.Instance.GetEnums(type, true); //Core.Instance.GetNameSetFromEnumType(type, true);

                    if (nameSet.Contains(data.rhs))
                    {
                        data.ReferencedObject = Core.Instance.Get(data.rhs, type);
                        data.ReferenceValid = true;
                        return true;
                    }
                }

            }

            return false;
        }

        private string InferTypeFromRHS(ScriptObject scriptObject, ScriptObjectBehaviourData data, string type)
        {
            string expected = type;
            if (expected == null)
                expected = scriptObject.lhsSchema.targetScope.ToString();

            if (expected == "value")
            {
                float val;
                if (Single.TryParse(data.rhs, out val))
                {
                    return expected;
                }

            }

            if (data.rhs == "yes" || data.rhs == "no")
            {
                return "bool";
            }

            return expected;
        }

        private bool CheckCompatibilityOfSides(ScriptObject obj, ScriptObjectBehaviourData data)
        {
            bool foundTypeMatch = false;

            if (obj.rhsSchema != null)
                foreach (var s in obj.lhsSchema.TypeList)
                {
                    if (obj.rhsSchema.TypeList.Contains(s))
                        foundTypeMatch = true;
                }

            if (obj.rhsSchema != null && foundTypeMatch)
                return true;

            if (obj.lhsSchema.TypeList.Contains("any"))
                return true;

            // can't determine type of rhs, so figure that out in validate value
            if (obj.rhsSchema == null)
                return true;

            return false;
        }
    }
}
