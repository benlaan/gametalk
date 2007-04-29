using System;
using System.Collections;
using System.Threading;
using Indy.Sockets;

using Laan.Library.Logging;
using Laan.GameLibrary.Data;

namespace Laan.GameLibrary
{

    public class ClientMessage
    {
        private byte[] _data;
        private IOHandlerSocket _socket;

        public ClientMessage(byte[] data, IOHandlerSocket socket)
        {
            _data = data;
            _socket = socket;
        }

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public IOHandlerSocket Socket
        {
            get { return _socket; }
            set { _socket = value; }
        }

    }

	public class GameServer : GameSocket
    {
		private GameServer() : base()
		{
			_tcpServer.OnDisconnect += new TIdServerThreadEvent(OnClientDisconnected);
		}

		/// Private Properties

		private ClientList              _clients;
		private Queue                   _messages;
		private string _name;
		private System.Threading.Thread _processor;
		private UDPServer               _udpServer;

		static private GameServer _instance = null;

		/// Public Methods

		public void AddMessage(StoredClient client, byte[] message)
		{
			if (client != null)
				client.AddMessage(message);
			else
				foreach(StoredClient c in _clients)
					c.AddMessage(message);
		}

		public void BeginRendezvous()
		{
			_udpServer.Active = true;
		}

		public void EndRendezvous()
		{
			_udpServer.Active = false;
		}

		protected override byte[] InternalOnServerExecute(Context context)
		{
			Log.WriteLine("GameServer: Receiving Client Message");

			byte[] data = base.InternalOnServerExecute(context);
            ClientMessage message = new ClientMessage(data, context.Connection.Socket);

			using (BinaryStreamReader reader = new BinaryStreamReader(data))
			{
				int id = reader.ReadInt32();
				if (id == Command.Login)
				{
					string clientName = reader.ReadString();
					string hostName = reader.ReadString();
					int port = reader.ReadInt32();

					Log.WriteLine("Client {0} has connected from {1}:{2}", clientName, hostName, port);

					AddClient(clientName, hostName, port);
				}
				else
				{
					// queue message to allow server implementation to process it
					// (i.e. act on the command sent by the client)
					_messages.Enqueue(message);
				}
			}
			return message.Data;
		}

		/// Private Methods

		internal void AddUpdateMessage(byte[] message)
		{
			foreach(StoredClient client in _clients)
				if(AllowClientUpdate(client, message))
					AddMessage(client, message);
		}

		internal void Initialise()
		{
			_clients = new ClientList();
			_messages = new Queue();

			_processor = new System.Threading.Thread(new ThreadStart(ProcessQueue));

			_udpServer = new UDPServer();

			_udpServer.DefaultPort = Config.RendezvousPort;
			_udpServer.BroadcastEnabled = true;
			_udpServer.ThreadedEvent = true;
			_udpServer.OnUDPRead += new TUDPReadEvent(OnRendezvousRead);
		}

		private void AddClient(string clientName, string hostName, int port)
		{
			StoredClient c = _clients.Find(clientName);
			if(c == null)
			{
				StoredClient newClient = new StoredClient(clientName, hostName, port);
				_clients.Add(newClient);
				if (OnNewClientConnectionEvent != null)
					OnNewClientConnectionEvent(this, _clients);
			}
		}

		private bool AllowClientUpdate(StoredClient client, byte[] message)
		{
			bool IsAllowed = true;

			// allow the implementor to alter the default behaviour if required
			if(OnAllowClientUpdateEvent != null)
				OnAllowClientUpdateEvent(this, message, ref IsAllowed);

			return IsAllowed;
		}

