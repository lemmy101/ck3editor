using System.ComponentModel.Design;

namespace JominiParse
{
    public class LocalizedString : ScriptObject
    {
        public string key;

        public override string ToScript()
        {
            return Name + " = " + key;
        }

        public override string ToString()
        {
            return Name + " = " + key + " (" + Core.Instance.GetLocalizedText(key) + ")";
        }

        public LocalizedString(ScriptObject parent, ScriptParsedSegment seg) : base(parent, seg)
        {
        }

        public string Localized()
        {
            return Core.Instance.GetLocalizedText(key);
        }
    }
}