using System;
using System.Collections.Generic;
using System.Text;

namespace JominiParse
{
    public static class StringListExtensions
    {
        public static HashSet<string> PrependToken(this HashSet<string> h, string token)
        {
            HashSet<string> s = new HashSet<string>();

            foreach (var s1 in h)
            {
                s.Add(token + s1);
            }

            return s;
        }
    }
}
