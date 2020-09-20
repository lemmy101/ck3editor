using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JominiParse
{
    public class LocalizationEntry
    {
        public string tag;
        public string english;
    }
    public class LocalizationParser
    {
        public Dictionary<string, LocalizationEntry> Load(string dir)
        {
            Dictionary<string, LocalizationEntry> map = new Dictionary<string, LocalizationEntry>();

            string odir = dir;
            LoadLocalizationDir(odir+"/", dir, map);

            return map;
        }

        public Dictionary<string, List<LocalizationEntry>> LocalizationFiles = new Dictionary<string, List<LocalizationEntry>>();

        private void LoadLocalizationDir(string odir, string dir, Dictionary<string, LocalizationEntry> results)
        {
            if (!Directory.Exists(dir))
                return;

            var dirs = Directory.GetDirectories(dir);

            foreach (var s in dirs)
            {
                LoadLocalizationDir(odir, s, results);
            }

            var files = Directory.GetFiles(dir);

            foreach (var file in files)
            {
                if (file.EndsWith(".yml"))
                    LoadLocalizationFile(odir, file, results);
            }
        }

        private void LoadLocalizationFile(string odir, string file, Dictionary<string, LocalizationEntry> results)
        {
            string text = System.IO.File.ReadAllText(file);
            string[] lines = text.Split(new char[] { '\n' });
            file = file.Replace("\\", "/");

            LocalizationFiles[file.Replace(odir, "")] = new List<LocalizationEntry>();

            for (int i = 1; i < lines.Length; i++)
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

                LocalizationEntry e = new LocalizationEntry() {tag = tag, english = str};

                results[tag] = e;
                LocalizationFiles[file.Replace(odir, "")].Add(e);
            }
        }
    }
}
