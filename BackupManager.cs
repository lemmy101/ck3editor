using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using JominiParse;

namespace CK3ScriptEditor
{
    public class BackupManager
    {
        public static BackupManager Instance = new BackupManager();
        public float AutosaveInterval { get; set; } = 15f;
        public int NumBackups { get; set; } = 30;

        public float MinutesSinceLastSave = 0;
        public int TickTimeMS
        {
            get { return (int) (AutosaveInterval * 60 * 1000); }
        }

        public void UpdateTick()
        {
            MinutesSinceLastSave += AutosaveInterval;

            DoRecentAutoBackup();
        }

        private void DoRecentAutoBackup()
        {
            if (Core.Instance.ModCK3Library == null)
                return;

            string destDirectory = Globals.CK3EdDataPath;

            destDirectory += "CK3EditBackups/";
            if (!Directory.Exists(destDirectory))
                Directory.CreateDirectory(destDirectory);

            string backupDir = destDirectory + Core.Instance.ModCK3Library.Name;

            for (int x = NumBackups - 2; x >= 0; x--)
            {
               // if (Directory.Exists(backupDir))
                {
                    string backupDirFrom = destDirectory + Core.Instance.ModCK3Library.Name + "_" + (x+1);
                    if(x==0)
                        backupDirFrom = destDirectory + Core.Instance.ModCK3Library.Name;

                    string backupDirFromTo = destDirectory + Core.Instance.ModCK3Library.Name + "_" + (x+2);
                    DeleteDir(backupDirFromTo + "/");
                    if(Directory.Exists(backupDirFrom + "/"))
                        Directory.Move(backupDirFrom + "/", backupDirFromTo + "/");

                }
            }
            DeleteDir(backupDir + "/");

            CopyDir(Globals.CK3ModPath + Core.Instance.ModCK3Library.Name + "/", backupDir + "/");

        }

        private void CopyDir(string f, string t)
        {
            if (!Directory.Exists(f))
                return;

            if (!Directory.Exists(t))
            {
                Directory.CreateDirectory(t);
            }
            string[] files = Directory.GetFiles(f);

            foreach (var file in files)
            {
                string filename = file.Substring(file.LastIndexOf("/")+1);

                File.Copy(f + filename, t + filename);
                
            }

            string[] dirs = Directory.GetDirectories(f);

            foreach (var s in dirs)
            {
                string last = s.Substring(s.LastIndexOf("/")+1);
                CopyDir(s+"/", t+last+"/");

            }
        }

        private void DeleteDir(string dir)
        {
            if (!Directory.Exists(dir))
                return;

            string[] files = Directory.GetFiles(dir);

            foreach (var file in files)
            {
                File.Delete(file);
            }

            string[] dirs = Directory.GetDirectories(dir);

            foreach (var s in dirs)
            {
                DeleteDir(s);

            }

            Directory.Delete(dir);
        }
    }
}
