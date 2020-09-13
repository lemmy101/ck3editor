using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace JominiParse
{
    public class ScriptFile
    {
        public string Filename { get; set; }
        public bool IsBase { get; set; }
        public bool Overridden { get; set; }
        public ScriptContext Context { get; set; }
        public string Namespace { get; set; }

        public Dictionary<string, ScriptObject> Map = new Dictionary<string, ScriptObject>();
    }
    public class EventConnection
    {
        public ScriptObject FromCommand { get; set; }
        public ScriptObject From { get; set; }
        public ScriptObject To { get; set; }
    }

    public class ScriptLibrary
    {
        public Dictionary<string, ScriptObject> AllTypeMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> CasusBelliTypeMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> BookmarkMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> BuildingMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> CharacterInteractionMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> CharactersMap = new Dictionary<string, ScriptObject>();
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
        public Dictionary<string, ScriptObject> DecisionMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptValue> ScriptValueMap = new Dictionary<string, ScriptValue>();
        public Dictionary<string, ScriptObject> EventMap = new Dictionary<string, ScriptObject>();
        public Dictionary<string, ScriptObject> ActivityMap = new Dictionary<string, ScriptObject>();

        public Dictionary<string, ScriptFile> FileMap = new Dictionary<string, ScriptFile>();
     
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

        private ScriptObject Get(string name)
        {
            if (!AllTypeMap.ContainsKey(name))
                return null;

            return AllTypeMap[name];
        }


        public ScriptObject GetCharacter(string str)
        {
            if (!CharactersMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetCharacter(str);

                return null;
            }

            return CharactersMap[str];
        }
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

        public ScriptObject GetEvent(string str)
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
        public ScriptObject GetActivity(string str)
        {
            if (!ActivityMap.ContainsKey(str))
            {
                if (Parent != null)
                    return Parent.GetActivity(str);

                return null;
            }

            return ActivityMap[str];
        }
        public ScriptObject GetDecision(string str)
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


        public void Remove(string key, ScriptContext valueContext)
        {
      
            AllTypeMap.Remove(key);
            CasusBelliTypeMap.Remove(key); //new Dicti
            BookmarkMap.Remove(key); //new Dictionary<
            BuildingMap.Remove(key); //new Dictionary<
            CharacterInteractionMap.Remove(key); //new
            CharactersMap.Remove(key); //new Dictionar
            CouncilPositionsMap.Remove(key); //new Dic
            CouncilTasksMap.Remove(key); //new Diction
            DefinesMap.Remove(key); //new Dictionary<s
            DynastyLegaciesMap.Remove(key); //new Dict
            DynastyPerksMap.Remove(key); //new Diction
            EventBackgroundsMap.Remove(key); //new Dic
            EventThemesMap.Remove(key); //new Dictiona
            FactionsMap.Remove(key); //new Dictionary<
            FocusMap.Remove(key); //new Dictionary<str
            GameRulesMap.Remove(key); //new Dictionary
            GovernmentsMap.Remove(key); //new Dictiona
            HoldingsMap.Remove(key); //new Dictionary<
            HookTypesMap.Remove(key); //new Dictionary
            ImportantActionsMap.Remove(key); //new Dic
            LandedTitleMap.Remove(key); //new Dictiona
            LawsMap.Remove(key); //new Dictionary<stri
            LifestylePerksMap.Remove(key); //new Dicti
            LifestylesMap.Remove(key); //new Dictionar
            ScriptedModifiersMap.Remove(key); //new Di
            StaticModifiersMap.Remove(key); //new Dict
            NicknamesMap.Remove(key); //new Dictionary
            OnActionsMap.Remove(key); //new Dictionary
            OptionModifiersMap.Remove(key); //new Dict

            DoctrinesMap.Remove(key); //new Dictionary
            FervorModifiersMap.Remove(key); //new Dict
            HolySitesMap.Remove(key); //new Dictionary
            ReligionFamilysMap.Remove(key); //new Dict
            ReligionsMap.Remove(key); //new Dictionary

            SchemesMap.Remove(key); //new Dictionary<s
            ScriptedCharacterTemplatesMap.Remove(key);
            ScriptedEffectsMap.Remove(key); //new Dict
            ScriptedListsMap.Remove(key); //new Dictio
            ScriptedRelationsMap.Remove(key); //new Di
            ScriptedRulesMap.Remove(key); //new Dictio
            ScriptedTriggersMap.Remove(key); //new Dic
            SecretTypesMap.Remove(key); //new Dictiona
            StoryCyclesMap.Remove(key); //new Dictiona
            SuccessionElectionsMap.Remove(key); //new
            TraitsMap.Remove(key); //new Dictionary<st

            VassalContractsMap.Remove(key); //new Dict
            DecisionMap.Remove(key); //new Dictionar
            ScriptValueMap.Remove(key); //new Dictionar
            EventMap.Remove(key); //new Dictionary<stri
            ActivityMap.Remove(key); //new Dictionar
        }

        public void Add(List<ScriptObject> objects, ScriptContext context)
        {

            foreach (var dec in objects)
            {
                ScriptFile f = null;
                {
                    if (FileMap.ContainsKey(dec.Filename))
                        f = FileMap[dec.Filename];
                    else
                    {
                        f = new ScriptFile();
                        f.IsBase = this == Core.Instance.BaseCK3Library;
                        f.Filename = dec.Filename;
                        f.Context = context;
                        FileMap[dec.Filename] = f;
                    }

                    dec.Context = context;
                    dec.ScriptFile = f;
                    f.Map[dec.Name] = dec;
                }
            }
        
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
                            DoFile(dec, context);
                        }

                    }
                }
                    break;
                case ScriptContext.Characters:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            CharactersMap[dec.Name] = dec;
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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
                            DoFile(dec, context);
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

                        DoFile(dec, context);
                    }

                }
                    break;
                case ScriptContext.CasusBelliType:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject;

                        CasusBelliTypeMap[dec.Name] = dec;

                        DoFile(dec, context);
                    }

                }
                    break;
                case ScriptContext.Bookmark:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject;

                        BookmarkMap[dec.Name] = dec;

                        DoFile(dec, context);
                    }

                }
                    break;
                case ScriptContext.Decisions:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;

                        DecisionMap[dec.Name] = dec;
                        DoFile(dec, context);


                    }
                }
                break;
                case ScriptContext.ScriptValues:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptValue;
                        if (dec != null)
                        {
                            ScriptValueMap[dec.Name] = dec;
                            DoFile(dec, context);
                        }

                    }
                }
                    break;
                case ScriptContext.Events:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            if(dec.Name.StartsWith("scripted_trigger "))
                            {
                                ScriptedTriggersMap[dec.Name.Substring(dec.Name.IndexOf(" ")+1)] = dec;
                            }
                            else
                            {
                                EventMap[dec.Name] = dec;
                            }
                            DoFile(dec, context);

                        }

                    }
                }
                    break;
                case ScriptContext.Activities:
                {
                    foreach (var scriptObject in objects)
                    {
                        var dec = scriptObject as ScriptObject;
                        if (dec != null)
                        {
                            ActivityMap[dec.Name] = dec;
                            DoFile(dec, context);
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
                    if (scriptObject.Library == Core.Instance.ModCK3Library)
                    {
                        var res = Parent.Get(scriptObject.Name);
                        if (res != null)
                            res.Overridden = true;
                    }

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
            if (Localization == null)
                return tag;

            if (!Localization.ContainsKey(tag))
            {
                if (Parent != null)
                    return Parent.GetLocalizedText(tag);

                return tag;
            }

            return Localization[tag].english;
        }

        public void RegisterRandomOnActionTrigger(ScriptObject node)
        {
            if (node.Children.Count > 0)
            {
                foreach (var scriptObject in node.Children)
                {
                    int result;
                    if (Int32.TryParse(scriptObject.Name, out result))
                    {
                        var e = scriptObject.GetStringValue();
                        // found a random event...
                        if (GetOnAction(e) != null)
                        {
                            trigger_event n = new trigger_event();

                            n.on_action = e;
                            n.Function = node;
                            n.Topmost = node.Topmost;
                            unprocessedTriggers.Add(n);
                        }
                    }
                }

            }
        }
        public void RegisteFirstValidOnActionTrigger(ScriptObject node)
        {
            if (node.Children.Count > 0)
            {
                foreach (var scriptObject in node.Children)
                {
                    int result;
                   
                    {
                        var e = scriptObject.Name;
                        // found a random event...
                        if (GetOnAction(e) != null)
                        {
                            trigger_event n = new trigger_event();

                            n.on_action = e;
                            n.Function = node;
                            n.Topmost = node.Topmost;
                            unprocessedTriggers.Add(n);
                        }
                    }
                }

            }
        }

        public void RegisterRandomEventsTrigger(ScriptObject node)
        {
            if (node.Children.Count > 0)
            {
                foreach (var scriptObject in node.Children)
                {
                    int result;
                    if (Int32.TryParse(scriptObject.Name, out result))
                    {
                        var e = scriptObject.GetStringValue();
                        // found a random event...
                        if (GetEvent(e) != null)
                        {
                            trigger_event n = new trigger_event();
                            if (e == "abduct_outcome.1001")
                            {

                            }
                            n.id = e;
                            n.Function = node;
                            n.Topmost = node.Topmost;
                            unprocessedTriggers.Add(n);
                        }
                    }
                }

            }
        }

        public void RegisterFirstValidEventsTrigger(ScriptObject node)
        {
            if (node.Children.Count > 0)
            {
                foreach (var scriptObject in node.Children)
                {
                    int result;
                 
                    {
                        var e = scriptObject.Name;
                        // found a random event...
                        if (GetEvent(e) != null)
                        {
                            trigger_event n = new trigger_event();
                           
                            n.id = e;
                            n.Function = node;
                            n.Topmost = node.Topmost;
                            unprocessedTriggers.Add(n);
                        }
                    }
                }

            }
        }

        public void RegisterTrigger(ScriptObject node)
        {
            if (node.Topmost.Name == "abduct_outcome.0001")
            {

            }
               
            if (node.Children.Count > 0)
            {
                var idNodes = node.Children.Where(a => a.Name == "id");
                var dayNodes = node.Children.Where(a => a.Name == "days");
                var onActionNodes = node.Children.Where(a => a.Name == "on_action");
                trigger_event n = new trigger_event();
                
                if (dayNodes.Any())
                {
                    string val = dayNodes.First().GetStringValue();
                    if (val == null)
                    {
                        var c = dayNodes.First().Children;
                        n.days_from = c[0].Name;
                        n.days_to  =c[1].Name;
                    }
                    else
                    {
                        n.days_from = val;
                        n.days_to = val;

                    }
                }
                string on_action = null;
                string id = null;

                if (idNodes.Any())
                {
                    id = idNodes.First().GetStringValue();
                }
                if (onActionNodes.Any())
                {
                    on_action = onActionNodes.First().GetStringValue();
                }

                n.on_action = on_action;
                n.Topmost = node.Topmost;
                n.Function = node;
                n.id = id;
                unprocessedTriggers.Add(n);

            }
            else
            {
                trigger_event n = new trigger_event();

                n.Topmost = node.Topmost;
                n.Function = node;
                n.days_from = null;
                n.days_to = null;
                n.id = node.GetStringValue();
                unprocessedTriggers.Add(n);
            }
        }

        public void ConnectEventNetwork()
        {
            foreach (var triggerEvent in unprocessedTriggers)
            {
                var fromTop = triggerEvent.Topmost;
                var from = triggerEvent;
                if (triggerEvent.id != null)
                {
                    if (!EventMap.ContainsKey(triggerEvent.id))
                        continue;

                    var to = EventMap[triggerEvent.id];
                //    fromTop.Connections.RemoveAll(a => a.To.Filename == to.Filename);
                }
                else
                {
                    if (!OnActionsMap.ContainsKey(triggerEvent.on_action))
                        continue;

                    var to = OnActionsMap[triggerEvent.on_action];
               //     fromTop.Connections.RemoveAll(a => a.To.Filename == to.Filename);

                }
            }

            for (var index = 0; index < unprocessedTriggers.Count; index++)
            {
                var triggerEvent = unprocessedTriggers[index];
                var fromTop = triggerEvent.Topmost;
                var from = triggerEvent;

                if (index == 1313)
                {

                }

                if (triggerEvent.id == "abduct_outcome.1001")
                {
                }

                if (triggerEvent.id != null)
                {
                    var to = GetEvent(triggerEvent.id);

                    if (to == null)
                        continue;

                //    fromTop.Connections.RemoveAll(a => a.To.Filename == to.Filename);

                    EventConnection c = new EventConnection();
                    c.From = fromTop;
                    c.FromCommand = @from.Function;
                    c.To = to;
                    c.To.AddEventConnection(c);
                    c.From.AddEventConnection(c);
                }
                else
                {
                    var to = GetOnAction(triggerEvent.on_action);

                    if (to == null)
                        continue;

                 //   fromTop.Connections.RemoveAll(a => a.To.Filename == to.Filename);

                    EventConnection c = new EventConnection();
                    c.From = fromTop;
                    c.FromCommand = @from.Function;
                    c.To = to;
                    c.To.AddEventConnection(c);
                    c.From.AddEventConnection(c);
                }
            }

            unprocessedTriggers.Clear();
        }
        private void DoFile(ScriptObject dec, ScriptContext context)
        {
            AllTypeMap[dec.Name] = dec;

            ScriptFile f;
            if (FileMap.ContainsKey(dec.Filename))
                f = FileMap[dec.Filename];
            else
            {
                f = new ScriptFile();
                f.IsBase = this == Core.Instance.BaseCK3Library;
                f.Filename = dec.Filename;
                f.Context = context;
                FileMap[dec.Filename] = f;
            }

            dec.Context = context;
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

        public ScriptFile EnsureFile(string filename, ScriptContext context)
        {
            if(FileMap.ContainsKey(filename))
            {
                return FileMap[filename];
            }

            ScriptFile file = new ScriptFile();
            file.IsBase = this == Core.Instance.BaseCK3Library;
            file.Filename = filename;
            file.Context = context;
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

            return file;
        }

        public void SaveBinary(string filename)
        {

            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                writer.Write(ScriptObject.TypeMap.Count);
                foreach (var typeMapValue in ScriptObject.TypeMap.Values)
                {
                    writer.Write(typeMapValue.GetHashCode());
                    writer.Write(typeMapValue.AssemblyQualifiedName);
                }

                writer.Write(FileMap.Count);
                foreach (var file in FileMap.Values)
                {
                    writer.Write(file.Filename);
                    writer.Write(file.Map.Count);
                    writer.Write((int)file.Context);
                    foreach (var scriptObject in file.Map.Values)
                    {
                        scriptObject.Write(writer);
                    }
                }
            }
        }

        public void LoadBinary(string fn)
        {
            using (BinaryReader reader = new BinaryReader(File.Open(fn, FileMode.Open)))
            {
                int typeCount = reader.ReadInt32();

                for(int x=0;x<typeCount;x++)
                {
                    int hash = reader.ReadInt32();
                    string name = reader.ReadString();
                    Type t = Type.GetType(name);
                    ScriptObject.TypeMap[hash] = t;
                }

                int nFiles = reader.ReadInt32();

                for(int x=0;x<nFiles;x++)
                {
                    string filename = reader.ReadString();
                    int nObjects = reader.ReadInt32();
                    ScriptFile sf = new ScriptFile();
                    sf.Filename = filename;
                    sf.IsBase = true;
                    
                    sf.Context = (ScriptContext) reader.ReadInt32();
                    for (int m = 0; m < nObjects; m++)
                    {
                        ScriptObject o = ScriptObject.LoadFromData(reader, null);
                        o.Read(reader, sf, null);
                        sf.Map[o.Name] = o;
                    }

                    FileMap[sf.Filename] = sf;

                    var l = sf.Map.Values.ToList().GroupBy(a => a.Context).ToList();
                    foreach (var pair in l)
                    {
                        Add(pair.ToList(), pair.Key);
                    }
                }
            }
        }

        public bool Has(ScriptObject inside)
        {
            if (AllTypeMap.ContainsKey(inside.Name))
            {
                return true;
            }

            return false;
        }

        public List<string> GetDirectoryListFromContext(ScriptContext context, string requiredNamespace)
        {
            List<string> results = new List<string>();
            foreach (var keyValuePair in FileMap)
            {
                if (keyValuePair.Value.Context == context)
                {
                    if (String.IsNullOrWhiteSpace(requiredNamespace) || (keyValuePair.Value.Namespace == requiredNamespace))
                    {
                        results.Add(keyValuePair.Value.Filename);
                    }
                }
            }

            return results;
        }

        public void ClearFile(string filename)
        {
            if(FileMap.ContainsKey(filename))
            {
                var file = FileMap[filename];

                foreach (var keyValuePair in file.Map)
                {
                    Remove(keyValuePair.Key, keyValuePair.Value.Context);
                }
            }
        }

    }
}
