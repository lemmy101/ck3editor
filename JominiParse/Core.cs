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
            SchemaManager.Instance.Init();
            BaseCK3Library = new ScriptLibrary();
            ModCK3Library = new ScriptLibrary();
            LoadingCK3Library = BaseCK3Library;
            BaseCK3Library.Path = Globals.CK3Path;
            EnumManager.Instance.Load();
            LoadCK3Scripts(BaseCK3Library, true, true);
            ScriptObject.ClearCachedScriptedEffects();
            //  BaseCK3Library.SaveBinary("binary.dat");
            //BaseCK3Library.LoadBinary("");
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
            ScriptObject.ClearCachedScriptedEffects();
        }
        public void LoadMod(string mod)
        {
            ModCK3Library = new ScriptLibrary();
            ModCK3Library.Parent = BaseCK3Library;
            ModCK3Library.Name = mod;
            ModCK3Library.Path = Globals.CK3ModPath + mod + "/";

            LoadingCK3Library = ModCK3Library;
            LoadCK3Scripts(ModCK3Library, false, false);

            PostInitialize();
            ScriptObject.ClearCachedScriptedEffects();
        }

        private void LoadModFiles(string mod)
        {
            
        }

        public string GetLocalizedText(string tag)
        {
            return ModCK3Library.GetLocalizedText(tag);
        }
        public bool HasLocalizedText(string tag)
        {
            return ModCK3Library.HasLocalizedText(tag);
        }

        ScriptContext GetContextFromDirectory(string dir)
        {
            var res = BaseCK3Library.ContextData.Where(a => a.Value.Directory != null && dir.StartsWith(a.Value.Directory)).ToList();

            if(res.Any())

            {
                return res[0].Key;
            }

            return ScriptContext.Event;
        }

        public void LoadCK3BaseFile(string filename)
        {
            if (BaseCK3Library.FileMap.ContainsKey(filename))
                return;

            LoadingCK3Library = BaseCK3Library;
            string directory = filename.Substring(0, filename.LastIndexOf("/"));

            ScriptContext context = GetContextFromDirectory(directory);
            string startDir = Globals.CK3Path;//"D:/SteamLibrary/steamapps/common/Crusader Kings III/";

            var results = FileTokenizer.Instance.LoadFile(startDir + filename,startDir, context, true);

            BaseCK3Library.Add(results, context);

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

            var results = FileTokenizer.Instance.LoadFile(startDir + filename, startDir, context, true);

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
            do
            {
                ScriptObject.DeferedPostInitializationList = ScriptObject.DeferedPostInitializationListNext;
                ScriptObject.DeferedPostInitializationListNext = new List<ScriptObject>();
                for (int i = 0; i < ScriptObject.DeferedPostInitializationList.Count; i++)
                {
                    var scriptObject = ScriptObject.DeferedPostInitializationList[i];
                    scriptObject.PostInitialize();

                }
                ScriptObject.DeferedPostInitializationList.Clear();

            } while (ScriptObject.DeferedPostInitializationListNext.Count > 0);

            /*while (ScriptObject.DeferedInitializationList.Count > 0)
            {
                var scriptObject = ScriptObject.DeferedInitializationList[0];
                scriptObject.PostInitialize();
            }
            */

        }

        public void LoadCK3Scripts(ScriptLibrary lib, bool save = true, bool load = true)
        {
            LoadingCK3Library = lib;

            // events...
            string startDir = lib.Path; //"D:/SteamLibrary/steamapps/common/Crusader Kings III/";

            LoadingCK3Library.LoadLocalizations(startDir + "localization/english");

            for (int x = 0; x < (int) ScriptContext.Max; x++)
            {
                if (LoadingCK3Library.ContextData.ContainsKey((ScriptContext) x))
                {
                    if (!string.IsNullOrEmpty(LoadingCK3Library.ContextData[(ScriptContext) x].Directory))
                    {
                        ScriptLibrary.ContextInfo info = LoadingCK3Library.ContextData[(ScriptContext) x];
                        if (info.Directory.EndsWith(".txt"))
                        {
                            var r = FileTokenizer.Instance.LoadFile(startDir + info.Directory, startDir,
                                (ScriptContext)x, save);
                            LoadingCK3Library.Add(r, (ScriptContext)x);
                        }
                        else
                        {
                            var r = FileTokenizer.Instance.LoadDirectory(startDir + info.Directory + "/", startDir,
                                (ScriptContext)x, save, load);
                            LoadingCK3Library.Add(r, (ScriptContext)x);

                        }

                    }
                }

            }


            for (int i = 0; i < ScriptObject.DeferedInitializationList.Count; i++)
            {
                var scriptObject = ScriptObject.DeferedInitializationList[i];
                scriptObject.Initialize();

            }

            ScriptObject.DeferedInitializationList.Clear();

            LoadingCK3Library.RecalculateGroups();


        }

        public void UpdateFile(string filename, string text)
        {
            VariableStore.Instance.RemoveAllVariablesFromFile(filename);
            ScriptObject.DeferedPostInitializationListNext.Clear();

            string startDir = ModCK3Library.Path;//"D:/SteamLibrary/steamapps/common/Crusader Kings III/";
            
            LoadingCK3Library = ModCK3Library;

            var c = GetContextFromDirectory(filename.Substring(0, filename.LastIndexOf("/")));

            try
            {
                var results = FileTokenizer.Instance.LoadText(text, startDir + filename, startDir, c);

                ModCK3Library.Add(results, c);
            }
            catch (Exception e)
            {
            }

            try
            {
          
                 for (int i = 0; i < ScriptObject.DeferedPostInitializationListNext.Count; i++)
                {
                    var scriptObject = ScriptObject.DeferedPostInitializationListNext[i];
                    scriptObject.Initialize();

                }
                
               
            }
            catch (Exception e)
            {
            }

            try
            {
                LoadingCK3Library.RecalculateGroups();

                do
                {
                    ScriptObject.DeferedPostInitializationList = ScriptObject.DeferedPostInitializationListNext;
                    ScriptObject.DeferedPostInitializationListNext = new List<ScriptObject>();
                    for (int i = 0; i < ScriptObject.DeferedPostInitializationList.Count; i++)
                    {
                        var scriptObject = ScriptObject.DeferedPostInitializationList[i];
                        scriptObject.PostInitialize();

                    }
                    ScriptObject.DeferedPostInitializationList.Clear();

                } while (ScriptObject.DeferedPostInitializationListNext.Count > 0);
            }
            catch (Exception e)
            {
            }


            ScriptObject.ClearCachedScriptedEffects();


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

        public List<string> GetNameSet(string type, ScriptLibrary lib, bool allowPrepend=false, bool addPrepend=false)
        {
            List<string> l = new List<string>();
            var where = lib.ContextData.Where(a => a.Value.Type == type);

            
            foreach (var keyValuePair in where)
            {
                if (keyValuePair.Value.Prepend != null && !allowPrepend)
                {

                }
                else
                {
                    if (keyValuePair.Value.Prepend != null && addPrepend)
                        l.AddRange(keyValuePair.Value.Keys().Select(a => string.Concat(keyValuePair.Value.Prepend, ":", a)));
                    else
                        l.AddRange(keyValuePair.Value.Keys());
                }
                    
            }
            var where2 = lib.GroupContextData.Where(a => a.Value.Type == type);

            foreach (var keyValuePair in where2)
            {
                l.AddRange(keyValuePair.Value.Keys());
            }

            return l;
        }

        public HashSet<string> GetNameSet(string type, bool modOnly, bool allowPrepend=false, bool addPrepend=false)
        {
            HashSet<string> eventNames = new HashSet<string>();

            eventNames.UnionWith(GetNameSet(type, ModCK3Library, allowPrepend, addPrepend));
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(GetNameSet(type, BaseCK3Library, allowPrepend,addPrepend));

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
            return ModCK3Library.Get(ScriptContext.Event, name);
        }

        public string GetDirectoryFromContext(ScriptContext context)
        {
            return BaseCK3Library.ContextData[context].Directory;
        }

        public HashSet<string> GetNameSetFromEnumType(string type, bool allowPrepend=false, bool addPrepend=false)
        {

            

            //  if (type == "building_type")
            //    return GetNameSet(ScriptContext.Buildings, false);
            return GetNameSet(type, false, allowPrepend, addPrepend);

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

        public bool AnyNameLists(string name)
        {
            if (BaseCK3Library.ContextData.Any(a => a.Value.Type == name))
                return true;
            if (BaseCK3Library.GroupContextData.Any(a => a.Value.Type == name))
                return true;

            return false;
        }

        public ScriptObject Get(string id, string expectedType)
        {
            if (ModCK3Library.ContextData.Any(a => a.Value.Type == expectedType))
            {
                var l = ModCK3Library.ContextData.Where(a => a.Value.Type == expectedType).ToList();

                foreach (var keyValuePair in l)
                {
                    if (keyValuePair.Value.Has(id))
                        return keyValuePair.Value.Get(id);
                }
            }
            
            if (BaseCK3Library.ContextData.Any(a => a.Value.Type == expectedType))
            {
                var l = BaseCK3Library.ContextData.Where(a => a.Value.Type == expectedType).ToList();

                foreach (var keyValuePair in l)
                {
                    if (keyValuePair.Value.Has(id))
                        return keyValuePair.Value.Get(id);

                }
            }

            return null;

        }

        public List<SmartFindResults> DoSmartFind(SmartFindOptions options)
        {
            List<SmartFindResults> results = new List<SmartFindResults>();

            if (options.SearchBase)
            {
                BaseCK3Library.DoSmartFind(options, results);
            }
            if (options.SearchMod)
            {
                ModCK3Library.DoSmartFind(options, results);
            }

            return results;
        }


        


    }
}
