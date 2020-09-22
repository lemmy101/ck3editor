#region

using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JominiParse;

#endregion

namespace CK3ScriptEditor
{
    public class ScriptCloningManager
    {
        public static ScriptCloningManager Instance = new ScriptCloningManager();

        public void Clone(ScriptObject scriptObject)
        {
            var dlg = new CloneToModDialog();
            dlg.Init(scriptObject.Filename, scriptObject.LineStart, scriptObject.LineEnd, scriptObject);
            if (dlg.ShowDialog(CK3ScriptEd.Instance) == DialogResult.OK)
            {
                var dir = Core.Instance.GetBaseDirectoryFromContext(dlg.Context);
                var basePath = scriptObject.Filename;
                var fullPath = dir.Append(dlg.ChosenFilename);
                fullPath.IsBase = false;


                var exists = fullPath.Exists;
                var textToImplant = dlg.GetTextToImplant(basePath);
                textToImplant = textToImplant.Replace("\r", "");
                if (exists)
                {
                    // need to insert it into the file....
                    var text = File.ReadAllText(fullPath.ToFullWindowsFilename());
                    text = text.Replace("\r", "");
                    var lines = text.Split('\n').ToList();

                    var lines2 = textToImplant.Split('\n').ToList();

                    lines.AddRange(lines2);


                    using (var fs = new FileStream(fullPath.ToFullWindowsFilename(), FileMode.Create))
                    {
                        // create a new file....
                        using (var outputFile = new StreamWriter(fs, Encoding.UTF8))
                        {
                            foreach (var line in lines)
                                outputFile.WriteLine(line);
                        }
                    }
                }
                else
                {
                    using (var fs = new FileStream(fullPath.ToFullWindowsFilename(), FileMode.Create))
                    {
                        // create a new file....
                        using (var outputFile = new StreamWriter(fs, Encoding.UTF8))
                        {
                            var lines = textToImplant.Split('\n').ToList();
                            if (dlg.ScriptObject.Namespace != null)
                            {
                                lines.Insert(0, "\n");
                                lines.Insert(0, "namespace = " + dlg.ScriptObject.Namespace);
                            }

                            foreach (var line in lines)
                                outputFile.WriteLine(line);
                        }
                    }
                }


                Core.Instance.ModCK3Library.EnsureFile(fullPath, dlg.ScriptObject.Context);
                Core.Instance.LoadCK3File(fullPath, false, true);
                CK3ScriptEd.Instance.projectExplorer.FillProjectView();
                CK3ScriptEd.Instance.soExplorer.UpdateScriptExplorer();
                CK3ScriptEd.Instance.CloseDocument(true, fullPath);
                var newLine = Core.Instance.ModCK3Library.GetFile(fullPath).Map[dlg.ScriptObject.Name].LineStart - 1;
                CK3ScriptEd.Instance.Goto(fullPath, newLine, false);
                CK3ScriptEd.Instance.Goto(fullPath, newLine, false);
            }
        }
    }
}