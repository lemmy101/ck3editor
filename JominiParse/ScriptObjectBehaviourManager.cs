using System;
using System.Collections.Generic;
using System.Globalization;
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
        RootObjectProperty,
        RootObjectPropertyBlock,
        InherentScopeBlock,
        InherentScopeToProperty,
        SavedScopeBlock,
        SavedScopeToProperty,
        FunctionSingleLine,
        FunctionMultiline,
        FunctionParameter,
        FunctionNamedFromParameterBlock,
        FunctionNamedFromParameterSingleLine,
        LogicalOperand,
        ScriptedTrigger,
        ScriptedEffect,
        Trigger,
        If,
        Else,
        TriggerIf,
        TriggerElse,
        Limit,
        GeneralBlock
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
        public ScriptObjectBehaviourType Type { get; set; }
        public bool IsBlock { get; set; }
        public bool ExpectTriggers { get; set; }
        public bool ParentExpectTriggers { get; set; }
        public bool ExpectFunctionParameters { get; set; }
        public bool ParentExpectEffects { get; set; }
        public bool ExpectEffects { get; set; }

        public bool IsScopedBlock
        {
            get
            {
                return Type == ScriptObjectBehaviourType.SavedScopeBlock ||
                       Type == ScriptObjectBehaviourType.InherentScopeBlock;
            }
        }
        public bool IsScope
        {
            get
            {
                return Type == ScriptObjectBehaviourType.SavedScopeBlock ||
                       Type == ScriptObjectBehaviourType.InherentScopeBlock ||
                       Type == ScriptObjectBehaviourType.SavedScopeToProperty ||
                       Type == ScriptObjectBehaviourType.InherentScopeToProperty;
            }
        }
        public bool IsInheritContents
        {
            get
            {
                return IsScopedBlock || Type == ScriptObjectBehaviourType.LogicalOperand || Type == ScriptObjectBehaviourType.If || Type == ScriptObjectBehaviourType.TriggerIf || Type == ScriptObjectBehaviourType.Else || Type == ScriptObjectBehaviourType.TriggerElse;
            }
        }
        public bool IsProperty
        {
            get
            {
                return Type == ScriptObjectBehaviourType.SavedScopeToProperty ||
                       Type == ScriptObjectBehaviourType.InherentScopeToProperty;
            }
        }

        public string TypeExpected { get; set; }
        public FunctionDef Function { get; set; }
        public FunctionProperty Parameter { get; set; }
        public string NameTypeExpected { get; set; }
        public ScriptObject ReferencedObject { get; set; }
        public bool ValueFound { get; set; }
        public bool ValueIsScope { get; set; }
        public FunctionDef ExpectedEffectFunction { get; set; }
        public FunctionDef ExpectedTriggerFunction { get; set; }
        public bool CanBeScope { get; set; }
        public bool IsFunction { 
            get
            {
                return Type == ScriptObjectBehaviourType.FunctionSingleLine ||
                       Type == ScriptObjectBehaviourType.FunctionMultiline;
            }

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

        ScriptObjectSchema ParentEffectiveSchema = new ScriptObjectSchema();
        ScriptObjectSchema EffectiveSchema = new ScriptObjectSchema();
        public static int BreakpointLine = 12;
        public static string BreakpointFile = "events/test_event.txt";


        public void ProcessObject(ScriptObject obj)
        {
            BreakpointLine = 45;
            //BreakpointFile = "events/health_events.txt";
            BreakpointFile = "events/test_event.txt";

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

            DetermineBehaviourType(obj, data);

            DetermineEffectiveSchema(obj, data);

            if (obj.Parent != null)
                DetermineBehaviourScope(obj, data);

            if (!data.IsBlock)
            {
                DetermineOperationType(obj, data);
            }
        }

        private void DetermineEffectiveSchema(ScriptObject obj, ScriptObjectBehaviourData data)
        {
            EffectiveSchema.children.Clear();
            if (obj.Schema != null)
            {

                foreach (var sc in obj.Schema.children)
                {
                    EffectiveSchema.children[sc.Key] = sc.Value;
                }
                var par = obj;
                if (par != null && data.IsInheritContents)
                {
                    par = par.Parent;
                    // hunt for first non scope/operand parent
                    while (par != null && par.Parent != null)
                    {
                        if (par.Schema != null && !par.BehaviourData.IsInheritContents)
                        {
                            foreach (var sc in par.Schema.children)
                            {
                                EffectiveSchema.children[sc.Key] = sc.Value;
                            }

                            break;

                        }
                        par = par.Parent;
                    }
                }

            }


            {
                var par = obj.Parent;
                ParentEffectiveSchema.children.Clear();
                if (obj.Parent == null || obj.Parent.Schema == null)
                {
                    return;
                }
                foreach (var sc in obj.Parent.Schema.children)
                {
                    ParentEffectiveSchema.children[sc.Key] = sc.Value;
                }

                // if we're inside a scope or operand we want the ability to add children based on parents...
                if (par != null && data.IsInheritContents)
                {
                    par = par.Parent;
                    // hunt for first non scope/operand parent
                    while (par != null && par.Parent != null)
                    {
                        if (par.Schema != null && !par.BehaviourData.IsInheritContents)
                        {
                            foreach (var sc in par.Schema.children)
                            {
                                ParentEffectiveSchema.children[sc.Key] = sc.Value;
                            }

                            break;

                        }
                        par = par.Parent;
                    }
                }
            }
        }

        private void DetermineBehaviourScope(ScriptObject obj, ScriptObjectBehaviourData data)
        {
            if (ParentEffectiveSchema.SupportsTriggers())
                data.ParentExpectTriggers = true;
            if (ParentEffectiveSchema.SupportsEffects())
                data.ParentExpectEffects = true;

            if (EffectiveSchema.SupportsTriggers())
                data.ExpectTriggers = true;
            if (EffectiveSchema.SupportsEffects())
                data.ExpectEffects = true;

            if ((data.Type == ScriptObjectBehaviourType.FunctionMultiline) && (data.ExpectTriggers || data.ExpectEffects))
            {
                data.Type = ScriptObjectBehaviourType.GeneralBlock;
            }

            if (data.Type == ScriptObjectBehaviourType.FunctionParameter)
            {
                data.ExpectFunctionParameters = true;
            }

            var scope = obj.Parent.GetScopeType();

            if (data.IsScope)
            {
                if (data.ExpectTriggers || data.ParentExpectTriggers)
                {
                    if (data.IsScopedBlock)
                    {
                        if (ScopeManager.Instance.isTriggerScope(scope, obj.Name))
                        {
                            bool success;
                            var newScope = ScopeManager.Instance.ChangeTriggerScope(scope, obj.Name, out success);
                            if (success)
                                data.ScopeType = newScope;
                        }
                        else if (ScopeManager.Instance.isTriggerScopeInside(scope, obj.Name, obj.Parent))
                        {
                            bool success;
                            var newScope = ScopeManager.Instance.ChangeTriggerScope(scope, obj.Name, out success, obj.Parent);
                            if (success)
                                data.ScopeType = newScope;

                        }
                    }
                }
                if (data.ExpectEffects || data.ParentExpectEffects)
                {
                    //if (data.IsScopelock)
                    {
                        if (ScopeManager.Instance.isEffectScope(scope, obj.Name))
                        {
                            bool success;
                            var newScope = ScopeManager.Instance.ChangeScope(scope, obj.Name, out success);
                            if (success)
                                data.ScopeType = newScope;
                        }
                        else if (ScopeManager.Instance.isEffectScopeInside(scope, obj.Name, obj.Parent))
                        {
                            bool success;
                            var newScope = ScopeManager.Instance.ChangeScope(scope, obj.Name, out success, obj.Parent);
                            if (success)
                                data.ScopeType = newScope;

                        }
                    }
                }

                if (data.IsScopedBlock)
                {
                    data.ExpectEffects = data.ParentData.ExpectEffects;
                    data.ExpectTriggers = data.ParentData.ExpectTriggers;
                }

            }
        }

        private void DetermineOperationType(ScriptObject obj, ScriptObjectBehaviourData data)
        {
            if (obj.Parent == null)
                return;

            bool acceptSchemaPropertyType = false;
            var scope = obj.Parent.GetScopeType();
            string typeExpected = null;
            if (data.ParentData.ExpectEffects && data.Type == ScriptObjectBehaviourType.FunctionSingleLine)
            {
                var effect = ScopeManager.Instance.GetEffect(scope, obj.Name);

                // successfully gotten an effect
                if (effect != null)
                {
                    if (effect.Properties.Count > 0)
                        data.CanBeScope = true;
                    typeExpected = effect.type;
                }
            }
            else if (data.ParentData.ExpectTriggers && (data.Type == ScriptObjectBehaviourType.FunctionSingleLine))
            {
                var Trigger = ScopeManager.Instance.GetTrigger(scope, obj.Name);

                // successfully gotten an effect
                if (Trigger != null)
                {
                    if (Trigger.Properties.Count > 0)
                        data.CanBeScope = true;
                    typeExpected = Trigger.type;
                }
            }

            else if (data.ParentData.ExpectTriggers && (data.Type == ScriptObjectBehaviourType.InherentScopeToProperty || data.Type == ScriptObjectBehaviourType.SavedScopeToProperty))
            {
                var Trigger = ScopeManager.Instance.GetTrigger(scope, obj.Name);

                // successfully gotten an effect
                if (Trigger != null)
                {
                    typeExpected = Trigger.type;
                    if (Trigger.Properties.Count > 0)
                        data.CanBeScope = true;
                }
                else
                {
                    if (ScopeManager.Instance.isTriggerScope(scope, obj.Name))
                    {
                        bool success;
                        var to = ScopeManager.Instance.ChangeTriggerScope(scope, obj.Name, out success, obj.Parent);
                        if (success)
                        {
                            typeExpected = to.ToString();
                        }
                    }
                    else if (ScopeManager.Instance.isTriggerScopeToParam(scope, obj.Name))
                    {
                        bool success;
                        var to = ScopeManager.Instance.ChangeTriggerScope(scope, obj.Name, out success, obj.Parent);
                        if (success)
                        {
                            typeExpected = to.ToString();
                        }
                    }
                    else if (ScopeManager.Instance.isTriggerScopeInside(scope, obj.Name, obj.Parent))
                    {
                        bool success;
                        var to = ScopeManager.Instance.ChangeTriggerScope(scope, obj.Name, out success, obj.Parent);
                        if (success)
                        {
                            typeExpected = to.ToString();
                        }
                    }
                }
            }

            // could be a parameter to a function...
            else if (data.ExpectFunctionParameters || data.Type == ScriptObjectBehaviourType.RootObjectProperty)
            {
                var parent = obj.Parent;


                if (parent.Parent != null)
                {
                    if (parent.Parent.BehaviourData.ParentExpectTriggers)
                    {
                        var Trigger = ScopeManager.Instance.GetTrigger(parent.Parent.GetScopeType(), parent.Name);

                        if (Trigger != null)
                        {
                            if (Trigger.Properties.Count > 0)
                                data.CanBeScope = true;

                            if (Trigger.Properties.Any(a => a.name == obj.Name))
                            {
                                var c = Trigger.Properties.First(a => a.name == obj.Name);
                                typeExpected = c.type;
                                data.Function = Trigger;
                                data.Parameter = c;
                            }
                            else
                            {
                                acceptSchemaPropertyType = true;
                            }

                        }
                    }
                    else if (parent.Parent.BehaviourData.ParentExpectEffects)
                    {
                        var effect = ScopeManager.Instance.GetEffect(parent.Parent.GetScopeType(), parent.Name);

                        if (effect != null)
                        {
                            if (effect.Properties.Count > 0)
                                data.CanBeScope = true;
                            if (effect.Properties.Any(a => a.name == obj.Name))
                            {
                                var c = effect.Properties.First(a => a.name == obj.Name);
                                typeExpected = c.type;
                                data.Function = effect;
                                data.Parameter = c;
                            }
                        }
                        else 
                        {
                            acceptSchemaPropertyType = true;

                        }
                    }
                    else 
                    {
                        acceptSchemaPropertyType = true;
                    }

                }
                else 
                {
                    acceptSchemaPropertyType = true;

                }

                if (obj.Schema != null)
                {
                    if (obj.Schema.children.Count > 0)
                    {
                        data.CanBeScope = true;
                    }
                }

            }

            if (acceptSchemaPropertyType && data.ParentData.ScriptObject.Schema != null && data.ParentData.ScriptObject.Schema.children.ContainsKey(obj.Name))
            {

                var param = data.ParentData.ScriptObject.Schema.children[obj.Name];

                typeExpected = param.Type;

            }

            if (typeExpected != null)
            {
                data.TypeExpected = typeExpected;
            }

            if (data.TypeExpected != null)
            {

                if (!data.IsBlock)
                {
                    string value = obj.GetStringValue();
                    if (value != null)
                    {

                        if (ScopeManager.Instance.isTriggerScope(scope, value) ||
                            ScopeManager.Instance.isEffectScope(scope, value))
                        {
                            bool success;
                            ScopeType to =
                                ScopeManager.Instance.ChangeTriggerScope(scope, value, out success, obj.Parent);

                            if (success)
                            {
                                if (to.ToString() == data.TypeExpected || data.TypeExpected == "scope")
                                {
                                    data.ValueFound = true;
                                }
                            }
                            else
                            {
                                to = ScopeManager.Instance.ChangeScope(scope, value, out success, obj.Parent);

                                if (success)
                                {
                                    if (to.ToString() == data.TypeExpected || data.TypeExpected == "scope")
                                    {
                                        data.ValueFound = true;
                                    }
                                }
                            }
                        }
                        if (!data.ValueFound && (ScopeManager.Instance.isTriggerScopeInside(scope, value, obj.Parent, ScriptObject.ScopeFindType.Value) ||
                                                 ScopeManager.Instance.isEffectScopeInside(scope, value, obj.Parent, ScriptObject.ScopeFindType.Value)))
                        {
                         
                            var savedScope = ScopeManager.Instance.GetSavedScope(scope, value, obj.Parent, ScriptObject.ScopeFindType.Value);

                       
                            if(savedScope != null)
                            {
                                if (savedScope.VarType.ToString().ToLower() == data.TypeExpected)
                                {
                                    data.ValueFound = true;
                                    data.ValueIsScope = true;
                                }
                            }
                        }
                        if (!data.ValueFound && (ScopeManager.Instance.isTriggerScopeInside(scope, value, obj.Parent, ScriptObject.ScopeFindType.Object) ||
                                                 ScopeManager.Instance.isEffectScopeInside(scope, value, obj.Parent, ScriptObject.ScopeFindType.Object)))
                        {
                            bool success;
                            ScopeType to =
                                ScopeManager.Instance.ChangeTriggerScope(scope, value, out success, obj.Parent);

                            if (success)
                            {
                                if (to.ToString() == data.TypeExpected || data.TypeExpected == "scope")
                                {
                                    data.ValueFound = true;
                                    data.ValueIsScope = true;
                                }
                            }
                            else
                            {
                                to = ScopeManager.Instance.ChangeScope(scope, value, out success, obj.Parent);

                                if (success)
                                {
                                    if (to.ToString() == data.TypeExpected || data.TypeExpected == "scope")
                                    {
                                        data.ValueFound = true;
                                        data.ValueIsScope = true;
                                    }
                                }
                            }
                        }
                        if (!data.ValueFound && ScopeManager.Instance.isTriggerScopeEndParamInside(scope, value, obj.Parent))

                        {
                            bool success;
                            ScopeType to =
                                ScopeManager.Instance.ChangeTriggerScope(scope, value.Substring(0, value.LastIndexOf(".")), out success, obj.Parent);

                            if (success)
                            {
                                var con = ScopeManager.Instance.GetTrigger(to,
                                    value.Substring(value.LastIndexOf(".") + 1));

                                if (con.type == data.TypeExpected)
                                {
                                    data.ValueFound = true;
                                }
                            }
                        }
                     
                        if (!data.ValueFound && Core.Instance.AnyNameLists(data.TypeExpected))
                        {

                            data.ReferencedObject = Core.Instance.Get(value, data.TypeExpected);
                            if (data.ReferencedObject != null)
                            {
                                data.ValueFound = true;
                            }
                        }

                        if (!data.ValueFound && data.TypeExpected == "localized")
                        {
                            var str = Core.Instance.GetLocalizedText(value);
                            if (str != value)
                                data.ValueFound = true;
                        }

                        if (!data.ValueFound)
                        {
                            var enums = EnumManager.Instance.GetEnums(data.TypeExpected);
                            if (enums != null && enums.Contains(value))
                            {
                                data.ValueFound = true;
                            }
                        }

                    }

                }
            }
        }

        private void DetermineBehaviourType(ScriptObject obj, ScriptObjectBehaviourData data)
        {
            string name = obj.Name;

            bool isBlock = false;
            if (obj.Parent == null)
            {
                data.Type = ScriptObjectBehaviourType.RootObject;
                return;
            }

            if (obj.IsBlock)
            {
                isBlock = true;
            }

            data.IsBlock = isBlock;

            var scope = obj.Parent.GetScopeType();


            if (name.StartsWith("root") || name.StartsWith("this") || name.StartsWith("prev"))
            {
                name = "scope:" + name;
            }

            if (isBlock)
            {
                if (ScopeManager.Instance.isTriggerScope(scope, name) || ScopeManager.Instance.isEffectScope(scope, name))
                {
                    data.Type = ScriptObjectBehaviourType.InherentScopeBlock;

                    //        InherentScopeBlocks.Add(name);
                }
                else if (ScopeManager.Instance.isTriggerScopeInside(scope, name, obj.Parent, ScriptObject.ScopeFindType.Object) || ScopeManager.Instance.isEffectScopeInside(scope, name, obj.Parent, ScriptObject.ScopeFindType.Object))
                {
                    data.Type = ScriptObjectBehaviourType.SavedScopeBlock;

                    //     SavedScopeBlocks.Add(name);

                }
                else if (ScopeManager.Instance.isTrigger(scope, name))
                {
                    data.Type = ScriptObjectBehaviourType.FunctionMultiline;
                    //     FunctionMultiline.Add(name);
                }
                else if (ScopeManager.Instance.isEffect(scope, name))
                {
                    data.Type = ScriptObjectBehaviourType.FunctionMultiline;
                    //      FunctionMultiline.Add(name);
                }
                else if (Operands.Contains(name))
                {
                    data.Type = ScriptObjectBehaviourType.LogicalOperand;
                }
                else if (name == "if" || name == "else_if")
                {
                    data.Type = ScriptObjectBehaviourType.If;
                }
                else if (name == "trigger_if" || name == "trigger_else_if")
                {
                    data.Type = ScriptObjectBehaviourType.TriggerIf;
                }
                else if (name == "else")
                {
                    data.Type = ScriptObjectBehaviourType.Else;
                }
                else if (name == "trigger_else")
                {
                    data.Type = ScriptObjectBehaviourType.TriggerElse;
                }
                else if (name == "limit")
                {
                    data.Type = ScriptObjectBehaviourType.Limit;
                }
                else if (name == "trigger")
                {
                    data.Type = ScriptObjectBehaviourType.Trigger;
                }
                else if (obj.Parent.Parent == null)
                {
                    data.Type = ScriptObjectBehaviourType.RootObjectPropertyBlock;

                    if (obj.Parent.Schema != null && !obj.Parent.Schema.HasChild(obj.Name))
                    {
                        if (!HandleNameFor(obj, data, name, isBlock))
                        {
                            data.Type = ScriptObjectBehaviourType.Unknown;
                        }
                    }

                    //       FunctionSingleLine.Add(name);
                }
                else if (obj.Parent != null && obj.Parent.Schema != null)
                {
                    if (obj.Parent.Schema.children.ContainsKey(name))
                    {
                        data.Type = ScriptObjectBehaviourType.FunctionMultiline;
                    }
                    else
                    {
                        var schema = obj.Parent.Schema;

                        var namedAs = schema.children.Where(a => a.Value.NamesFrom != null).ToList();

                        foreach (var keyValuePair in namedAs)
                        {
                            SchemaChild namesFrom = keyValuePair.Value;

                            HandleNamesFrom(obj, data, isBlock, namesFrom);
                        }
                    }
                }
            }
            else
            {
                if (ScopeManager.Instance.isTriggerScopeToParam(scope, name) || ScopeManager.Instance.isEffectScopeToParam(scope, name))
                {
                    data.Type = ScriptObjectBehaviourType.InherentScopeToProperty;

                    //      InherentScopeToProperty.Add(name);
                }
                else if (ScopeManager.Instance.isTriggerScopeEndParamInside(scope, name,
                    obj.Parent) || ScopeManager.Instance.isEffectScopeInside(scope, name,
                             obj.Parent))
                {
                    data.Type = ScriptObjectBehaviourType.SavedScopeToProperty;
                    //         SavedScopeToProperty.Add(name);

                }
                else if ((ScopeManager.Instance.isTrigger(scope, name) || ScopeManager.Instance.isEffect(scope, name)) && !data.ParentData.IsFunction)
                {
                    data.Type = ScriptObjectBehaviourType.FunctionSingleLine;
                    //     FunctionSingleLine.Add(name);
                    data.ExpectedTriggerFunction = ScopeManager.Instance.GetTrigger(scope, name);
                    data.ExpectedEffectFunction = ScopeManager.Instance.GetEffect(scope, name);
                }
                else
                {
                    if (obj.Parent.Parent == null)
                    {
                        data.Type = ScriptObjectBehaviourType.RootObjectProperty;
                        //        FunctionSingleLine.Add(name);
                    }
                    else if (obj.Parent.Schema != null && obj.Parent.Schema.children.ContainsKey(name))
                    {
                        data.Type = ScriptObjectBehaviourType.FunctionParameter;
                    }
                    else if (obj.Parent.BehaviourData.IsFunction && (ScopeManager.Instance.isTrigger(obj.Parent.GetScopeType(), obj.Parent.Name) || ScopeManager.Instance.isEffect(obj.Parent.GetScopeType(), obj.Parent.Name)))
                    {
                        data.Type = ScriptObjectBehaviourType.FunctionParameter;

                        if (!HandleNameFor(obj, data, name, isBlock))
                        {

                        }

                    }
                    else if (obj.Parent != null)
                    {
                        data.Type = ScriptObjectBehaviourType.FunctionParameter;
                        if (!HandleNameFor(obj, data, name, isBlock))
                        {
                            data.Type = ScriptObjectBehaviourType.Unknown;
                        }
                    }
                }

            }

            if (data.Type == ScriptObjectBehaviourType.Unknown)
            {
                string str = name;

                while (obj != null)
                {
                    obj = obj.Parent;
                    if (obj != null)
                        str = name + "->" + str;
                }

                UnknownProperties.Add(str);
            }

        }

        private bool HandleNameFor(ScriptObject obj, ScriptObjectBehaviourData data, string name, bool isBlock)
        {
            if (!name.StartsWith("scope:"))
            {
                if (obj.Parent.Schema != null)
                {
                    var schema = obj.Parent.Schema;

                    var namedAs = schema.children.Where(a => a.Value.NamesFrom != null).ToList();

                    foreach (var keyValuePair in namedAs)
                    {
                        HandleNamesFrom(obj, data, isBlock, keyValuePair.Value);
                        if (data.Type != ScriptObjectBehaviourType.Unknown)
                            return true;
                    }
                }
            }
            else
            {
            }

            return false;
        }

        private void HandleNamesFrom(ScriptObject obj, ScriptObjectBehaviourData data, bool isBlock, SchemaChild schemaChild)
        {
            string namesFrom = schemaChild.NamesFrom;
            if (namesFrom == "num")
            {
                int result;
                if (Int32.TryParse(obj.Name, out result))
                {
                    data.TypeExpected = schemaChild.Type;
                    data.NameTypeExpected = namesFrom;
                    data.Type = ScriptObjectBehaviourType.FunctionNamedFromParameterBlock;
                    if (!isBlock)
                        data.Type = ScriptObjectBehaviourType.FunctionNamedFromParameterSingleLine;
                }
            }
            else
            {
                var types = EnumManager.Instance.GetEnums(namesFrom);

                if (types.Contains(obj.Name))
                {
                    data.TypeExpected = schemaChild.Type;
                    data.NameTypeExpected = namesFrom;
                    data.Type = ScriptObjectBehaviourType.FunctionNamedFromParameterBlock;
                    if (!isBlock)
                        data.Type = ScriptObjectBehaviourType.FunctionNamedFromParameterSingleLine;

                }
            }
        }

        public void PrintDebug()
        {
            foreach (var s in UnknownProperties)
            {
                //        Console.WriteLine(s);
            }
        }
    }
}
