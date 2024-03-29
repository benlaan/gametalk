using System;
using System.Collections;
using System.Threading;
using Indy.Sockets;
using System.Reflection;

using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

using log4net;

namespace Laan.GameLibrary
{

    public class ClientMessage
    {

        public ClientMessage(byte[] data, IOHandlerSocket socket)
        {
            Data = data;
            Socket = socket;
        }

        public byte[] Data { get; set; }
        public IOHandlerSocket Socket { get; set; }
    }

	public class GameServer : GameSocket
	{
		private ClientNodeList          _clients;
		private Queue                   _messages;
		private System.Threading.Thread _processor;
		private UDPServer               _udpServer;

		static private GameServer _instance = null;

		private GameServer() : base()
		{
			_tcpServer.OnDisconnect += new TIdServerThreadEvent(OnClientDisconnected);
		}

		internal void Initialise()
		{
			_clients = new ClientNodeList();
			_messages = new Queue();

			_processor = new System.Threading.Thread(new ThreadStart(ProcessQueue));

			_udpServer = new UDPServer();

			_udpServer.DefaultPort = Config.RendezvousPort;
			_udpServer.BroadcastEnabled = true;
			_udpServer.ThreadedEvent = true;
			_udpServer.OnUDPRead += new TUDPReadEvent(OnRendezvousRead);
		}

		private ClientNode AddClient(string clientName, string hostName, int port)
		{
			ClientNode clientNode = _clients.Find(clientName);
			if(clientNode == null)
			{
				clientNode = new ClientNode(clientName, hostName, port);
                _clients.Add(clientNode);
				if (OnNewClientConnectionEvent != null)
					OnNewClientConnectionEvent(this, _clients);
			}
            return clientNode;
		}

        private void SendEntityStoreNewClient(ClientNode client)
        {
            // stream all entities and their scalar properties
            foreach (BaseEntity entity in ServerDataStore.Instance)
            {
                BinaryStreamWriter writer = new BinaryStreamWriter(entity.StreamSize);
                writer.WriteByte(MessageCode.Create);

                entity.Serialise(writer);
                client.AddMessage(new PendingMessage(writer));
            }

            // stream all entities child entity properties
            foreach (BaseEntity entity in ServerDataStore.Instance)
                entity.SerialiseChildEntities(client);
        }

        private bool RootEntityAlreadySent()
        {
            return _clients.Count > 1;
        }

		/// <summary>
		/// Allows the implementor to alter the default behaviour if required
		/// thus restricting information to clients for business reasons
		/// (eg. Fog of War)
		/// </summary>
		private bool AllowClientUpdate(ClientNode client, PendingMessage message)
		{
			bool IsAllowed = true;

			// allow the implementor to alter the default behaviour if required
			if(OnAllowClientUpdateEvent != null)
				OnAllowClientUpdateEvent(this, message, ref IsAllowed);

			return IsAllowed;
		}

		/// <summary>
		/// Event triggered by UDP received.  Provides a user hook to indicate
		/// whether the message contains the correct keyword
		/// If it is successful, sends back a formatted message indicating the server's
		/// game name, location and port in the format "Name:Host:Port"
		/// </summary>
		private void OnRendezvousRead(object sender, byte[] data, SocketHandle binding)
		{
			string receivedText = System.Text.Encoding.ASCII.GetString(data);

			bool isValid = false;

			Log.Debug(
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

				Log.Debug("Sending UDP response: " + sSend);
				_udpServer.Send(binding.PeerIP, binding.PeerPort, sSend);
			}
		}