		private void OnRendezvousRead(object sender, byte[] data, SocketHandle binding)
		{
			string receivedText = System.Text.Encoding.ASCII.GetString(data);

			bool isValid = false;

			Log.WriteLine(
				String.Format("UDP Received '{0}' from {1} on port {2}",
					receivedText,
					binding.PeerIP,
					binding.PeerPort
				)
			);

			// if there is an event attached, allow the consumer to determine if the
			// received text is valid.
			if(OnRendezvousReceivedEvent != null)
				OnRendezvousReceivedEvent(this, receivedText, ref isValid);

			// if its valid, send the response thus: "Name:Host:Port"
			if(isValid)
			{
				string sSend =
					String.Format("{0}:{1}:{2}",
						GameServer.Instance.Name,
						Environment.MachineName,
						Config.InboundPort
					);

				Log.WriteLine("Sending UDP response: " + sSend);
				_udpServer.Send(binding.PeerIP, binding.PeerPort, sSend);
			}
		}

		private void ProcessMessage(ClientMessage message)
		{
			if(OnProcessMessageEvent != null)
				OnProcessMessageEvent(this, message);
		}

		/// Private Events

		private void ProcessQueue()
		{
			ClientMessage m;

			// repeat until Server is inactive
			while(this.Active)
			{

				// if a client generated message is awaiting processing
				while (_messages.Count > 0)
				{
					// get the message
					lock(this._messages)
					{
						m = (ClientMessage)_messages.Dequeue();
//						Log.WriteLine("Dequeing Message: " + Message.ToString(m));
					}

					// allow custom game processing of message to occur
					ProcessMessage(m);
				}
				// broadcast any updates that each client may have
				// back to the client
				UpdateClients();

				System.Threading.Thread.Sleep(Config.ThreadWait);
			}
		}

		private void UpdateClients()
		{

			foreach(StoredClient c in _clients)
			{
				lock (c.PendingMessages)
				{
					if(c.PendingMessages.Count > 0)
					{
						Log.WriteLine("Sending Pending Message to {0}:{1}", c.Host, c.Port);

						_tcpClient.Connect(c.Host, c.Port);
						try
						{
							for(int i = 0; i < c.PendingMessages.Count; i++)
							{
								byte[] m = (byte[])c.PendingMessages[i];
								WriteToSocket(_tcpClient.Socket, m);
//						   		Log.WriteLine("Message: {0}", Message.ToString(m)));
							}
							c.PendingMessages.Clear();
						}
						finally
						{
							_tcpClient.Disconnect();
						}
					}
				}
			}
		}

		/// Public Properties

		public bool Active
		{

			get { return _tcpServer.Active; }
			set {
				_tcpServer.Active = value;

				if(value) {
					_processor.Start();
					Log.WriteLine("Listening (TCP) on Port {0}", Port);
				}
				else {
					_processor.Abort();
					Log.WriteLine("Stopped Listening (TCP) on Port {0}", Port);
				}
			}
		}

		static public GameServer Instance
		{
			get {
				if (_instance == null)
				{
					_instance = new GameServer();
					_instance.Initialise();
				}
				return _instance;
			}
		}

		public int Port
		{
			get { return _tcpServer.DefaultPort; }
			set {
				if(!_tcpServer.Active)
					_tcpServer.DefaultPort = value;
			}
		}

		public int RendezvousPort
		{
			get { return _udpServer.DefaultPort; }
			set {
				if(!_udpServer.Active)
					_udpServer.DefaultPort = value;
			}
		}
                                                                         
 		protected void OnClientDisconnected(Context context)
		{
			int port = context.Binding().PeerPort;
			if (OnClientDisconnectionEvent != null)
				OnClientDisconnectionEvent(this, this._clients);
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		/// Public Events
		public event OnAllowClientUpdateEventHandler   OnAllowClientUpdateEvent;
		public event OnNewClientConnectionEventHandler OnNewClientConnectionEvent;
		public event OnClientDisconnectionEventHandler OnClientDisconnectionEvent;
		public event OnProcessMessageEventHandler      OnProcessMessageEvent;
		public event OnRendezvousReceivedEventHandler  OnRendezvousReceivedEvent;
	}

}
