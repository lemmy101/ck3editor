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
            Enums["prison_type"] = new HashSet<string>() { "dungeon", "house_arrest" };
            Enums["sexuality"] = new HashSet<string>() { "asexual", "homosexual", "bisexual", "homosexual", "heterosexual" };
            Enums["council_task_type"] = new HashSet<string>() { "task_type_general", "task_type_county" };
            Enums["task_progress_type"] = new HashSet<string>() { "task_progress_infinite", "task_progress_value" };

            Enums["skill"] = new HashSet<string>() { "diplomacy" , "martial" , "stewardship" , "intrigue" , "learning" , "prowess" };
            Enums["county_target"] = new HashSet<string>() { "realm", "all", "neighbor_land", "domain" };
            Enums["gender"] = new HashSet<string>() { "male", "female" };

            Enums["building_type"] = new HashSet<string>() { "special", "duchy_capital" };
            Enums["greeting"] = new HashSet<string>() { "positive" };
            Enums["culture_group_type"] = new HashSet<string>() { "culture_group_military", "culture_group_regional", "culture_group_civic" };

            Enums["regiment_category"] = new HashSet<string>() { "heavy_infantry", "heavy_cavalry", "archers" };
            
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

        public HashSet<string> GetEnums(string type, bool allowPrepend=false, bool addPrepend=false)
        {
            if(type == null)
                return new HashSet<string>();
            if (!Enums.ContainsKey(type))
            {
                return Core.Instance.GetNameSetFromEnumType(type, allowPrepend,addPrepend);
            }

            return Enums[type];
        }
    }
}
