namespace JominiParse
{
    public class Globals
    {
        public static int DataVersion = 1;
        public static string CK3Path { get; set; } 

        public static string
            CK3DocPath { get; set; } 

        public static string CK3EdDataPath => CK3DocPath + "CK3EditorData/";

        public static string CK3ModPath => CK3DocPath + "mod/";

        public static string CK3LogPath => CK3DocPath + "logs/";
    }
}