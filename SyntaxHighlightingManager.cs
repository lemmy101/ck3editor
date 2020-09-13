using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.TextEditor.Document;
using JominiParse;

namespace CK3ScriptEditor
{
    public class SyntaxHighlightingManager
    {
        public static SyntaxHighlightingManager Instance = new SyntaxHighlightingManager();

        public void DoDocument(IDocument doc, ScriptFile file)
        {
            foreach (var scriptObject in file.Map.Values)
            {
                DoScriptObject(scriptObject, doc);
            }

        }

        private List<string> baseCommands = new List<string>()
        {
            "if",
            "else_if",
            "else",
            "trigger_if",
            "trigger_else_if",
            "trigger_else",
            "limit",
            "NOT",
            "NOR",
            "OR",
            "AND",
            "NAND",

        };

        private Color FunctionColor = Color.White;// Color.FromArgb(173, 139, 236);
        private Color ScopeColor = Color.FromArgb(139, 229, 236);
        private Color OverriddenColor = Color.FromArgb(128, 96, 96);
        private Color StringColor = Color.FromArgb(241, 250, 140);
        private Color NumberColor = Color.FromArgb(189, 147, 249);
        
        private Color BaseCommandColor = Color.FromArgb(228, 112, 198);
        private Color ReferencedObjectColor = Color.FromArgb(208, 176, 113);
        private Color LocalizedStringColor = Color.FromArgb(133, 113, 208);
        

