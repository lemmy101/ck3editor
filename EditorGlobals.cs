using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JominiParse;

namespace CK3ScriptEditor
{
    public enum FontSize
    {
        Normal,
        Large,
    }
    public class EditorGlobals
    {
        private static FontSize _fontSize = FontSize.Normal;

        public static FontSize FontSize
        {
            get => _fontSize;
            set
            {
                var last = _fontSize;
                _fontSize = value;
                if (last != value && CK3ScriptEd.Instance != null)
                {

                    CK3ScriptEd.Instance.UpdateAllWindows();

                    CK3ScriptEd.Instance.ChangeFontAllScriptWindows();
                    CK3ScriptEd.Instance.UpdateFont();

                    CK3EditorPreferencesManager.Instance.Save();

                }

            }
        }
    }
}
