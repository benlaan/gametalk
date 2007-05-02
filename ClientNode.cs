using System;
using System.Collections;

namespace Laan.GameLibrary
{

	/// <summary>
	/// ClientNode encapsulates the information for a connecting to a GameClient
	/// </summary>
	public class ClientNode
    {
		public ClientNode(string name, string host, int port)
		{
			_pendingMessages = new ArrayList();
			Name = name;
			Host = host;
			Port = port;
			_isHosting = !_hasHost;
			_hasHost = true;
		}

		private static bool _hasHost = false;

		public bool Active;
		public string Host;
		public string Name;
		public int    Port;
		private bool _isHosting;
		private ArrayList _pendingMessages;

		public void AddMessage(byte[] message)
		{
			_pendingMessages.Add(message);
		}

		public bool IsHosting
		{
			get { return _isHosting; }
			set { _isHosting = value; }
		}

		public ArrayList PendingMessages
		{
			get { return _pendingMessages; }
		}
    }

    /// <summary>
	/// ClientNodeList is a container for Client objects
	/// </summary>
	public class ClientNodeList : ArrayList
    {
		public ClientNode Find(string name)
		{
			foreach(ClientNode c in this)
			{
				if(c.Name == name)
					return c;
			}
			return null;
		}

		public new ClientNode this[int Index]
		{
			get {
				return (ClientNode)base[Index];
			}
			set {
				base[Index] = value;
			}
		}
    }
}
