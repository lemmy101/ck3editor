using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Controls;
using DarkUI.Docking;
using JominiParse;

namespace CK3ScriptEditor
{
    public partial class ObjectDetailsExplorer : DarkToolWindow
    {
        private ScriptObject obj;

        public ObjectDetailsExplorer()
        {
            DockArea = DarkDockArea.Right;
            InitializeComponent();
            Fill();
        }

        public void SetObject(ScriptObject o)
        {
            var oldo = obj;
            obj = o.Topmost;

            if(o != oldo)
                Fill();
        }
        private void Fill()
        {
            referencedByList.Items.Clear();
            referencesList.Items.Clear();
            scopesList.Items.Clear();

            if (obj == null)
                return;
            string str = obj.Topmost.Name;

            if (str.Contains(" "))
                str = str.Split(' ')[1];

            var ConnectionsIn = ReferenceManager.Instance.GetConnectionsTo(str);

            foreach (var eventConnection in ConnectionsIn)
            {
                AddIncoming(eventConnection.From, eventConnection);
            }
            
            var Connections = ReferenceManager.Instance.GetConnectionsFrom(str).Distinct().ToList();

            foreach (var eventConnection in Connections)
            {
                AddOutgoing(eventConnection.ToTag, eventConnection);
            }
            
            foreach (var objScriptScope in obj.scriptScopes.OrderBy(a=>a.Value.Name))
            {
                // add local script scopes...
                AddScriptScope(objScriptScope.Value, "local");
            }

            HashSet<ScriptObject.ScriptScope> Scopes = new HashSet<ScriptObject.ScriptScope>();
            visited.Clear();
            foreach (var eventConnection in ConnectionsIn)
            {
                GetScriptScopesFromReferences(eventConnection.From, Scopes);
            }

            foreach (var scope in Scopes.OrderBy(a=>a.Name))
            {
                AddScriptScope(scope, "inherited");

            }

            var l = scopesList.Items.OrderBy(a => a.Text).ToList();
            scopesList.Items.Clear();

            foreach (var darkListItem in l)
                scopesList.Items.Add(darkListItem);
        }
        List<ScriptObject> visited = new List<ScriptObject>();
        private void GetScriptScopesFromReferences(ScriptObject eventConnectionFrom, HashSet<ScriptObject.ScriptScope> scopes)
        {
            if (visited.Contains(eventConnectionFrom))
                return;

            visited.Add(eventConnectionFrom);

            foreach (var keyValuePair in eventConnectionFrom.scriptScopes)
            {
                if (!keyValuePair.Value.Temporary)
                    scopes.Add(keyValuePair.Value);
            }


            var ConnectionsIn = ReferenceManager.Instance.GetConnectionsTo(eventConnectionFrom.Name).Distinct();

            foreach (var eventConnection in ConnectionsIn)
            {
                GetScriptScopesFromReferences(eventConnection.From, scopes);
            }


        }

        private void AddScriptScope(ScriptObject.ScriptScope objScriptScope, string type)
        {
            string name = objScriptScope.Name;

            int col2 = (int) (scopesList.Width / 2.7f);
            int col3 = (int) (col2 * 1.6f);
            col2 /= 6;
            col3 /= 6;

            while (name.Length < col2)
                name += " ";

            name += objScriptScope.To.ToString().ToLower();

            while (name.Length < col3)
                name += " ";

            name += (objScriptScope.Temporary ? "temp " : "");

            name += type + " " + (objScriptScope.IsValue ? "scope value" : "scope");

            var i = new DarkListItem(name);
            i.Tag = objScriptScope;
            scopesList.Items.Add(i);
        }

        private void AddOutgoing(string eventConnectionTo, ReferenceManager.EventConnection c)
        {
            string str = eventConnectionTo;

            if (str.Contains(" "))
                str = str.Split(' ')[1];

            var i = new DarkListItem(str);
            i.Tag = c;
            referencesList.Items.Add(i);
        }

        private void AddIncoming(ScriptObject eventConnectionFrom, ReferenceManager.EventConnection c)
        {
            string str = eventConnectionFrom.Name;

            if (str.Contains(" "))
                str = str.Split(' ')[1];

            var i = new DarkListItem(str);
            i.Tag = c;
            referencedByList.Items.Add(i);
        }

        private void referencesList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (referencesList.SelectedIndices.Count > 0)
            {
                var item = referencesList.Items[referencesList.SelectedIndices[0]].Tag as ReferenceManager.EventConnection;
                var i = Core.Instance.Get(item.ToTag);
                CK3ScriptEd.Instance.Goto(i.Filename, i.LineStart - 1, false);
            }
        }

        private void referencedByList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (referencedByList.SelectedIndices.Count > 0)
            {
                var item = referencedByList.Items[referencedByList.SelectedIndices[0]].Tag as ReferenceManager.EventConnection;
                CK3ScriptEd.Instance.Goto(item.FromCommand.Filename, item.FromCommand.LineStart-1, false);
            }

        }

        private void scopesList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (scopesList.SelectedIndices.Count > 0)
            {
                var item = scopesList.Items[scopesList.SelectedIndices[0]].Tag as ScriptObject.ScriptScope;
                if(item.Declared != null)
                    CK3ScriptEd.Instance.Goto(item.Declared.Filename, item.Declared.LineStart - 1, false);
                
            }

        }
    }
}
