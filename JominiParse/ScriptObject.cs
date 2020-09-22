#region

using System.Collections.Generic;
using System.IO;
using System.Linq;

#endregion

namespace JominiParse
{
    public class ScriptObject
    {
        public enum ScopeFindType
        {
            Object,
            Value,
            Any
        }

        public enum ScopeVarType
        {
            @bool,
            value,
            none,
            @string,
            flag
        }

        private static readonly List<ScriptObject> visited = new List<ScriptObject>();

        public List<ScriptObject> Children = new List<ScriptObject>();
        private bool isScope;
        private bool isTriggerEnd;
        public SchemaNode lhsSchema;
        public Dictionary<string, Variable> localVariables = new Dictionary<string, Variable>();
        public Dictionary<string, ScriptList> scriptLists = new Dictionary<string, ScriptList>();

        public Dictionary<string, ScriptScope> scriptScopes = new Dictionary<string, ScriptScope>();

        public ScriptObject(ScriptObject parent, ScriptParsedSegment seg, SchemaNode schema = null)
        {
            Op = seg.op;
            IsBlock = seg.isBlock;
            if (parent == null)
            {
                Core.Instance.DeferedPostInitializationListNext.Add(this);
                Core.Instance.DeferedInitializationList.Add(this);
            }

            if (seg == null) return;

            Name = seg.name;
            Filename = seg.filename;
            LineStart = seg.lineNumbers.First();
            LineEnd = seg.lineNumbers.Last();
            Parent = parent;
            Library = Core.Instance.LoadingCK3Library;

            if (LineStart == ScriptObjectBehaviourManager.BreakpointLine &&
                Topmost.Filename.ToRelativeFilename().Contains(ScriptObjectBehaviourManager.BreakpointFile))
            {
            }

            lhsSchema = schema;

            foreach (var scriptParsedSegment in seg.children)
            {
                ScriptObject so = null;
                if (scriptParsedSegment.value.Count > 0)
                    so = ScriptValueParser.Instance.ParseScriptValue(this, scriptParsedSegment);
                else
                    so = ScriptObjectFactory.Instance.CreateScriptObject(Context, scriptParsedSegment, this,
                        Namespace);

                Children.Add(so);

                OnPostInitializeChild(so);
            }
        }


        private static HashSet<string> CachedScriptedEffects
        {
            get
            {
                if (Core.Instance._cachedScriptedEffects == null)
                    Core.Instance._cachedScriptedEffects =
                        Core.Instance.GetNameSet(ScriptContext.ScriptedEffects, false);

                return Core.Instance._cachedScriptedEffects;
            }
        }

        private static HashSet<string> CachedScriptedTriggers
        {
            get
            {
                if (Core.Instance._cachedScriptedTriggers == null)
                    Core.Instance._cachedScriptedTriggers =
                        Core.Instance.GetNameSet(ScriptContext.ScriptedTriggers, false);

                return Core.Instance._cachedScriptedTriggers;
            }
        }

        public bool IsScriptedTriggerCall { get; set; }

        public bool IsScriptedEffectCall { get; set; }

        public ScriptObjectBehaviourData BehaviourData { get; set; }

        public ScriptLibrary Library { get; set; }

        public ScriptObject Topmost
        {
            get
            {
                if (Parent == null)
                    return this;

                return Parent.Topmost;
            }
        }

        private ScopeType ScopeType { get; set; } = ScopeType.inheritparent;
        public ScriptObject Parent { get; set; }

        public int LineEnd { get; set; }

        public int LineStart { get; set; }

        public RefFilename Filename { get; set; }
        public virtual string Name { get; set; }
        public ScriptContext Context { get; set; }
        public bool IsBlock { get; set; }

        public string Op { get; set; } = "=";
        public SchemaNode rhsSchema { get; set; }
        public int DeferedCount { get; set; }
        public string Namespace { get; set; }
        public ScriptFile ScriptFile { get; set; }
        public bool Overridden { get; set; }

        protected ScriptObject FindChild(string name)
        {
            var f = Children.Where(a => a.Name == name);

            if (f.Any())
                return f.First();

            return null;
        }

        protected List<ScriptObject> FindChildren(string name)
        {
            var f = Children.Where(a => a.Name == name).ToList();

            return f;
        }

        public virtual void OnPostInitializeChild(ScriptObject child)
        {
        }

        public static void ClearCachedScriptedEffects()
        {
            Core.Instance._cachedScriptedEffects = null;
            Core.Instance._cachedScriptedTriggers = null;
        }

