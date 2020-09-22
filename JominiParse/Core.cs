#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#endregion

namespace JominiParse
{
    public class Core
    {
        public static Core Instance = new Core();

        public HashSet<string> _cachedScriptedEffects = null;
        public HashSet<string> _cachedScriptedTriggers = null;

        public ScriptLibrary BaseCK3Library; // = new ScriptLibrary();

        public List<ScriptObject> BehaviourRecalculateList = new List<ScriptObject>();

        public List<ScriptObject> DeferedInitializationList = new List<ScriptObject>();
        public List<ScriptObject> DeferedPostInitializationList = new List<ScriptObject>();
        public List<ScriptObject> DeferedPostInitializationListNext = new List<ScriptObject>();
        public ScriptLibrary LoadingCK3Library;
        public ScriptLibrary ModCK3Library; // = new ScriptLibrary();


        public Core()
        {
            Wipe();
        }

        private void Wipe()
        {
            SchemaManager.Instance = new SchemaManager();
            CoreIntellisenseHandler.Instance = new CoreIntellisenseHandler();
            EnumExtractorUtility.Instance = new EnumExtractorUtility();
            EnumManager.Instance = new EnumManager();
            FileTokenizer.Instance = new FileTokenizer();
            JomaniScriptDocLogsToSchemaConverter.Instance = new JomaniScriptDocLogsToSchemaConverter();
            ReferenceManager.Instance = new ReferenceManager();
            BaseCK3Library = null;
            ModCK3Library = null;
            LoadingCK3Library = null;
        }

        public void Init()
        {
            Wipe();

            SchemaManager.Instance.Init();
            BaseCK3Library = new ScriptLibrary();
            ModCK3Library = new ScriptLibrary();
            ModCK3Library.Parent = BaseCK3Library;
            LoadingCK3Library = BaseCK3Library;

            EnumManager.Instance.Load();
            LoadCK3Scripts(BaseCK3Library);
            ScriptObject.ClearCachedScriptedEffects();

            ProcessBaseFileBehaviour();

            SchemaManager.Instance.SaveBinary();
        }

        private void ProcessBaseFileBehaviour()
        {
            var binFilename = "behaviourData.bin";
            binFilename = Globals.CK3EdDataPath.Replace("\\", "/") + binFilename;
            BinaryReader reader = null;
            BinaryWriter writer = null;
            if (File.Exists(binFilename))
                reader = new BinaryReader(File.Open(binFilename, FileMode.Open));
            else
                writer = new BinaryWriter(File.Open(binFilename, FileMode.Create));

            if (reader != null)
            {
                var v = reader.ReadInt32();
                if (v != Globals.DataVersion)
                {
                    reader.Close();
                    reader = null;
                    writer = new BinaryWriter(File.Open(binFilename, FileMode.Create));
                }
            }

            if (writer != null) writer.Write(Globals.DataVersion);

            PostInitialize(writer, reader);

            if (writer != null)
            {
                writer.Flush();
                writer.Close();
            }

            if (reader != null) reader.Close();
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

            LoadingCK3Library = ModCK3Library;
            LoadCK3Scripts(ModCK3Library, false, false);

            PostInitialize();

            RePostProcessUntilComplete();

            ScriptObject.ClearCachedScriptedEffects();
        }

        private void RePostProcessUntilComplete()
        {
            var list = Instance.BehaviourRecalculateList.ToList();
            while (list.Count > 0)
            {
                foreach (var scriptObject in list) scriptObject.PostInitialize(null, null);
                var n = list.Count;
                list = Instance.BehaviourRecalculateList.ToList();
                Instance.BehaviourRecalculateList.Clear();
                if (list.Count == n)
                    // unable to resolve any more
                    break;
            }

            Instance.BehaviourRecalculateList.Clear();
        }

        public string GetLocalizedText(string tag)
        {
            return ModCK3Library.GetLocalizedText(tag);
        }

        public bool HasLocalizedText(string tag)
        {
            return ModCK3Library.HasLocalizedText(tag);
        }

        public ScriptContext GetContextFromDirectory(RefFilename dir)
        {
            var res = BaseCK3Library.ContextData
                .Where(a => a.Value.Directory != null && dir.ToRelativeFilename().StartsWith(a.Value.Directory))
                .ToList();

            if (res.Any()) return res[0].Key;

            return ScriptContext.Event;
        }

        public void LoadCK3BaseFile(RefFilename filename)
        {
            if (BaseCK3Library.HasFile(filename))
                return;

            LoadingCK3Library = BaseCK3Library;

            var context = GetContextFromDirectory(filename);

            var results = FileTokenizer.Instance.LoadFile(filename, context, true);

            BaseCK3Library.Add(results, context);
        }