        private void DoScriptObject(ScriptObject scriptObject, IDocument doc)
        {
            
            int line = scriptObject.LineStart - 1;

            bool avoidRed = scriptObject.Topmost.Name.StartsWith("scripted_trigger");
         
            string name = scriptObject.Name;

            var lineSeg = doc.LineSegmentCollection[line];
           
            var col = Color.Red;
            bool scopeConditionsAreValid = false;

            bool bold = false;
            bool italic = false;
            if (scriptObject == scriptObject.Topmost && scriptObject.Overridden)
            {
                ColorEntireObject(OverriddenColor, doc, scriptObject.LineStart-1, scriptObject.LineEnd);
                return;
            }

            if (scriptObject.Topmost.Overridden)
            {
                return;
            }
            
            if (name.StartsWith("scope:recipient.primary_title.tier"))
            {

            }
            bool scopeEffectsAreValid = false;
            bool scopeTriggerAreValid = false;
            if (scriptObject.Parent != null && scriptObject.Parent.Schema != null)
            {
                var parentSchema = scriptObject.Parent.Schema;

                if (parentSchema.children.ContainsKey("scopeconditions"))
                {
                    scopeConditionsAreValid = true;
                }
                if (parentSchema.children.ContainsKey("scopeeffects"))
                {
                    scopeEffectsAreValid = true;
                }

                var par = scriptObject.Parent;

                while(par != null && par.Parent != null)
                {
                    bool isScope = false;

                    if (ScopeManager.Instance.isConditionScope(par.Parent.GetScopeType(), par.Name))
                    {
                        isScope = true;
                    }
                    else if (ScopeManager.Instance.isEffectScope(par.Parent.GetScopeType(), par.Name))
                    {
                        isScope = true;
                    }
                    else if (ScopeManager.Instance.isConditionScopeInside(par.Parent.GetScopeType(), par.Name, par.Parent))
                    {
                        isScope = true;
                    }
                    else if (ScopeManager.Instance.isEffectScopeInside(par.Parent.GetScopeType(), par.Name, par.Parent))
                    {
                        isScope = true;
                    }
                    else if (par.Name == "OR" || par.Name == "AND" || par.Name == "NOT" || par.Name == "NOR")
                        isScope = true;

                    if (!isScope)
                    {
                        if (par.Name == "trigger")
                        {
                            scopeTriggerAreValid = true;
                        }
                        break;
                    }
                    par = par.Parent;
                }
               
                if(parentSchema.children.ContainsKey(name))
                {
                    col = FunctionColor;
                }
            }

            if (scriptObject.Parent == null)
            {
                col = ReferencedObjectColor;
            }
                
            if (scriptObject.Parent != null)
            {
                var scope = scriptObject.Parent.GetScopeType();

                if (scopeConditionsAreValid && ScopeManager.Instance.isCondition(scope, name))
                {
                    col = FunctionColor;

                    if (ScopeManager.Instance.GetCondition(scope, name).treatAsScope)
                        col = ScopeColor;
                }

                if (scopeEffectsAreValid && ScopeManager.Instance.isEffect(scope, name))
                {
                    col = FunctionColor;
                }

                if (scopeConditionsAreValid && ScopeManager.Instance.isConditionScope(scope, name))
                {
                    col = ScopeColor;
                }
                if (scopeEffectsAreValid && ScopeManager.Instance.isEffectScope(scope, name))
                {
                    col = ScopeColor;
                }
                if (scopeConditionsAreValid && ScopeManager.Instance.isConditionScopeInside(scope, name, scriptObject.Parent))
                {
                    col = ScopeColor;
                }
                if (scopeEffectsAreValid && ScopeManager.Instance.isEffectScopeInside(scope, name, scriptObject.Parent))
                {
                    col = ScopeColor;
                }

                if(scopeConditionsAreValid && ScopeManager.Instance.isConditionScopeEndParamInside(scope, name, scriptObject.Parent))
                {
                    col = ScopeColor;
                }
            }
            if (baseCommands.Contains(name))
            {
                col = BaseCommandColor;

            }

            if (col == Color.Red && scopeEffectsAreValid)
            {

                // try scripted effects?

                if (Core.Instance.GetScriptedEffectNameSet(false).Contains(name))
                {
                    col = ReferencedObjectColor;
                }
            }

            string nameForType = null;
            if(col == Color.Red)
            {
                if (scriptObject.Parent != null && scriptObject.Parent.Schema != null)
                {
                    var namesFromList = scriptObject.Parent.Schema.children.Where(a => a.Value.NamesFrom != null).ToList();

                    foreach (var keyValuePair in namesFromList)
                    {
                        string namesFrom = keyValuePair.Value.NamesFrom;

                        if(namesFrom == "num")
                        {
                            int result;
                            if(Int32.TryParse(name, out result))
                            {
                                col = NumberColor;
                                break;
                            }
                        }
                        else
                        {
                            var list = Core.Instance.GetNameSetFromEnumType(namesFrom);

                            if (list.Contains(name))
                            {
                                col = FunctionColor;
                                nameForType = keyValuePair.Value.Type;
                                break;
                            }
                        }
                   
                    }
                }
            }
            if(col == Color.Red)
            {
                if(scopeTriggerAreValid)
                {
                    var list = Core.Instance.GetNameSetFromEnumType("scripted_trigger");

                    if (list.Contains(name))
                    {
                        col = FunctionColor;
                    }
                }
            }
            if (scriptObject.Topmost.Overridden)
                col = OverriddenColor;

            if (col == ScopeColor)
                bold = true;

            if (col == ReferencedObjectColor)
                bold = true;

            if (col == Color.Red)
                bold = true;

            if (avoidRed)
                col = FunctionColor;

            if (name.StartsWith("scripted_trigger "))
            {
                ColourName("scripted_trigger ", doc, lineSeg, BaseCommandColor, bold, italic);
                ColourName(name.Substring(name.IndexOf(' ')+1), doc, lineSeg, ReferencedObjectColor, bold, italic);
            }
            else
            {
                ColourName(name, doc, lineSeg, col, bold, italic);
            }

            


            if (scriptObject is ScriptValue && !(scriptObject is StaticScriptValue))
            {
                col = Color.Red;
                italic = false;

                var strVal = (scriptObject as ScriptValue).GetStringValue();
                if (strVal == "minor_gold_value")
                {

                }
                if(strVal != "yes" && strVal != "no")
                {
                    if (strVal == "is_in_event_abduct_outcome_0001")
                    {

                    }
                    if (scriptObject.Parent != null && scriptObject.Parent.Schema != null)
                    {
                        var parentSchema = scriptObject.Parent.Schema;
                        var ch = parentSchema.GetChild(name);
                        var scope = scriptObject.Parent.GetScopeType();

                      
                         if (ch != null)
                         {

                             col = TestPropertyType(scriptObject, ch.Type, strVal, col, scopeConditionsAreValid,
                                 scopeEffectsAreValid);

                             if (ch is SchemaEnum)
                             {
                                 if ((ch as SchemaEnum).choices.Contains(strVal))
                                 {
                                     col = FunctionColor;
                                 }

                             }

                             if (Core.Instance.GetNameSetFromEnumType(ch.Type).Contains(strVal))
                             {
                                 col = ReferencedObjectColor;
                                 italic = true;
                             }
                             else if (ch.Type == "localized")
                             {
                                 col = LocalizedStringColor;
                             }
                             else if (ch.Type == "any")
                             {
                                 col = FunctionColor;
                                 italic = true;
                                 bold = false;

                             }
                             else if (ch.Type == "string")
                             {
                                 col = StringColor;
                                 italic = true;
                                 bold = false;

                             }
                        }
                        else if (scopeConditionsAreValid)
                        {
                            var con = ScopeManager.Instance.GetCondition(scope, name);
                            if (con != null && (Core.Instance.GetNameSetFromEnumType(con.type).Contains(strVal) || con.type == "any" || con.type == "string"))
                            {
                                col = ReferencedObjectColor;
                                if (con.type == "any")
                                {
                                    col = FunctionColor;
                                    italic = true;
                                    bold = false;

                                }
                                if (con.type == "string")
                                {
                                    col = StringColor;
                                    italic = true;
                                    bold = false;

                                }
                            }
                            else
                            {
                                if (con != null)
                                {
                                    col = TestPropertyType(scriptObject,con.type, strVal, col, scopeConditionsAreValid, scopeEffectsAreValid);


                                }
                                else
                                {
                                    // singular can be tested against another
                                    ScopeType resScope;
                                    if (ScopeManager.Instance.isSingularConditionScope(scope, name, out resScope, scriptObject.Parent))
                                    {
                                        bool requiresTest = true;
                                        ScopeType required = resScope;

                                        {
                                            var str = strVal;
                                            if (CheckIsValidScope(str, scriptObject, scopeConditionsAreValid, scopeEffectsAreValid, required))
                                            {
                                                col = ScopeColor;

                                            }
                                            if (!str.StartsWith("scope:"))
                                                str = "scope:" + str;
                                            if (CheckIsValidScope(str, scriptObject, scopeConditionsAreValid, scopeEffectsAreValid, required))
                                            {
                                                col = ScopeColor;

                                            }
                                        }
                                    }
                                    if (ScopeManager.Instance.isConditionScopeEndParamInside(scope, name, scriptObject.Parent))
                                    {
                                        var ns = ScopeManager.Instance.getConditionScopeInside(scope,
                                            name.Substring(0, name.LastIndexOf(".")), scriptObject.Parent);
                                        var condition = ScopeManager.Instance.GetCondition(ns,
                                            name.Substring(name.LastIndexOf(".") + 1));

                                        if(condition != null)
                                        {
                                            col = TestPropertyType(scriptObject, condition.type, strVal, col, scopeConditionsAreValid, scopeEffectsAreValid);

                                        }
                                    }
                                }
                            }
                        }
                        else if (scopeEffectsAreValid)
                        {
                            var con = ScopeManager.Instance.GetEffect(scope, name);
                            if (con != null && (Core.Instance.GetNameSetFromEnumType(con.type).Contains(strVal) || con.type == "any"))
                            {
                                col = ReferencedObjectColor;
                                if (con.type == "any")
                                {
                                    col = FunctionColor;
                                    italic = true;
                                    bold = false;

                                }
                            }
                            else
                            {
                                if (con != null)
                                {
                                    var t = con.type;
                                    col = TestPropertyType(scriptObject, t, strVal, col, scopeConditionsAreValid, scopeEffectsAreValid);
                                }
                            }

                        }

                         if(col == Color.Red && nameForType != null)
                        {
                            col = TestPropertyType(scriptObject, nameForType, strVal, col, scopeConditionsAreValid, scopeEffectsAreValid);

                        }

                        if (scriptObject.Topmost.Overridden)
                            col = OverriddenColor;

                        if (col == ScopeColor)
                            bold = true;

                        if (col == ReferencedObjectColor)
                            bold = true;

                        if (col == Color.Red)
                            bold = true;

                        if (avoidRed)
                            col = FunctionColor;
                        ColourName(strVal, doc, lineSeg, col, bold, italic);
                    }
                }
                else
                {
                    if (avoidRed)
                        col = FunctionColor;
                    ColourName(strVal, doc, lineSeg, BaseCommandColor, bold, italic);

                }

            }

            foreach (var scriptObjectChild in scriptObject.Children)
            {
                DoScriptObject(scriptObjectChild, doc);
            }
        }

