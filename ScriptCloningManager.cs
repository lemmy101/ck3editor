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
                RefFilename dir = Core.Instance.GetDirectoryFromContext(dlg.Context);

                RefFilename fullPath = dir.Append(dlg.ChosenFilename);
                
                fullPath.IsBase = false;

                bool exists = fullPath.Exists;
                string textToImplant = dlg.GetTextToImplant(fullPath);
                textToImplant = textToImplant.Replace("\r", "");
                if (exists)
                {
                    // need to insert it into the file....
                    string text = System.IO.File.ReadAllText(fullPath.ToFullWindowsFilename());
                    text = text.Replace("\r", "");
                    var lines = text.Split(new char[] { '\n' }).ToList();

                    var lines2 = textToImplant.Split(new char[] { '\n' }).ToList();

                    lines.AddRange(lines2);

                   

                    using (FileStream fs = new FileStream(fullPath.ToFullWindowsFilename(), FileMode.Create))
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
                    using (FileStream fs = new FileStream(fullPath.ToFullWindowsFilename(), FileMode.Create))
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



                Core.Instance.ModCK3Library.EnsureFile(fullPath, dlg.ScriptObject.Context);
                Core.Instance.LoadCK3File(fullPath, false, true);
                CK3ScriptEd.Instance.projectExplorer.FillProjectView();
                CK3ScriptEd.Instance.soExplorer.UpdateScriptExplorer();
                CK3ScriptEd.Instance.CloseDocument(true, fullPath);
                int newLine = Core.Instance.ModCK3Library.GetFile(fullPath).Map[dlg.ScriptObject.Name].LineStart - 1;
                CK3ScriptEd.Instance.Goto(fullPath, newLine, false); CK3ScriptEd.Instance.Goto(fullPath, newLine, false);

            }
        }
    }
}
