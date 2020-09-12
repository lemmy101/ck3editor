using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JominiParse;

namespace CK3ScriptEditor
{
    public class ScriptCloningManager
    {
        public static ScriptCloningManager Instance = new ScriptCloningManager();

        public void Clone(ScriptObject scriptObject)
        {
            CloneToModDialog dlg = new CloneToModDialog();
            dlg.Init(scriptObject.Filename, scriptObject.LineStart, scriptObject.LineEnd, scriptObject);
            if(dlg.ShowDialog(CK3ScriptEd.Instance) == DialogResult.OK)
            {
                string dir = Core.Instance.GetDirectoryFromContext(dlg.Context) + "/";

                string path = dir + dlg.ChosenFilename;

                string fullPath = Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/" + path;
                string fullBasePath = Globals.CK3Path + dlg.ScriptObject.Filename;
                bool exists = File.Exists(fullPath);
                string textToImplant = dlg.GetTextToImplant(fullBasePath);
                textToImplant = textToImplant.Replace("\r", "");
                if (exists)
                {
                    // need to insert it into the file....
                    string text = System.IO.File.ReadAllText(fullPath);
                    text = text.Replace("\r", "");
                    var lines = text.Split(new char[] { '\n' }).ToList();

                    var file = Core.Instance.GetFile(path, false);

                    {
                        var lines2 = textToImplant.Split(new char[] { '\n' }).ToList();

                        lines.AddRange(lines2);

                    }

                    using (FileStream fs = new FileStream(fullPath, FileMode.Create))
                    {
                        // create a new file....
                        using (StreamWriter outputFile = new StreamWriter(fs, Encoding.UTF8))
                        {

                            foreach (string line in lines)
                                outputFile.WriteLine(line);
                        }
                    }
                }
                else
                {
                    using (FileStream fs = new FileStream(fullPath, FileMode.Create))
                    {
                        // create a new file....
                        using (StreamWriter outputFile = new StreamWriter(fs, Encoding.UTF8))
                        {
                            var lines = textToImplant.Split(new char[] { '\n' }).ToList();
                            if (dlg.ScriptObject.Namespace != null)
                            {
                                lines.Insert(0, "\n");
                                lines.Insert(0, "namespace = " + dlg.ScriptObject.Namespace);
                            }

                            foreach (string line in lines)
                                outputFile.WriteLine(line);
                        }
                    }
                }



                Core.Instance.ModCK3Library.EnsureFile(path, dlg.ScriptObject.Context);
                Core.Instance.LoadCK3File(path, false, true);
                CK3ScriptEd.Instance.projectExplorer.FillProjectView();
                CK3ScriptEd.Instance.soExplorer.UpdateScriptExplorer();
                CK3ScriptEd.Instance.CloseDocument(true, path);
                int newLine = Core.Instance.ModCK3Library.GetFile(path).Map[dlg.ScriptObject.Name].LineStart - 1;
                CK3ScriptEd.Instance.Goto(path, newLine, false); CK3ScriptEd.Instance.Goto(path, newLine, false);

            }
        }
    }
}