        private Color TestPropertyType(ScriptObject scriptObject, string t, string strVal, Color col,
            bool scopeConditionsAreValid, bool scopeEffectsAreValid)
        {
            bool requiresTest = true;
            ScopeType required = ScopeType.any;

            switch (t)
            {
                case "num":
                {
                    if (Core.Instance.GetScriptValueNameSet(false).Contains(strVal))
                        col = ReferencedObjectColor;
                }
                    break;
                case "scope":
                    
                    break;
                case "string":
                {
                    requiresTest = false;
                    col = StringColor;
                }
                    break;
                default:
                {
                    if (!Enum.TryParse(t, out required))
                    {
                        requiresTest = false;
                    }
                }
                    break;
            }

            if (t == "this")
            {
                requiresTest = true;
                required = scriptObject.Parent.GetScopeType();
            }
            if (t == "prev")
            {
                requiresTest = true;
                required = scriptObject.Parent.GetPrevScopeType();
            }
            if (t == "root")
            {
                requiresTest = true;
                required = scriptObject.Parent.GetRootScopeType();
            }

            if (requiresTest)
            {
                var str = strVal;
                if (CheckIsValidScope(str, scriptObject, scopeConditionsAreValid, scopeEffectsAreValid, required))
                {
                    col = ScopeColor;
                }
                if (!str.StartsWith("scope:"))
                    str = "scope:" + str;
                if (CheckIsValidScope(str, scriptObject, scopeConditionsAreValid, scopeEffectsAreValid, required))
                {
                    col = ScopeColor;
                }
            }
            else if(t != null)
            {
                var list = EnumManager.Instance.GetEnums(t);

                if(list.Contains(strVal))
                {
                    col = ReferencedObjectColor;
                }
            }



          

            return col;
        }