        public virtual void Initialize()
        {
            if (Parent != null && CachedScriptedTriggers.Contains(Name))
            {
                Core.Instance.LoadingCK3Library.RegisterScriptTriggerCall(this);
                IsScriptedTriggerCall = true;
            }

            if (Parent != null && CachedScriptedEffects.Contains(Name))
            {
                Core.Instance.LoadingCK3Library.RegisterScriptEffectCall(this);
                IsScriptedEffectCall = true;
            }

            if (Name == "events") Core.Instance.LoadingCK3Library.RegisterFirstValidEventsTrigger(this);

            if (Name == "trigger_event") Core.Instance.LoadingCK3Library.RegisterTrigger(this);

            if (Name == "first_valid_on_action") Core.Instance.LoadingCK3Library.RegisteFirstValidOnActionTrigger(this);

            if (Name == "random_on_action") Core.Instance.LoadingCK3Library.RegisterRandomOnActionTrigger(this);

            if (Name == "on_actions") Core.Instance.LoadingCK3Library.RegisteFirstValidOnActionTrigger(this);

            if (Name == "random_events") Core.Instance.LoadingCK3Library.RegisterRandomEventsTrigger(this);

            if (Name == "first_valid") Core.Instance.LoadingCK3Library.RegisterFirstValidEventsTrigger(this);

            HandleScopeDeclarationFunctions(this, Parent);

            foreach (var scriptObject in Children) scriptObject.Initialize();
        }

        public virtual void PostInitialize(BinaryWriter writer, BinaryReader reader)
        {
            ScriptObjectBehaviourManager.Instance.ProcessObject(writer, reader, this);
        }

        private void HandleScopeDeclarationFunctions(ScriptObject scriptObject, ScriptObject parent)
        {
            if (Name == "set_local_variable") VariableStore.Instance.RegisterSetLocalVariable(this);

            if (Name == "set_variable") VariableStore.Instance.RegisterSetScopedVariable(this);

            if (Name == "set_global_variable") VariableStore.Instance.RegisterSetGlobalVariable(this);

            if (Name == "add_to_temporary_list") scriptObject.Topmost.AddList(this, true);

            if (Name == "add_to_list") scriptObject.Topmost.AddList(this, false);

            if (Name == "save_scope_as") scriptObject.Topmost.AddScope(this, false);

            if (Name == "save_scope_value_as")
                scriptObject.Topmost.AddScopeVar(this, false);
            else if (Name == "save_temporary_scope_as") scriptObject.Topmost.AddScope(this, true);
        }

        public void GetValidScriptScopesInit(List<ScriptScope> results, bool allowTemp = true,
            ScopeFindType values = ScopeFindType.Object)
        {
            visited.Clear();
            GetValidScriptScopes(results, allowTemp, values);
        }

        private void GetValidScriptScopes(List<ScriptScope> results, bool allowTemp = true,
            ScopeFindType values = ScopeFindType.Object)
        {
            if (Topmost != this || allowTemp) visited.Clear();

            if (Topmost != this)
            {
                Topmost.GetValidScriptScopes(results, allowTemp, values);
                return;
            }

            if (visited.Contains(this))
                return;

            visited.Add(this);

            if (allowTemp)
            {
                foreach (var scriptScopesValue in scriptScopes.Values)
                    if (!results.Any(a => a.Name == scriptScopesValue.Name && a.Show(values)))
                        results.Add(scriptScopesValue);

            }
            else
            {
                foreach (var scriptScopesValue in scriptScopes.Values)
                    if (!scriptScopesValue.Temporary &&
                        !results.Any(a => a.Name == scriptScopesValue.Name && a.Show(values)))
                        results.Add(scriptScopesValue);
            }

            var Connections = ReferenceManager.Instance.GetConnectionsTo(Topmost.Name);
            foreach (var eventConnection in Connections)
                eventConnection.From.GetValidScriptScopes(results, false);
        }

        public void GetValidLocalVariablesInit(List<Variable> results)
        {
            visited.Clear();
            GetValidLocalVariables(results);
        }

