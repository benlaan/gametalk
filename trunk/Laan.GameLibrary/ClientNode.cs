using System;
using System.Collections;
using System.Collections.Generic;
using Laan.GameLibrary.Data;

namespace Laan.GameLibrary
{

    public class PendingMessage
    {
        public PendingMessage(BinaryStreamWriter writer)
        {
            Data = writer.DataStream;
            Length = writer.Length;
        }

        public int Length { get; private set; }
        public byte[] Data { get; private set; }
    }

    public interface IClientNode
    {
        void AddMessage(PendingMessage message);
    }



    /// <summary>
	/// ClientNode encapsulates the information for a connecting to a GameClient
	/// </summary>
    public class ClientNode : IClientNode
    {
		public ClientNode(string name, string host, int port)
		{
            _pendingMessages = new List<PendingMessage>();
			Name = name;
			Host = host;
			Port = port;
		}

		public  bool                 Active;
		public  string               Host;
		public  string               Name;
		public  int                  Port;
        private List<PendingMessage> _pendingMessages;

        public void AddMessage(PendingMessage message)
		{
			_pendingMessages.Add(message);
		}

        public List<PendingMessage> PendingMessages
		{
			get { return _pendingMessages; }
		}
    }

    /// <summary>
	/// ClientNodeList is a container for Client objects
	/// </summary>
	public class ClientNodeList : List<ClientNode>
    {
		public ClientNode Find(string name)
		{
            return base.Find(node => node.Name == name);
		}
    }
}
