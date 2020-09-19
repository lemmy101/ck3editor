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
            string filename = Globals.CK3EdDataPath + "OpenFileWindows.txt";
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
                
                writeFile.WriteLine("openIndex:"+index);
            }
    
        }

        public void Load()
        {
            string filename = Globals.CK3EdDataPath + "OpenFileWindows.txt";
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
                    CK3ScriptEd.Instance.SetActiveEditor(
                        windows[Convert.ToInt32(line.Substring(line.IndexOf(':') + 1))] as TextEditorControl);
                }
                else windows.Add(CK3ScriptEd.Instance.GetTextEditor(line.Split(':')[1], line.Split(':')[0] == "base"));
            }
        }
    }
}
