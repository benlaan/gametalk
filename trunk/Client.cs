using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using Indy.Sockets;

using Laan.Library.Logging;
using Laan.GameLibrary.Data;

namespace Laan.GameLibrary
{

	public class GameClient : GameSocket
    {
        /// Public Constructor

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
			Log.WriteLine("Client Socket: attempting to connect to.. {0}:{1}", _tcpClient.Host, _tcpClient.Port);

			_tcpClient.Connect();
			Log.WriteLine("Client Socket: Connected to {0}:{1}", _tcpClient.Host, _tcpClient.Port);
			SendConnectionProtocol(userName);
			Log.WriteLine("ProtocolSent: {0}", userName);

			// establish listener to get updates from Server
			Log.WriteLine("Server Socket: commencing listening .. {0}", _tcpServer.DefaultPort);
			_tcpServer.Active = true;
			Log.WriteLine("Server Socket: Listening on {0}", _tcpServer.DefaultPort);

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
            Log.WriteLine("Client: Sending Message: {0}: {1})", this, Message.ToString(message));
			WriteToSocket(_tcpClient.Socket, message);
			Log.WriteLine("Client: Message Sent");
		}

        // start broadcasting for a GameServer that responds
        // to a particular text
        public void StartRendezvous(string rendezvousText)
        {
             _rendezvousText = rendezvousText;
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

				SendMessage(writer.DataStream);

				Log.WriteLine("MessageSent(Login)");
			}
		}

        // Called on the timer, to commencing searching for GameServer
        // listeners
        private void OnBroadcastRendevous(object sender, EventArgs e)
        {
            try
            {
                // send the Rendezvous text
                _udpClient.Send(_rendezvousText);

                // receive the response
                string sData = _udpClient.ReceiveString(1);

                // test for valid input - should be "Host:Port"
                IsBroadcastFound(sData);

                Log.WriteLine("Broadcasting..");
            }
            catch (Exception ex)
            {
                Log.WriteLine(ex.Message);
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
                Debug.Assert(data.Length == 2, "Incorrect Broadcast Format - should be Host:Port");
                OnBroadcastFoundEvent(this, data[0], Int32.Parse(data[1]));
            }
        }

        protected override byte[] InternalOnServerExecute(Context context)
        {
            byte[] message = base.InternalOnServerExecute(context);

			if (OnProcessMessageEvent != null)
				OnProcessMessageEvent(this, message);
			/*
                need to respond to the message by either:

                a) Building the instance (typed), and adding it to the correct list
                b) finding the instance and modifying it's data
                c) finding the instance and removing it from the correct list

            */

            return message;
        }

		public event OnProcessMessageEventHandler OnProcessMessageEvent;
		public event OnBroadcastFoundEventHandler OnBroadcastFoundEvent;
  }
}
