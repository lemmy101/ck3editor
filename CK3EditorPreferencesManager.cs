#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using JominiParse;

#endregion

namespace CK3ScriptEditor
{
    public class CK3EditorPreferencesManager
    {
        public static CK3EditorPreferencesManager Instance = new CK3EditorPreferencesManager();

        public void Save()
        {
            {
                if (!Directory.Exists(Globals.CK3EdDataPath))
                    Directory.CreateDirectory(Globals.CK3EdDataPath);
                var filename = Globals.CK3EdDataPath + "Preferences.txt";

                using (TextWriter writeFile = new StreamWriter(filename))
                {
                    writeFile.WriteLine("FontSize=" + EditorGlobals.FontSize);

                }
            }

            {
                if (!Directory.Exists(Globals.CK3EdDataPath))
                    Directory.CreateDirectory(Globals.CK3EdDataPath);
                var filename = Globals.CK3EdDataPath + "Directories.txt";

                using (TextWriter writeFile = new StreamWriter(filename))
                {
                    writeFile.WriteLine(Globals.CK3Path);
                    writeFile.WriteLine(Globals.CK3DocPath);
                }
            }
            if (Core.Instance.ModCK3Library != null)
            {
                var filename = Globals.CK3EdDataPath + "LoadedMod.txt";

                using (TextWriter writeFile = new StreamWriter(filename))
                {
                    writeFile.WriteLine(Core.Instance.ModCK3Library.Name);
                }
            }

            if (CK3ScriptEd.Instance != null)
            {
                var filename = Globals.CK3EdDataPath + Core.Instance.ModCK3Library.Name + "_OpenFileWindows.txt";
                var files = CK3ScriptEd.Instance.GetOpenWindowsFilenameList();
                if (files == null)
                    return;

                using (TextWriter writeFile = new StreamWriter(filename))
                {
                    foreach (var file in files) writeFile.WriteLine(file);

                    var index = CK3ScriptEd.Instance.GetOpenWindowsFilenameListIndex();

                    index = Math.Max(0, index);
                    writeFile.WriteLine("openIndex:" + index);
                }
            }
        }

        public void LoadDirectories()
        {
            {
                var filename = Globals.CK3EdDataPath + "Directories.txt";

                if (!File.Exists(filename))
                    return;

                var text = File.ReadAllText(filename);

                var lines = text.Replace("\r", "").Split('\n');
                Globals.CK3Path = lines[0].Trim();
                Globals.CK3DocPath = lines[1].Trim();
            }
            {
                var filename = Globals.CK3EdDataPath + "Preferences.txt";

                if (!File.Exists(filename))
                    return;

                var text = File.ReadAllText(filename);

                var lines = text.Replace("\r", "").Split('\n');

                foreach (var line in lines)
                {
                    string[] split = line.Split('=');
                    if (split.Length < 2)
                        continue;

                    if (split[0] == "FontSize")
                    {
                        FontSize font = FontSize.Normal;
                        if (Enum.TryParse(split[1].Trim(), out font))
                        {
                            EditorGlobals.FontSize = font;
                        }
                    }
                }
            }
        }

        public void Load()
        {
          


            if (Core.Instance.ModCK3Library == null)
            {
                var filename = Globals.CK3EdDataPath + "LoadedMod.txt";

                if (!File.Exists(filename))
                    return;

                var text = File.ReadAllText(filename);

                var lines = text.Replace("\r", "").Split('\n');

                if (Directory.Exists(Globals.CK3ModPath + lines[0].Trim()))
                    CK3ScriptEd.Instance.Load(lines[0].Trim());
                else
                    return;
            }

            if (Core.Instance.ModCK3Library == null)
                return;

            LoadWindows();
        }

        public void LoadWindows()
        {
            if (Core.Instance.ModCK3Library == null)
                return;

            {
                var filename = Globals.CK3EdDataPath + Core.Instance.ModCK3Library.Name + "_OpenFileWindows.txt";

                if (!File.Exists(filename))
                    return;

                var text = File.ReadAllText(filename);

                var lines = text.Replace("\r", "").Split('\n');
                var windows = new List<Control>();
                for (var index = 0; index < lines.Length; index++)
                {
                    var line = lines[index];
                    if (line.Trim().Length == 0)
                        continue;

                    line = line.Replace("\\", "/");

                    if (line.StartsWith("openIndex:"))
                    {
                        var i = Convert.ToInt32(line.Substring(line.IndexOf(':') + 1));
                        i = Math.Max(i, 0);
                        if (windows.Count > i)
                            CK3ScriptEd.Instance.SetActiveEditor(
                                windows[i] as TextEditorControl);
                    }
                    else
                    {
                        var fromBase = line.Split(':')[0] == "base";

                        if (fromBase)
                        {
                            var f = Globals.CK3Path + filename;
                            if (!File.Exists(f))
                                continue;
                        }
                        else
                        {
                            var f = Core.Instance.ModCK3Library.Path + filename;
                            if (!File.Exists(f))
                                continue;
                        }

                        windows.Add(CK3ScriptEd.Instance.GetTextEditor(new RefFilename(line.Split(':')[1], fromBase)));
                    }
                }
            }
        }
    }
}