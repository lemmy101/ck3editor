using System;
using System.Collections.Generic;
using System.Linq;

namespace JominiParse
{

    public class ScriptObject
    {
        public string Namespace { get; set; }
        public ScriptObjectSchema Schema;
        public SchemaChild SchemaChild;
        public ScriptFile ScriptFile { get; set; }
        public bool Overridden { get; set; }
        public ScriptObject(ScriptObject parent, ScriptParsedSegment seg)
        {
            this.Name = seg.name;
            this.Filename = seg.filename;
            this.LineStart = seg.lineNumbers.First();
            this.LineEnd = seg.lineNumbers.Last();
            this.Parent = parent;
            this.Library = Core.Instance.LoadingCK3Library;
         
            Schema = SchemaManager.Instance.GetSchema(GetType());
            if (parent != null)
            {
                parent.Children.Add(this);
                if (parent.Schema != null && seg.name != null)
                {
                    SchemaChild = parent.Schema.GetChild(seg.name);
                    
                    if (SchemaChild != null)
                        Schema = SchemaManager.Instance.GetSchema(SchemaChild.Type);
                }
                if (Schema == null && seg.name != null)
                {
                    if (seg.name == "NOT")
                    {

                    }
                    Schema = SchemaManager.Instance.GetSchema(seg.name);

                }


            }
        }

        public ScriptLibrary Library { get; set; }

        public ScriptObject Topmost
        {
            get
            {
                if (Parent == null)
                    return this;

                return Parent.Topmost;
            }
        }

        public virtual ScopeType GetScopeType()
        {
            return ScopeType;
        }
        public virtual void SetScopeType(ScopeType type)
        {
            ScopeType = type;
        }

        protected ScopeType ScopeType { get; set; }
        public ScriptObject Parent { get; set; }

        public int LineEnd { get; set; }

        public int LineStart { get; set; }

        public string Filename { get; set; }
        public virtual string Name { get; set; }

        protected string TabFormat(string str, int depth = 0)
        {
            string[] splitLines = str.Split('\n');
            List<string> results = new List<string>();
            for (var index = 0; index < splitLines.Length; index++)
            {
                var splitLine = splitLines[index];
                splitLine = splitLine.Trim();
                if(splitLine.Length==0)
                    continue;
                if (splitLine.Contains("}"))
                    depth--;


                for (int i = 0; i < depth; i++)
                {
                    splitLine = "     " + splitLine;
                }

                if (splitLine.Contains("{"))
                {
                    if(depth == 0)
                        results.Add("");
                    depth++;
                }

                
                results.Add(splitLine);
            }

            return String.Join("\n", results);
        }
        protected string GetStart(string name)
        {
            return name + @" = { 
            ";
        }
        protected string GetEnd()
        {
            return @" 
            }";
        }
        //  public abstract void Parse(string name, ScriptParsedSegment segment);
        public virtual string ToScript()
        {
              return "";
        }


        public List<EventConnection> Connections = new List<EventConnection>();
        public List<ScriptObject> Children = new List<ScriptObject>();

        public void AddEventConnection(EventConnection eventConnection)
        {
            Connections.Add(eventConnection);
        }
    }
}