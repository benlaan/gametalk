using System;
using System.Diagnostics;
using System.IO;
using Indy.Sockets;
using System.Text;
using System.Runtime.Serialization;

namespace Laan.GameLibrary
{
    
	public delegate void OnMessageReceivedEventHandler(object sender, byte[] message);
	public delegate void OnRendezvousReceivedEventHandler(object sender, string data, ref bool isValid);
	public delegate void OnBroadcastFoundEventHandler(object sender, string Host, int Port);
	public delegate void OnAllowClientUpdateEventHandler(object sender, byte[] message, ref bool isValid);
	public delegate void OnNewClientConnectionEventHandler(object sender, ClientList clients);

    public abstract class GameSocket
    {
        
        /// Public Constructor

        public GameSocket()
        {
            _tcpServer = new TCPServer();
			_tcpServer.DefaultPort = Config.InboundPort;
			_tcpServer.OnExecute += new TIdServerThreadEvent(OnServerExecute);
            
            _tcpClient = new TCPClient();
            _tcpClient.Port = Config.OutboundPort;
            _tcpClient.Host = Config.OutboundHost;
        }
        
        protected Indy.Sockets.TCPServer _tcpServer;
		protected Indy.Sockets.TCPClient _tcpClient;

        // ---------------- Private Events --------------------------------

		private void OnServerExecute(Context AContext)
		{
			InternalOnServerExecute(AContext);
		}

		// --------------- Protected Methods ------------------------------

        protected void WriteToSocket(IOHandlerSocket socket, byte[] data)
        {
            socket.WriteLn(data.Length.ToString());
            socket.WriteDirect(ref data);
        }

        protected byte[] ReadFromSocket(IOHandlerSocket socket)
        {
            byte[] data = null;
            int iSize = Int32.Parse(socket.ReadLn());
            socket.ReadBytes(ref data, iSize, false);
            return data;
		}

		protected virtual byte[] InternalOnServerExecute(Context context)
		{
			Debug.WriteLine("GameSocket: Receiving Message");

			byte[] data = ReadFromSocket(context.Connection.Socket);

			Debug.WriteLine("GameSocket: Message Received: " +  Message.ToString(data));

			if(OnClientMessageEvent != null)
				OnClientMessageEvent(this, data);

			return data;
		}

		// -------------- Public Events ----------------------------------

		public event OnMessageReceivedEventHandler OnClientMessageEvent;

    }
}
