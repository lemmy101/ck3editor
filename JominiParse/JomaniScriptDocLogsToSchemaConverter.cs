using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text;
using System.Xml;

namespace JominiParse
{
    public class JomaniScriptDocLogsToSchemaConverter
    {
        public static JomaniScriptDocLogsToSchemaConverter Instance = new JomaniScriptDocLogsToSchemaConverter();
        class Effect
        {
            public string Name { get; set; }
            public string Doc { get; set; } = "";
            public bool IsList { get; set; }
            public bool RequiresData { get; set; }
            public bool GlobalLink { get; set; }
            public bool WildCard { get; set; }
            public string Type { get; set; }

            public List<string> Usage = new List<string>();
            public List<string> SupportedScopes = new List<string>();
            public readonly List<string> TargetScopes = new List<string>();
        }

        public class Modifier
        {
            public string Name { get; set; }
            public string Category { get; set; }
        }
        class Trigger
        {
            public string Name { get; set; }
            public string Doc { get; set; } = "";
            public List<string> Usage = new List<string>();
            public List<string> SupportedScopes = new List<string>();
            public List<string> TargetScopes = new List<string>();
        }
        List<Modifier> modifier = new List<Modifier>();
        List<Effect> effects = new List<Effect>();
        List<Effect> lists = new List<Effect>();
        List<Trigger> triggers = new List<Trigger>();
        public void Export(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string srcDir = Globals.CK3LogPath;

                effects.Clear();

         //       ConvertTriggers(srcDir, dir, "triggers.log");
            //    effects.Clear();
            //   ConvertEffects(srcDir, dir, "effects.log");
            //   effects.Clear();


            //  ConvertEventTargets(srcDir, dir, "event_targets.log");

         //   ConvertModifiers(srcDir, dir, "modifiers.log");

            effects.Clear();

        }

        private void ConvertModifiers(string srcDir, string destDir, string filename)
        {
            string srcFile = srcDir + filename;

            string text = System.IO.File.ReadAllText(srcFile);

            var lines = text.Replace("\r", "").Split(new char[] { '\n' });

            XmlDocument schemaBase = new XmlDocument();

            schemaBase.InnerXml = "<Schema></Schema>";

            string str = "";


            for (var index = 1; index < lines.Length; index++)
            {
                if (lines[index].Trim().Length == 0)
                    continue;
                
                string[] strl = lines[index].Split(',');
                
                strl[0] = strl[0].Replace("Tag: ", "");
                strl[1] = strl[1].Replace("Categories: ", "");

                str += "<Child name=\"" + strl[0].Trim() + "\" category=\"" + strl[1].Trim() + "\" type=\"modifier\" />\n";

            }
            schemaBase.DocumentElement.InnerXml = str;

            schemaBase.Save(destDir + "/Modifiers.xml");
        }

