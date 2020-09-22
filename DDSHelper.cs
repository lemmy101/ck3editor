#region

using System.Drawing;

#endregion

namespace CK3ScriptEditor
{
    public class DDSHelper
    {
        public static DDSHelper Instance = new DDSHelper();

        public Image Load(string file)
        {
            return DevIL.DevIL.LoadBitmap(file);
        }
    }
}