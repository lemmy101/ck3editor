using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace JominiParse
{
    public class ScriptObject
    {
        public static Dictionary<int, Type> TypeMap = new Dictionary<int, Type>();
        
        public static List<ScriptObject> DeferedInitializationList = new List<ScriptObject>();

        public string Namespace { get; set; }
        public SchemaNode Schema;
        public ScriptFile ScriptFile { get; set; }
        public bool Overridden { get; set; }

        public enum ScopeVarType
        {
            Bool,
            num,
            None,
            String
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
            public ScopeVarType VarType { get; set; }

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
                else if (values != ScopeFindType.Object && IsValue)
                    return true;

                return false;
            }
        }

        public Dictionary<string, ScriptScope> scriptScopes = new Dictionary<string, ScriptScope>();
        public Dictionary<string, ScriptList> scriptLists = new Dictionary<string, ScriptList>();

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

        public virtual void PostRead()
        {

        }

        public virtual void OnPostInitializeChild(ScriptObject child)
        {

        }

        public virtual void Initialize()
        {
            EnumExtractorUtility.Instance.Add(this);

            if (Name == "events")
            {
                Core.Instance.LoadingCK3Library.RegisterFirstValidEventsTrigger(this);
            }
            
            if (Name == "trigger_event")
            {
                Core.Instance.LoadingCK3Library.RegisterTrigger(this);
            }
            if (Name == "first_valid_on_action")
            {
                Core.Instance.LoadingCK3Library.RegisteFirstValidOnActionTrigger(this);
            }
            if (Name == "random_on_action")
            {
                Core.Instance.LoadingCK3Library.RegisterRandomOnActionTrigger(this);
            }
            if (Name == "on_actions")
            {
                Core.Instance.LoadingCK3Library.RegisteFirstValidOnActionTrigger(this);
            }

            if (Name == "random_events")
            {
                Core.Instance.LoadingCK3Library.RegisterRandomEventsTrigger(this);
            }
            if (Name == "first_valid")
            {
                Core.Instance.LoadingCK3Library.RegisterFirstValidEventsTrigger(this);
            }
            HandleScopeDeclarationFunctions(this, this.Parent);

            foreach (var scriptObject in Children)
            {
                scriptObject.Initialize();
            }
        }

        public ScriptObjectBehaviourData BehaviourData { get; set; }

        public virtual void PostInitialize()
        {

            if (Name == "root")
                SetScopeType(Topmost.GetScopeType());

            ScriptObjectBehaviourManager.Instance.ProcessObject(this);

            foreach (var scriptObject in Children)
            {
                scriptObject.PostInitialize();
            }
        }

        public ScriptObject(ScriptObject parent, ScriptParsedSegment seg, SchemaNode schema = null)
        {


            Op = seg.op;
            IsBlock = seg.isBlock;
            if (parent == null)
            {
                ScriptObject.DeferedInitializationList.Add(this);
            }

            if (seg == null)
            {
                return;
            }

            this.Name = seg.name;
            this.Filename = seg.filename;
            this.LineStart = seg.lineNumbers.First();
            this.LineEnd = seg.lineNumbers.Last();
            this.Parent = parent;
            this.Library = Core.Instance.LoadingCK3Library;

            if (LineStart == ScriptObjectBehaviourManager.BreakpointLine &&
                Topmost.Filename.Contains(ScriptObjectBehaviourManager.BreakpointFile))
            {

            }

            if (Name == "liege")
            {
            }

            Schema = schema;


            if (parent != null)
            {

                parent.Children.Add(this);


                if (Parent != null)
                {
                    if (GetScopeType() == ScopeType.none)
                        SetScopeType(ScopeType.inheritparent);
                }

                foreach (var scriptParsedSegment in seg.children)
                {
                    ScriptObject so = null;
                    if (scriptParsedSegment.value.Count > 0)
                    {

                        so = ScriptValueParser.Instance.ParseScriptValue(this, scriptParsedSegment);

                    }
                    else
                    {
                        so = ScriptObjectFactory.Instance.CreateScriptObject(Context, scriptParsedSegment, this,
                            Namespace);
                    }

                    if (BlockType == BlockType.effect_block)
                    {
                        //     ScopeManager.Instance.AddScopeFunction(so.GetScopeType(), so.Name);

                    }

                    OnPostInitializeChild(so);
                }


            }
        }

        private void HandleScopeDeclarationFunctions(ScriptObject scriptObject, ScriptObject parent)
        {
            
            if (Name == "add_to_temporary_list")
            {
                scriptObject.Topmost.AddList(this, true);
            }
            if (Name == "add_to_list")
            {
                scriptObject.Topmost.AddList(this, false);
            }
            if (Name == "save_scope_as")
            {
                scriptObject.Topmost.AddScope(this, false);
            }
            if (Name == "save_scope_value_as")
            {
                scriptObject.Topmost.AddScopeVar(this, false);
            }
            else if (Name == "save_temporary_scope_as")
            {
                scriptObject.Topmost.AddScope(this, true);
            }

        }

        public enum ScopeFindType
        {
            Object,
            Value,
            Any,
        }
        static List<ScriptObject> visited = new List<ScriptObject>();
        public void GetValidScriptScopes(List<ScriptScope> results, bool allowTemp = true, ScopeFindType values =ScopeFindType.Object)
        {
            if (Topmost != this || allowTemp)
            {
                visited.Clear();
            }
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
                {
                    if(!results.Any(a=>a.Name == scriptScopesValue.Name && a.Show(values)))
                        results.Add(scriptScopesValue);
                }

//                results.AddRange(scriptScopes.Values);
            }
            else
            {
                foreach (var scriptScopesValue in scriptScopes.Values)
                {
                    if (!scriptScopesValue.Temporary && !results.Any(a => a.Name == scriptScopesValue.Name && a.Show(values)))
                        results.Add(scriptScopesValue);
                }

            }

            var Connections = ReferenceManager.Instance.GetConnectionsTo(this.Topmost.Name);
            foreach (var eventConnection in Connections)
            {
//                if (this == eventConnection.To && eventConnection.From != this)
                {
                    eventConnection.From.GetValidScriptScopes(results, false);
                }
            }
        }

        private void AddScope(ScriptObject scope_command, bool temporary)
        {
            if (!(this == Topmost))
            {
                AddScope(scope_command, temporary);
                return;
            }
            ScriptScope s = new ScriptScope();
            var sc = (scope_command as ScriptValue);
            if (sc != null)
            {

                s.Temporary = temporary;
                s.Name = sc.GetStringValue();
                if (scriptScopes.ContainsKey(s.Name))
                    return;

                if (s.Name == "departure" && scope_command.LineStart == 44)
                {

                }

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

            ScriptList s = new ScriptList();
            var sc = (addToListCommand as ScriptValue);
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

        private void AddScopeVar(ScriptObject scope_command, bool temporary, ScopeVarType varType)
        {
            
            if (!(this == Topmost))
            {
                AddScopeVar(scope_command, temporary);
                return;
            }
            ScriptScope s = new ScriptScope();
            s.IsValue = true;
            s.VarType = varType;
          //  var sc = (scope_command as ScriptValue);
         //   if (sc != null)
            {

                s.Temporary = temporary;
                s.Name = scope_command.GetChildStringValue("name");//.GetStringValue();
                if (s.Name == null)
                    return;
                if (scriptScopes.ContainsKey(s.Name))
                    return;

                s.ToObj = scope_command.Parent;
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
            ScopeVarType type = ScopeVarType.Bool;

            if (w.Count() > 0)
            {
                var f = w.First();

                string s = f.GetStringValue();

                if (!(s == "yes" || s == "no"))
                {
                    type = ScopeVarType.num;
                }

            }
            AddScopeVar(scope_command, temporary, type);

        }

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
            if(type == ScopeType.any)
            {

            }
            ScopeType = type;
        }

        private BlockType BlockType { get; set; }
        private ScopeType ScopeType { get; set; } = ScopeType.inheritparent;
        public ScriptObject Parent { get; set; }

        public int LineEnd { get; set; }

        public int LineStart { get; set; }

        public string Filename { get; set; }
        public virtual string Name { get; set; }
        public ScriptContext Context { get; set; }
        public bool IsBlock { get; set; }

        public virtual void Write(BinaryWriter writer)
        {
            writer.Write(GetType().GetHashCode());
            writer.Write(Name);
            writer.Write(LineStart);
            writer.Write(LineEnd);
            writer.Write((int)ScopeType);
            writer.Write((int)Context);
            writer.Write(Children.Count);
            foreach (var scriptObject in Children)
            {
                scriptObject.Write(writer);
            }
        }

        public virtual void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
           

        }
        protected string TabFormat(string str, int depth = 0)
        {
            string[] splitLines = str.Split('\n');
            List<string> results = new List<string>();
            for (var index = 0; index < splitLines.Length; index++)
            {
                var splitLine = splitLines[index];
                splitLine = splitLine.Trim();
                if(splitLine.Length==0)
                    continue;
                if (splitLine.Contains("}"))
                    depth--;


                for (int i = 0; i < depth; i++)
                {
                    splitLine = "     " + splitLine;
                }

                if (splitLine.Contains("{"))
                {
                    if(depth == 0)
                        results.Add("");
                    depth++;
                }

                
                results.Add(splitLine);
            }

            return String.Join("\n", results);
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


        public List<ScriptObject> Children = new List<ScriptObject>();
        private bool isScope;
        private bool isTriggerEnd;

      

        public static ScriptObject LoadFromData(BinaryReader reader, ScriptObject parent)
        {
            int id = reader.ReadInt32();

            var type = TypeMap[id];

            ScriptObject o = (ScriptObject)Activator.CreateInstance(type, parent, null);

            return o;
        }

        public ScriptScope AddScriptScope(string name, ScriptObject scriptObject, ScopeType to, bool temporary, bool requiresScopeTag)
        {
            var ss = new ScriptScope()
                {Declared = null, Name = name, Temporary = temporary, To = to, RequiresScopeTag = requiresScopeTag};
            scriptScopes[name] = ss;
            return ss;
        }

        public string GetStringValue()
        {
            if (!(this is ScriptValue))
                return null;

            return ((this as ScriptValue).GetStringValue());
        }

        public ScopeType GetPrevScopeType()
        {
            var par = this;
            while (par != null)
            {
                if (par.ScopeType != ScopeType.inheritparent)
                {
                    return par.Parent.GetScopeType();
                }

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

        public ScopeVarType GetVarType()
        {
            string val = GetStringValue();

            if (val == null)
                return ScopeVarType.None;

            if (val == "yes" || val == "no")
                return ScriptObject.ScopeVarType.Bool;

            if (val.Contains("\""))
                return ScriptObject.ScopeVarType.String;


            return ScriptObject.ScopeVarType.num;
        }

        public bool HasChild(string child)
        {
            return Children.Any(a => a.Name == child);
        }

        public void DoSmartFind(SmartFindOptions options, List<SmartFindResults> results)
        {
            if (this.Name == "culture")
            {

            }

            if (this.BehaviourData == null)
                return;

            if (CheckMatchesSmartFind(options))
            {
                if(CheckNameMatchSmartFind(options))
                {
                    SmartFindResults r = new SmartFindResults();
                    r.ScriptObject = this;
                    r.IsModResult = !Topmost.ScriptFile.IsBase;
                    results.Add(r);
                }
            }

            foreach (var scriptObject in Children)
            {
                scriptObject.DoSmartFind(options, results);
            }
        }

        private bool CheckNameMatchSmartFind(SmartFindOptions options)
        {
            if (options.SearchValues && MatchTextSmartFind(options, GetStringValue()))
                return true;

            return MatchTextSmartFind(options, Name);
        }

        private bool MatchTextSmartFind(SmartFindOptions options, string str)
        {
            if (str == null)
                return false;

            string toFind = options.TextToFind;
            if (!options.CaseSensitive)
            {
                str = str.ToLower();
                toFind = toFind.ToLower();
            }

            if (options.FindWholeWorld)
                return str == toFind;
            else
            {
                return str.Contains(toFind);
            }

        }

        private bool CheckMatchesSmartFind(SmartFindOptions options)
        {
            if (!options.ScopesToSearch.Contains(GetScopeType()))
                return false;

            if (!options.SearchOverridden &&
                this.Overridden)
            {
                return false;
            }

            if (options.SearchSavedScopes &&
                (this.BehaviourData.Type == ScriptObjectBehaviourType.SavedScopeBlock || this.BehaviourData.Type == ScriptObjectBehaviourType.SavedScopeToProperty))
            {
                return true;
            }

            if (options.SearchFunctionParameters &&
                (this.BehaviourData.Type == ScriptObjectBehaviourType.FunctionParameter))
            {
                return true;
            }

            if (options.SearchTriggerFunctions &&
                (this.BehaviourData.Type == ScriptObjectBehaviourType.FunctionMultiline ||
                  this.BehaviourData.Type == ScriptObjectBehaviourType.FunctionSingleLine || this.BehaviourData.Type == ScriptObjectBehaviourType.InherentScopeToProperty) && BehaviourData.ParentExpectTriggers)
            {
                return true;
            }
            if (options.SearchEffectFunctions &&
                (this.BehaviourData.Type == ScriptObjectBehaviourType.FunctionMultiline ||
                  this.BehaviourData.Type == ScriptObjectBehaviourType.FunctionSingleLine) && BehaviourData.ParentExpectEffects)
            {
                return true;
            }

            return false;
        }

        public string Op { get; set; } = "=";
    }
}