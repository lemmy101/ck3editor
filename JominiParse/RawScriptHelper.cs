﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JominiParse
{
    public class RawScriptHelper
    {
        public static bool DoScriptsMatchIgnoreWhitespace(List<string> a, List<string> b)
        {
            int bIndex = 0;
            for (var index = 0; index < a.Count; index++)
            {
                var s = a[index];
                if (s.Trim().Length == 0)
                {
                    continue;
                }

                if (s.Trim() != b[bIndex].Trim())
                {
                    if(b[bIndex].Trim().Length > 0)
                        return false;
                    else
                    {
                        while (b[bIndex].Trim().Length == 0)
                        {
                            bIndex++;
                        }
                    }
                    
                    if (bIndex >= b.Count || s.Trim() != b[bIndex].Trim())
                        return false;

                    bIndex++;
                }
                else
                {
                    bIndex++;
                }
            }

            if (bIndex < b.Count-1)
            {
                while (bIndex < b.Count - 1)
                {
                    bIndex++;
                    if (b[bIndex].Trim().Length > 0)
                        return false;
                }
            }
            return true;
        }
        public static List<string> GetScriptFromFile(ScriptFile file, int startLine = 0, int endLine = -1)
        {
            string fullPath = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/" + file.Filename;
            string fullBasePath = Globals.CK3Path + file.Filename;

            if (file.IsBase)
                fullPath = fullBasePath;

            string text = System.IO.File.ReadAllText(fullPath);

            var lines = text.Split(new char[] { '\n' }).ToList();

            int from = startLine-1;
            int to = endLine - 1;
            if (endLine == -1)
                to = lines.Count-1;

            var toImplant = lines.GetRange(from, (to - from) + 1);

            return toImplant;
        }

        public static List<string> GetScriptFromFile(ScriptObject scriptObject)
        {
            return GetScriptFromFile(scriptObject.ScriptFile, scriptObject.LineStart, scriptObject.LineEnd);
        }
        public static List<string> GetScriptFromFileFromBase(ScriptObject scriptObject)
        {
            var baseFile = Core.Instance.BaseCK3Library.GetFile(scriptObject.ScriptFile.Filename);

            if(baseFile==null)
                return new List<string>();

            if(!baseFile.Map.ContainsKey(scriptObject.Name))
                return new List<string>();

            scriptObject = baseFile.Map[scriptObject.Name];

            var lines = GetScriptFromFile(scriptObject.ScriptFile, scriptObject.LineStart, scriptObject.LineEnd);

            return lines;
        }

        public static void SaveScriptObjectsToFile(ScriptFile scriptFile, List<ScriptObject> obj, string filename)
        {
            filename = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/" + filename;
            List<string> newFileLines = new List<string>();
            if(scriptFile.Namespace != null)
            {
                newFileLines.Add("namespace = " + scriptFile.Namespace);
                newFileLines.Add("\n");
            }
            foreach (var scriptObject in obj)
            {
                newFileLines.AddRange(GetScriptFromFile(scriptObject));
                newFileLines.Add("\n");
            }

            using (FileStream fs = new FileStream(filename, FileMode.Create))
            {
                // create a new file....
                using (StreamWriter outputFile = new StreamWriter(fs, Encoding.UTF8))
                {
                    for (var index = 0; index < newFileLines.Count; index++)
                    {
                        string line = newFileLines[index];
                        line = line.Replace("\r", "");
                        outputFile.WriteLine(line);
                    }
                }
            }
        }
    }
}
