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

        public void DoDocument(IDocument doc, Color backgroundColor, ScriptFile file)
        {
            foreach (var scriptObject in file.Map.Values)
            {
              //  ColorEntireObject(Color.White, backgroundColor, doc, scriptObject.LineStart - 1, scriptObject.LineEnd);
                DoScriptObject(scriptObject, backgroundColor, doc);
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
            "alternative_limit",
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

        ScriptObjectSchema AssumedSchema = new ScriptObjectSchema();

        private void DoScriptObject(ScriptObject scriptObject, Color backgroundColor, IDocument doc)
        {
            string name = scriptObject.Name;
            int line = scriptObject.LineStart - 1;
            var lineSeg = doc.LineSegmentCollection[line];
            if(name == "add_trait")
            {

            }
            var col = Color.Red;

            bool bold = false;
            bool italic = false;
            if (scriptObject == scriptObject.Topmost && scriptObject.Overridden)
            {
                ColorEntireObject(OverriddenColor, backgroundColor, doc, scriptObject.LineStart - 1, scriptObject.LineEnd);
                return;
            }

            if (scriptObject.Topmost.Overridden)
            {
                return;
            }

            var data = scriptObject.BehaviourData;
            if (data == null)
                return;
            switch (data.Type)
            {
                case ScriptObjectBehaviourType.FunctionParameter:
                    col = FunctionColor;
                    break;
                case ScriptObjectBehaviourType.FunctionMultiline:
                case ScriptObjectBehaviourType.FunctionSingleLine:
                    col = FunctionColor;
                    bold = true;
                    break;
                case ScriptObjectBehaviourType.InherentScopeBlock:
                case ScriptObjectBehaviourType.InherentScopeToProperty:
                case ScriptObjectBehaviourType.SavedScopeBlock:
                case ScriptObjectBehaviourType.SavedScopeToProperty:
                    col = ScopeColor;
                    bold = true;
                    break;
                case ScriptObjectBehaviourType.RootObject:
                    col = ReferencedObjectColor;
                    bold = true;
                    break;
                case ScriptObjectBehaviourType.RootObjectProperty:
                case ScriptObjectBehaviourType.RootObjectPropertyBlock:
                case ScriptObjectBehaviourType.Limit:
                case ScriptObjectBehaviourType.If:
                case ScriptObjectBehaviourType.TriggerIf:
                case ScriptObjectBehaviourType.Else:
                case ScriptObjectBehaviourType.TriggerElse:
                case ScriptObjectBehaviourType.Trigger:
                case ScriptObjectBehaviourType.FunctionNamedFromParameterSingleLine:
                case ScriptObjectBehaviourType.FunctionNamedFromParameterBlock:
                    col = BaseCommandColor;
                    bold = true;
                    break;
                  
            }

            if (baseCommands.Contains(name))
                col = BaseCommandColor;
            else
            {
                
            }


            ColourName(name, doc, lineSeg, col, backgroundColor, bold, italic);


            if (scriptObject is ScriptValue && !(scriptObject is StaticScriptValue))
            {
                col = Color.Red;

                var strVal = (scriptObject as ScriptValue).GetStringValue();

                if (data.ReferencedObject != null)
                {
                    col = ReferencedObjectColor;
                    italic = true;
                    bold = true;
                    ColourName(strVal, doc, lineSeg, col, backgroundColor, bold, italic);
                }
                else
                {
                    if (data.ValueFound)
                    {
                        if (data.ValueIsScope)
                        {
                            col = ScopeColor;
                            bold = true;
                        }
                        else if (data.TypeExpected == "localized")
                            col = LocalizedStringColor;
                        else
                            col = FunctionColor;
                    }
                    else if (data.TypeExpected == "string" && strVal != null)
                    {
                        col = StringColor;
                    }
                    else if (data.TypeExpected == "any" && strVal != null)
                    {
                        col = FunctionColor;
                        italic = true;
                    }

                    ColourName(strVal, doc,  lineSeg, col, backgroundColor, bold, italic);

                }
            }

            foreach (var scriptObjectChild in scriptObject.Children)
            {
                DoScriptObject(scriptObjectChild, backgroundColor, doc);
            }
        }

        private void DoScriptObjectOld(ScriptObject scriptObject, IDocument doc)
        {
            AssumedSchema.children.Clear();

            string name = scriptObject.Name;

            if(name == "culture")
            {

            }
            int line = scriptObject.LineStart - 1;

            bool parentAvoidRed = false;

            var p = scriptObject.Parent;
           

      
            bool avoidRed = scriptObject.Topmost.Name.StartsWith("scripted_trigger") || parentAvoidRed;

            var lineSeg = doc.LineSegmentCollection[line];

            var col = Color.Red;
            bool scopeConditionsAreValid = false;

            bool bold = false;
            bool italic = false;
            if (scriptObject == scriptObject.Topmost && scriptObject.Overridden)
            {
           //     ColorEntireObject(OverriddenColor, backgroundColor, doc,  scriptObject.LineStart - 1, scriptObject.LineEnd);
                return;
            }

            if (scriptObject.Topmost.Overridden)
            {
                return;
            }

            if (scriptObject.Parent != null && scriptObject.Parent.Schema != null)
            {
                foreach (var sc in scriptObject.Parent.Schema.children)
                {
                    AssumedSchema.children[sc.Key] = sc.Value;
                }
            }

          
            bool scopeEffectsAreValid = false;
            bool scopeTriggerAreValid = false;
            if (scriptObject.Parent != null)
            {
           
                var par = scriptObject.Parent;

                while (par != null && par.Parent != null)
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
                        if (par.Name == "trigger" || par.Name == "limit" || par.Name == "alternative_limit" || par.Name == "modifier")
                        {
                            scopeTriggerAreValid = true;
                        }

                        if (par != null && par.Schema != null)
                        {
                            foreach (var sc in par.Schema.children)
                            {
                                AssumedSchema.children[sc.Key] = sc.Value;
                            }
                        }

                        break;
                    }
                    par = par.Parent;
                }
                if (AssumedSchema.children.ContainsKey("scopeconditions"))
                {
                    scopeConditionsAreValid = true;
                }
                if (AssumedSchema.children.ContainsKey("scopeeffects"))
                {
                    scopeEffectsAreValid = true;
                }

                if (AssumedSchema.children.ContainsKey(name))
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

                if (scopeConditionsAreValid && ScopeManager.Instance.isConditionScopeEndParamInside(scope, name, scriptObject.Parent))
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

                if (Core.Instance.GetNameSet(ScriptContext.ScriptedEffects, false).Contains(name))
                {
                    col = ReferencedObjectColor;
                }
            }

            string nameForType = null;
            if (col == Color.Red)
            {
                if (scriptObject.Parent != null && AssumedSchema != null)
                {
                    var namesFromList = AssumedSchema.children.Where(a => a.Value.NamesFrom != null).ToList();

                    foreach (var keyValuePair in namesFromList)
                    {
                        string namesFrom = keyValuePair.Value.NamesFrom;

                        if (namesFrom == "num")
                        {
                            int result;
                            if (Int32.TryParse(name, out result))
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
                                col = ReferencedObjectColor;
                                nameForType = keyValuePair.Value.Type;
                                break;
                            }
                        }

                    }
                }
            }
            if (col == Color.Red)
            {
                if (scopeTriggerAreValid)
                {
                    var list = Core.Instance.GetNameSetFromEnumType("scripted_trigger");

                    if (list.Contains(name))
                    {
                        col = ReferencedObjectColor;
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
              //  ColourName("scripted_trigger ", doc, lineSeg, BaseCommandColor, bold, italic);
             //   ColourName(name.Substring(name.IndexOf(' ') + 1), doc, lineSeg, ReferencedObjectColor, bold, italic);
            }
            else
            {
             //   ColourName(name, doc, lineSeg, col, bold, italic);
            }




            if (scriptObject is ScriptValue && !(scriptObject is StaticScriptValue))
            {
                col = Color.Red;
                italic = false;

                var strVal = (scriptObject as ScriptValue).GetStringValue();
                if (strVal == "minor_gold_value")
                {

                }
                if (strVal != "yes" && strVal != "no")
                {
                    if (strVal == "is_in_event_abduct_outcome_0001")
                    {

                    }
                    if (scriptObject.Parent != null && AssumedSchema != null)
                    {
                        var parentSchema = AssumedSchema;
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
                                    col = TestPropertyType(scriptObject, con.type, strVal, col, scopeConditionsAreValid, scopeEffectsAreValid);


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

                                    bool isInside =
                                        ScopeManager.Instance.isConditionScopeEndParamInside(scope, name,
                                            scriptObject.Parent);

                                    if (isInside && name.Contains("."))
                                    {
                                        var ns = ScopeManager.Instance.getConditionScopeInside(scope,
                                            name.Substring(0, name.LastIndexOf(".")), scriptObject.Parent);
                                        var condition = ScopeManager.Instance.GetCondition(ns,
                                            name.Substring(name.LastIndexOf(".") + 1));

                                        if (condition != null)
                                        {
                                            col = TestPropertyType(scriptObject, condition.type, strVal, col, scopeConditionsAreValid, scopeEffectsAreValid);

                                        }
                                    } else if (isInside)
                                    {
                                        var ns = ScopeManager.Instance.getConditionScopeInside(scope,
                                            name, scriptObject.Parent);
                                        var condition = ScopeManager.Instance.GetCondition(ns,
                                            name + 1);

                                        if (condition != null)
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

                        if (col == Color.Red && nameForType != null)
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
                  //      ColourName(strVal, doc, lineSeg, col, bold, italic);
                    }
                }
                else
                {
                    if (avoidRed)
                        col = FunctionColor;
            //        ColourName(strVal, doc, lineSeg, BaseCommandColor, bold, italic);

                }

            }

            foreach (var scriptObjectChild in scriptObject.Children)
            {
         //       DoScriptObject(scriptObjectChild, doc);
            }
        }

        private Color TestPropertyType(ScriptObject scriptObject, string t, string strVal, Color col,
            bool scopeConditionsAreValid, bool scopeEffectsAreValid)
        {
            bool requiresTest = true;
            ScopeType required = ScopeType.any;
            ScriptObject.ScopeFindType findType = ScriptObject.ScopeFindType.Object;
            switch (t)
            {
                case "num":
                    {
                        if (Core.Instance.GetNameSet(ScriptContext.ScriptValues, false).Contains(strVal))
                            col = ReferencedObjectColor;
                        else if (Core.Instance.LocalVarListFromObjectFile(scriptObject, ScriptObject.ScopeVarType.Number).Contains(strVal))
                            col = ReferencedObjectColor;


                    }
                    break;
                case "bool":
                    if (Core.Instance.LocalVarListFromObjectFile(scriptObject, ScriptObject.ScopeVarType.Bool).Contains(strVal))
                        col = ReferencedObjectColor;
                    break;
                case "scope":

                    break;
                case "list":
                    if (scriptObject.Topmost.scriptLists.ContainsKey(strVal))
                    {
                        col = ReferencedObjectColor;
                        requiresTest = false;
                    }
                    break;

                case "scopevar":
                    findType = ScriptObject.ScopeFindType.Value;
                    break;
                case "scopeany":
                    findType = ScriptObject.ScopeFindType.Any;
                    break;
                case "string":
                    {
                        if (Core.Instance.LocalVarListFromObjectFile(scriptObject, ScriptObject.ScopeVarType.String).Contains(strVal))
                            col = ReferencedObjectColor;
                        else
                        {
                            requiresTest = false;
                            col = StringColor;

                        }
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
                if (CheckIsValidScope(str, scriptObject, scopeConditionsAreValid, scopeEffectsAreValid, required, findType))
                {
                    col = ScopeColor;
                }
                if (!str.StartsWith("scope:"))
                    str = "scope:" + str;
                if (CheckIsValidScope(str, scriptObject, scopeConditionsAreValid, scopeEffectsAreValid, required, findType))
                {
                    col = ScopeColor;
                }
            }
            else if (t != null)
            {
                var list = EnumManager.Instance.GetEnums(t);

                if (list.Contains(strVal))
                {
                    col = ReferencedObjectColor;
                }
                else
                {
                    var r = Core.Instance.GetNameSetFromEnumType(t);

                    if (r.Contains(strVal))
                        col = ReferencedObjectColor;
                }
            }





            return col;
        }


        private bool CheckIsValidScope(string strVal, ScriptObject scriptObject, bool scopeConditionsAreValid, bool scopeEffectsAreValid, ScopeType typeRequired = ScopeType.any, ScriptObject.ScopeFindType findType = ScriptObject.ScopeFindType.Object)
        {
            if (strVal == "scope:target.location.county.holder.capital_county")
            {

            }
            //   if (scopeConditionsAreValid)
            {
                bool success;
                var res = ScopeManager.Instance.ChangeConditionScope(scriptObject.GetScopeType(), strVal, out success,
                    scriptObject, findType);

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
                    scriptObject, findType);

                if (success)
                {
                    if (typeRequired != ScopeType.any)
                        return res == typeRequired;

                    return true;
                }
            }

            return false;
        }


        private void ColorEntireObject(Color col, Color backgroundColor, IDocument doc, int f, int t)
        {
            for (int x = f; x < t; x++)
            {
                int xx = Math.Min(doc.LineSegmentCollection.Count - 1, x);
                ColorEntireLine(col, doc, backgroundColor, doc.LineSegmentCollection[xx]);
            }
        }

        private void ColorEntireLine(Color col, IDocument doc, Color backgroundColor, LineSegment line)
        {
            foreach (var lineWord in line.Words)
            {
                if (lineWord.SyntaxColor != null)
                    lineWord.SyntaxColor = new HighlightColor(col, backgroundColor, false, false);
            }
        }
        private void ColourName(string name, IDocument doc, LineSegment line, Color color, Color backgroundColor, bool bold, bool italic)
        {
            if (!line.ColorWord(name, color, bold, italic, backgroundColor))
            {

            }
        }
    }
}
