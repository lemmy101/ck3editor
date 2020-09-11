using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JominiParse.EffectCommands;

namespace JominiParse
{
    public class ScriptFile
    {
        public string Filename { get; set; }
        public bool IsBase { get; set; }
        public bool Overridden { get; set; }

        public Dictionary<string, ScriptObject> Map = new Dictionary<string, ScriptObject>();
    }
    public class EventConnection
    {
        public ScriptObject FromCommand { get; set; }
        public ScriptObject From { get; set; }
        public ScriptEvent To { get; set; }
    }
    public class ScriptLibrary
    {
        
        public Dictionary<string, ScriptObject> CasusBelliTypeMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> BookmarkMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> BuildingMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> CharacterInteractionMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> CouncilPositionsMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> CouncilTasksMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> DefinesMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> DynastyLegaciesMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> DynastyPerksMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> EventBackgroundsMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> EventThemesMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> FactionsMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> FocusMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> GameRulesMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> GovernmentsMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> HoldingsMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> HookTypesMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> ImportantActionsMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> LandedTitleMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> LawsMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> LifestylePerksMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> LifestylesMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> ScriptedModifiersMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> StaticModifiersMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> NicknamesMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> OnActionsMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> OptionModifiersMap = new Dictionary<string, ScriptObject>();

        public Dictionary<string, ScriptObject> DoctrinesMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> FervorModifiersMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> HolySitesMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> ReligionFamilysMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> ReligionsMap = new Dictionary<string, ScriptObject>();

        public Dictionary<string, ScriptObject> SchemesMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> ScriptedCharacterTemplatesMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> ScriptedEffectsMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> ScriptedListsMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> ScriptedRelationsMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> ScriptedRulesMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> ScriptedTriggersMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> SecretTypesMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> StoryCyclesMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> SuccessionElectionsMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> TraitsMap = new Dictionary<string, ScriptObject>();

        public Dictionary<string, ScriptObject> VassalContractsMap = new Dictionary<string, ScriptObject>();
        

        public Dictionary<string, ScriptFile> FileMap = new Dictionary<string, ScriptFile>();
        public Dictionary<string, ScriptDecision> DecisionMap = new Dictionary<string, ScriptDecision>();
        public Dictionary<string, ScriptValue> ScriptValueMap = new Dictionary<string, ScriptValue>();
        public Dictionary<string, ScriptEvent> EventMap = new Dictionary<string, ScriptEvent>();
        public Dictionary<string, ScriptActivity> ActivityMap = new Dictionary<string, ScriptActivity>();

        List<trigger_event> unprocessedTriggers = new List<trigger_event>();

        public void LoadLocalizations(string dir)
        {
            var results = LocalizationParser.Instance.Load(dir);

            this.Localization = results;
        }

