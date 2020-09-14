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

        Dictionary<string, ScriptObject> LocalVars = new Dictionary<string, ScriptObject>();

        public void AddLocalVars(List<ScriptObject> vars)
        {
            foreach (var scriptObject in vars)
                LocalVars[scriptObject.Name] = scriptObject;


        }

        public ScriptObject.ScopeVarType GetVarType(string name)
        {
            if (!LocalVars.ContainsKey(name))
                return ScriptObject.ScopeVarType.None;

            return GetVarType(LocalVars[name]);
        }
        public ScriptObject.ScopeVarType GetVarType(ScriptObject v)
        {
            return v.GetVarType();

        }

        public List<string> LocalVarNamelist()
        {
            return LocalVars.Keys.ToList();
        }
        public List<string> LocalVarNamelist(ScriptObject.ScopeVarType type)
        {
            var l = LocalVars.Keys.ToList();

            return l.Where(a => GetVarType(a) == type).ToList();

        }
    }

    public class ScriptLibrary
    {
        public Dictionary<string, ScriptObject> AllTypeMap = new Dictionary<string, ScriptObject>();
   
        public Dictionary<string, ScriptFile> FileMap = new Dictionary<string, ScriptFile>();

        public class GroupContextInfo
        {
            public ScriptContext GroupItemContext { get; set; }
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
            public string Directory;
            public List<ScriptGroupContext> Groups = new List<ScriptGroupContext>();
            Dictionary<string, ScriptObject> Map = new Dictionary<string, ScriptObject>();

            public void Add(string s, ScriptObject o)
            {
                Map[s] = o;
            }

            public void Remove(string s)
            {
                Map.Remove(s);
            }
            //            public delegate HashSet<string> GetNameSet(bool modOnly);
            public ScriptObject Get(string s)
            {
                if (!Map.ContainsKey(s))
                    return null;

                return Map[s];
            }
            public ScriptValue GetValue(string s)
            {
                if (!Map.ContainsKey(s))
                    return null;

                return Map[s] as ScriptValue;
            }

            public bool Has(string s)
            {
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

        public Dictionary<ScriptGroupContext, GroupContextInfo> GroupContextData =
            new Dictionary<ScriptGroupContext, GroupContextInfo>()
            {
                {
                    ScriptGroupContext.TraitGroups,
                    new GroupContextInfo() {GroupItemContext = ScriptContext.Traits, GroupVariable = "group"}
                }
            };

        public Dictionary<ScriptContext, ContextInfo> ContextData = new Dictionary<ScriptContext, ContextInfo>()
        {
            { ScriptContext.Events, new ContextInfo() {Directory = "events"}},
            { ScriptContext.CultureGroups, new ContextInfo() {Directory = "common/culture/cultures"}},
            { ScriptContext.CulturalInnovations, new ContextInfo() {Directory = "common/culture/innovations"}},
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
            { ScriptContext.Traits, new ContextInfo() {Directory = "common/traits", Groups = new List<ScriptGroupContext>() {ScriptGroupContext.TraitGroups}}},
            { ScriptContext.VassalContracts, new ContextInfo() {Directory = "common/vassal_contracts"}},

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
    }
}
