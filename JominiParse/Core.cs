using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JominiParse
{

    public class Core
    {
        public static Core Instance = new Core();

        public ScriptLibrary BaseCK3Library = new ScriptLibrary();
        public ScriptLibrary ModCK3Library = new ScriptLibrary();
        public ScriptLibrary LoadingCK3Library = null;

        public void Init()
        {
            BaseCK3Library = new ScriptLibrary();
            ModCK3Library = new ScriptLibrary();
            LoadingCK3Library = BaseCK3Library;
            BaseCK3Library.Path = Globals.CK3Path;
            EnumManager.Instance.Load();
            LoadCK3Scripts(BaseCK3Library);
            
        }

        public void CreateOrLoadMod(string mod)
        {
            if (Directory.Exists(Globals.CK3ModPath + mod + "/"))
            {
                LoadMod(mod);
                return;
            }

            Directory.CreateDirectory(Globals.CK3ModPath + mod + "/");
            ModCK3Library = new ScriptLibrary();
            ModCK3Library.Parent = BaseCK3Library;
            ModCK3Library.Name = mod;

            PostInitialize();

        }
        public void LoadMod(string mod)
        {
            ModCK3Library = new ScriptLibrary();
            ModCK3Library.Parent = BaseCK3Library;
            ModCK3Library.Name = mod;
            ModCK3Library.Path = Globals.CK3ModPath + mod + "/";

            LoadingCK3Library = ModCK3Library;
            LoadCK3Scripts(ModCK3Library);

            PostInitialize();
        }

        private void LoadModFiles(string mod)
        {
            
        }

        public string GetLocalizedText(string tag)
        {
            return ModCK3Library.GetLocalizedText(tag);
        }

        ScriptContext GetContextFromDirectory(string dir)
        {
            var res = ContextData.Where(a => dir.StartsWith(a.Value.Directory)).ToList();

            if(res.Any())

            {
                return res[0].Key;
            }

            return ScriptContext.Events;
        }

        public void LoadCK3BaseFile(string filename)
        {
            if (BaseCK3Library.FileMap.ContainsKey(filename))
                return;

            LoadingCK3Library = BaseCK3Library;
            string directory = filename.Substring(0, filename.LastIndexOf("/"));

            ScriptContext context = GetContextFromDirectory(directory);
            string startDir = Globals.CK3Path;//"D:/SteamLibrary/steamapps/common/Crusader Kings III/";

            var results = FileTokenizer.Instance.LoadFile(startDir + filename,startDir, context);

            BaseCK3Library.Add(results, context);

            BaseCK3Library.ConnectEventNetwork();
        }
        public bool LoadCK3File(string filename, bool forceBase=false, bool forceReload = false)
        {
            bool fromBase = false;
            if (!ModCK3Library.FileMap.ContainsKey(filename))
                fromBase = true;

            fromBase = fromBase | forceBase;

            LoadingCK3Library = fromBase ? BaseCK3Library : ModCK3Library;
            if (!forceReload)
            {
                if (fromBase)
                {
                    if (BaseCK3Library.FileMap.ContainsKey(filename))
                        return fromBase;
                }
                else
                {
                    if (ModCK3Library.FileMap.ContainsKey(filename))
                        return fromBase;
                }

            }
            else
            {
                LoadingCK3Library.ClearFile(filename);
            }

           
            string directory = filename.Substring(0, filename.LastIndexOf("/"));

            ScriptContext context = GetContextFromDirectory(directory);
            string startDir = LoadingCK3Library.Path;//"D:/SteamLibrary/steamapps/common/Crusader Kings III/";

            var results = FileTokenizer.Instance.LoadFile(startDir + filename, startDir, context);

            LoadingCK3Library.Add(results, context);

            LoadingCK3Library.ConnectEventNetwork();

            return fromBase;
        }

        public ScriptFile GetFile(string file, bool isBaseFile)
        {
            if (isBaseFile)
                return BaseCK3Library.GetFile(file);

            return ModCK3Library.GetFile(file);
        }
        public ScriptFile GetFile(string file)
        {
            return ModCK3Library.GetFile(file);
        }
       
        public class ContextInfo
        {
            public string Directory;
            public string Dictionary;
//            public delegate HashSet<string> GetNameSet(bool modOnly);
        }

        private Dictionary<ScriptContext, ContextInfo> ContextData = new Dictionary<ScriptContext, ContextInfo>()
        {
            { ScriptContext.Events, new ContextInfo() {Directory = "events"}},
            { ScriptContext.Decisions, new ContextInfo() {Directory = "common/decisions"}},
            { ScriptContext.ScriptValues, new ContextInfo() {Directory = "common/script_values"}},
            { ScriptContext.Activities, new ContextInfo() {Directory = "common/activities"}},
            { ScriptContext.Bookmark, new ContextInfo() {Directory = "common/bookmarks"}},
            { ScriptContext.Buildings, new ContextInfo() {Directory = "common/buildings"}},
            { ScriptContext.CasusBelliType, new ContextInfo() {Directory = "common/casus_belli_types"}},
            { ScriptContext.CharacterInteractions, new ContextInfo() {Directory = "common/character_interactions"}},
            { ScriptContext.Characters, new ContextInfo() {Directory = "history/characters"}},
            { ScriptContext.CouncilPositions, new ContextInfo() {Directory = "common/council_positions"}},
            { ScriptContext.CouncilTasks, new ContextInfo() {Directory = "common/council_tasks"}},
            { ScriptContext.Defines, new ContextInfo() {Directory = "common/Defines"}},
            { ScriptContext.DynastyLegacies, new ContextInfo() {Directory = "common/dynasty_legacies"}},
            { ScriptContext.DynastyPerks, new ContextInfo() {Directory = "common/dynasty_perks"}},
            { ScriptContext.EventBackgrounds, new ContextInfo() {Directory = "common/event_backgrounds"}},
            { ScriptContext.EventThemes, new ContextInfo() {Directory = "common/event_themes"}},
            { ScriptContext.Factions, new ContextInfo() {Directory = "common/factions"}},
            { ScriptContext.Focuses, new ContextInfo() {Directory = "common/focuses"}},
            { ScriptContext.GameRules, new ContextInfo() {Directory = "common/game_rules"}},
            { ScriptContext.Governments, new ContextInfo() {Directory = "common/governments"}},
            { ScriptContext.Holdings, new ContextInfo() {Directory = "common/holdings"}},
            { ScriptContext.HookTypes, new ContextInfo() {Directory = "common/hook_types"}},
            { ScriptContext.ImportantActions, new ContextInfo() {Directory = "common/important_actions"}},
            { ScriptContext.LandedTitles, new ContextInfo() {Directory = "common/landed_titles"}},

            { ScriptContext.Laws, new ContextInfo() {Directory = "common/laws"}},
            { ScriptContext.LifestylePerks, new ContextInfo() {Directory = "common/lifestyle_perks"}},
            { ScriptContext.Lifestyles, new ContextInfo() {Directory = "common/lifestyles"}},
            { ScriptContext.StaticModifiers, new ContextInfo() {Directory = "common/modifiers"}},
            { ScriptContext.Nicknames, new ContextInfo() {Directory = "common/nicknames"}},
            { ScriptContext.OnActions, new ContextInfo() {Directory = "common/on_action"}},
            { ScriptContext.OptionModifiers, new ContextInfo() {Directory = "common/opinion_modifiers"}},
            { ScriptContext.Doctrines, new ContextInfo() {Directory = "common/religion/doctrines"}},
            { ScriptContext.FervorModifiers, new ContextInfo() {Directory = "common/religion/fervor_modifiers"}},

            { ScriptContext.HolySites, new ContextInfo() {Directory = "common/religion/holy_sites"}},
            { ScriptContext.ReligionFamilys, new ContextInfo() {Directory = "common/religion/religion_families"}},
            { ScriptContext.Religions, new ContextInfo() {Directory = "common/religion/religions"}},

            { ScriptContext.Schemes, new ContextInfo() {Directory = "common/schemes"}},
            { ScriptContext.ScriptedCharacterTemplates, new ContextInfo() {Directory = "common/scripted_character_templates"}},
            { ScriptContext.ScriptedEffects, new ContextInfo() {Directory = "common/scripted_effects"}},
            { ScriptContext.ScriptedLists, new ContextInfo() {Directory = "common/scripted_lists"}},
            { ScriptContext.ScriptedModifiers, new ContextInfo() {Directory = "common/scripted_modifiers"}},
            { ScriptContext.ScriptedRelations, new ContextInfo() {Directory = "common/scripted_relations"}},
            { ScriptContext.ScriptedRules, new ContextInfo() {Directory = "common/scripted_rules"}},
            { ScriptContext.ScriptedTriggers, new ContextInfo() {Directory = "common/scripted_triggers"}},
            { ScriptContext.SecretTypes, new ContextInfo() {Directory = "common/secret_types"}},
            { ScriptContext.StoryCycles, new ContextInfo() {Directory = "common/story_cycles"}},
            { ScriptContext.SuccessionElections, new ContextInfo() {Directory = "common/succession_election"}},
            { ScriptContext.Traits, new ContextInfo() {Directory = "common/traits"}},
            { ScriptContext.VassalContracts, new ContextInfo() {Directory = "common/vassal_contracts"}},
   
        };

        public void PostInitialize()
        {

            ModCK3Library.ConnectEventNetwork();

            foreach (var scriptObject in ScriptObject.DeferedInitializationList)
            {
                scriptObject.PostInitialize();
            }

            ScriptObject.DeferedInitializationList.Clear();
        }
        public void LoadCK3Scripts(ScriptLibrary lib)
        {
            LoadingCK3Library = lib;

            // events...
            string startDir = lib.Path;//"D:/SteamLibrary/steamapps/common/Crusader Kings III/";

            LoadingCK3Library.LoadLocalizations(startDir + "localization/english");

            for(int x=0;x<(int)ScriptContext.Max;x++)
            {
                if(ContextData.ContainsKey((ScriptContext)x))
                {
                    ContextInfo info = ContextData[(ScriptContext)x];
                    var r = FileTokenizer.Instance.LoadDirectory(startDir + info.Directory + "/", startDir, (ScriptContext)x);
                    LoadingCK3Library.Add(r, (ScriptContext)x);
                }

            }
            foreach (var scriptObject in ScriptObject.DeferedInitializationList)
            {
                scriptObject.Initialize();
            }
        

        }


        public void UpdateFile(string filename, string text)
        {
            string startDir = ModCK3Library.Path;//"D:/SteamLibrary/steamapps/common/Crusader Kings III/";
            
            LoadingCK3Library = ModCK3Library;

            var c = GetContextFromDirectory(filename.Substring(filename.LastIndexOf("/")));

            try
            {
                var results = FileTokenizer.Instance.LoadText(text, startDir + filename, startDir, c);

                ModCK3Library.Add(results, c);
            }
            catch (Exception e)
            {
            }
            foreach (var scriptObject in ScriptObject.DeferedInitializationList)
            {
                scriptObject.Initialize();
            }

            ModCK3Library.ConnectEventNetwork();
           
            foreach (var scriptObject in ScriptObject.DeferedInitializationList)
            {
                scriptObject.PostInitialize();
            }
            ScriptObject.DeferedInitializationList.Clear();

        }

        public HashSet<string> GetNameSet(bool modOnly, ScriptContext context)
        {
            HashSet<string> eventNames = new HashSet<string>();
            
            switch (context)
            {


                case ScriptContext.Focuses:
                    return GetFocusNameSet(modOnly);
                    break;
                case ScriptContext.Characters:
                    return GetCharacterNameSet(modOnly);
                    break;

                case ScriptContext.DynastyPerks:
                    return GetDynastyPerkNameSet(modOnly);
                    break;
                case ScriptContext.DynastyLegacies:
                    return GetDynastyLegacyNameSet(modOnly);
                    break;

                case ScriptContext.Defines:
                    return GetDefineNameSet(modOnly);
                    break;
                case ScriptContext.CouncilTasks:
                    return GetCouncilTaskNameSet(modOnly);
                    break;
                case ScriptContext.CouncilPositions:
                    return GetCouncilPositionNameSet(modOnly);
                    break;
                case ScriptContext.CharacterInteractions:
                    return GetCharacterInteractionNameSet(modOnly);
                    break;
                case ScriptContext.CasusBelliType:
                    return GetCasusBelliTypeNameSet(modOnly);
                    break;
                case ScriptContext.Bookmark:
                    return GetBookmarkNameSet(modOnly);
                    break;
                case ScriptContext.Events:
                    return GetEventNameSet(modOnly);
                    break;
                case ScriptContext.Decisions:
                    return GetDecisionNameSet(modOnly);
                    break;
                case ScriptContext.ScriptValues:
                    return GetScriptValueNameSet(modOnly);
                    break;
                case ScriptContext.Activities:
                    return GetActivityNameSet(modOnly);
                    break;
                case ScriptContext.Buildings:
                    return GetBuildingNameSet(modOnly);
                    break;

                case ScriptContext.EventBackgrounds:
                    return GetEventBackgroundNameSet(modOnly);
                    break;

                case ScriptContext.EventThemes:
                    return GetEventThemeNameSet(modOnly);
                    break;

                case ScriptContext.Factions:
                    return GetFactionNameSet(modOnly);
                    break;

                case ScriptContext.GameRules:
                    return GetGameRuleNameSet(modOnly);
                    break;

                case ScriptContext.Governments:
                    return GetGovernmentNameSet(modOnly);
                    break;
                case ScriptContext.Holdings:
                    return GetHoldingNameSet(modOnly);
                    break;

                case ScriptContext.HookTypes:
                    return GetHookTypeNameSet(modOnly);
                    break;

                case ScriptContext.ImportantActions:
                    return GetImportantActionNameSet(modOnly);
                    break;

                case ScriptContext.LandedTitles:
                    return GetLandedTitleNameSet(modOnly);
                    break;

                case ScriptContext.Laws:
                    return GetLawNameSet(modOnly);
                    break;

                case ScriptContext.LifestylePerks:
                    return GetLifestylePerkNameSet(modOnly);
                    break;

                case ScriptContext.Lifestyles:
                    return GetLifestyleNameSet(modOnly);
                    break;

                case ScriptContext.ScriptedModifiers:
                    return GetScriptedModifierNameSet(modOnly);
                    break;

                case ScriptContext.StaticModifiers:
                    return GetStaticModifierNameSet(modOnly);
                    break;

                case ScriptContext.Nicknames:
                    return GetNicknameNameSet(modOnly);
                    break;

                case ScriptContext.OnActions:
                    return GetOnActionNameSet(modOnly);
                    break;

                case ScriptContext.OptionModifiers:
                    return GetOptionModifierNameSet(modOnly);
                    break;

                case ScriptContext.Doctrines:
                    return GetDoctrineNameSet(modOnly);
                    break;

                case ScriptContext.FervorModifiers:
                    return GetFervorModifierNameSet(modOnly);
                    break;

                case ScriptContext.HolySites:
                    return GetHolySiteNameSet(modOnly);
                    break;

                case ScriptContext.ReligionFamilys:
                    return GetReligionFamilyNameSet(modOnly);
                    break;

                case ScriptContext.Religions:
                    return GetReligionNameSet(modOnly);
                    break;

                case ScriptContext.Schemes:
                    return GetSchemeNameSet(modOnly);
                    break;

                case ScriptContext.ScriptedCharacterTemplates:
                    return GetScriptedCharacterTemplateNameSet(modOnly);
                    break;

                case ScriptContext.ScriptedEffects:
                    return GetScriptedEffectNameSet(modOnly);
                    break;

                case ScriptContext.ScriptedLists:
                    return GetScriptedListNameSet(modOnly);
                    break;

                case ScriptContext.ScriptedRelations:
                    return GetScriptedRelationNameSet(modOnly);
                    break;

                case ScriptContext.ScriptedRules:
                    return GetScriptedRuleNameSet(modOnly);
                    break;

                case ScriptContext.ScriptedTriggers:
                    return GetScriptedTriggerNameSet(modOnly);
                    break;

                case ScriptContext.SecretTypes:
                    return GetSecretTypeNameSet(modOnly);
                    break;

                case ScriptContext.StoryCycles:
                    return GetStoryCycleNameSet(modOnly);
                    break;

                case ScriptContext.SuccessionElections:
                    return GetSuccessionElectionNameSet(modOnly);
                    break;

                case ScriptContext.Traits:
                    return GetTraitNameSet(modOnly);
                    break;

                case ScriptContext.VassalContracts:
                    return GetVassalContractNameSet(modOnly);
                    break;
            }

            return eventNames;
        }

        #region Get Objects

        public HashSet<string> GetVassalContractNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.VassalContractsMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.VassalContractsMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetCharacterNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.CharactersMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.CharactersMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetTraitNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.TraitsMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.TraitsMap.Keys);

            foreach (var v in ModCK3Library.TraitsMap.Values)
            {
                var r = v.Children.Where(a => a.Name == "group");
                if (r.Any())
                    eventNames.Add(r.First().GetStringValue());
            }

            foreach (var v in BaseCK3Library.TraitsMap.Values)
            {
                var r = v.Children.Where(a => a.Name == "group");
                if (r.Any())
                    eventNames.Add(r.First().GetStringValue());
            }

            return eventNames;
        }
        public HashSet<string> GetSuccessionElectionNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.SuccessionElectionsMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.SuccessionElectionsMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetScriptedTriggerNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.ScriptedTriggersMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.ScriptedTriggersMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetStoryCycleNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.StoryCyclesMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.StoryCyclesMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetSecretTypeNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.SecretTypesMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.SecretTypesMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetScriptedRelationNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.ScriptedRelationsMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.ScriptedRelationsMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetScriptedRuleNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.ScriptedRulesMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.ScriptedRulesMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetScriptedListNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.ScriptedListsMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.ScriptedListsMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetScriptedEffectNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.ScriptedEffectsMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.ScriptedEffectsMap.Keys);

            return eventNames;
        }


        public HashSet<string> GetScriptedCharacterTemplateNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.ScriptedCharacterTemplatesMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.ScriptedCharacterTemplatesMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetSchemeNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.SchemesMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.SchemesMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetReligionNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.ReligionsMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.ReligionsMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetReligionFamilyNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.ReligionFamilysMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.ReligionFamilysMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetHolySiteNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.HolySitesMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.HolySitesMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetFervorModifierNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.FervorModifiersMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.FervorModifiersMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetDoctrineNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.DoctrinesMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.DoctrinesMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetOptionModifierNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.OptionModifiersMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.OptionModifiersMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetOnActionNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.OnActionsMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.OnActionsMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetNicknameNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.NicknamesMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.NicknamesMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetStaticModifierNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.StaticModifiersMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.StaticModifiersMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetModifierNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.StaticModifiersMap.Keys);
            eventNames.UnionWith(ModCK3Library.ScriptedModifiersMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.StaticModifiersMap.Keys);
            eventNames.UnionWith(BaseCK3Library.ScriptedModifiersMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetScriptedModifierNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.ScriptedModifiersMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.ScriptedModifiersMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetLifestyleNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.LifestylesMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.LifestylesMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetLifestylePerkNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.LifestylePerksMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.LifestylePerksMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetLawNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.LawsMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.LawsMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetImportantActionNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.ImportantActionsMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.ImportantActionsMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetLandedTitleNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.LandedTitleMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.LandedTitleMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetHookTypeNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.HookTypesMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.HookTypesMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetFactionNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.FactionsMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.FactionsMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetDynastyLegacyNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.DynastyLegaciesMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.DynastyLegaciesMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetDefineNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.DefinesMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.DefinesMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetCouncilTaskNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.CouncilTasksMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.CouncilTasksMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetCouncilPositionNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.CouncilPositionsMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.CouncilPositionsMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetBuildingNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.BuildingMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.BuildingMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetActivityNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.ActivityMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.ActivityMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetScriptValueNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.ScriptValueMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.ScriptValueMap.Keys);

            return eventNames;
        }
        
        public HashSet<string> GetCasusBelliTypeNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.CasusBelliTypeMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.CasusBelliTypeMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetBookmarkNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.BookmarkMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.BookmarkMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetEventNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.EventMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.EventMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetDecisionNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.DecisionMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.DecisionMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetCharacterInteractionNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.CharacterInteractionMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.CharacterInteractionMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetDynastyPerkNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.DynastyPerksMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.DynastyPerksMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetEventBackgroundNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.EventBackgroundsMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.EventBackgroundsMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetEventThemeNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.EventThemesMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.EventThemesMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetFocusNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.FocusMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.FocusMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetGameRuleNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.GameRulesMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.GameRulesMap.Keys);

            return eventNames;
        }
        public HashSet<string> GetGovernmentNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.GovernmentsMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.GovernmentsMap.Keys);

            return eventNames;
        }

        public HashSet<string> GetHoldingNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.HoldingsMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.HoldingsMap.Keys);

            return eventNames;
        }

        #endregion


        public ScriptObject Get(string name, ScriptContext context, bool forceBase=false)
        {
            var lib = ModCK3Library;

            if (forceBase)
                lib = BaseCK3Library;

            switch (context)
            {

                case ScriptContext.VassalContracts:
                    return lib.GetVassalContract(name);
                    break;


                case ScriptContext.Characters:
                    return lib.GetCharacter(name);
                    break;


                case ScriptContext.Traits:
                    return lib.GetTrait(name);
                    break;

                case ScriptContext.SuccessionElections:
                    return lib.GetSuccessionElection(name);
                    break;


                case ScriptContext.StoryCycles:
                    return lib.GetStoryCycle(name);
                    break;


                case ScriptContext.SecretTypes:
                    return lib.GetSecretType(name);
                    break;


                case ScriptContext.ScriptedTriggers:
                    return lib.GetScriptedTrigger(name);
                    break;


                case ScriptContext.ScriptedRules:
                    return lib.GetScriptedRule(name);
                    break;

                case ScriptContext.ScriptedRelations:
                    return lib.GetScriptedRelation(name);
                    break;


                case ScriptContext.ScriptedLists:
                    return lib.GetScriptedList(name);
                    break;


                case ScriptContext.ScriptedEffects:
                    return lib.GetScriptedEffect(name);
                    break;



                case ScriptContext.ScriptedCharacterTemplates:
                    return lib.GetScriptedCharacterTemplate(name);
                    break;


                case ScriptContext.Schemes:
                    return lib.GetScheme(name);
                    break;

                case ScriptContext.Religions:
                    return lib.GetReligion(name);
                    break;


                case ScriptContext.ReligionFamilys:
                    return lib.GetReligionFamily(name);
                    break;

                case ScriptContext.HolySites:
                    return lib.GetHolySite(name);
                    break;


                case ScriptContext.FervorModifiers:
                    return lib.GetFervorModifier(name);
                    break;


                case ScriptContext.Doctrines:
                    return lib.GetDoctrine(name);
                    break;


                case ScriptContext.OptionModifiers:
                    return lib.GetOptionModifier(name);
                    break;


                case ScriptContext.OnActions:
                    return lib.GetOnAction(name);
                    break;


                case ScriptContext.Nicknames:
                    return lib.GetNickname(name);
                    break;


                case ScriptContext.StaticModifiers:
                    return lib.GetStaticModifier(name);
                    break;


                case ScriptContext.ScriptedModifiers:
                    return lib.GetScriptedModifier(name);
                    break;

                case ScriptContext.Lifestyles:
                    return lib.GetLifestyle(name);
                    break;

                case ScriptContext.LifestylePerks:
                    return lib.GetLifestylePerk(name);
                    break;


                case ScriptContext.Laws:
                    return lib.GetLaw(name);
                    break;


                case ScriptContext.LandedTitles:
                    return lib.GetLandedTitle(name);
                    break;


                case ScriptContext.ImportantActions:
                    return lib.GetImportantAction(name);
                    break;

                case ScriptContext.HookTypes:
                    return lib.GetHookType(name);
                    break;


                case ScriptContext.Holdings:
                    return lib.GetHolding(name);
                    break;


                case ScriptContext.Governments:
                    return lib.GetGovernment(name);
                    break;

                case ScriptContext.GameRules:
                    return lib.GetGameRule(name);
                    break;

                case ScriptContext.Focuses:
                    return lib.GetFocus(name);
                    break;


                case ScriptContext.Factions:
                    return lib.GetFaction(name);
                    break;


                case ScriptContext.EventThemes:
                    return lib.GetEventTheme(name);
                    break;


                case ScriptContext.EventBackgrounds:
                    return lib.GetEventBackground(name);
                    break;


                case ScriptContext.DynastyPerks:
                    return lib.GetDynastyPerk(name);
                    break;


                case ScriptContext.DynastyLegacies:
                    return lib.GetDynastyLegacy(name);
                    break;


                case ScriptContext.CouncilTasks:
                    return lib.GetCouncilTask(name);
                    break;

                case ScriptContext.Defines:
                    return lib.GetDefine(name);
                    break;

                case ScriptContext.CouncilPositions:
                    return lib.GetCouncilPosition(name);
                    break;
                case ScriptContext.CharacterInteractions:
                    return lib.GetCharacterInteraction(name);
                    break;
                case ScriptContext.CasusBelliType:
                    return lib.GetCasusBelliType(name);
                    break;
                case ScriptContext.Bookmark:
                    return lib.GetBookmark(name);
                    break;
                case ScriptContext.Buildings:
                    return lib.GetBuilding(name);
                    break;
                case ScriptContext.Events:
                    return lib.GetEvent(name);
                    break;
                case ScriptContext.Decisions:
                    return lib.GetDecision(name);
                    break;
                case ScriptContext.Activities:
                    return lib.GetActivity(name);
                    break;
                case ScriptContext.ScriptValues:
                    return lib.GetScriptValue(name);
                    break;
            }

            return null;
        }

        public ScriptObject GetEvent(string name)
        {
            return ModCK3Library.GetEvent(name);
        }

        public string GetDirectoryFromContext(ScriptContext context)
        {
            return ContextData[context].Directory;
        }

        public HashSet<string> GetNameSetFromEnumType(string type)
        {
            if (type == "scripted_trigger")
                return GetScriptedTriggerNameSet(false);
            if (type == "opinion")
                return GetOptionModifierNameSet(false);
            if (type == "hook")
                return GetHookTypeNameSet(false);
            if (type == "on_action")
                return GetOnActionNameSet(false);
            if (type == "modifier")
                return GetStaticModifierNameSet(false);
            if (type == "event_theme")
                return GetEventThemeNameSet(false);
            if (type == "trait")
                return GetTraitNameSet(false);
            if (type == "decision")
                return GetDecisionNameSet(false);
            if (type == "event")
                return GetEventNameSet(false);
  //          if (type == "tooltip")
//                return GetEventNameSet(false);

            return new HashSet<string>();
        }
    }
}
