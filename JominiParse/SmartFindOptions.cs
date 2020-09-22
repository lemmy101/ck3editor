#region

using System.Collections.Generic;

#endregion

namespace JominiParse
{
    public class SmartFindOptions
    {
        public List<ScopeType> ScopesToSearch = new List<ScopeType>();
        public string TextToFind { get; set; }
        public bool CaseSensitive { get; set; }
        public bool FindWholeWorld { get; set; }
        public bool SearchBase { get; set; }
        public bool SearchMod { get; set; }
        public bool SmartDoFind { get; set; }
        public bool SearchOverridden { get; set; }
        public bool ShowChildren { get; set; }
        public bool SearchRHS { get; set; }
        public bool SearchLHS { get; set; }
    }

    public class SmartFindResults
    {
        public ScriptObject ScriptObject { get; set; }
        public bool IsModResult { get; set; }
    }
}