        private void ConvertEventTargets(string srcDir, string destDir, string filename)
        {
            string srcFile = srcDir + filename;

            string text = System.IO.File.ReadAllText(srcFile);

            var lines = text.Replace("\r", "").Split(new char[] { '\n' });


            for (var index = 1; index < lines.Length; index++)
            {
                if (index > 4530)
                {
                    int gfd = 0;
                }

                var line = lines[index];
                if (line.StartsWith("----------"))
                    continue;
                if (line.Trim().Length == 0)
                    continue;

                bool isList = false;
                if (line.StartsWith("usage:"))
                    continue;
                if (line.StartsWith("random_") || line.StartsWith("every_") || line.StartsWith("ordered_"))
                {
                    index = GotoNextEntry(index, lines);
                    continue;
                }

                var e = new Effect();

                e.Name = line.Substring(0, line.IndexOf("-")).Trim();
                e.Doc = line.Substring(line.IndexOf("-") + 1).Trim();

                index++;

                //  index = DoUsage(lines, index, e);

                index = DoWildCard(lines, index, e);
                index = DoRequiresData(lines, index, e);
                index = DoGlobalLink(lines, index, e);
                index = DoInputScopes(lines, index, e);
                index = DoOutputScopes(lines, index, e);
        
                if (isList)
                {
                    e.Name = e.Name.Substring(4);
                }


                effects.Add(e);

            }

            XmlDocument schemaBase = new XmlDocument();

            schemaBase.InnerXml = "<Schema></Schema>";

            string str = "";

            foreach (var effect in effects)
            {
                string scopesText = "";
                string targetsText = "";
                string wildCardText = "";
                string requiresData = "";
                string globalLink = "";
                

                if (effect.WildCard)
                    wildCardText = " wildcard=\"yes\"";
                if (effect.RequiresData)
                    requiresData = " requiresData=\"yes\"";
                if (effect.GlobalLink)
                    globalLink = " globalLink=\"yes\"";

                if (effect.SupportedScopes.Count > 0)
                {
                    scopesText = " supportedScope=\"" + string.Join(",", effect.SupportedScopes).Replace(",_", ",") + "\"";
                }

                if (effect.TargetScopes.Count > 0)
                {
                    targetsText = "targetScope=\"" + string.Join(",", effect.TargetScopes).Replace(",_", ",") + "\"";
                }
                if (effect.Doc.Trim().StartsWith("Unknown"))
                    effect.Doc = "";

                str += "<Child name=\"" + effect.Name + "\" " + scopesText + " " + targetsText + wildCardText + requiresData + globalLink +" doc = \"" + SecurityElement.Escape(effect.Doc) + "\"/>\n";
            }

            schemaBase.DocumentElement.InnerXml = str;

            schemaBase.Save(destDir + "/EventTargets.xml");

        }
        private void ConvertEffects(string srcDir, string destDir, string filename)
        {
            string srcFile = srcDir + filename;

            string text = System.IO.File.ReadAllText(srcFile);

            var lines = text.Replace("\r", "").Split(new char[] { '\n' });


            for (var index = 1; index < lines.Length; index++)
            {
                if (index > 4530)
                {
                    int gfd = 0;
                }

                var line = lines[index];
                if (line.StartsWith("----------"))
                    continue;
                if (line.Trim().Length == 0)
                    continue;

                bool isList = false;
                if (line.StartsWith("usage:"))
                    continue;
                if (line.StartsWith("random_") || line.StartsWith("every_") || line.StartsWith("ordered_"))
                {
                    index = GotoNextEntry(index, lines);
                    continue;
                }

                var e = new Effect();

                e.Name = line.Substring(0, line.IndexOf("-")).Trim();
                e.Doc = line.Substring(line.IndexOf("-") + 1).Trim();

                index++;

                index = DoUsage(lines, index, e);

                index = DoSupportedScopes(lines, index, e);
                index = DoTargetScopes(lines, index, e);

                if (isList)
                {
                    e.Name = e.Name.Substring(4);
                }


                effects.Add(e);

            }

            XmlDocument schemaBase = new XmlDocument();

            schemaBase.InnerXml = "<Schema></Schema>";

            string str = "";

            foreach (var effect in effects)
            {
                string scopesText = "";
                string targetsText = "";

                if (effect.SupportedScopes.Count > 0)
                {
                    scopesText = "supportedScope=\"" + string.Join(",", effect.SupportedScopes).Replace(",_", ",") + "\"";
                }
                if (effect.TargetScopes.Count > 0)
                {
                    targetsText = "targetScope=\"" + string.Join(",", effect.TargetScopes).Replace(",_", ",") + "\"";
                }

                str += "<Child name=\"" + effect.Name + "\" " + scopesText + " " + targetsText + " doc = \"" + SecurityElement.Escape(effect.Doc) + "\"/>\n";
            }

            schemaBase.DocumentElement.InnerXml = str;

            schemaBase.Save(destDir + "/Effects.xml");

        }
        private void ConvertTriggers(string srcDir, string destDir, string filename)
        {
            string srcFile = srcDir + filename;

            string text = System.IO.File.ReadAllText(srcFile);

            var lines = text.Replace("\r", "").Split(new char[] { '\n' });


            for (var index = 1; index < lines.Length; index++)
            {
                bool isList = false;
                if (index > 4530)
                {
                    int gfd = 0;
                }
                var line = lines[index];
                if (line.StartsWith("----------"))
                    continue;
                if (line.Trim().Length == 0)
                    continue;

                if (line.StartsWith("usage:"))
                    continue;
                if (line.StartsWith("any_"))
                {
                    index = GotoNextEntry(index, lines);
                    isList = true;
                    continue;
                }

                var e = new Effect();

                e.Name = line.Substring(0, line.IndexOf("-")).Trim();

                if (e.Name == "any_kingdom")
                {

                }


                e.Doc = line.Substring(line.IndexOf("-") + 1).Trim();

                index++;

                index = DoUsage(lines, index, e);

                index = DoTraits(lines, index, e);

                index = DoSupportedScopes(lines, index, e);
                index = DoTargetScopes(lines, index, e);

                if (isList)
                {
                    e.Name = e.Name.Substring(4);
                    e.IsList = true;
                }

                effects.Add(e);
            }

            XmlDocument schemaBase = new XmlDocument();

            XmlDocument schemaBase2 = new XmlDocument();

            schemaBase.InnerXml = "<Schema></Schema>";
            schemaBase2.InnerXml = "<Schema></Schema>";

            string str = "";
            string str2 = "";

            foreach (var effect in effects)
            {
                string scopesText = "";
                string targetsText = "";
                string isListText = "";

                if (effect.SupportedScopes.Count > 0)
                {
                    scopesText = " supportedScope=\"" + string.Join(",", effect.SupportedScopes).Replace(",_", ",") + "\"";
                }
                if (effect.TargetScopes.Count > 0)
                {
                    targetsText = " targetScope=\"" + string.Join(",", effect.TargetScopes).Replace(",_", ",") + "\"";
                }

                if (effect.IsList)
                {
                    isListText = " scriptList=\"yes\"";
                    str2 += "<Child name=\"" + effect.Name + "\"" + scopesText + targetsText + isListText + " doc = \"" + SecurityElement.Escape(effect.Doc) + "\"/>\n";
                }
                else
                {
                    string typeText = " type=\"\"";

                    if (effect.Type != null)
                        typeText = " type=\"" + SecurityElement.Escape(effect.Type) + "\"";
                    str += "<Child name=\"" + effect.Name + "\"" + scopesText + targetsText + typeText + " doc = \"" + SecurityElement.Escape(effect.Doc) + "\"/>\n";
                }
            }

            schemaBase.DocumentElement.InnerXml = str;
            schemaBase2.DocumentElement.InnerXml = str2;

            schemaBase.Save(destDir + "/Triggers2.xml");
          //  schemaBase2.Save(destDir + "/ScriptLists.xml");

        }

