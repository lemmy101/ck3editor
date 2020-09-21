using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using JominiParse;

namespace CK3ScriptEditor
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Globals.CK3DocPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).Replace("\\", "/");

            Globals.CK3DocPath += "/Paradox Interactive/Crusader Kings III/";

            CK3EditorPreferencesManager.Instance.LoadDirectories();
        
            while (Globals.CK3Path == null || !Directory.Exists(Globals.CK3Path))
            {
                FolderBrowserDialog bd = new FolderBrowserDialog();
                bd.Description = "Please select the CK3 game directory.";
                if (bd.ShowDialog(null) == DialogResult.OK)
                {
                    if (File.Exists(bd.SelectedPath + "\\binaries\\ck3.exe"))
                    {
                        Globals.CK3Path = bd.SelectedPath.Replace("\\", "/") + "/game/";
                    }
                    else
                    {
                        if (MessageBox.Show("Could not find CK3 at specified directory.", "Error",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                        {
                            Application.Exit();
                            return;
                        }
                    }
                }
                else
                {
                    Application.Exit();
                    return;

                }
            }
            
            CK3EditorPreferencesManager.Instance.Save();

            Application.Run(new CK3ScriptEd());
            //Application.Run(new EventPreviewForm());
        }
    }
}
