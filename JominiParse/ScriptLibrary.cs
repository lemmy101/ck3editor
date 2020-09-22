#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace JominiParse
{
    public class ScriptLibrary
    {
        public Dictionary<string, ScriptObject> AllTypeMap = new Dictionary<string, ScriptObject>();

        public Dictionary<ScriptContext, ContextInfo> ContextData = new Dictionary<ScriptContext, ContextInfo>
        {
            {ScriptContext.Event, new ContextInfo {Directory = "events", Type = "event", Category = "Events"}},
            {
                ScriptContext.EventBackgrounds,
                new ContextInfo {Directory = "common/event_backgrounds", Type = "event_background", Category = "Events"}
            },
            {
                ScriptContext.EventThemes,
                new ContextInfo {Directory = "common/event_themes", Type = "event_theme", Category = "Events"}
            },

            {
                ScriptContext.CultureGroups,
                new ContextInfo {Directory = "common/culture/cultures", Type = "culture_group", Category = "Culture"}
            },
            {
                ScriptContext.Cultures,
                new ContextInfo
                {
                    ChildOf = ScriptContext.CultureGroups, Type = "culture",
                    IgnoreChildren = "graphical_cultures,mercenary_names", Category = "Culture"
                }
            },
            {
                ScriptContext.CulturalInnovations,
                new ContextInfo {Directory = "common/culture/innovations", Type = "innovation", Category = "Culture"}
            },
            {
                ScriptContext.CultureEra,
                new ContextInfo {Directory = "common/culture/eras", Type = "culture_era", Category = "Culture"}
            },

            {ScriptContext.Decisions, new ContextInfo {Directory = "common/decisions", Type = "decision"}},
            {ScriptContext.Activities, new ContextInfo {Directory = "common/activities", Type = "activity"}},
            {ScriptContext.Bookmark, new ContextInfo {Directory = "common/bookmarks", Type = "bookmark"}},
            {ScriptContext.Buildings, new ContextInfo {Directory = "common/buildings", Type = "building"}},

            {
                ScriptContext.CasusBelliGroups,
                new ContextInfo
                    {Directory = "common/casus_belli_groups", Type = "casus_belli_group", Category = "Casus Belli"}
            },
            {
                ScriptContext.CasusBelliType,
                new ContextInfo
                    {Directory = "common/casus_belli_types", Type = "casus_belli_type", Category = "Casus Belli"}
            },
            {
                ScriptContext.CharacterInteractionCategory,
                new ContextInfo
                {
                    Directory = "common/character_interaction_categories", Type = "character_interaction_category",
                    Category = "Character Interactions"
                }
            },

            {
                ScriptContext.CharacterInteractions,
                new ContextInfo
                {
                    Directory = "common/character_interactions", Type = "character_interaction",
                    Category = "Character Interactions"
                }
            },
            {
                ScriptContext.Characters,
                new ContextInfo {Directory = "history/characters", Type = "character", Prepend = "character"}
            },
            {
                ScriptContext.CouncilPositions,
                new ContextInfo
                    {Directory = "common/council_positions", Type = "council_position", Category = "Council"}
            },
            {
                ScriptContext.CouncilTasks,
                new ContextInfo {Directory = "common/council_tasks", Type = "council_task", Category = "Council"}
            },
            {ScriptContext.Defines, new ContextInfo {Directory = "common/defines", Type = "define"}},
            {
                ScriptContext.DynastyLegacies,
                new ContextInfo {Directory = "common/dynasty_legacies", Type = "legacy", Category = "Dynasty"}
            },
            {
                ScriptContext.DynastyPerks,
                new ContextInfo {Directory = "common/dynasty_perks", Type = "dynasty_perk", Category = "Dynasty"}
            },
            {
                ScriptContext.DynastyHouses,
                new ContextInfo {Directory = "common/dynasty_houses", Type = "dynasty_house", Category = "Dynasty"}
            },
            {ScriptContext.Factions, new ContextInfo {Directory = "common/factions", Type = "faction_type"}},
            {
                ScriptContext.GameRuleCategories,
                new ContextInfo {Directory = "common/game_rules", Type = "game_rule_category"}
            },
            {ScriptContext.GameRules, new ContextInfo {ChildOf = ScriptContext.GameRuleCategories, Type = "game_rule"}},
            {ScriptContext.Governments, new ContextInfo {Directory = "common/governments", Type = "government"}},
            {ScriptContext.Holdings, new ContextInfo {Directory = "common/holdings", Type = "holding"}},
            {ScriptContext.HookTypes, new ContextInfo {Directory = "common/hook_types", Type = "hook"}},
            {
                ScriptContext.ImportantActions,
                new ContextInfo {Directory = "common/important_actions", Type = "important_action"}
            },
            {
                ScriptContext.LandedTitles,
                new ContextInfo
                {
                    Directory = "common/landed_titles", Type = "title", Prepend = "title", Recurse = true,
                    IgnoreChildren = "color,color2,ai_primary_priority,male_names,cultural_names"
                }
            },

            {ScriptContext.LawGroup, new ContextInfo {Directory = "common/laws", Type = "law_group"}},
            {ScriptContext.Laws, new ContextInfo {ChildOf = ScriptContext.LawGroup, Type = "law"}},
            {
                ScriptContext.LifestylePerks,
                new ContextInfo {Directory = "common/lifestyle_perks", Type = "perk", Category = "Lifestyles"}
            },
            {
                ScriptContext.Focuses,
                new ContextInfo {Directory = "common/focuses", Type = "focus", Category = "Lifestyles"}
            },
            {
                ScriptContext.Lifestyle,
                new ContextInfo {Directory = "common/lifestyles", Type = "lifestyle", Category = "Lifestyles"}
            },
            {ScriptContext.StaticModifiers, new ContextInfo {Directory = "common/modifiers", Type = "modifier"}},
            {ScriptContext.Nicknames, new ContextInfo {Directory = "common/nicknames", Type = "nickname"}},
            {ScriptContext.OnActions, new ContextInfo {Directory = "common/on_action", Type = "on_action"}},
            {
                ScriptContext.OptionModifiers,
                new ContextInfo {Directory = "common/opinion_modifiers", Type = "opinion_modifier_base"}
            },
            {
                ScriptContext.DoctrineGroups,
                new ContextInfo
                    {Directory = "common/religion/doctrines", Type = "doctrine_group", Category = "Religion"}
            },
            {
                ScriptContext.Doctrines,
                new ContextInfo {ChildOf = ScriptContext.DoctrineGroups, Type = "doctrine", Category = "Religion"}
            },
            {
                ScriptContext.FervorModifiers,
                new ContextInfo
                    {Directory = "common/religion/fervor_modifiers", Type = "fervor_modifier", Category = "Religion"}
            },

            {
                ScriptContext.HolySites,
                new ContextInfo {Directory = "common/religion/holy_sites", Type = "holy_site", Category = "Religion"}
            },
            {
                ScriptContext.ReligionFamilys,
                new ContextInfo
                    {Directory = "common/religion/religion_families", Type = "religion_family", Category = "Religion"}
            },
            {
                ScriptContext.Religions,
                new ContextInfo {Directory = "common/religion/religions", Type = "religion", Category = "Religion"}
            },
            {
                ScriptContext.Faiths,
                new ContextInfo
                {
                    ChildOf = ScriptContext.Religions, Type = "faith", Depth = 1, InsideChild = "faiths",
                    IgnoreChildren =
                        "traits,reserved_male_names,reserved_female_names,custom_faith_icons,holy_order_names,holy_order_maa,localization",
                    Category = "Religion"
                }
            },

            {
                ScriptContext.Schemes,
                new ContextInfo {Directory = "common/schemes", Type = "scheme_type", Category = "Schemes"}
            },
            {
                ScriptContext.ScriptedCharacterTemplates,
                new ContextInfo
                    {Directory = "common/scripted_character_templates", Type = "scripted_character_template"}
            },
            {
                ScriptContext.ScriptedEffects,
                new ContextInfo
                    {Directory = "common/scripted_effects", Type = "scripted_effect", Category = "Scripted Objects"}
            },
            {
                ScriptContext.ScriptedLists,
                new ContextInfo
                    {Directory = "common/scripted_lists", Type = "scripted_list", Category = "Scripted Objects"}
            },
            {
                ScriptContext.ScriptedModifiers,
                new ContextInfo
                    {Directory = "common/scripted_modifiers", Type = "scripted_modifier", Category = "Scripted Objects"}
            },
            {
                ScriptContext.ScriptedRelations,
                new ContextInfo
                    {Directory = "common/scripted_relations", Type = "scripted_relation", Category = "Scripted Objects"}
            },
            {
                ScriptContext.ScriptedRules,
                new ContextInfo
                    {Directory = "common/scripted_rules", Type = "scripted_rule", Category = "Scripted Objects"}
            },
            {
                ScriptContext.ScriptedTriggers,
                new ContextInfo
                {
                    Directory = "common/scripted_triggers", Type = "scripted_trigger", Trim = "scripted_trigger",
                    Category = "Scripted Objects"
                }
            },
            {
                ScriptContext.ScriptedValues,
                new ContextInfo {Directory = "common/script_values", Type = "value", Category = "Scripted Objects"}
            },
            {
                ScriptContext.SecretTypes,
                new ContextInfo {Directory = "common/secret_types", Type = "secret_type", Category = "Schemes"}
            },
            {ScriptContext.StoryCycles, new ContextInfo {Directory = "common/story_cycles", Type = "story_cycle"}},
            {
                ScriptContext.SuccessionElections,
                new ContextInfo {Directory = "common/succession_election", Type = "succession_election"}
            },
            {
                ScriptContext.Traits,
                new ContextInfo
                {
                    Directory = "common/traits", Type = "trait",
                    Groups = new List<ScriptGroupContext> {ScriptGroupContext.TraitGroups}
                }
            },
            {
                ScriptContext.TriggerLocalization,
                new ContextInfo {Directory = "common/trigger_localization", Type = "trigger_localization"}
            },
            {
                ScriptContext.VassalContracts,
                new ContextInfo {Directory = "common/vassal_contracts", Type = "vassal_contract"}
            },
            {
                ScriptContext.VassalContractObligation,
                new ContextInfo
                {
                    ChildOf = ScriptContext.VassalContracts, InsideChild = "obligation_levels",
                    Type = "vassal_contract_obligation"
                }
            },
            {ScriptContext.RegimentType, new ContextInfo {Directory = "common/regiment_types", Type = "regiment_type"}},
            {
                ScriptContext.GeographicRegion,
                new ContextInfo {Directory = "map_data/geographical_region.txt", Type = "geographic_region"}
            }
            // { ScriptContext.Messages, new ContextInfo() {Directory = "common/messages", Type = "message_type"}},
        };

        private readonly Dictionary<RefFilename, ScriptFile> FileMap = new Dictionary<RefFilename, ScriptFile>();


        public Dictionary<ScriptGroupContext, GroupContextInfo> GroupContextData =
            new Dictionary<ScriptGroupContext, GroupContextInfo>
            {
                {
                    ScriptGroupContext.TraitGroups,
                    new GroupContextInfo
                        {GroupItemContext = ScriptContext.Traits, GroupVariable = "group", Type = "trait"}
                }
            };

        public LocalizationParser LocalizationParser = new LocalizationParser();

        public Dictionary<string, LocalizationEntry> Localization { get; set; }
        public ScriptLibrary Parent { get; set; }
        public string Name { get; set; }

        public string Path
        {
            get
            {
                if (Core.Instance.BaseCK3Library == this)
                    return Globals.CK3Path;

                return Globals.CK3ModPath + Name + "/";
            }
        }

        public bool HasFile(RefFilename file)
        {
            var f = new RefFilename(file.ToRelativeFilename(), this == Core.Instance.BaseCK3Library);

            if (FileMap.ContainsKey(f))
                return true;

            return false;
        }

        public ScriptFile GetFile(RefFilename file)
        {
            var f = new RefFilename(file.ToRelativeFilename(), this == Core.Instance.BaseCK3Library);

            if (FileMap.ContainsKey(f))
                return FileMap[f];

            if (Parent != null)
                return Parent.GetFile(file);

            return null;
        }


        public void LoadLocalizations(string dir)
        {
            var results = LocalizationParser.Load(dir);

            Localization = results;
        }


        public bool Has(string name)
        {
            var b = AllTypeMap.ContainsKey(name);

            if (!b && Parent != null)
                return Parent.Has(name);

            return b;
        }

        public bool Has(ScriptContext context, string name)
        {
            var v = ContextData[context].Has(name);

            if (!v && Parent != null)
                return Parent.Has(context, name);

            return v;
        }

        public ScriptObject Get(string name)
        {
            if (!AllTypeMap.ContainsKey(name))
            {
                if (Parent != null)
                    return Parent.Get(name);
                return null;
            }

            return AllTypeMap[name];
        }

        public ScriptObject Get(ScriptContext context, string name)
        {
            var v = ContextData[context].Get(name);

            if (v == null && Parent != null)
                return Parent.Get(context, name);

            return v;
        }


        public void Remove(string key, ScriptContext valueContext)
        {
            AllTypeMap.Remove(key);

            ContextData[valueContext].Remove(key);
        }

        public void Add(List<ScriptObject> objects, ScriptContext context)
        {
            foreach (var dec in objects)
            {
                ScriptFile f = null;
                {
                    if (FileMap.ContainsKey(dec.Filename))
                    {
                        f = FileMap[dec.Filename];
                    }
                    else
                    {
                        f = new ScriptFile();
                        f.Filename = dec.Filename;
                        f.Context = context;
                        FileMap[dec.Filename] = f;
                    }

                    dec.Context = context;
                    dec.ScriptFile = f;
                    var name = dec.Name;

                    //if(ContextData[context].Trim != null)
                    {
                        //   name = name.Substring(name.IndexOf(ContextData[context].Trim) + ContextData[context].Trim.Length);
                    }
                    f.Map[name] = dec;
                }
            }

            var vals = objects.Where(a => a.Name.StartsWith("@")).ToList();

            objects.RemoveAll(a => vals.Contains(a));

            if (vals.Count > 0) vals[0].ScriptFile.AddLocalVars(vals);
            foreach (var scriptObject in objects)
            {
                var dec = scriptObject;
                if (dec != null)
                {
                    if (dec.Name.StartsWith("scripted_trigger"))
                        ContextData[ScriptContext.ScriptedTriggers]
                            .Add(dec.Name.Substring(dec.Name.IndexOf(' ') + 1), dec);
                    else
                        ContextData[context].Add(dec.Name, dec);
                    DoFile(dec, context);
                }
            }

            // if file exists in base scripts, all entires are overridden
            if (Parent != null)
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

                    foreach (var mapValue in f.Map.Values) mapValue.Overridden = true;
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

        public bool HasLocalizedText(string tag, bool canCheckBase = true)
        {
            if (Localization == null)
                return false;

            if (!Localization.ContainsKey(tag))
            {
                if (Parent != null && canCheckBase)
                    return Parent.HasLocalizedText(tag);

                return false;
            }

            return true;
        }

        public void RegisterRandomOnActionTrigger(ScriptObject node)
        {
            if (node.Children.Count > 0)
                foreach (var scriptObject in node.Children)
                {
                    int result;
                    if (int.TryParse(scriptObject.Name, out result))
                    {
                        var e = scriptObject.GetStringValue();
                        // found a random event...
                        if (Core.Instance.Get(ScriptContext.OnActions, e) != null)
                        {
                            var n = new trigger_event();

                            n.on_action = e;
                            n.Function = node;
                            n.Topmost = node.Topmost;
                            ReferenceManager.Instance.AddConnection(n.Topmost, n.Function, e);
                        }
                    }
                }
        }

        public void RegisteFirstValidOnActionTrigger(ScriptObject node)
        {
            if (node.Children.Count > 0)
                foreach (var scriptObject in node.Children)
                {
                    int result;

                    {
                        var e = scriptObject.Name;
                        // found a random event...
                        if (Core.Instance.Get(ScriptContext.OnActions, e) != null)
                        {
                            var n = new trigger_event();

                            n.on_action = e;
                            n.Function = node;
                            n.Topmost = node.Topmost;
                            ReferenceManager.Instance.AddConnection(n.Topmost, n.Function, e);
                        }
                    }
                }
        }

        public void RegisterRandomEventsTrigger(ScriptObject node)
        {
            if (node.Children.Count > 0)
                foreach (var scriptObject in node.Children)
                {
                    int result;
                    if (int.TryParse(scriptObject.Name, out result))
                    {
                        var e = scriptObject.GetStringValue();
                        // found a random event...
                        if (Core.Instance.Get(ScriptContext.Event, e) != null)
                        {
                            var n = new trigger_event();
                            if (e == "abduct_outcome.1001")
                            {
                            }

                            n.id = e;
                            n.Function = node;
                            n.Topmost = node.Topmost;
                            ReferenceManager.Instance.AddConnection(n.Topmost, n.Function, e);
                        }
                    }
                }
        }

        public void RegisterFirstValidEventsTrigger(ScriptObject node)
        {
            if (node.Children.Count > 0)
                foreach (var scriptObject in node.Children)
                {
                    int result;

                    {
                        var e = scriptObject.Name;
                        // found a random event...
                        if (Core.Instance.Get(ScriptContext.Event, e) != null)
                        {
                            var n = new trigger_event();

                            n.id = e;
                            n.Function = node;
                            n.Topmost = node.Topmost;
                            ReferenceManager.Instance.AddConnection(n.Topmost, n.Function, e);
                        }
                    }
                }
        }

        public void RegisterScriptEffectCall(ScriptObject node)
        {
            ReferenceManager.Instance.AddConnection(node.Topmost, node, node.Name);
        }

        public void RegisterScriptTriggerCall(ScriptObject node)
        {
            ReferenceManager.Instance.AddConnection(node.Topmost, node, node.Name);
        }

        public void RegisterTrigger(ScriptObject node)
        {
            if (node.Children.Count > 0)
            {
                var idNodes = node.Children.Where(a => a.Name == "id");
                var dayNodes = node.Children.Where(a => a.Name == "days");
                var onActionNodes = node.Children.Where(a => a.Name == "on_action");
                var n = new trigger_event();

                if (dayNodes.Any())
                {
                    var val = dayNodes.First().GetStringValue();
                    if (val == null)
                    {
                        var c = dayNodes.First().Children;
                        n.days_from = c[0].Name;
                        n.days_to = c[1].Name;
                    }
                    else
                    {
                        n.days_from = val;
                        n.days_to = val;
                    }
                }

                string on_action = null;
                string id = null;

                if (idNodes.Any()) id = idNodes.First().GetStringValue();
                if (onActionNodes.Any()) on_action = onActionNodes.First().GetStringValue();

                n.on_action = on_action;
                n.Topmost = node.Topmost;
                n.Function = node;
                n.id = id;
                ReferenceManager.Instance.AddConnection(n.Topmost, n.Function, on_action != null ? on_action : id);
            }
            else
            {
                var n = new trigger_event();

                n.Topmost = node.Topmost;
                n.Function = node;
                n.days_from = null;
                n.days_to = null;
                n.id = node.GetStringValue();
                ReferenceManager.Instance.AddConnection(n.Topmost, n.Function, node.GetStringValue());
            }
        }


        private void DoFile(ScriptObject dec, ScriptContext context)
        {
            var s = dec.Name;
            if (dec.Name.StartsWith("scripted_trigger")) s = dec.Name.Substring(dec.Name.IndexOf(' ') + 1);


            AllTypeMap[s] = dec;

            ScriptFile f;
            if (FileMap.ContainsKey(dec.Filename))
            {
                f = FileMap[dec.Filename];
            }
            else
            {
                f = new ScriptFile();
                f.Filename = dec.Filename;
                f.Context = context;
                FileMap[dec.Filename] = f;
            }

            dec.Context = context;
            dec.ScriptFile = f;
            f.Map[s] = dec;
        }


        public ScriptFile EnsureFile(RefFilename filename, ScriptContext context)
        {
            if (FileMap.ContainsKey(filename)) return FileMap[filename];

            var file = new ScriptFile();

            file.Filename = filename;
            file.Context = context;
            FileMap[filename] = file;

            if (!file.IsBase && Parent != null && Parent.FileMap.ContainsKey(filename))
            {
                var f = Parent.FileMap[filename];
                f.Overridden = true;
                foreach (var mapValue in f.Map.Values) mapValue.Overridden = true;
            }

            return file;
        }

        public bool Has(ScriptObject inside)
        {
            if (AllTypeMap.ContainsKey(inside.Name)) return true;

            return false;
        }

        public List<string> GetDirectoryListFromLocalization()
        {
            var results = new List<string>();

            results.AddRange(LocalizationParser.LocalizationFiles.Keys);

            return results;
        }

        public List<RefFilename> GetDirectoryListFromContext(ScriptContext context, string requiredNamespace)
        {
            var results = new List<RefFilename>();
            foreach (var keyValuePair in FileMap)
                if (keyValuePair.Value.Context == context)
                    if (string.IsNullOrWhiteSpace(requiredNamespace) ||
                        keyValuePair.Value.Namespace == requiredNamespace)
                        results.Add(keyValuePair.Value.Filename);

            return results;
        }

        public void ClearFile(RefFilename filename)
        {
            if (FileMap.ContainsKey(filename))
            {
                var file = FileMap[filename];

                foreach (var keyValuePair in file.Map) Remove(keyValuePair.Key, keyValuePair.Value.Context);
                file.Map.Clear();
            }
        }

        public void RecalculateGroups()
        {
            foreach (var contextDataValue in ContextData.Values)
                if (contextDataValue.ChildOf != ScriptContext.None)
                {
                    var from = ContextData[contextDataValue.ChildOf];

                    if (contextDataValue.InsideChild != null)
                    {
                        var vals = from.Values();
                        var ignore = contextDataValue.IgnoreChildren == null
                            ? new string[0]
                            : contextDataValue.IgnoreChildren.Split(',');
                        foreach (var scriptObject in vals)
                        {
                            // only blocks...
                            var list = scriptObject.Children
                                .Where(a => a.IsBlock && contextDataValue.InsideChild == a.Name).ToList();

                            foreach (var b in list)
                            {
                                var list2 = b.Children.Where(a => a.IsBlock && !ignore.Contains(a.Name)).ToList();

                                foreach (var o in list2)
                                {
                                    contextDataValue.Add(o.Name, o);
                                    o.ScriptFile = scriptObject.ScriptFile;
                                }
                            }
                        }
                    }
                    else
                    {
                        var vals = from.Values();
                        var ignore = contextDataValue.IgnoreChildren == null
                            ? new string[0]
                            : contextDataValue.IgnoreChildren.Split(',');
                        foreach (var scriptObject in vals)
                        {
                            // only blocks...
                            var list = scriptObject.Children.Where(a => a.IsBlock && !ignore.Contains(a.Name)).ToList();

                            foreach (var o in list)
                            {
                                contextDataValue.Add(o.Name, o);
                                o.ScriptFile = scriptObject.ScriptFile;
                            }
                        }
                    }
                }
                else
                {
                    var ignore = contextDataValue.IgnoreChildren == null
                        ? new string[0]
                        : contextDataValue.IgnoreChildren.Split(',');
                    if (contextDataValue.Recurse)
                    {
                        var newObjects = new List<ScriptObject>();
                        var v = contextDataValue.Values();
                        foreach (var a in v) DoRecurse(contextDataValue, a, newObjects, ignore);
                        foreach (var scriptObject in newObjects)
                        {
                            scriptObject.ScriptFile = scriptObject.Topmost.ScriptFile;
                            contextDataValue.Add(scriptObject.Name, scriptObject);
                        }
                    }
                }

            foreach (var groupContextInfo in GroupContextData.Values) groupContextInfo.Clear();

            foreach (var value in GroupContextData.Values)
            {
                var getFrom = ContextData[value.GroupItemContext];

                var vals = getFrom.Values();

                foreach (var scriptObject in vals)
                {
                    var group = scriptObject.GetChildStringValue("group");
                    if (group != null) value.Add(group, scriptObject);
                }
            }
        }

        private void DoRecurse(ContextInfo contextDataValue, ScriptObject scriptObject, List<ScriptObject> newObjects,
            string[] ignore)
        {
            var children = scriptObject.Children.Where(a => !ignore.Contains(a.Name) && a.IsBlock).ToList();

            foreach (var child in children)
            {
                newObjects.Add(child);
                DoRecurse(contextDataValue, child, newObjects, ignore);
            }
        }

        public void DoSmartFind(SmartFindOptions options, List<SmartFindResults> results)
        {
            foreach (var fileMapValue in FileMap.Values) fileMapValue.DoSmartFind(options, results);
        }

        public bool AddFile(RefFilename file)
        {
            var f = new ScriptFile();

            f.Filename = new RefFilename(file.ToRelativeFilename(), this == Core.Instance.BaseCK3Library);
            var l = Core.Instance.BaseCK3Library.ContextData.Where(a =>
                a.Value.Directory != null && f.Filename.ToRelativeDirectoryOfFile().Contains(a.Value.Directory));
            if (l.Count() == 0)
                return false;
            f.Context = l.First().Key;
            FileMap[file] = f;
            return true;
        }

        public class GroupContextInfo
        {
            private readonly Dictionary<string, List<ScriptObject>> Map = new Dictionary<string, List<ScriptObject>>();
            public ScriptContext GroupItemContext { get; set; }
            public string Type { get; set; }
            public string GroupVariable { get; set; }

            public void Add(string s, ScriptObject o)
            {
                if (Map.ContainsKey(s))
                    Map[s].Add(o);
                else
                    Map[s] = new List<ScriptObject> {o};
            }

            public void Remove(string s)
            {
                Map.Remove(s);
            }

            //            public delegate HashSet<string> GetNameSet(bool modOnly);
            public List<ScriptObject> Get(string s)
            {
                if (!Map.ContainsKey(s))
                    return null;

                return Map[s];
            }

            public bool Has(string s)
            {
                return Map.ContainsKey(s);
            }

            public IEnumerable<string> Keys()
            {
                return Map.Keys;
            }

            public IEnumerable<List<ScriptObject>> Values()
            {
                return Map.Values;
            }

            public void Clear()
            {
                foreach (var scriptObjects in Map.Values)
                    scriptObjects.Clear();
            }
        }

        public class ContextInfo
        {
            private string _directory;

            public List<ScriptGroupContext> Groups = new List<ScriptGroupContext>();
            private readonly Dictionary<string, ScriptObject> Map = new Dictionary<string, ScriptObject>();

            public string Directory
            {
                get => _directory;
                set => _directory = value.ToLower();
            }

            public string Type { get; set; }
            public ScriptContext ChildOf { get; set; }
            public string Prepend { get; set; }
            public string IgnoreChildren { get; set; }
            public bool Recurse { get; set; }
            public string Trim { get; set; }
            public int Depth { get; set; }
            public string InsideChild { get; set; }
            public string Category { get; set; }

            public void Add(string s, ScriptObject o)
            {
                if (Prepend != null)
                    s = s.Replace(Prepend + ":", "");

                Map[s] = o;
            }

            public void Remove(string s)
            {
                if (Prepend != null)
                    s = s.Replace(Prepend + ":", "");

                Map.Remove(s);
            }

            //            public delegate HashSet<string> GetNameSet(bool modOnly);
            public ScriptObject Get(string s)
            {
                if (Prepend != null)
                    s = s.Replace(Prepend + ":", "");

                if (!Map.ContainsKey(s))
                    return null;

                return Map[s];
            }

            public ScriptValue GetValue(string s)
            {
                if (Prepend != null)
                    s = s.Replace(Prepend + ":", "");

                if (!Map.ContainsKey(s))
                    return null;

                return Map[s] as ScriptValue;
            }

            public bool Has(string s)
            {
                if (Prepend != null)
                    s = s.Replace(Prepend + ":", "");
                return Map.ContainsKey(s);
            }

            public IEnumerable<string> Keys()
            {
                return Map.Keys;
            }

            public IEnumerable<ScriptObject> Values()
            {
                return Map.Values;
            }
        }
    }
}