        private int DoTargetScopes(string[] lines, int index, Effect effect)
        {
            index = SkipBlank(lines, index, effect);
            if (index >= lines.Length)
                return index;

            string line = lines[index];


            // is a usage line...
            if (line.StartsWith("Supported Targets: "))
            {
                effect.TargetScopes.Add(line.Replace("Supported Targets: ", "").Trim().Replace(" ", "_"));
                index++;
                index = SkipBlank(lines, index, effect);
                if (index >= lines.Length)
                    return index;
                line = lines[index];
            }
            return index;
        }

        private int SkipBlank(string[] lines, int index, Effect effect)
        {
            while (index < lines.Length && (lines[index].Trim().Length == 0 || NotAcceptStart(effect, lines[index])))
            {
                index++;
            }

            return index;
        }

        private bool NotAcceptStart(Effect effect, string text)
        {
            if (text.StartsWith(effect.Name))
                return false;
            if (text.StartsWith("Supported Scopes: "))
                return false;
            if (text.StartsWith("Traits: "))
                return false;
            if (text.StartsWith("----"))
                return false;
            if (text.StartsWith("Supported Targets: "))
                return false;
            if (text.StartsWith("Input Scopes: "))
                return false;
            if (text.StartsWith("Output Scopes: "))
                return false;
            if (text.StartsWith("Global Link: "))
                return false;
            if (text.StartsWith("Wild Card: "))
                return false;
             if (text.StartsWith("Requires Data: "))
                return false;
            return true;
        }

