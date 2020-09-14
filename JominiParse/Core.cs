using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

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
            var res = BaseCK3Library.ContextData.Where(a => dir.StartsWith(a.Value.Directory)).ToList();

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

        }
        public bool LoadCK3File(string filename, bool forceBase=false, bool forceReload = false)
        {
            ScriptObject.DeferedInitializationList.Clear();
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
       
       
        public void PostInitialize()
        {

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
                if(LoadingCK3Library.ContextData.ContainsKey((ScriptContext)x))
                {
                    ScriptLibrary.ContextInfo info = LoadingCK3Library.ContextData[(ScriptContext)x];
                    var r = FileTokenizer.Instance.LoadDirectory(startDir + info.Directory + "/", startDir, (ScriptContext)x);
                    LoadingCK3Library.Add(r, (ScriptContext)x);
                }

            }
            foreach (var scriptObject in ScriptObject.DeferedInitializationList)
            {
                scriptObject.Initialize();
            }

            LoadingCK3Library.RecalculateGroups();

        }


        public void UpdateFile(string filename, string text)
        {
            ScriptObject.DeferedInitializationList.Clear();

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

            foreach (var scriptObject in ScriptObject.DeferedInitializationList)
            {
                scriptObject.PostInitialize();
            }
            ScriptObject.DeferedInitializationList.Clear();


            LoadingCK3Library.RecalculateGroups();
        }

        
        public HashSet<string> GetGroupNameList(ScriptGroupContext context, bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.GroupContextData[context].Keys());
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.GroupContextData[context].Keys());

            return eventNames;
        }
        public HashSet<string> GetNameSet(ScriptContext context, bool modOnly)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.ContextData[context].Keys());
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.ContextData[context].Keys());

            return eventNames;
        }


        public ScriptObject Get(ScriptContext context, string name, bool forceBase = false)
        {
            var lib = ModCK3Library;

            if (forceBase)
                lib = BaseCK3Library;

            var res = lib.Get(context, name);

            if (res == null)
            {
                if (lib.Parent != null)
                {
                    return lib.Parent.Get(context, name);
                }
            }

            return res;

        }

        public ScriptObject Get(string name, bool forceBase = false)
        {
            var lib = ModCK3Library;

            if (forceBase)
                lib = BaseCK3Library;

            var res = lib.Get(name);

            if (res == null)
            {
                if (lib.Parent != null)
                {
                    return lib.Parent.Get(name);
                }
            }

            return res;

        }

        public ScriptObject GetEvent(string name)
        {
            return ModCK3Library.Get(ScriptContext.Events, name);
        }

        public string GetDirectoryFromContext(ScriptContext context)
        {
            return BaseCK3Library.ContextData[context].Directory;
        }

        public HashSet<string> GetNameSetFromEnumType(string type)
        {

            //  if (type == "building_type")
            //    return GetNameSet(ScriptContext.Buildings, false);
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
            //          if (type == "tooltip")
            //                return GetEventNameSet(false);

            return new HashSet<string>();
        }

        public HashSet<string> LocalVarListFromObjectFile(ScriptObject o)
        {
            HashSet<string> s = new HashSet<string>();
            var v = o.Topmost.ScriptFile.LocalVarNamelist(o.GetVarType());
            foreach (var d in v)
            {
                s.Add(d);
            }

            return s;
        }
        public HashSet<string> LocalVarListFromObjectFile(ScriptObject o, ScriptObject.ScopeVarType type)
        {
            HashSet<string> s = new HashSet<string>();
            var v = o.Topmost.ScriptFile.LocalVarNamelist(o.GetVarType());
            foreach (var d in v)
            {
                s.Add(d);
            }
            return s;
        }
    }
}
