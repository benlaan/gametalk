using System;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Indy.Sockets;

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
        private OnBroadcastFoundEventHandler _onBroadcastFoundEvent;
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

        public void Connect(string Host, int Port)
        {
            this._tcpClient.Host = Host;
            this._tcpClient.Port = Port;

            this.Connect();
        }

        // Used to establish the client to server tcp connection, used when
        // sending information to the server
        public void Connect()
		{
			// establish connection to GameServer
			Debug.WriteLine(String.Format("Client Socket: attempting to connect to.. {0}:{1}", _tcpClient.Host, _tcpClient.Port));
			_tcpClient.Connect();

			// establish listener to get updates from Server
			Debug.WriteLine(String.Format("Server Socket: commencing listening .. {0}", _tcpServer.DefaultPort));
			_tcpServer.Active = true;

			SendConnectionProtocol();

			_active = true;

			Debug.WriteLine(String.Format("Client Socket: Connected to {0}:{1}", _tcpClient.Host, _tcpClient.Port));
			Debug.WriteLine(String.Format("Server Socket: Listening on {0}", _tcpServer.DefaultPort));
        }

        public bool Active
        {
            get {return _active;}
        }

        // When the server is no longer required.
        public void Disconnect()
        {
            StopRendezvous();
            _tcpClient.Disconnect();
            _tcpServer.Active = false;
            _active = false;
        }

        // The mechansim by which the client initiates a communication
        // to the server
        public void SendMessage(byte[] message)
        {
            Debug.WriteLine(String.Format("Client: Sending Message: {0}: {1})", this, Message.ToString(message)));
			WriteToSocket(_tcpClient.Socket, message);
			Debug.WriteLine("Client: Message Sent");
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
        private void SendConnectionProtocol()
        {
            // Connection protocol consists of user name,
            // machine name and inbound message port (ie. for tcpServer)
            _tcpClient.Socket.WriteLn(Config.UserName);
            _tcpClient.Socket.WriteLn(Environment.MachineName);
            _tcpClient.Socket.WriteLn(Config.InboundPort.ToString());
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

                Debug.WriteLine("Broadcasting..");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                StopRendezvous();
            }
        }

        // Validates the input
        private void IsBroadcastFound(string sData)
        {
            if((sData != "") && (_onBroadcastFoundEvent != null))
            {
                string[] data = Regex.Split(sData, ":");
                Debug.Assert(data.Length == 2, "Incorrect Broadcast Format - should be Host:Port");
                _onBroadcastFoundEvent(this, data[0], Int32.Parse(data[1]));
            }
        }

        protected override byte[] InternalOnServerExecute(Context context)
        {
            byte[] message = base.InternalOnServerExecute(context);

            /*
                need to respond to the message by either:

                a) Building the instance (typed), and adding it to the correct list
                b) finding the instance and modifying it's data
                c) finding the instance and removing it from the correct list

            */

            return message;
        }

        public event OnBroadcastFoundEventHandler OnBroadcastFoundEvent
        {
            add {
                _onBroadcastFoundEvent += value;
            }
            remove {
                _onBroadcastFoundEvent -= value;
            }
        }
  }
}
