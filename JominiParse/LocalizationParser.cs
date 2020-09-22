#region

using System.Collections.Generic;
using System.IO;

#endregion

namespace JominiParse
{
    public class LocalizationEntry
    {
        public string english;
        public string tag;
    }

    public class LocalizationParser
    {
        public Dictionary<string, List<LocalizationEntry>> LocalizationFiles =
            new Dictionary<string, List<LocalizationEntry>>();

        public Dictionary<string, LocalizationEntry> Load(string dir)
        {
            var map = new Dictionary<string, LocalizationEntry>();

            var odir = dir;
            LoadLocalizationDir(odir + "/", dir, map);

            return map;
        }

        private void LoadLocalizationDir(string odir, string dir, Dictionary<string, LocalizationEntry> results)
        {
            if (!Directory.Exists(dir))
                return;

            var dirs = Directory.GetDirectories(dir);

            foreach (var s in dirs) LoadLocalizationDir(odir, s, results);

            var files = Directory.GetFiles(dir);

            foreach (var file in files)
                if (file.EndsWith(".yml"))
                    LoadLocalizationFile(odir, file, results);
        }

        private void LoadLocalizationFile(string odir, string file, Dictionary<string, LocalizationEntry> results)
        {
            var text = File.ReadAllText(file);
            var lines = text.Split('\n');
            file = file.Replace("\\", "/");

            LocalizationFiles[file.Replace(odir, "")] = new List<LocalizationEntry>();

            for (var i = 1; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (line.StartsWith("#"))
                    continue;

                //  line = line.Split('#')[0].Trim();
                if (line.Length == 0)
                    continue;

                var tag = line.Substring(0, line.IndexOf(":"));
                var str = line.Substring(line.IndexOf(" ") + 2).Trim();
                str = str.Substring(0, str.Length - 1);

                var e = new LocalizationEntry {tag = tag, english = str};

                results[tag] = e;
                LocalizationFiles[file.Replace(odir, "")].Add(e);
            }
        }
    }
}