        private int DoGlobalLink(string[] lines, int index, Effect effect)
        {
            index = SkipBlank(lines, index, effect);
            if (index >= lines.Length)
                return index;
            string line = lines[index];

            // is a usage line...
            if (line.StartsWith("Global Link: "))
            {
                effect.GlobalLink = true;
                index++;
                index = SkipBlank(lines, index, effect);
                if (index >= lines.Length)
                    return index;
                line = lines[index];
            }
            return index;
        }
        private int DoWildCard(string[] lines, int index, Effect effect)
        {
            index = SkipBlank(lines, index, effect);
            if (index >= lines.Length)
                return index;
            string line = lines[index];

            // is a usage line...
            if (line.StartsWith("Wild Card: "))
            {
                effect.WildCard = true;
                index++;
                index = SkipBlank(lines, index, effect);
                if (index >= lines.Length)
                    return index;
                line = lines[index];
            }
            return index;
        }
        private int DoRequiresData(string[] lines, int index, Effect effect)
        {
            index = SkipBlank(lines, index, effect);
            if (index >= lines.Length)
                return index;
            string line = lines[index];

            // is a usage line...
            if (line.StartsWith("Requires Data: "))
            {
                effect.RequiresData = true;
                index++;
                index = SkipBlank(lines, index, effect);
                if (index >= lines.Length)
                    return index;
                line = lines[index];
            }
            return index;
        }
        
        private int DoInputScopes(string[] lines, int index, Effect effect)
        {
            index = SkipBlank(lines, index, effect);
            if (index >= lines.Length)
                return index;
            string line = lines[index];

            // is a usage line...
            if (line.StartsWith("Input Scopes: "))
            {
                effect.SupportedScopes.Add(line.Replace("Input Scopes: ", "").Replace(" ", "_"));
                index++;
                index = SkipBlank(lines, index, effect);
                if (index >= lines.Length)
                    return index;
                line = lines[index];
            }
            return index;
        }
        private int DoOutputScopes(string[] lines, int index, Effect effect)
        {
            index = SkipBlank(lines, index, effect);
            if (index >= lines.Length)
                return index;
            string line = lines[index];

            // is a usage line...
            if (line.StartsWith("Output Scopes: "))
            {
                effect.TargetScopes.Add(line.Replace("Output Scopes: ", "").Replace(" ", "_"));
                index++;
                index = SkipBlank(lines, index, effect);
                if (index >= lines.Length)
                    return index;
                line = lines[index];
            }
            return index;
        }
        private int DoSupportedScopes(string[] lines, int index, Effect effect)
        {
            index = SkipBlank(lines, index, effect);
            if (index >= lines.Length)
                return index;
            string line = lines[index];

            // is a usage line...
            if (line.StartsWith("Supported Scopes: "))
            {
                effect.SupportedScopes.Add(line.Replace("Supported Scopes: ", "").Replace(" ", "_"));
                index++;
                index = SkipBlank(lines, index, effect);
                if (index >= lines.Length)
                    return index;
                line = lines[index];
            }
            return index;
        }
        private int DoUsage(string[] lines, int index, Effect effect)
        {

            index = SkipBlank(lines, index, effect);
            if (index >= lines.Length)
                return index;
            string line = lines[index];
            // is a usage line...
            while (line.StartsWith(effect.Name))
            {
                effect.Usage.Add(line);
                index++;
                index = SkipBlank(lines, index, effect);
                if (index >= lines.Length)
                    return index;
                line = lines[index];
            }
            return index;
        }
        private int DoTraits(string[] lines, int index, Effect effect)
        {

            index = SkipBlank(lines, index, effect);
            if (index >= lines.Length)
                return index;
            string line = lines[index];
            // is a usage line...
            if (line.StartsWith("Traits: "))
            {
                if (line.Contains("<, <="))
                    effect.Type = "value";
                else if (line.Contains("yes/no"))
                    effect.Type = "bool";
                else effect.Type = line.Replace("Traits: ", "");
                index++;
                index = SkipBlank(lines, index, effect);
                if (index >= lines.Length)
                    return index;
                line = lines[index];
            }
            return index;
        }

        private int GotoNextEntry(int index, string[] lines)
        {
            while (index < lines.Length && !lines[index].StartsWith("---"))
                index++;

            return index;
        }
    }
}
