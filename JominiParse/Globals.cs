using System;
using System.Collections.Generic;
using System.Text;

namespace JominiParse
{
    public class Globals
    {
        public static string CK3Path { get; set; }// = "D:/SteamLibrary/steamapps/common/Crusader Kings III/game/";
        public static string CK3DocPath { get; set; }// = "C:/Users/LEMMYOFFICE/Documents/Paradox Interactive/Crusader Kings III/";

        public static string CK3EdDataPath
        {
            get { return CK3DocPath + "CK3EditorData/"; }
        }
        public static string CK3ModPath
        {
            get { return CK3DocPath + "mod/"; }
        }
        public static string CK3LogPath
        {
            get { return CK3DocPath + "logs/"; }
        }

        public static int DataVersion = 1;
    }
}