        public bool LoadCK3File(RefFilename filename, bool forceBase = false, bool forceReload = false)
        {
            var fromBase = filename.IsBase;

            LoadingCK3Library = fromBase ? BaseCK3Library : ModCK3Library;
            if (!forceReload)
            {
                if (fromBase)
                {
                    if (BaseCK3Library.HasFile(filename))
                        return fromBase;
                }
                else
                {
                    if (ModCK3Library.HasFile(filename))
                        return fromBase;
                }
            }
            else
            {
                LoadingCK3Library.ClearFile(filename);
            }


            // string directory = filename.Substring(0, Math.Max(filename.LastIndexOf("/"), 0));

            var context = GetContextFromDirectory(filename);

            var results = FileTokenizer.Instance.LoadFile(filename, context, true);

            LoadingCK3Library.Add(results, context);

            PostInitialize();

            return fromBase;
        }

        public ScriptFile GetFile(RefFilename file, bool isBaseFile)
        {
            if (isBaseFile)
                return BaseCK3Library.GetFile(file);

            return ModCK3Library.GetFile(file);
        }

        public ScriptFile GetFile(RefFilename file)
        {
            return ModCK3Library.GetFile(file);
        }


        public void PostInitialize(BinaryWriter writer = null, BinaryReader reader = null)
        {
            do
            {
                Instance.DeferedPostInitializationList = Instance.DeferedPostInitializationListNext;
                Instance.DeferedPostInitializationListNext = new List<ScriptObject>();
                for (var i = 0; i < Instance.DeferedPostInitializationList.Count; i++)
                {
                    var scriptObject = Instance.DeferedPostInitializationList[i];
                    scriptObject.PostInitialize(writer, reader);
                }

                Instance.DeferedPostInitializationList.Clear();
            } while (Instance.DeferedPostInitializationListNext.Count > 0);
        }

        public void LoadCK3Scripts(ScriptLibrary lib, bool save = true, bool load = true)
        {
            LoadingCK3Library = lib;

            // events...
            var startDir = lib.Path; //"D:/SteamLibrary/steamapps/common/Crusader Kings III/";

            LoadingCK3Library.LoadLocalizations(startDir + "localization/english");

            for (var x = 0; x < (int) ScriptContext.Max; x++)
                if (LoadingCK3Library.ContextData.ContainsKey((ScriptContext) x))
                    if (!string.IsNullOrEmpty(LoadingCK3Library.ContextData[(ScriptContext) x].Directory))
                    {
                        var info = LoadingCK3Library.ContextData[(ScriptContext) x];
                        if (info.Directory.EndsWith(".txt"))
                        {
                            var r = FileTokenizer.Instance.LoadFile(
                                new RefFilename(info.Directory, lib == BaseCK3Library),
                                (ScriptContext) x, save);
                            LoadingCK3Library.Add(r, (ScriptContext) x);
                        }
                        else
                        {
                            var r = FileTokenizer.Instance.LoadDirectory(
                                new RefFilename(info.Directory + "/", lib == BaseCK3Library),
                                (ScriptContext) x, save, load);
                            LoadingCK3Library.Add(r, (ScriptContext) x);
                        }
                    }


            for (var i = 0; i < Instance.DeferedInitializationList.Count; i++)
            {
                var scriptObject = Instance.DeferedInitializationList[i];

                scriptObject.Initialize();
            }

            Instance.DeferedInitializationList.Clear();

            LoadingCK3Library.RecalculateGroups();
        }

        public void UpdateFile(RefFilename filename, string text)
        {
            VariableStore.Instance.RemoveAllVariablesFromFile(filename);
            Instance.DeferedPostInitializationListNext.Clear();

            ModCK3Library.ClearFile(filename);

            LoadingCK3Library = ModCK3Library;

            var c = GetContextFromDirectory(filename);

            try
            {
                var results = FileTokenizer.Instance.LoadText(text, filename, c);

                ModCK3Library.Add(results, c);
            }
            catch (Exception e)
            {
            }

            try
            {
                for (var i = 0; i < Instance.DeferedPostInitializationListNext.Count; i++)
                {
                    var scriptObject = Instance.DeferedPostInitializationListNext[i];
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
                    Instance.DeferedPostInitializationList = Instance.DeferedPostInitializationListNext;
                    Instance.DeferedPostInitializationListNext = new List<ScriptObject>();
                    for (var i = 0; i < Instance.DeferedPostInitializationList.Count; i++)
                    {
                        var scriptObject = Instance.DeferedPostInitializationList[i];
                        scriptObject.PostInitialize(null, null);
                    }

                    Instance.DeferedPostInitializationList.Clear();
                } while (Instance.DeferedPostInitializationListNext.Count > 0);
            }
            catch (Exception e)
            {
            }


            ScriptObject.ClearCachedScriptedEffects();
        }


