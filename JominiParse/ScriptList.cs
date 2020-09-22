namespace JominiParse
{
    public class ScriptList
    {
        public string Name { get; set; }
        public ScopeVarType VarType { get; set; }
        public ScopeType VarScopeType { get; set; }
        public bool IsVarList { get; set; }
        public ScriptObject Declared { get; set; }
        public bool Temporary { get; set; }
    }
}