        private void GetValidLocalVariables(List<Variable> results)
        {
            if (Topmost != this) visited.Clear();

            if (Topmost != this)
            {
                Topmost.GetValidLocalVariables(results);
                return;
            }

            if (visited.Contains(this))
                return;

            visited.Add(this);

            {
                foreach (var scriptScopesValue in localVariables.Values)
                    if (!results.Any(a => a.Name == scriptScopesValue.Name))
                        results.Add(scriptScopesValue);
            }

            var Connections = ReferenceManager.Instance.GetConnectionsTo(Topmost.Name);
            foreach (var eventConnection in Connections)
                //                if (this == eventConnection.To && eventConnection.From != this)
                eventConnection.From.GetValidLocalVariables(results);
        }

        private void AddScope(ScriptObject scope_command, bool temporary)
        {
            if (!(this == Topmost))
            {
                AddScope(scope_command, temporary);
                return;
            }

            var s = new ScriptScope();
            var sc = scope_command as ScriptValue;
            if (sc != null)
            {
                s.Temporary = temporary;
                s.Name = sc.GetStringValue();
                if (scriptScopes.ContainsKey(s.Name))
                    return;

                s.RequiresScopeTag = true;
                s.ToObj = scope_command.Parent;
                s.Declared = scope_command;
                if (scope_command == null)
                {
                }

                scriptScopes[s.Name] = s;
            }
        }

        private void AddList(ScriptObject addToListCommand, bool temp)
        {
            if (!(this == Topmost))
            {
                AddList(addToListCommand, temp);
                return;
            }

            var s = new ScriptList();
            var sc = addToListCommand as ScriptValue;
            if (sc != null)
            {
                s.Name = sc.GetStringValue();
                s.Temporary = temp;
                if (scriptLists.ContainsKey(s.Name))
                    return;

                s.Declared = addToListCommand;
                scriptLists[s.Name] = s;
            }
        }

        private void AddScopeVar(ScriptObject scope_command, bool temporary, ScopeType varType)
        {
            if (!(this == Topmost))
            {
                AddScopeVar(scope_command, temporary);
                return;
            }

            var s = new ScriptScope();
            s.IsValue = true;
            s.To = varType;
            {
                s.Temporary = temporary;
                s.Name = scope_command.GetChildStringValue("name"); //.GetStringValue();
                if (s.Name == null)
                    return;
                if (scriptScopes.ContainsKey(s.Name))
                    return;

                s.Declared = scope_command;
                if (scope_command == null)
                {
                }

                scriptScopes[s.Name] = s;
            }
        }

        public string GetChildStringValue(string name)
        {
            if (!Children.Any(a => a.Name == name))
                return null;

            return Children.Where(a => a.Name == name).ToList()[0].GetStringValue();
        }

        private void AddScopeVar(ScriptObject scope_command, bool temporary)
        {
            var w = scope_command.Children.Where(a => a.Name == "value");
            var type = ScopeType.@bool;

            if (w.Count() > 0)
            {
                var f = w.First();

                var s = f.GetStringValue();

                if (!(s == "yes" || s == "no")) type = ScopeType.value;

                if (s != null && s.StartsWith("flag:"))
                    type = ScopeType.flag;
            }

            AddScopeVar(scope_command, temporary, type);
        }

        public virtual ScopeType GetScopeType()
        {
            if (ScopeType == ScopeType.inheritparent)
            {
                if (Parent == null)
                    return ScopeType.character;

                return Parent.GetScopeType();
            }

            return ScopeType;
        }

        public virtual void SetScopeType(ScopeType type)
        {
            if (type == ScopeType.any)
            {
            }

            ScopeType = type;
        }

        public virtual void Write(BinaryWriter writer)
        {
            writer.Write(GetType().FullName.GetHashCode());
            writer.Write(Name);
            writer.Write(LineStart);
            writer.Write(LineEnd);
            writer.Write((int) ScopeType);
            writer.Write((int) Context);

            BehaviourData.Write(writer);

            writer.Write(Children.Count);

            foreach (var scriptObject in Children) scriptObject.Write(writer);
        }

        public virtual void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
        }

        protected string TabFormat(string str, int depth = 0)
        {
            var splitLines = str.Split('\n');
            var results = new List<string>();
            for (var index = 0; index < splitLines.Length; index++)
            {
                var splitLine = splitLines[index];
                splitLine = splitLine.Trim();
                if (splitLine.Length == 0)
                    continue;
                if (splitLine.Contains("}"))
                    depth--;

                for (var i = 0; i < depth; i++) splitLine = "     " + splitLine;

                if (splitLine.Contains("{"))
                {
                    if (depth == 0)
                        results.Add("");
                    depth++;
                }

                results.Add(splitLine);
            }

            return string.Join("\n", results);
        }

