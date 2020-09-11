using System;
using System.Collections.Generic;
using System.IO;
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

        public override void Read(BinaryReader reader, ScriptFile file, ScriptObject parent)
        {
            base.Read(reader, file, parent);

            picture = (ScriptValue)FindChild("picture");
            ai_check_interval = (ScriptValue)FindChild("ai_check_interval");
            major = (ScriptValue)FindChild("major");
            desc = (LocalizedString)FindChild("desc");
            selection_tooltip = (LocalizedString)FindChild("selection_tooltip");
            cooldown = (ScriptPeriod)FindChild("cooldown");
            is_shown = (ConditionBlock)FindChild("is_shown");
            is_valid = (ConditionBlock)FindChild("is_valid");
            is_valid_showing_failures_only = (ConditionBlock)FindChild("is_valid_showing_failures_only");
            cost = (ScriptCost)FindChild("cost");
            ai_will_do = (ValueModifier)FindChild("ai_will_do");
            ai_potential = (ConditionBlock)FindChild("ai_potential");
            effect = (EffectBlock)FindChild("effect");
   
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
