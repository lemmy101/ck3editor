using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
