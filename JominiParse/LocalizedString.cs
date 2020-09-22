#region

using System.IO;

#endregion

namespace JominiParse
{
    public class LocalizedString : ScriptObject
    {
        public string key;

        public LocalizedString(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }

        public override string ToString()
        {
            return Name + " = " + key + " (" + Core.Instance.GetLocalizedText(key) + ")";
        }

        public string Localized()
        {
            return Core.Instance.GetLocalizedText(key);
        }
    }
}