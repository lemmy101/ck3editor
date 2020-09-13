using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JominiParse
{
    public class EnumManager
    {
        public static EnumManager Instance = new EnumManager();
        Dictionary<string, List<string>> Enums = new Dictionary<string, List<string>>();
        public void Load()
        {
            var files = Directory.GetFiles("./HardcodedEnums");

            foreach (var file in files)
            {
                Load(file);
            }
        }

        private void Load(string file)
        {
            string text = System.IO.File.ReadAllText(file).Replace("\r", "");

            string[] strs = text.Split('\n');


            Enums[file.Substring(file.LastIndexOf("\\") + 1).Replace(".txt", "")] = strs.ToList();
        }

        public List<string> GetEnums(string type)
        {
            if(!Enums.ContainsKey(type))
                return new List<string>();

            return Enums[type];
        }
    }
}
