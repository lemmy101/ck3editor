namespace JominiParse
{
    public enum VariableType
    {
        Scoped,
        Global,
        Local
    }

    public class Variable
    {
        public VariableType VariableType { get; set; }
        public string Name { get; set; }
        public bool IsValue { get; set; }
        public ScopeType VarType { get; set; }
        public ScriptObject ToObj { get; set; }
        public ScriptObject Declared { get; set; }
        public bool RequiresVarTag { get; set; }
        public ScopeType InsideScope { get; set; }
    }
}