		/// <summary>
		/// Handles the forwarding of messages to the appropriate entity
		/// that is located in the data store and returns the required
		/// response (if required) to the client
		/// </summary>
		private void ProcessMessage(ClientMessage message)
		{
			using (BinaryStreamReader reader = new BinaryStreamReader(message.Data))
			{
				int id = reader.ReadInt32();
				BaseEntity entity = ServerDataStore.Instance.Find(id);

				if (entity == null)
					throw new Exception(String.Format("entity {0} not found", id));

				Laan.GameLibrary.Entity.Server server = (entity.Communication() as Laan.GameLibrary.Entity.Server);
				byte[] response = server.ProcessCommand(reader);
                if (response != null)
					WriteToSocket(message.Socket, response);
			}

			// allow custom processing by the game server instance, if desired
			if(OnProcessMessageEvent != null)
				OnProcessMessageEvent(this, message);
		}

		/// <summary>
		/// Threaded event responsible for processing the pending message queue
		/// including the notification of results to the clients
		/// </summary>
        private void ProcessQueue()
        {
            ClientMessage m;

            // repeat until Server is inactive
            while (this.Active)
            {

                // if a client generated message is awaiting processing
                while (_messages.Count > 0)
                {
                    // get the message
                    lock (this._messages)
                    {
                        m = (ClientMessage)_messages.Dequeue();
                        //Log.Debug("Dequeing Message: " + Message.ToString(m));
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
			foreach(ClientNode c in _clients)
			{
				lock (c.PendingMessages)
				{
					if(c.PendingMessages.Count > 0)
					{
						Log.Debug(String.Format("Sending Pending Message to {0}:{1}", c.Host, c.Port));

						_tcpClient.Connect(c.Host, c.Port);
						try
						{
                            foreach (PendingMessage pendingMessage in c.PendingMessages)
                            {
                                WriteToSocket(_tcpClient.Socket, pendingMessage);
                                // Log.Debug("Message: {0}", Message.ToString(pendingMessage)));
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

		public void BeginRendezvous()
		{
			_udpServer.Active = true;
		}

		public void EndRendezvous()
		{
			_udpServer.Active = false;
		}

		internal void AddUpdateMessage(BinaryStreamWriter writer)
		{
            PendingMessage message = new PendingMessage(writer);

			foreach(ClientNode client in _clients)
				if(AllowClientUpdate(client, message))
					AddMessage(client, message);
		}

		protected override byte[] InternalOnServerExecute(Context context)
		{
			Log.Info("GameServer: Receiving Client Message");

			byte[] data = base.InternalOnServerExecute(context);
            if (data == null)
                return null;

			ClientMessage message = new ClientMessage(data, context.Connection.Socket);

			using (BinaryStreamReader reader = new BinaryStreamReader(data))
			{
				int id = reader.ReadInt32();
				if (id == Command.Login)
				{
					string clientName = reader.ReadString();
					string hostName   = reader.ReadString();
					int    port       = reader.ReadInt32();

					Log.Debug(String.Format("Client {0} has connected from {1}:{2}", clientName, hostName, port));

					ClientNode client = AddClient(clientName, hostName, port);
                    if (RootEntityAlreadySent())
                        SendEntityStoreNewClient(client);
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

		public void AddMessage(ClientNode client, PendingMessage message)
		{
			if (client != null)
				client.AddMessage(message);
			else
				foreach(ClientNode c in _clients)
					c.AddMessage(message);
		}

		public bool Active
		{
			get { return _tcpServer.Active; }
			set {
				_tcpServer.Active = value;

				if(value)
				{
					_processor.Start();
					Log.Debug(String.Format("Listening (TCP) on Port {0}", Port));
				}
				else
				{
					_processor.Abort();
					Log.Debug(String.Format("Stopped Listening (TCP) on Port {0}", Port));
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

        public string Name { get; set; }

		/// Public Events
		public event OnAllowClientUpdateEventHandler   OnAllowClientUpdateEvent;
		public event OnNewClientConnectionEventHandler OnNewClientConnectionEvent;
		public event OnClientDisconnectionEventHandler OnClientDisconnectionEvent;
		public event OnProcessMessageEventHandler      OnProcessMessageEvent;
		public event OnRendezvousReceivedEventHandler  OnRendezvousReceivedEvent;
	}
}