        protected string GetStart(string name)
        {
            return name + @" = {
            ";
        }

        protected string GetEnd()
        {
            return @"
            }";
        }

        //  public abstract void Parse(string name, ScriptParsedSegment segment);
        public virtual string ToScript()
        {
            return "";
        }

        public ScriptScope AddScriptScope(string name, ScriptObject scriptObject, ScopeType to, bool temporary,
            bool requiresScopeTag)
        {
            var ss = new ScriptScope
            {
                Declared = null,
                Name = name,
                Temporary = temporary,
                To = to,
                RequiresScopeTag = requiresScopeTag,
                IsValue = to == ScopeType.value || to == ScopeType.@bool || to == ScopeType.@string ||
                          to == ScopeType.flag
            };
            scriptScopes[name] = ss;
            return ss;
        }

        public string GetStringValue()
        {
            if (!(this is ScriptValue))
                return null;

            return (this as ScriptValue).GetStringValue();
        }

        public ScopeType GetPrevScopeType()
        {
            var par = this;
            while (par != null)
            {
                if (par.ScopeType != ScopeType.inheritparent) return par.Parent.GetScopeType();

                par = par.Parent;
            }

            return GetScopeType();
        }

        public ScopeType GetRootScopeType()
        {
            return Topmost.GetScopeType();
        }

        public bool IsScope()
        {
            return isScope;
        }

        public bool IsTriggerEnd()
        {
            return isTriggerEnd;
        }

        public ScopeType GetVarType()
        {
            var val = GetStringValue();

            if (val == null)
                return ScopeType.none;

            if (val == "yes" || val == "no")
                return ScopeType.@bool;

            if (val.Contains("\""))
                return ScopeType.@string;

            if (val.StartsWith("flag:"))
                return ScopeType.flag;

            return ScopeType.value;
        }

        public bool HasChild(string child)
        {
            return Children.Any(a => a.Name == child);
        }

        public void DoSmartFind(SmartFindOptions options, List<SmartFindResults> results)
        {
            if (Name == "culture")
            {
            }

            if (BehaviourData == null)
                return;

            if (CheckMatchesSmartFind(options))
                if (CheckNameMatchSmartFind(options))
                {
                    var r = new SmartFindResults();
                    r.ScriptObject = this;
                    r.IsModResult = !Topmost.ScriptFile.IsBase;
                    results.Add(r);
                }

            foreach (var scriptObject in Children) scriptObject.DoSmartFind(options, results);
        }

        private bool CheckNameMatchSmartFind(SmartFindOptions options)
        {
            if (options.SearchRHS && MatchTextSmartFind(options, GetStringValue()))
                return true;

            if (options.SearchLHS)
                return MatchTextSmartFind(options, Name);

            return false;
        }

        private bool MatchTextSmartFind(SmartFindOptions options, string str)
        {
            if (str == null)
                return false;

            var toFind = options.TextToFind;
            if (!options.CaseSensitive)
            {
                str = str.ToLower();
                toFind = toFind.ToLower();
            }

            if (options.FindWholeWorld)
                return str == toFind;
            return str.Contains(toFind);
        }

        private bool CheckMatchesSmartFind(SmartFindOptions options)
        {
            if (!options.ScopesToSearch.Contains(GetScopeType()))
                return false;

            if (!options.SearchOverridden &&
                Overridden)
                return false;

            return true;
        }

        public class ScriptList
        {
            public string Name { get; set; }
            public ScopeVarType VarType { get; set; }
            public ScopeType VarScopeType { get; set; }
            public bool IsVarList { get; set; }
            public ScriptObject Declared { get; set; }
            public bool Temporary { get; set; }
        }

        public class ScriptScope
        {
            private ScopeType toScope;
            public bool Temporary { get; set; }
            public string Name { get; set; }
            public bool IsValue { get; set; }

            public ScopeType To
            {
                get
                {
                    if (ToObj != null)
                        return ToObj.GetScopeType();

                    return toScope;
                }
                set
                {
                    toScope = value;
                    ToObj = null;
                }
            }

            public ScriptObject ToObj { get; set; }

            public ScriptObject Declared { get; set; }
            public bool RequiresScopeTag { get; set; }

            public bool Show(ScopeFindType values)
            {
                if (values == ScopeFindType.Any)
                    return true;
                if (values == ScopeFindType.Object && !IsValue)
                    return true;
                if (values != ScopeFindType.Object && IsValue)
                    return true;

                return false;
            }
        }
    }
}