        public Dictionary<string, LocalizationEntry> Localization { get; set; }
        public ScriptLibrary Parent { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        #region Get Objects

        public ScriptObject GetVassalContract(string str)
        {
            if (!VassalContractsMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetVassalContract(str);

                return null;
            }

            return VassalContractsMap[str];
        }
        public ScriptObject GetSuccessionElection(string str)
        {
            if (!SuccessionElectionsMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetSuccessionElection(str);

                return null;
            }

            return SuccessionElectionsMap[str];
        }

        public ScriptObject GetTrait(string str)
        {
            if (!TraitsMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetTrait(str);

                return null;
            }

            return TraitsMap[str];
        }

        public ScriptObject GetScriptedTrigger(string str)
        {
            if (!ScriptedTriggersMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetScriptedTrigger(str);

                return null;
            }

            return ScriptedTriggersMap[str];
        }

        public ScriptObject GetSecretType(string str)
        {
            if (!SecretTypesMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetSecretType(str);

                return null;
            }

            return SecretTypesMap[str];
        }


        public ScriptObject GetStoryCycle(string str)
        {
            if (!StoryCyclesMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetStoryCycle(str);

                return null;
            }

            return StoryCyclesMap[str];
        }

        public ScriptObject GetScriptedList(string str)
        {
            if (!ScriptedListsMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetScriptedList(str);

                return null;
            }

            return ScriptedListsMap[str];
        }

        public ScriptObject GetScriptedRelation(string str)
        {
            if (!ScriptedRelationsMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetScriptedRelation(str);

                return null;
            }

            return ScriptedRelationsMap[str];
        }

        public ScriptObject GetScriptedRule(string str)
        {
            if (!ScriptedRulesMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetScriptedRule(str);

                return null;
            }

            return ScriptedRulesMap[str];
        }
        public ScriptObject GetScriptedCharacterTemplate(string str)
        {
            if (!ScriptedCharacterTemplatesMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetScriptedCharacterTemplate(str);

                return null;
            }

            return ScriptedCharacterTemplatesMap[str];
        }

        public ScriptObject GetScriptedEffect(string str)
        {
            if (!ScriptedEffectsMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetScriptedEffect(str);

                return null;
            }

            return ScriptedEffectsMap[str];
        }

        public ScriptObject GetReligionFamily(string str)
        {
            if (!ReligionFamilysMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetReligionFamily(str);

                return null;
            }

            return ReligionFamilysMap[str];
        }

        public ScriptObject GetReligion(string str)
        {
            if (!ReligionsMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetReligion(str);

                return null;
            }

            return ReligionsMap[str];
        }

        public ScriptObject GetScheme(string str)
        {
            if (!SchemesMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetScheme(str);

                return null;
            }

            return SchemesMap[str];
        }
        public ScriptObject GetDoctrine(string str)
        {
            if (!DoctrinesMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetDoctrine(str);

                return null;
            }

            return DoctrinesMap[str];
        }

        public ScriptObject GetFervorModifier(string str)
        {
            if (!FervorModifiersMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetFervorModifier(str);

                return null;
            }

            return FervorModifiersMap[str];
        }

        public ScriptObject GetHolySite(string str)
        {
            if (!HolySitesMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetHolySite(str);

                return null;
            }

            return HolySitesMap[str];
        }

        public ScriptObject GetNickname(string str)
        {
            if (!NicknamesMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetNickname(str);

                return null;
            }

            return NicknamesMap[str];
        }

        public ScriptObject GetOnAction(string str)
        {
            if (!OnActionsMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetOnAction(str);

                return null;
            }

            return OnActionsMap[str];
        }

        public ScriptObject GetOptionModifier(string str)
        {
            if (!OptionModifiersMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetOptionModifier(str);

                return null;
            }

            return OptionModifiersMap[str];
        }
        public ScriptObject GetLifestyle(string str)
        {
            if (!LifestylesMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetLifestyle(str);

                return null;
            }

            return LifestylesMap[str];
        }

        public ScriptObject GetScriptedModifier(string str)
        {
            if (!ScriptedModifiersMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetScriptedModifier(str);

                return null;
            }

            return ScriptedModifiersMap[str];
        }
        public ScriptObject GetStaticModifier(string str)
        {
            if (!StaticModifiersMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetStaticModifier(str);

                return null;
            }

            return StaticModifiersMap[str];
        }
        public ScriptObject GetLaw(string str)
        {
            if (!LawsMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetLaw(str);

                return null;
            }

            return LawsMap[str];
        }

        public ScriptObject GetLifestylePerk(string str)
        {
            if (!LifestylePerksMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetLifestylePerk(str);

                return null;
            }

            return LifestylePerksMap[str];
        }
        public ScriptObject GetImportantAction(string str)
        {
            if (!ImportantActionsMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetImportantAction(str);

                return null;
            }

            return ImportantActionsMap[str];
        }

        public ScriptObject GetLandedTitle(string str)
        {
            if (!LandedTitleMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetLandedTitle(str);

                return null;
            }

            return LandedTitleMap[str];
        }
        public ScriptObject GetDefine(string str)
        {
            if (!DefinesMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetDefine(str);

                return null;
            }

            return DefinesMap[str];
        }

        public ScriptEvent GetEvent(string str)
        {
            if (!EventMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetEvent(str);

                return null;
            }

            return EventMap[str];
        }


        public ScriptObject GetCouncilTask(string str)
        {
            if (!CouncilTasksMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetCouncilTask(str);

                return null;
            }

            return CouncilTasksMap[str];
        }
        public ScriptValue GetScriptValue(string str)
        {
            if (!ScriptValueMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetScriptValue(str);

                return null;
            }

            return ScriptValueMap[str];
        }

        public ScriptObject GetCharacterInteraction(string str)
        {
            if (!CharacterInteractionMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetCharacterInteraction(str);

                return null;
            }

            return CharacterInteractionMap[str];
        }

        public ScriptObject GetBuilding(string str)
        {
            if (!BuildingMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetBuilding(str);

                return null;
            }

            return BuildingMap[str];
        }
        
        public ScriptObject GetCasusBelliType(string str)
        {
            if (!CasusBelliTypeMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetCasusBelliType(str);

                return null;
            }

            return CasusBelliTypeMap[str];
        }
        public ScriptObject GetBookmark(string str)
        {
            if (!BookmarkMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetBookmark(str);

                return null;
            }

            return BookmarkMap[str];
        }
        public ScriptActivity GetActivity(string str)
        {
            if (!ActivityMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetActivity(str);

                return null;
            }

            return ActivityMap[str];
        }
        public ScriptDecision GetDecision(string str)
        {
            if (!DecisionMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetDecision(str);

                return null;
            }

            return DecisionMap[str];
        }

        public ScriptObject GetCouncilPosition(string str)
        {
            if (!CouncilPositionsMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetCouncilPosition(str);

                return null;
            }

            return CouncilPositionsMap[str];
        }
        public ScriptObject GetDynastyLegacy(string str)
        {
            if (!DynastyLegaciesMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetDynastyLegacy(str);

                return null;
            }

            return DynastyLegaciesMap[str];
        }
        public ScriptObject GetDynastyPerk(string str)
        {
            if (!DynastyPerksMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetDynastyPerk(str);

                return null;
            }

            return DynastyPerksMap[str];
        }
        public ScriptObject GetEventBackground(string str)
        {
            if (!EventBackgroundsMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetEventBackground(str);

                return null;
            }

            return EventBackgroundsMap[str];
        }

        public ScriptObject GetEventTheme(string str)
        {
            if (!EventThemesMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetEventTheme(str);

                return null;
            }

            return EventThemesMap[str];
        }

        public ScriptObject GetFaction(string str)
        {
            if (!FactionsMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetFaction(str);

                return null;
            }

            return FactionsMap[str];
        }

        public ScriptObject GetFocus(string str)
        {
            if (!FocusMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetFocus(str);

                return null;
            }

            return FocusMap[str];
        }

        public ScriptObject GetGameRule(string str)
        {
            if (!GameRulesMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetGameRule(str);

                return null;
            }

            return GameRulesMap[str];
        }

        public ScriptObject GetGovernment(string str)
        {
            if (!GovernmentsMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetGovernment(str);

                return null;
            }

            return GovernmentsMap[str];
        }

        public ScriptObject GetHolding(string str)
        {
            if (!HoldingsMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetHolding(str);

                return null;
            }

            return HoldingsMap[str];
        }

        public ScriptObject GetHookType(string str)
        {
            if (!HookTypesMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetHookType(str);

                return null;
            }

            return HookTypesMap[str];
        }

        #endregion


        public void Add(List<ScriptObject> objects, ScriptContext context)
        {
            switch (context)
            {

                case ScriptContext.VassalContracts:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            VassalContractsMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;


                case ScriptContext.Traits:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            TraitsMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.SuccessionElections:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            SuccessionElectionsMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.StoryCycles:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            StoryCyclesMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.SecretTypes:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            SecretTypesMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.ScriptedTriggers:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            ScriptedTriggersMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.ScriptedRules:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            ScriptedRulesMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.ScriptedRelations:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            ScriptedRelationsMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.ScriptedLists:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            ScriptedListsMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.ScriptedEffects:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            ScriptedEffectsMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;


                case ScriptContext.ScriptedCharacterTemplates:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            ScriptedCharacterTemplatesMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.Schemes:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            SchemesMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.Religions:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            ReligionsMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.ReligionFamilys:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            ReligionFamilysMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.HolySites:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            HolySitesMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.FervorModifiers:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            FervorModifiersMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;


                case ScriptContext.Doctrines:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            DoctrinesMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.OptionModifiers:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            OptionModifiersMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.OnActions:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            OnActionsMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;


                case ScriptContext.Nicknames:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            NicknamesMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.StaticModifiers:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            StaticModifiersMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.ScriptedModifiers:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            ScriptedModifiersMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.Lifestyles:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            LifestylesMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.LifestylePerks:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            LifestylePerksMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.Laws:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            LawsMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.LandedTitles:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            LandedTitleMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.ImportantActions:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            ImportantActionsMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.HookTypes:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            HookTypesMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.Holdings:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            HoldingsMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.Governments:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            GovernmentsMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.GameRules:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            GameRulesMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.Focuses:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            FocusMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.Factions:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            FactionsMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.EventThemes:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            EventThemesMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.EventBackgrounds:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            EventBackgroundsMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.DynastyPerks:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            DynastyPerksMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;


                case ScriptContext.DynastyLegacies:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            DynastyLegaciesMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.Defines:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            DefinesMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.CouncilTasks:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            CouncilTasksMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;
                case ScriptContext.CouncilPositions:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            CouncilPositionsMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.CharacterInteractions:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            CharacterInteractionMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                    }
                }
                    break;

                case ScriptContext.Buildings:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject;

                        BuildingMap[dec.Name] = dec;

                        DoFile(dec);
                    }

                }
                    break;
                case ScriptContext.CasusBelliType:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject;

