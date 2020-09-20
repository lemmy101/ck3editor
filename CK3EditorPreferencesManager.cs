using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ICSharpCode.TextEditor;
using JominiParse;

namespace CK3ScriptEditor
{
    public class CK3EditorPreferencesManager
    {
        public static CK3EditorPreferencesManager Instance = new CK3EditorPreferencesManager();

        public void Save()
        {
            {
                string filename = Globals.CK3EdDataPath + "Directories.txt";

                using (System.IO.TextWriter writeFile = new StreamWriter(filename))
                {
                    writeFile.WriteLine(Globals.CK3Path);
                    writeFile.WriteLine(Globals.CK3DocPath);
                }
            }
            if(Core.Instance.ModCK3Library != null)
            {
                string filename = Globals.CK3EdDataPath + "LoadedMod.txt";

                using (System.IO.TextWriter writeFile = new StreamWriter(filename))
                {
                    writeFile.WriteLine(Core.Instance.ModCK3Library.Name);
                }
            }
            if (CK3ScriptEd.Instance != null)
            {
                string filename = Globals.CK3EdDataPath + Core.Instance.ModCK3Library.Name + "_OpenFileWindows.txt";
                List<string> files = CK3ScriptEd.Instance.GetOpenWindowsFilenameList();
                if (files == null)
                    return;

                using (System.IO.TextWriter writeFile = new StreamWriter(filename))
                {

                    foreach (var file in files)
                    {
                        writeFile.WriteLine(file);
                    }

                    int index = CK3ScriptEd.Instance.GetOpenWindowsFilenameListIndex();

                    index = Math.Max(0, index);
                    writeFile.WriteLine("openIndex:" + index);
                }
            }
    
        }

        public void LoadDirectories()
        {
            {
                string filename = Globals.CK3EdDataPath + "Directories.txt";

                if (!File.Exists(filename))
                    return;

                string text = System.IO.File.ReadAllText(filename);

                string[] lines = text.Replace("\r", "").Split('\n');
                Globals.CK3Path = lines[0].Trim();
                Globals.CK3DocPath = lines[1].Trim();
            }
        }

        public void Load()
        {
            if(Core.Instance.ModCK3Library == null)
            {
                string filename = Globals.CK3EdDataPath + "LoadedMod.txt";

                if (!File.Exists(filename))
                    return;

                string text = System.IO.File.ReadAllText(filename);

                string[] lines = text.Replace("\r", "").Split('\n');
                CK3ScriptEd.Instance.Load(lines[0].Trim());
              
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
                string filename = Globals.CK3EdDataPath + Core.Instance.ModCK3Library.Name + "_OpenFileWindows.txt";

                if (!File.Exists(filename))
                    return;

                string text = System.IO.File.ReadAllText(filename);

                string[] lines = text.Replace("\r", "").Split('\n');
                List<Control> windows = new List<Control>();
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
                        {
                            CK3ScriptEd.Instance.SetActiveEditor(
                                windows[i] as TextEditorControl);

                        }
                    }
                    else windows.Add(CK3ScriptEd.Instance.GetTextEditor(line.Split(':')[1], line.Split(':')[0] == "base"));
                }
            }
        }
    }
}
