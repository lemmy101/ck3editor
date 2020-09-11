using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JominiParse
{
    public class ScriptDecision : ScriptObject
    {
        public static List<String> validTokens = new List<string>();

        public override string ToScript()
        {
            return TabFormat(@"
" + Name + @" = { 
    picture = " + picture?.ToScript() + @"
    " + major?.ToScript() + @"
    " + ai_check_interval?.ToScript() + @"
    " + desc?.ToScript() + @"
    " + selection_tooltip?.ToScript() + @"
    " + cooldown?.ToScript() + @"
    " + cost?.ToScript() + @"
    " + is_shown?.ToScript() + @"
    " + is_valid?.ToScript() + @"
    " + is_valid_showing_failures_only?.ToScript() + @"
    " + ai_will_do?.ToScript() + @"
    " + ai_potential?.ToScript() + @"
    " + effect?.ToScript() + @"
}", 0);


        }

        public ScriptValue picture { get; set; }
        public ScriptValue ai_check_interval { get; set; }
        public ScriptValue major { get; set; }
        public LocalizedString desc { get; set; }
        public LocalizedString selection_tooltip { get; set; }
        public ScriptPeriod cooldown { get; set; }
        public ConditionBlock is_shown { get; set; }
         public ConditionBlock is_valid { get; set; }
        public ConditionBlock is_valid_showing_failures_only { get; set; }
        public ScriptCost cost { get; set; }
        public ValueModifier ai_will_do { get; set; }
        public ConditionBlock ai_potential { get; set; }
        public EffectBlock effect { get; set; }
      
        public ScriptDecision(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }
    }
}
