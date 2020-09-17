using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace JominiParse
{
    public class ScriptLibrary
    {
        public Dictionary<string, ScriptObject> AllTypeMap = new Dictionary<string, ScriptObject>();
   
        public Dictionary<string, ScriptFile> FileMap = new Dictionary<string, ScriptFile>();

        public class GroupContextInfo
        {
            public ScriptContext GroupItemContext { get; set; }
            public string Type { get; set; }
            public string GroupVariable { get; set; }

            Dictionary<string, List<ScriptObject>> Map = new Dictionary<string, List<ScriptObject>>();

            public void Add(string s, ScriptObject o)
            {
                if(Map.ContainsKey(s))
                    Map[s].Add(o);
                else
                    Map[s] = new List<ScriptObject>() {o};
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
            public string Directory { get; set; }
            public string Type { get; set; }
            public ScriptContext ChildOf { get; set; }
            public string Prepend { get; set; }
            public string IgnoreChildren { get; set; }
            public bool Recurse { get; set; }

            public List<ScriptGroupContext> Groups = new List<ScriptGroupContext>();
            Dictionary<string, ScriptObject> Map = new Dictionary<string, ScriptObject>();

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

        /*
               if (type == "innovation")
                return GetNameSet(ScriptContext.CulturalInnovations, false);
            if (type == "building")
                return GetNameSet(ScriptContext.Buildings, false);
            if (type == "event_background")
                return GetNameSet(ScriptContext.EventBackgrounds, false);
            if (type == "scheme_type")
                return GetNameSet(ScriptContext.Schemes, false);
            if (type == "relation")
                return GetNameSet(ScriptContext.ScriptedRelations, false);
            if (type == "scripted_modifier")
                return GetNameSet(ScriptContext.ScriptedModifiers, false);
            if (type == "scripted_trigger")
                return GetNameSet(ScriptContext.ScriptedTriggers, false);
            if (type == "opinion")
                return GetNameSet(ScriptContext.OptionModifiers, false);
            if (type == "hook")
                return GetNameSet(ScriptContext.HookTypes, false);
            if (type == "on_action")
                return GetNameSet(ScriptContext.OnActions, false);
            if (type == "modifier")
                return GetNameSet(ScriptContext.StaticModifiers, false);
            if (type == "event_theme")
                return GetNameSet(ScriptContext.EventThemes, false);
                 if (type == "perk")
                return GetNameSet(ScriptContext.LifestylePerks, false);
            if (type == "decision")
                return GetNameSet(ScriptContext.Decisions, false);
            if (type == "event")
                return GetNameSet(ScriptContext.Events, false);
            if(type == "culture_group")
                return GetNameSet(ScriptContext.CultureGroups, false).PrependToken("culture_group:");
            if (type == "trait")
            {
                var a = GetNameSet(ScriptContext.Traits, false);
                var b = GetGroupNameList(ScriptGroupContext.TraitGroups, false);
                var h = new HashSet<string>();

                foreach (var v in a)
                {
                    h.Add(v);
                }
                foreach (var v in b)
                {
                    h.Add(v);
                }

                return h;
            }
         *
         */

        public Dictionary<ScriptGroupContext, GroupContextInfo> GroupContextData =
            new Dictionary<ScriptGroupContext, GroupContextInfo>()
            {
                {
                    ScriptGroupContext.TraitGroups,
                    new GroupContextInfo() {GroupItemContext = ScriptContext.Traits, GroupVariable = "group", Type="trait"}
                }
            };

        public Dictionary<ScriptContext, ContextInfo> ContextData = new Dictionary<ScriptContext, ContextInfo>()
        {
            { ScriptContext.Events, new ContextInfo() {Directory = "events", Type="event"}},
            { ScriptContext.CultureGroups, new ContextInfo() {Directory = "common/culture/cultures", Type="culture_group"}},
            { ScriptContext.Cultures, new ContextInfo() {ChildOf = ScriptContext.CultureGroups, Type="culture", Prepend="culture", IgnoreChildren="graphical_cultures,mercenary_names"}},
            { ScriptContext.CulturalInnovations, new ContextInfo() {Directory = "common/culture/innovations", Type="innovation"}},
            { ScriptContext.Decisions, new ContextInfo() {Directory = "common/decisions", Type="decision"}},
            { ScriptContext.Activities, new ContextInfo() {Directory = "common/activities", Type = "activity"}},
            { ScriptContext.Bookmark, new ContextInfo() {Directory = "common/bookmarks", Type="bookmark"}},
            { ScriptContext.Buildings, new ContextInfo() {Directory = "common/buildings", Type="building"}},
            { ScriptContext.CasusBelliType, new ContextInfo() {Directory = "common/casus_belli_types", Type="casus_belli_types"}},
            { ScriptContext.CharacterInteractions, new ContextInfo() {Directory = "common/character_interactions", Type="character_interactions"}},
            { ScriptContext.Characters, new ContextInfo() {Directory = "history/characters", Type = "character", Prepend = "character"}},
            { ScriptContext.CouncilPositions, new ContextInfo() {Directory = "common/council_positions", Type="council_position"}},
            { ScriptContext.CouncilTasks, new ContextInfo() {Directory = "common/council_tasks", Type="council_task"}},
            { ScriptContext.Defines, new ContextInfo() {Directory = "common/Defines", Type="define"}},
            { ScriptContext.DynastyLegacies, new ContextInfo() {Directory = "common/dynasty_legacies", Type="legacy"}},
            { ScriptContext.DynastyPerks, new ContextInfo() {Directory = "common/dynasty_perks", Type="dynasty_perk"}},
            { ScriptContext.EventBackgrounds, new ContextInfo() {Directory = "common/event_backgrounds", Type="event_background"}},
            { ScriptContext.EventThemes, new ContextInfo() {Directory = "common/event_themes", Type="event_theme"}},
            { ScriptContext.Factions, new ContextInfo() {Directory = "common/factions", Type = "faction"}},
            { ScriptContext.Focuses, new ContextInfo() {Directory = "common/focuses", Type = "focus"}},
            { ScriptContext.GameRuleCategories, new ContextInfo() {Directory = "common/game_rules", Type="game_rule_category"}},
            { ScriptContext.GameRules, new ContextInfo() {ChildOf = ScriptContext.GameRuleCategories, Type="game_rule"}},
            { ScriptContext.Governments, new ContextInfo() {Directory = "common/governments", Type="government"}},
            { ScriptContext.Holdings, new ContextInfo() {Directory = "common/holdings", Type="holding"}},
            { ScriptContext.HookTypes, new ContextInfo() {Directory = "common/hook_types", Type="hook"}},
            { ScriptContext.ImportantActions, new ContextInfo() {Directory = "common/important_actions", Type="important_action"}},
            { ScriptContext.LandedTitles, new ContextInfo() {Directory = "common/landed_titles", Type="title", Prepend = "title", Recurse=true, IgnoreChildren = "color,color2,ai_primary_priority,male_names,cultural_names"}},

            { ScriptContext.Laws, new ContextInfo() {Directory = "common/laws", Type="law"}},
            { ScriptContext.LifestylePerks, new ContextInfo() {Directory = "common/lifestyle_perks", Type="perk"}},
            { ScriptContext.Lifestyles, new ContextInfo() {Directory = "common/lifestyles", Type="lifestyle"}},
            { ScriptContext.StaticModifiers, new ContextInfo() {Directory = "common/modifiers", Type="modifier"}},
            { ScriptContext.Nicknames, new ContextInfo() {Directory = "common/nicknames", Type="nickname"}},
            { ScriptContext.OnActions, new ContextInfo() {Directory = "common/on_action", Type="on_action"}},
            { ScriptContext.OptionModifiers, new ContextInfo() {Directory = "common/opinion_modifiers", Type="opinion_modifier"}},
            { ScriptContext.Doctrines, new ContextInfo() {Directory = "common/religion/doctrines", Type="doctrine"}},
            { ScriptContext.FervorModifiers, new ContextInfo() {Directory = "common/religion/fervor_modifiers", Type="fervor_modifier"}},

            { ScriptContext.HolySites, new ContextInfo() {Directory = "common/religion/holy_sites", Type="holy_site"}},
            { ScriptContext.ReligionFamilys, new ContextInfo() {Directory = "common/religion/religion_families", Type="religion_family"}},
            { ScriptContext.Religions, new ContextInfo() {Directory = "common/religion/religions", Type="religion"}},

            { ScriptContext.Schemes, new ContextInfo() {Directory = "common/schemes", Type="scheme_type"}},
            { ScriptContext.ScriptedCharacterTemplates, new ContextInfo() {Directory = "common/scripted_character_templates", Type="scripted_character_template"}},
            { ScriptContext.ScriptedEffects, new ContextInfo() {Directory = "common/scripted_effects", Type="scripted_effect"}},
            { ScriptContext.ScriptedLists, new ContextInfo() {Directory = "common/scripted_lists", Type="scripted_list"}},
            { ScriptContext.ScriptedModifiers, new ContextInfo() {Directory = "common/scripted_modifiers", Type="scripted_modifier"}},
            { ScriptContext.ScriptedRelations, new ContextInfo() {Directory = "common/scripted_relations", Type="scripted_relation"}},
            { ScriptContext.ScriptedRules, new ContextInfo() {Directory = "common/scripted_rules", Type="scripted_rule"}},
            { ScriptContext.ScriptedTriggers, new ContextInfo() {Directory = "common/scripted_triggers", Type="scripted_trigger"}},
            { ScriptContext.ScriptedValues, new ContextInfo() {Directory = "common/script_values", Type="num"}},
            { ScriptContext.SecretTypes, new ContextInfo() {Directory = "common/secret_types", Type="secret_type"}},
            { ScriptContext.StoryCycles, new ContextInfo() {Directory = "common/story_cycles", Type="story_cycle"}},
            { ScriptContext.SuccessionElections, new ContextInfo() {Directory = "common/succession_election", Type="succession_election"}},
            { ScriptContext.Traits, new ContextInfo() {Directory = "common/traits", Type="trait", Groups = new List<ScriptGroupContext>() {ScriptGroupContext.TraitGroups}}},
            { ScriptContext.VassalContracts, new ContextInfo() {Directory = "common/vassal_contracts", Type = "vassal_contract"}},

        };


        public void LoadLocalizations(string dir)
        {
            var results = LocalizationParser.Instance.Load(dir);

            this.Localization = results;
        }

        public Dictionary<string, LocalizationEntry> Localization { get; set; }
        public ScriptLibrary Parent { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }


        public bool Has(string name)
        {
            bool b = AllTypeMap.ContainsKey(name);

            if(!b && Parent != null)
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
            
            var vals = objects.Where(a => a.Name.StartsWith("@")).ToList();

            objects.RemoveAll(a => vals.Contains(a));

            if (vals.Count > 0)
            {
                vals[0].ScriptFile.AddLocalVars(vals);
            }
            foreach (var scriptObject in objects)
            {
                var dec = scriptObject as ScriptObject;
                if (dec != null)
                {
                    if (dec.Name.StartsWith("scripted_trigger"))
                    {
                        ContextData[ScriptContext.ScriptedTriggers].Add(dec.Name.Substring(dec.Name.IndexOf(' ') + 1), dec);

                    }
                    else
                    {
                        ContextData[context].Add(dec.Name, dec);

                    }
                    DoFile(dec, context);
                }

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
        public bool HasLocalizedText(string tag)
        {
            if (Localization == null)
                return false;

            if (!Localization.ContainsKey(tag))
            {
                if (Parent != null)
                    return Parent.HasLocalizedText(tag);

                return false;
            }

            return true;
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
                        if (ContextData[ScriptContext.OnActions].Get(e) != null)
                        {
                            trigger_event n = new trigger_event();

                            n.on_action = e;
                            n.Function = node;
                            n.Topmost = node.Topmost;
                            ReferenceManager.Instance.AddConnection(n.Topmost, n.Function, e);

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
                        if (ContextData[ScriptContext.OnActions].Get(e) != null)
                        {
                            trigger_event n = new trigger_event();

                            n.on_action = e;
                            n.Function = node;
                            n.Topmost = node.Topmost;
                            ReferenceManager.Instance.AddConnection(n.Topmost, n.Function, e);
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
                        if (ContextData[ScriptContext.Events].Get(e) != null)
                        {
                            trigger_event n = new trigger_event();
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
                        if (ContextData[ScriptContext.Events].Get(e) != null)
                        {
                            trigger_event n = new trigger_event();
                           
                            n.id = e;
                            n.Function = node;
                            n.Topmost = node.Topmost;
                            ReferenceManager.Instance.AddConnection(n.Topmost, n.Function, e);
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
                ReferenceManager.Instance.AddConnection(n.Topmost, n.Function, on_action != null ? on_action : id);

            }
            else
            {
                trigger_event n = new trigger_event();

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

        public void RecalculateGroups()
        {
            foreach (var contextDataValue in ContextData.Values)
            {
                if (contextDataValue.ChildOf != ScriptContext.None)
                {
                    var from = ContextData[contextDataValue.ChildOf];

                    var vals = from.Values();
                    string[] ignore = contextDataValue.IgnoreChildren == null ? new string[0] : contextDataValue.IgnoreChildren.Split(',');
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
                else
                {
                    string[] ignore = contextDataValue.IgnoreChildren == null ? new string[0] : contextDataValue.IgnoreChildren.Split(',');
                    if (contextDataValue.Recurse)
                    {
                        List<ScriptObject> newObjects = new List<ScriptObject>();
                        var v = contextDataValue.Values();
                        foreach (var a in v)
                        {
                            DoRecurse(contextDataValue, a, newObjects, ignore);

                         
                        }
                        foreach (var scriptObject in newObjects)
                        {
                            scriptObject.ScriptFile = scriptObject.Topmost.ScriptFile;
                            contextDataValue.Add(scriptObject.Name, scriptObject);
                        }
                    }


                }
            }
            foreach (var groupContextInfo in GroupContextData.Values)
            {
                groupContextInfo.Clear();
            }

            foreach (var value in GroupContextData.Values)
            {
                var getFrom = this.ContextData[value.GroupItemContext];

                var vals = getFrom.Values();

                foreach (var scriptObject in vals)
                {
                    var group = scriptObject.GetChildStringValue("group");
                    if (group != null)
                    {
                        value.Add(group, scriptObject);
                    }
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
            foreach (var fileMapValue in FileMap.Values)
            {
                fileMapValue.DoSmartFind(options, results);
            }
        }
    }
}
