using System;
using System.Collections;

namespace Laan.GameLibrary
{

    /// <summary>
    /// Client encapsulates the information for a connecting to a GameClient
    /// </summary>
    public class StoredClient : Object
    {

        public string Name;
        public int    Port;
        public string Host;

        private ArrayList _pendingMessages;

        public StoredClient(string name, string host, int port)
        {
            _pendingMessages = new ArrayList();
            Name = name;
            Host = host;
            Port = port;
        }

        public void AddMessage(byte[] message)
        {
            _pendingMessages.Add(message);
        }

        public ArrayList PendingMessages {
            get { return _pendingMessages; }
        }
    }

    /// <summary>
    /// ClientList is a container for Client objects
    /// </summary>
    public class ClientList : ArrayList
    {
        public new StoredClient this[int Index]
        {
            get {
                return (StoredClient)base[Index];
            }
            set {
                base[Index] = value;
            }
        }

        public StoredClient Find(string name)
        {
            foreach(StoredClient c in this)
            {
                if(c.Name == name)
                    return c;
            }
            return null;
        }
    }
}
