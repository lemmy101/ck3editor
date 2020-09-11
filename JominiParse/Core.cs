using System;
using System.Collections.Generic;
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
            LoadCK3Scripts(BaseCK3Library);
        }

        public void CreateMod(string mod)
        {
            ModCK3Library = new ScriptLibrary();
            ModCK3Library.Parent = BaseCK3Library;
            ModCK3Library.Name = mod;
        }
        public void LoadMod(string mod)
        {
            ModCK3Library = new ScriptLibrary();
            ModCK3Library.Parent = BaseCK3Library;
            ModCK3Library.Name = mod;
            ModCK3Library.Path = Globals.CK3ModPath + mod + "/";

            LoadingCK3Library = ModCK3Library;
            LoadCK3Scripts(ModCK3Library);
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
            if (dir.StartsWith("common/decisions"))
                return ScriptContext.Decisions;
            if (dir.StartsWith("common/activities"))
                return ScriptContext.Activities;
            if (dir.StartsWith("common/script_values"))
                return ScriptContext.ScriptValues;
            if (dir.StartsWith("common/scripted_modifiers"))
                return ScriptContext.ScriptedModifiers;
            if (dir.StartsWith("events"))
                return ScriptContext.Events;

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
        public bool LoadCK3File(string filename, bool forceBase=false)
        {
            bool fromBase = false;
            if (!ModCK3Library.FileMap.ContainsKey(filename))
                fromBase = true;

            fromBase = fromBase | forceBase;

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

            LoadingCK3Library = fromBase ? BaseCK3Library : ModCK3Library;
           
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

        public void LoadCK3Scripts(ScriptLibrary lib)
        {
            LoadingCK3Library = lib;

            // events...
            string startDir = lib.Path;//"D:/SteamLibrary/steamapps/common/Crusader Kings III/";

            LoadingCK3Library.LoadLocalizations(startDir + "localization/english");
  
            var results = FileTokenizer.Instance.LoadDirectory(startDir+"events", startDir, ScriptContext.Events);
            LoadingCK3Library.Add(results, ScriptContext.Events);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/decisions", startDir, ScriptContext.Decisions);
            LoadingCK3Library.Add(results, ScriptContext.Decisions);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/script_values", startDir, ScriptContext.ScriptValues);
            LoadingCK3Library.Add(results, ScriptContext.ScriptValues);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/activities", startDir, ScriptContext.Activities);
            LoadingCK3Library.Add(results, ScriptContext.Activities);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/bookmarks", startDir, ScriptContext.Bookmark);
            LoadingCK3Library.Add(results, ScriptContext.Bookmark);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/buildings", startDir, ScriptContext.Buildings);
            LoadingCK3Library.Add(results, ScriptContext.Buildings);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/casus_belli_types", startDir, ScriptContext.CasusBelliType);
            LoadingCK3Library.Add(results, ScriptContext.CasusBelliType);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/character_interactions", startDir, ScriptContext.CharacterInteractions);
            LoadingCK3Library.Add(results, ScriptContext.CharacterInteractions);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/council_positions", startDir, ScriptContext.CouncilPositions);
            LoadingCK3Library.Add(results, ScriptContext.CouncilPositions);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/council_tasks", startDir, ScriptContext.CouncilTasks);
            LoadingCK3Library.Add(results, ScriptContext.CouncilTasks);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/defines", startDir, ScriptContext.Defines);
            LoadingCK3Library.Add(results, ScriptContext.Defines);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/dynasty_legacies", startDir, ScriptContext.DynastyLegacies);
            LoadingCK3Library.Add(results, ScriptContext.DynastyLegacies);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/dynasty_perks", startDir, ScriptContext.DynastyPerks);
            LoadingCK3Library.Add(results, ScriptContext.DynastyPerks);
            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/event_backgrounds", startDir, ScriptContext.EventBackgrounds);
            LoadingCK3Library.Add(results, ScriptContext.EventBackgrounds);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/event_themes", startDir, ScriptContext.EventThemes);
            LoadingCK3Library.Add(results, ScriptContext.EventThemes);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/factions", startDir, ScriptContext.Factions);
            LoadingCK3Library.Add(results, ScriptContext.Factions);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/focuses", startDir, ScriptContext.Focuses);
            LoadingCK3Library.Add(results, ScriptContext.Focuses);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/game_rules", startDir, ScriptContext.GameRules);
            LoadingCK3Library.Add(results, ScriptContext.GameRules);
            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/governments", startDir, ScriptContext.Governments);
            LoadingCK3Library.Add(results, ScriptContext.Governments);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/holdings", startDir, ScriptContext.Holdings);
            LoadingCK3Library.Add(results, ScriptContext.Holdings);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/hook_types", startDir, ScriptContext.HookTypes);
            LoadingCK3Library.Add(results, ScriptContext.HookTypes);
            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/important_actions", startDir, ScriptContext.ImportantActions);
            LoadingCK3Library.Add(results, ScriptContext.ImportantActions);
            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/landed_titles", startDir, ScriptContext.LandedTitles);
            LoadingCK3Library.Add(results, ScriptContext.LandedTitles);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/laws", startDir, ScriptContext.Laws);
            LoadingCK3Library.Add(results, ScriptContext.Laws);
            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/lifestyle_perks", startDir, ScriptContext.LifestylePerks);
            LoadingCK3Library.Add(results, ScriptContext.LifestylePerks);
            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/lifestyles", startDir, ScriptContext.Lifestyles);
            LoadingCK3Library.Add(results, ScriptContext.Lifestyles);
            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/modifiers", startDir, ScriptContext.StaticModifiers);
            LoadingCK3Library.Add(results, ScriptContext.StaticModifiers);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/nicknames", startDir, ScriptContext.Nicknames);
            LoadingCK3Library.Add(results, ScriptContext.Nicknames);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/on_action", startDir, ScriptContext.OnActions);
            LoadingCK3Library.Add(results, ScriptContext.OnActions);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/opinion_modifiers", startDir, ScriptContext.OptionModifiers);
            LoadingCK3Library.Add(results, ScriptContext.OptionModifiers);


            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/religion/doctrines", startDir, ScriptContext.Doctrines);
            LoadingCK3Library.Add(results, ScriptContext.Doctrines);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/religion/fervor_modifiers", startDir, ScriptContext.FervorModifiers);
            LoadingCK3Library.Add(results, ScriptContext.FervorModifiers);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/religion/holy_sites", startDir, ScriptContext.HolySites);
            LoadingCK3Library.Add(results, ScriptContext.HolySites);
            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/religion/religion_families", startDir, ScriptContext.ReligionFamilys);
            LoadingCK3Library.Add(results, ScriptContext.ReligionFamilys);
            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/religion/religions", startDir, ScriptContext.Religions);
            LoadingCK3Library.Add(results, ScriptContext.Religions);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/religion/schemes", startDir, ScriptContext.Schemes);
            LoadingCK3Library.Add(results, ScriptContext.Schemes);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/scripted_character_templates", startDir, ScriptContext.ScriptedCharacterTemplates);
            LoadingCK3Library.Add(results, ScriptContext.ScriptedCharacterTemplates);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/scripted_effects", startDir, ScriptContext.ScriptedEffects);
            LoadingCK3Library.Add(results, ScriptContext.ScriptedEffects);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/scripted_lists", startDir, ScriptContext.ScriptedLists);
            LoadingCK3Library.Add(results, ScriptContext.ScriptedLists);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/scripted_modifiers", startDir, ScriptContext.ScriptedModifiers);
            LoadingCK3Library.Add(results, ScriptContext.ScriptedModifiers);
            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/scripted_relations", startDir, ScriptContext.ScriptedRelations);
            LoadingCK3Library.Add(results, ScriptContext.ScriptedRelations);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/scripted_rules", startDir, ScriptContext.ScriptedRules);
            LoadingCK3Library.Add(results, ScriptContext.ScriptedRules);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/scripted_triggers", startDir, ScriptContext.ScriptedTriggers);
            LoadingCK3Library.Add(results, ScriptContext.ScriptedTriggers);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/secret_types", startDir, ScriptContext.SecretTypes);
            LoadingCK3Library.Add(results, ScriptContext.SecretTypes);
            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/story_cycles", startDir, ScriptContext.StoryCycles);
            LoadingCK3Library.Add(results, ScriptContext.StoryCycles);
            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/succession_election", startDir, ScriptContext.SuccessionElections);
            LoadingCK3Library.Add(results, ScriptContext.SuccessionElections);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/traits", startDir, ScriptContext.Traits);
            LoadingCK3Library.Add(results, ScriptContext.Traits);

            results = FileTokenizer.Instance.LoadDirectory(startDir + "common/vassal_contracts", startDir, ScriptContext.VassalContracts);
            LoadingCK3Library.Add(results, ScriptContext.VassalContracts);
            

            LoadingCK3Library.ConnectEventNetwork();

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
            ModCK3Library.ConnectEventNetwork();

        }

        public HashSet<string> GetNameSet(bool modOnly, ScriptContext context)
        {
            HashSet<string> eventNames = new HashSet<string>();
            
            switch (context)
            {


                case ScriptContext.Focuses:
                    return GetFocusNameSet(modOnly);
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

        public HashSet<string> GetTraitNameSet(bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.TraitsMap.Keys);
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.TraitsMap.Keys);

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


        public ScriptObject Get(string name, ScriptContext context)
        {
            switch (context)
            {

                case ScriptContext.VassalContracts:
                    return ModCK3Library.GetVassalContract(name);
                    break;


                case ScriptContext.Traits:
                    return ModCK3Library.GetTrait(name);
                    break;

                case ScriptContext.SuccessionElections:
                    return ModCK3Library.GetSuccessionElection(name);
                    break;


                case ScriptContext.StoryCycles:
                    return ModCK3Library.GetStoryCycle(name);
                    break;


                case ScriptContext.SecretTypes:
                    return ModCK3Library.GetSecretType(name);
                    break;


                case ScriptContext.ScriptedTriggers:
                    return ModCK3Library.GetScriptedTrigger(name);
                    break;


                case ScriptContext.ScriptedRules:
                    return ModCK3Library.GetScriptedRule(name);
                    break;

                case ScriptContext.ScriptedRelations:
                    return ModCK3Library.GetScriptedRelation(name);
                    break;


                case ScriptContext.ScriptedLists:
                    return ModCK3Library.GetScriptedList(name);
                    break;


                case ScriptContext.ScriptedEffects:
                    return ModCK3Library.GetScriptedEffect(name);
                    break;



                case ScriptContext.ScriptedCharacterTemplates:
                    return ModCK3Library.GetScriptedCharacterTemplate(name);
                    break;


                case ScriptContext.Schemes:
                    return ModCK3Library.GetScheme(name);
                    break;

                case ScriptContext.Religions:
                    return ModCK3Library.GetReligion(name);
                    break;


                case ScriptContext.ReligionFamilys:
                    return ModCK3Library.GetReligionFamily(name);
                    break;

                case ScriptContext.HolySites:
                    return ModCK3Library.GetHolySite(name);
                    break;


                case ScriptContext.FervorModifiers:
                    return ModCK3Library.GetFervorModifier(name);
                    break;


                case ScriptContext.Doctrines:
                    return ModCK3Library.GetDoctrine(name);
                    break;


                case ScriptContext.OptionModifiers:
                    return ModCK3Library.GetOptionModifier(name);
                    break;


                case ScriptContext.OnActions:
                    return ModCK3Library.GetOnAction(name);
                    break;


                case ScriptContext.Nicknames:
                    return ModCK3Library.GetNickname(name);
                    break;


                case ScriptContext.StaticModifiers:
                    return ModCK3Library.GetStaticModifier(name);
                    break;


                case ScriptContext.ScriptedModifiers:
                    return ModCK3Library.GetScriptedModifier(name);
                    break;

                case ScriptContext.Lifestyles:
                    return ModCK3Library.GetLifestyle(name);
                    break;

                case ScriptContext.LifestylePerks:
                    return ModCK3Library.GetLifestylePerk(name);
                    break;


                case ScriptContext.Laws:
                    return ModCK3Library.GetLaw(name);
                    break;


                case ScriptContext.LandedTitles:
                    return ModCK3Library.GetLandedTitle(name);
                    break;


                case ScriptContext.ImportantActions:
                    return ModCK3Library.GetImportantAction(name);
                    break;

                case ScriptContext.HookTypes:
                    return ModCK3Library.GetHookType(name);
                    break;


                case ScriptContext.Holdings:
                    return ModCK3Library.GetHolding(name);
                    break;


                case ScriptContext.Governments:
                    return ModCK3Library.GetGovernment(name);
                    break;

                case ScriptContext.GameRules:
                    return ModCK3Library.GetGameRule(name);
                    break;

                case ScriptContext.Focuses:
                    return ModCK3Library.GetFocus(name);
                    break;


                case ScriptContext.Factions:
                    return ModCK3Library.GetFaction(name);
                    break;


                case ScriptContext.EventThemes:
                    return ModCK3Library.GetEventTheme(name);
                    break;


                case ScriptContext.EventBackgrounds:
                    return ModCK3Library.GetEventBackground(name);
                    break;


                case ScriptContext.DynastyPerks:
                    return ModCK3Library.GetDynastyPerk(name);
                    break;


                case ScriptContext.DynastyLegacies:
                    return ModCK3Library.GetDynastyLegacy(name);
                    break;


                case ScriptContext.CouncilTasks:
                    return ModCK3Library.GetCouncilTask(name);
                    break;

                case ScriptContext.Defines:
                    return ModCK3Library.GetDefine(name);
                    break;

                case ScriptContext.CouncilPositions:
                    return ModCK3Library.GetCouncilPosition(name);
                    break;
                case ScriptContext.CharacterInteractions:
                    return ModCK3Library.GetCharacterInteraction(name);
                    break;
                case ScriptContext.CasusBelliType:
                    return ModCK3Library.GetCasusBelliType(name);
                    break;
                case ScriptContext.Bookmark:
                    return ModCK3Library.GetBookmark(name);
                    break;
                case ScriptContext.Buildings:
                    return ModCK3Library.GetBuilding(name);
                    break;
                case ScriptContext.Events:
                    return ModCK3Library.GetEvent(name);
                    break;
                case ScriptContext.Decisions:
                    return ModCK3Library.GetDecision(name);
                    break;
                case ScriptContext.Activities:
                    return ModCK3Library.GetActivity(name);
                    break;
                case ScriptContext.ScriptValues:
                    return ModCK3Library.GetScriptValue(name);
                    break;
            }

            return null;
        }

        public ScriptEvent GetEvent(string name)
        {
            return ModCK3Library.GetEvent(name);
        }
    }
}