                        CasusBelliTypeMap[dec.Name] = dec;

                        DoFile(dec);
                    }

                }
                    break;
                case ScriptContext.Bookmark:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject;

                        BookmarkMap[dec.Name] = dec;

                        DoFile(dec);
                    }

                }
                    break;
                case ScriptContext.Decisions:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptDecision;

                        DecisionMap[dec.Name] = dec;
                        DoFile(dec);


                    }
                }
                break;
                case ScriptContext.ScriptValues:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptValue;

                        ScriptValueMap[dec.Name] = dec;
                        DoFile(dec);


                    }
                }
                    break;
                case ScriptContext.Events:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptEvent;
                        if (dec != null)
                        {
                            EventMap[dec.Name] = dec;
                            DoFile(dec);

                        }

                    }
                }
                    break;
                case ScriptContext.Activities:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptActivity;
                        if (dec != null)
                        {
                            ActivityMap[dec.Name] = dec;
                            DoFile(dec);
                        }

                        }
                }
                    break;
            }
            // if file exists in base scripts, all entires are overridden
            if (Parent != null)
            {
                foreach (var scriptObject in objects)
                {
                    if (!Parent.FileMap.ContainsKey(scriptObject.Filename))
                        continue;

                    var f = Parent.FileMap[scriptObject.Filename];

                    foreach (var mapValue in f.Map.Values)
                    {
                        mapValue.Overridden = true;
                    }
                }
            }
         
        }

        public string GetLocalizedText(string tag)
        {
            if (!Localization.ContainsKey(tag))
            {
                if (Parent != null)
                    return Parent.GetLocalizedText(tag);

                return tag;
            }

            return Localization[tag].english;
        }

        public void RegisterTrigger(trigger_event triggerEvent)
        {
            unprocessedTriggers.Add(triggerEvent);
        }

        public void ConnectEventNetwork()
        {
            foreach (var triggerEvent in unprocessedTriggers)
            {
                var fromTop = triggerEvent.Topmost;
                var from = triggerEvent;
                if (!EventMap.ContainsKey(triggerEvent.id.GetStringValue()))
                    continue;

                var to = EventMap[triggerEvent.id.GetStringValue()];

                fromTop.Connections.RemoveAll(a => a.To.Filename == to.Filename);
            }

            foreach (var triggerEvent in unprocessedTriggers)
            {
                var fromTop = triggerEvent.Topmost;
                var from = triggerEvent;

                var to = GetEvent(triggerEvent.id.GetStringValue());

                if (to == null)
                    continue;

                fromTop.Connections.RemoveAll(a => a.To.Filename == to.Filename);

                EventConnection c = new EventConnection();
                c.From = fromTop;
                c.FromCommand = from;
                c.To = to;
                c.To.AddEventConnection(c);
                c.From.AddEventConnection(c);
            }

            unprocessedTriggers.Clear();
        }
        private void DoFile(ScriptObject dec)
        {
            ScriptFile f;
            if (FileMap.ContainsKey(dec.Filename))
                f = FileMap[dec.Filename];
            else
            {
                f = new ScriptFile();
                f.IsBase = this == Core.Instance.BaseCK3Library;
                f.Filename = dec.Filename;
                FileMap[dec.Filename] = f;
            }

            dec.ScriptFile = f;
            f.Map[dec.Name] = dec;
        }

        public ScriptFile GetFile(string file)
        {
            if (FileMap.ContainsKey(file))
                return FileMap[file];

            if (Parent != null)
                return Parent.GetFile(file);

            return null;
        }

        public void EnsureFile(string filename)
        {
            if(FileMap.ContainsKey(filename))
            {
                return;
            }

            ScriptFile file = new ScriptFile();
            file.IsBase = this == Core.Instance.BaseCK3Library;
            file.Filename = filename;
            FileMap[filename] = file;

            if (!file.IsBase && Parent != null && Parent.FileMap.ContainsKey(filename))
            {
                var f = Parent.FileMap[filename];
                f.Overridden = true;
                foreach (var mapValue in f.Map.Values)
                {
                    mapValue.Overridden = true;
                }
            }
        }

    }
}
