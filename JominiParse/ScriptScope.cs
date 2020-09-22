namespace JominiParse
{
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