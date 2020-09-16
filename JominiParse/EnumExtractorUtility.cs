using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JominiParse
{
    public class EnumExtractorUtility
    {
        public static EnumExtractorUtility Instance = new EnumExtractorUtility();

        public List<string> commandsValuesToExtract = new List<string>()
        {
        //    "has_doctrine_parameter",

        };

        public Dictionary<string, string> childExtractor = new Dictionary<string, string>()
        {
            { "compare_modifier","value" }
        };

        HashSet<string> toExport = new HashSet<string>();
        public void Add(ScriptObject obj)
        {
            if (obj.Name != null && commandsValuesToExtract.Contains(obj.Name))
            {
                toExport.Add(obj.GetStringValue());
            }
            if (obj.Name != null && childExtractor.ContainsKey(obj.Name))
            {
                toExport.Add(obj.GetChildStringValue(childExtractor[obj.Name]));
            }

        }

        public void Export(string filename)
        {
            if (commandsValuesToExtract.Count > 0 || childExtractor.Count > 0)
            {
                var l = toExport.OrderBy(a => a).ToList();
                using (StreamWriter writer = File.CreateText(filename))
                {
                    foreach (var e in l)
                    {
                        writer.WriteLine(e);
                    }
                }


            }
        }
    }
}
