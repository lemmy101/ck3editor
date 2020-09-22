#region

using System.Collections.Generic;

#endregion

namespace JominiParse
{
    public static class StringListExtensions
    {
        public static HashSet<string> PrependToken(this HashSet<string> h, string token)
        {
            var s = new HashSet<string>();

            foreach (var s1 in h) s.Add(token + s1);

            return s;
        }
    }
}