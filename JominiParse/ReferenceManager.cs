using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JominiParse
{
    public class ReferenceManager
    {

        public class EventConnection
        {
            public ScriptObject FromCommand { get; set; }
            public ScriptObject From { get; set; }
            public string ToTag { get; set; }
        }

        public static ReferenceManager Instance = new ReferenceManager();

        public Dictionary<string, List<EventConnection>> FromMap = new Dictionary<string, List<EventConnection>>();
        public Dictionary<string, List<EventConnection>> ToMap = new Dictionary<string, List<EventConnection>>();

        public void AddConnection(ScriptObject From, ScriptObject FromCommand, string ToTag)
        {
            var connection = new EventConnection();

            connection.From = From;
            connection.FromCommand = FromCommand;
            connection.ToTag = ToTag;
            if (ToTag == null)
                return;

            if (FromMap.ContainsKey(connection.From.Name))
                FromMap[connection.From.Name].Add(connection);
            else
                FromMap[connection.From.Name] = new List<EventConnection>() { connection };

            if (ToMap.ContainsKey(connection.ToTag))
                ToMap[connection.ToTag].Add(connection);
            else
                ToMap[connection.ToTag] = new List<EventConnection>() { connection };

        }

        public void ClearConnectionsFrom(string fromTag)
        {
            if (!FromMap.ContainsKey(fromTag))
                return;
            
            var f = FromMap[fromTag];

            foreach (var eventConnection in f)
            {
                if (ToMap.ContainsKey(eventConnection.ToTag))
                {
                    var v = ToMap[eventConnection.ToTag];

                    v.RemoveAll(a => a.From.Name == fromTag);
                }
            }

            FromMap.Remove(fromTag);
        }

        public List<EventConnection> GetConnectionsTo(string to)
        {
            if (!ToMap.ContainsKey(to))
            {
                if (to.StartsWith("scripted_trigger "))
                    to = to.Substring("scripted_trigger ".Length);
                if (ToMap.ContainsKey(to))
                    return ToMap[to];

                return new List<EventConnection>();
            }

            return ToMap[to];
        }
        public List<EventConnection> GetConnectionsFrom(string from)
        {
            if (!FromMap.ContainsKey(from))
            {
                if (FromMap.ContainsKey("scripted_trigger " + from))
                    return FromMap["scripted_trigger " + from];

                return new List<EventConnection>(); 

            }


            return FromMap[from];
        }
    }
}
