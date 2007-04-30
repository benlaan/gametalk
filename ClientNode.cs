using System;
using System.Collections;

namespace Laan.GameLibrary
{

	/// <summary>
	/// ClientNode encapsulates the information for a connecting to a GameClient
	/// </summary>
	public class ClientNode
    {

        public string Name;
        public int    Port;
		public string Host;
		public bool Active;

		private ArrayList _pendingMessages;

		public ClientNode(string name, string host, int port)
		{
			_pendingMessages = new ArrayList();
			Name = name;
			Host = host;
			Port = port;
			_active = false;
		}

        public void AddMessage(byte[] message)
		{
			_pendingMessages.Add(message);
		}

		public ArrayList PendingMessages
		{
            get { return _pendingMessages; }
        }
    }

    /// <summary>
    /// ClientList is a container for Client objects
    /// </summary>
    public class ClientNodeList : ArrayList
    {
        public new ClientNode this[int Index]
        {
            get {
                return (ClientNode)base[Index];
            }
            set {
                base[Index] = value;
            }
        }

        public ClientNode Find(string name)
        {
            foreach(ClientNode c in this)
            {
                if(c.Name == name)
                    return c;
            }
            return null;
        }
    }
}
