using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace JominiParse
{
    public class EnumManager
    {
        public static EnumManager Instance = new EnumManager();
        Dictionary<string, HashSet<string>> Enums = new Dictionary<string, HashSet<string>>();
        public void Load()
        {
            var files = Directory.GetFiles("./HardcodedEnums");

            foreach (var file in files)
            {
                Load(file);
            }

            Enums["bool"] = new HashSet<string>() { "yes", "no" };
            Enums["event_type"] = new HashSet<string>() { "character_event", "letter_event" };
        }

        private void Load(string file)
        {
            string text = System.IO.File.ReadAllText(file).Replace("\r", "");

            string[] strs = text.Split('\n');

            var l = strs.ToList();

            HashSet<string> m = new HashSet<string>();

            m.UnionWith(l);

            Enums[file.Substring(file.LastIndexOf("\\") + 1).Replace(".txt", "")] = m;
        }

        public HashSet<string> GetEnums(string[] types)
        {
            HashSet<string> results = new HashSet<string>();

            foreach (var type in types)
            {
                results.UnionWith(GetEnums(type));
            }

           
            return results;
        }

        public HashSet<string> GetEnums(string type, bool allowPrepend=false)
        {
            if (!Enums.ContainsKey(type))
            {
                return Core.Instance.GetNameSetFromEnumType(type, allowPrepend);
            }

            return Enums[type];
        }
    }
}
