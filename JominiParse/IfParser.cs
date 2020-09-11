using System.Collections.Generic;
using System.Linq;

namespace JominiParse
{
    class IfParser
    {
        public static IfParser Instance = new IfParser();

        public If ParseIf(ScriptObject parent, List<ScriptParsedSegment> scriptParsedSegments, ScriptContext context)
        {
           

            If i = new If(parent, scriptParsedSegments[0]);

            i.SetScopeType(parent.GetScopeType());
            
            var aa = scriptParsedSegments[0].children.Where(a => a.name == "limit");
            int index = 0;
            if (aa.Count() > 0)
            {
                var a = aa.First();

                i.Limit = new Limit(i, a);
                i.Limit.Parse(a, context);
                index = 1;
            }

            var then = scriptParsedSegments[0].children.GetRange(index, scriptParsedSegments[0].children.Count - index).ToList();

            FileTokenizer.Instance.ParseResults(i, then, context, i.Then);

            foreach (var scriptObject in i.Then)
            {
                scriptObject.IsThen = true;
            }
            // have else / elseifs
            if (scriptParsedSegments.Count > 1)
            {
                
                i.Else = ParseIf(parent, scriptParsedSegments.GetRange(1, scriptParsedSegments.Count - 1), context);

            }

            return i;

        }

    }
}