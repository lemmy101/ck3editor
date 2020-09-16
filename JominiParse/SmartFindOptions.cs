using System.Collections.Generic;

namespace JominiParse
{
    public class SmartFindOptions
    {
        public string TextToFind { get; set; }
        public bool CaseSensitive { get; set; }
        public bool FindWholeWorld { get; set; }
        public bool SearchBase { get; set; }
        public bool SearchMod { get; set; }
        public bool SmartDoFind { get; set; }

        public bool SearchOverridden { get; set; }
        public bool SearchSavedScopes { get; set; }
        public bool SearchFunctionParameters { get; set; }
        public bool SearchValues { get; set; }

        public bool SearchConditionFunctions { get; set; }
        public bool SearchEffectFunctions { get; set; }
        public bool ShowChildren { get; set; }


        public List<ScopeType> ScopesToSearch = new List<ScopeType>();
    }

    public class SmartFindResults
    {
        public ScriptObject ScriptObject { get; set; }
        public bool IsModResult { get; set; }
    }
}