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
        private Color BaseCommandColor = Color.FromArgb(228, 112, 198);
        private Color ReferencedObjectColor = Color.FromArgb(208, 176, 113);
        private Color LocalizedStringColor = Color.FromArgb(133, 113, 208);
        

        private void DoScriptObject(ScriptObject scriptObject, IDocument doc)
        {
            int line = scriptObject.LineStart - 1;

            string name = scriptObject.Name;
            var lineSeg = doc.LineSegmentCollection[line];

            var col = Color.Red;
            bool scopeConditionsAreValid = false;

            bool bold = true;
            bool italic = false;
    
            bool scopeEffectsAreValid = false;
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
                var scope = scriptObject.GetScopeType();

                if (scopeConditionsAreValid && ScopeManager.Instance.isCondition(scope, name))
                {
                    col = FunctionColor;

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
            }
            if (baseCommands.Contains(name))
            {
                col = BaseCommandColor;

            }

            if (col == Color.Red)
            {

            }
            ColourName(name, doc, lineSeg, col, bold, italic);


            if (scriptObject is ScriptValue && !(scriptObject is StaticScriptValue))
            {
                col = Color.Red;
                italic = false;

                var strVal = (scriptObject as ScriptValue).GetStringValue();
                if (strVal == "lemmy")
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

                            bool couldBeScope = true;
                            ScopeType required = ScopeType.any;

                            switch (ch.Type)
                            {
                                case "scope":

                                    break;
                                default:
                                {
                                    if (!Enum.TryParse(ch.Type, out required))
                                    {
                                        couldBeScope = false;
                                    }
                                }
                                break;
                            }

                            if (couldBeScope)
                            {
                                var str = strVal;
                                if (!str.StartsWith("scope:"))
                                    str = "scope:" + str;
                                if (CheckIsValidScope(str, scriptObject, scopeConditionsAreValid, scopeEffectsAreValid, required))
                                {
                                    col = ScopeColor;

                                }
                            }


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

                        }
                        else if (scopeConditionsAreValid)
                        {
                            var con = ScopeManager.Instance.GetCondition(scope, name);
                            if (con != null && (Core.Instance.GetNameSetFromEnumType(con.type).Contains(strVal) || con.type=="any"))
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
                                    bool requiresTest = true;
                                    ScopeType required = ScopeType.any;

                                    switch (con.type)
                                    {
                                        case "scope":

                                            break;
                                        default:
                                        {
                                            if (!Enum.TryParse(con.type, out required))
                                            {
                                                requiresTest = false;
                                            }
                                        }
                                            break;
                                    }

                                    if (requiresTest)
                                    {
                                        var str = strVal;
                                        if (!str.StartsWith("scope:"))
                                            str = "scope:" + str;
                                        if (CheckIsValidScope(str, scriptObject, scopeConditionsAreValid, scopeEffectsAreValid, required))
                                        {
                                            col = ScopeColor;

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
                                    bool requiresTest = true;
                                    ScopeType required = ScopeType.any;

                                    switch (con.type)
                                    {
                                        case "scope":

                                            break;
                                        default:
                                        {
                                            if (!Enum.TryParse(con.type, out required))
                                            {
                                                requiresTest = false;
                                            }
                                        }
                                            break;
                                    }

                                    if (requiresTest)
                                    {
                                        var str = strVal;
                                        if (!str.StartsWith("scope:"))
                                            str = "scope:" + str;
                                        if (CheckIsValidScope(str, scriptObject, scopeConditionsAreValid, scopeEffectsAreValid, required))
                                        {
                                            col = ScopeColor;

                                        }
                                    }
                                  
                                   
                                }
                            }
                        }
                        
                        

                        ColourName(strVal, doc, lineSeg, col, bold, italic);
                    }
                }
               
            }

            foreach (var scriptObjectChild in scriptObject.Children)
            {
                DoScriptObject(scriptObjectChild, doc);
            }
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



        private void ColourName(string name, IDocument doc, LineSegment line, Color color, bool bold, bool italic)
        {
            if (!line.ColorWord(name, color, bold, italic))
            {
          
            }
        }
    }
}
