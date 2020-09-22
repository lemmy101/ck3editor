#region

using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DarkUI.Controls;
using DarkUI.Docking;
using JominiParse;

#endregion

namespace CK3ScriptEditor
{
    public partial class ObjectDetailsExplorer : DarkToolWindow
    {
        private ScriptObject obj;
        private readonly List<ScriptObject> visited = new List<ScriptObject>();

        public ObjectDetailsExplorer()
        {
            DockArea = DarkDockArea.Right;
            InitializeComponent();
            Fill();
        }

        public void SetObject(ScriptObject o)
        {
            var oldo = obj;
            if (o != null)
                obj = o.Topmost;

            if (o != oldo || o == null)
                Fill();
        }

        private void Fill()
        {
            referencedByList.Items.Clear();
            referencesList.Items.Clear();
            scopesList.Items.Clear();

            if (obj == null)
                return;
            var str = obj.Topmost.Name;

            if (str.Contains(" "))
                str = str.Split(' ')[1];

            var ConnectionsIn = ReferenceManager.Instance.GetConnectionsTo(str);

            foreach (var eventConnection in ConnectionsIn) AddIncoming(eventConnection.From, eventConnection);

            var Connections = ReferenceManager.Instance.GetConnectionsFrom(str).Distinct().ToList();

            foreach (var eventConnection in Connections) AddOutgoing(eventConnection.ToTag, eventConnection);
            scopesList.SuspendEvents(true);

            foreach (var objScriptScope in obj.scriptScopes.OrderBy(a => a.Value.Name))
                // add local script scopes...
                AddScriptScope(objScriptScope.Value, "local");

            if (ConnectionsIn.Count > 0)
            {
                var Scopes = new HashSet<ScriptScope>();
                var scopesThis = new HashSet<ScriptScope>();
                scopesThis.Clear();
                // get first reference list...
                visited.Clear();
                GetScriptScopesFromReferences(ConnectionsIn[0].From, scopesThis);
                Scopes.UnionWith(scopesThis);


                for (var index = 1; index < ConnectionsIn.Count; index++)
                {
                    visited.Clear();
                    scopesThis.Clear();
                    var eventConnection = ConnectionsIn[index];
                    GetScriptScopesFromReferences(eventConnection.From, scopesThis);

                    // remove any that don't appear in every reference...
                    Scopes.RemoveWhere(a => !scopesThis.Any(b => b.Name == a.Name));
                }

                foreach (var scope in Scopes.OrderBy(a => a.Name)) AddScriptScope(scope, "inherited");
            }


            scopesList.SuspendEvents(false);

            var l = scopesList.Items.OrderBy(a => a.Text).ToList();
            scopesList.Items.Clear();

            foreach (var darkListItem in l)
                scopesList.Items.Add(darkListItem);
        }

        private void GetScriptScopesFromReferences(ScriptObject eventConnectionFrom,
            HashSet<ScriptScope> scopes)
        {
            if (visited.Contains(eventConnectionFrom))
                return;


            foreach (var keyValuePair in eventConnectionFrom.scriptScopes)
                if (!keyValuePair.Value.Temporary)
                    scopes.Add(keyValuePair.Value);


            var ConnectionsIn = ReferenceManager.Instance.GetConnectionsTo(eventConnectionFrom.Name).Distinct()
                .ToList();
            visited.Add(eventConnectionFrom);

            if (ConnectionsIn.Count > 0)
            {
                var Scopes = new HashSet<ScriptScope>();
                var scopesThis = new HashSet<ScriptScope>();
                scopesThis.Clear();
                // get first reference list...

                GetScriptScopesFromReferences(ConnectionsIn[0].From, scopesThis);
                Scopes.UnionWith(scopesThis);


                for (var index = 1; index < ConnectionsIn.Count; index++)
                {
                    scopesThis.Clear();
                    var eventConnection = ConnectionsIn[index];
                    GetScriptScopesFromReferences(eventConnection.From, scopesThis);

                    // remove any that don't appear in every reference...
                    if (scopesThis.Count > 0)
                        Scopes.RemoveWhere(a => !scopesThis.Any(b => b.Name == a.Name));
                }

                scopes.UnionWith(Scopes);
            }
        }

        private void AddScriptScope(ScriptScope objScriptScope, string type)
        {
            var name = objScriptScope.Name;
            var col2 = (int) (scopesList.Width / 2.7f);
            var col3 = (int) (col2 * 1.6f);
            col2 /= 6;
            col3 /= 6;

            while (name.Length < col2)
                name += " ";

            name += objScriptScope.To.ToString().ToLower();

            while (name.Length < col3)
                name += " ";

            name += objScriptScope.Temporary ? "temp " : "";

            name += type + " " + (objScriptScope.IsValue ? "scope value" : "scope");
            if (scopesList.Items.Any(a => a.Text == name))
                return;

            var i = new DarkListItem(name);
            i.Tag = objScriptScope;
            scopesList.Items.Add(i);
        }

        private void AddOutgoing(string eventConnectionTo, ReferenceManager.EventConnection c)
        {
            var str = eventConnectionTo;

            if (str.Contains(" "))
                str = str.Split(' ')[1];

            var i = new DarkListItem(str);
            i.Tag = c;
            referencesList.Items.Add(i);
        }

        private void AddIncoming(ScriptObject eventConnectionFrom, ReferenceManager.EventConnection c)
        {
            var str = eventConnectionFrom.Name;

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
                var item =
                    referencesList.Items[referencesList.SelectedIndices[0]].Tag as ReferenceManager.EventConnection;
                var i = Core.Instance.Get(item.ToTag);
                CK3ScriptEd.Instance.Goto(i.Filename, i.LineStart - 1, false);
            }
        }

        private void referencedByList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (referencedByList.SelectedIndices.Count > 0)
            {
                var item =
                    referencedByList.Items[referencedByList.SelectedIndices[0]].Tag as ReferenceManager.EventConnection;
                CK3ScriptEd.Instance.Goto(item.FromCommand.Filename, item.FromCommand.LineStart - 1, false);
            }
        }

        private void scopesList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (scopesList.SelectedIndices.Count > 0)
            {
                var item = scopesList.Items[scopesList.SelectedIndices[0]].Tag as ScriptScope;
                if (item.Declared != null)
                    CK3ScriptEd.Instance.Goto(item.Declared.Filename, item.Declared.LineStart - 1, false);
            }
        }
    }
}