        public HashSet<string> GetGroupNameList(ScriptGroupContext context, bool modOnly)
        {
            var eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.GroupContextData[context].Keys());
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.GroupContextData[context].Keys());

            return eventNames;
        }

        public HashSet<string> GetNameSet(ScriptContext context, bool modOnly)
        {
            var eventNames = new HashSet<string>();

            eventNames.UnionWith(ModCK3Library.ContextData[context].Keys());
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(BaseCK3Library.ContextData[context].Keys());

            return eventNames;
        }

        public List<string> GetNameSet(string type, ScriptLibrary lib, bool allowPrepend = false,
            bool addPrepend = false)
        {
            var l = new List<string>();
            var where = lib.ContextData.Where(a => a.Value.Type == type);


            foreach (var keyValuePair in where)
                if (keyValuePair.Value.Prepend != null && !allowPrepend)
                {
                }
                else
                {
                    if (keyValuePair.Value.Prepend != null && addPrepend)
                        l.AddRange(keyValuePair.Value.Keys()
                            .Select(a => string.Concat(keyValuePair.Value.Prepend, ":", a)));
                    else
                        l.AddRange(keyValuePair.Value.Keys());
                }

            var where2 = lib.GroupContextData.Where(a => a.Value.Type == type);

            foreach (var keyValuePair in where2) l.AddRange(keyValuePair.Value.Keys());

            return l;
        }

        public HashSet<string> GetNameSet(string type, bool modOnly, bool allowPrepend = false, bool addPrepend = false)
        {
            var eventNames = new HashSet<string>();

            eventNames.UnionWith(GetNameSet(type, ModCK3Library, allowPrepend, addPrepend));
            if (modOnly)
                return eventNames;

            eventNames.UnionWith(GetNameSet(type, BaseCK3Library, allowPrepend, addPrepend));

            return eventNames;
        }


        public ScriptObject Get(ScriptContext context, string name, bool forceBase = false)
        {
            var lib = ModCK3Library;

            if (forceBase)
                lib = BaseCK3Library;

            var res = lib.Get(context, name);

            if (res == null)
                if (lib.Parent != null)
                    return lib.Parent.Get(context, name);

            return res;
        }

        public ScriptObject Get(string name, bool forceBase = false)
        {
            var lib = ModCK3Library;

            if (forceBase)
                lib = BaseCK3Library;

            var res = lib.Get(name);

            if (res == null)
                if (lib.Parent != null)
                    return lib.Parent.Get(name);

            return res;
        }

        public ScriptObject GetEvent(string name)
        {
            return ModCK3Library.Get(ScriptContext.Event, name);
        }

        public RefFilename GetBaseDirectoryFromContext(ScriptContext context)
        {
            return new RefFilename(BaseCK3Library.ContextData[context].Directory, true);
        }

        public RefFilename GetModDirectoryFromContext(ScriptContext context)
        {
            return new RefFilename(BaseCK3Library.ContextData[context].Directory, false);
        }

        public HashSet<string> GetNameSetFromEnumType(string type, bool allowPrepend = false, bool addPrepend = false)
        {
            //  if (type == "building_type")
            //    return GetNameSet(ScriptContext.Buildings, false);
            return GetNameSet(type, false, allowPrepend, addPrepend);
        }

        public HashSet<string> LocalVarListFromObjectFile(ScriptObject o)
        {
            var s = new HashSet<string>();
            var v = o.Topmost.ScriptFile.LocalVarNamelist(o.GetVarType());
            foreach (var d in v) s.Add(d);

            return s;
        }

        public HashSet<string> LocalVarListFromObjectFile(ScriptObject o, ScopeVarType type)
        {
            var s = new HashSet<string>();
            var v = o.Topmost.ScriptFile.LocalVarNamelist(o.GetVarType());
            foreach (var d in v) s.Add(d);

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
                    if (keyValuePair.Value.Has(id))
                        return keyValuePair.Value.Get(id);
            }

            if (BaseCK3Library.ContextData.Any(a => a.Value.Type == expectedType))
            {
                var l = BaseCK3Library.ContextData.Where(a => a.Value.Type == expectedType).ToList();

                foreach (var keyValuePair in l)
                    if (keyValuePair.Value.Has(id))
                        return keyValuePair.Value.Get(id);
            }

            return null;
        }

        public List<SmartFindResults> DoSmartFind(SmartFindOptions options)
        {
            var results = new List<SmartFindResults>();

            if (options.SearchBase) BaseCK3Library.DoSmartFind(options, results);

            if (options.SearchMod) ModCK3Library.DoSmartFind(options, results);

            return results;
        }
    }
}