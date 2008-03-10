using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using Indy.Sockets;

using log4net;
using Laan.GameLibrary.Data;

namespace Laan.GameLibrary
{

	public class GameClient : GameSocket
    {
        /// Public Constructor

        const int cRENDEVOUS_COUNT = 10;

        static GameClient _instance = null;

        private GameClient() : base()
        {

        }

        static public GameClient Instance
        {
            get {
                if (_instance == null)
                {
                    _instance = new GameClient();
                    _instance.Initialise();
                }
                return _instance;
            }
        }

        private Timer _timer;

		private UDPClient _udpClient;
        private string _rendezvousText = "Rendezvous";
        private bool _active = false;
        private int _rendezvousCount = cRENDEVOUS_COUNT;

        public void Initialise()
        {
            _udpClient = new UDPClient();
			_udpClient.Host = "";
			_udpClient.Port = Config.RendezvousPort;

            _timer = new Timer();
            _timer.Interval = Config.RendezvousFrequency;
            _timer.Tick += new EventHandler(OnBroadcastRendevous);
		}

		public void Connect(string Host, int Port, string userName)
		{
			this._tcpClient.Host = Host;
			this._tcpClient.Port = Port;

			this.Connect(userName);
        }

        // Used to establish the client to server tcp connection, used when
        // sending information to the server
		public void Connect(string userName)
		{
			// establish connection to GameServer
            //Log.Debug("Client Socket: attempting to connect to.. {0}:{1}", _tcpClient.Host, _tcpClient.Port);

			_tcpClient.Connect();
			Log.Debug(String.Format("Client Socket: Connected to {0}:{1}", _tcpClient.Host, _tcpClient.Port));
			SendConnectionProtocol(userName);
            //Log.Debug("ProtocolSent: {0}", userName);

			// establish listener to get updates from Server
            //Log.Debug("Server Socket: commencing listening .. {0}", _tcpServer.DefaultPort);
			_tcpServer.Active = true;
            //Log.Debug("Server Socket: Listening on {0}", _tcpServer.DefaultPort);

			_active = true;
		}

        public bool Active
		{
			get { return _active; }
        }

		// When the server is no longer required.
        public void Disconnect()
		{
			StopRendezvous();
            _tcpClient.Disconnect();
            _tcpServer.Active = false;
			_active = false;
		}

		// The mechansim by which the client initiates a communication to the server
        public void SendMessage(byte[] message)
        {
            this.SendMessage(message, false);
        }

        public byte[] SendMessage(byte[] message, bool expectResult)
        {
            Log.Debug(String.Format("Client: Sending Message: {0}: {1})", this, Message.ToString(message)));
			WriteToSocket(_tcpClient.Socket, message);
			Log.Debug("Client: Message Sent");
            if (expectResult)
                return ReadFromSocket(_tcpClient.Socket);
            else
                return null;
		}

        // start broadcasting for a GameServer that responds
        // to a particular text
        public void StartRendezvous(string rendezvousText)
        {
			 _rendezvousText = rendezvousText;
             _rendezvousCount = cRENDEVOUS_COUNT;

			 OnBroadcastRendevous(this, new EventArgs());
            _timer.Start();
		}

        // stop broadcasting
        public void StopRendezvous()
        {
            _timer.Stop();
		}

		// After a connection is established, inform Server
        // of client details, to allow server to establish Update mechanism
        private void SendConnectionProtocol(string userName)
		{
			using (BinaryStreamWriter writer = new BinaryStreamWriter(12))
			{
				// Connection protocol consists of user name,
				// machine name and inbound message port (ie. for tcpServer)
				writer.WriteInt32(Command.Login);
				writer.WriteString(userName);
				writer.WriteString(Environment.MachineName);
				writer.WriteInt32(Config.InboundPort);

				SendMessage(writer.DataStream, false);

				Log.Info("MessageSent(Login)");
			}
		}

        // Called on the timer, to commencing searching for GameServer
        // listeners
        private void OnBroadcastRendevous(object sender, EventArgs e)
        {
            try
			{
                //Log.Debug("Sending Broadcast to {0}", _udpClient.Port);

				// send the Rendezvous text
				_udpClient.Broadcast(_rendezvousText, _udpClient.Port);

                // receive the response
                string sData = _udpClient.ReceiveString(1);

                // test for valid input - should be "Name:Host:Port"
                IsBroadcastFound(sData);

                Log.Info("Broadcasting..");

                _rendezvousCount--;

                if (_rendezvousCount == 0)
                    StopRendezvous();
			}
			catch (Exception ex)
            {
                Log.Debug(ex.Message);
				StopRendezvous();
				throw;
            }
        }

        // Validates the input
        private void IsBroadcastFound(string sData)
        {
            if((sData != "") && (OnBroadcastFoundEvent != null))
            {
                string[] data = Regex.Split(sData, ":");
				Debug.Assert(data.Length == 3, "Incorrect Broadcast Format - should be Name:Host:Port");
				OnBroadcastFoundEvent(this, data[0], data[1], Int32.Parse(data[2]));
            }
        }

        protected override byte[] InternalOnServerExecute(Context context)
        {
            byte[] message = base.InternalOnServerExecute(context);

			if (OnProcessMessageEvent != null)
				OnProcessMessageEvent(this,
                    new ClientMessage(message, context.Connection.Socket)
                );
			/*
                need to respond to the message by either:

                a) Building the instance (typed), and adding it to the correct list
                b) finding the instance and modifying it's data
                c) finding the instance and removing it from the correct list

            */

            return message;
        }

        public bool Finding
        {
            get { return _timer.Enabled; }
        }

		public event OnProcessMessageEventHandler OnProcessMessageEvent;
		public event OnBroadcastFoundEventHandler OnBroadcastFoundEvent;
  }
}