        private bool CheckIsValidScope(string strVal, ScriptObject scriptObject, bool scopeConditionsAreValid, bool scopeEffectsAreValid, ScopeType typeRequired = ScopeType.any)
        {
            if (strVal == "scope:target.location.county.holder.capital_county")
            {

            }
         //   if (scopeConditionsAreValid)
            {
                bool success;
                var res = ScopeManager.Instance.ChangeConditionScope(scriptObject.GetScopeType(), strVal, out success,
                    scriptObject);

                if (success)
                {
                    if (typeRequired != ScopeType.any)
                        return res == typeRequired;

                    return true;
                }
            }
        //    if (scopeEffectsAreValid)
            {
                bool success;
                var res = ScopeManager.Instance.ChangeScope(scriptObject.GetScopeType(), strVal, out success,
                    scriptObject);

                if (success)
                {
                    if (typeRequired != ScopeType.any)
                        return res == typeRequired;

                    return true;
                }
            }

            return false;
        }


        private void ColorEntireObject(Color col, IDocument doc, int f, int t)
        {
            for (int x = f; x < t; x++)
            {
                ColorEntireLine(col, doc, doc.LineSegmentCollection[x]);
            }
        }

        private void ColorEntireLine(Color col, IDocument doc, LineSegment line)
        {
            foreach (var lineWord in line.Words)
            {
                if (lineWord.SyntaxColor != null)
                    lineWord.SyntaxColor.Color = col;
            }
        }
        private void ColourName(string name, IDocument doc, LineSegment line, Color color, bool bold, bool italic)
        {
            if (!line.ColorWord(name, color, bold, italic))
            {
          
            }
        }
    }
}
