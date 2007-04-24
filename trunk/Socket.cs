using System;
using System.IO;
using Indy.Sockets;
using System.Text;
using System.Runtime.Serialization;

using Laan.Library.Logging;

namespace Laan.GameLibrary
{

	public delegate void OnRendezvousReceivedEventHandler(object sender, string data, ref bool isValid);
	public delegate void OnBroadcastFoundEventHandler(object sender, string Host, int Port);

	public delegate void OnProcessMessageEventHandler(object sender, byte[] message);
	public delegate void OnAllowClientUpdateEventHandler(object sender, byte[] message, ref bool isValid);
	public delegate void OnNewClientConnectionEventHandler(object sender, ClientList clients);

    class Command
	{
		internal const int Login  = 0;
		internal const int Logout = 1;
	}

    public abstract class GameSocket
    {
        
        /// Public Constructor

        public GameSocket()
        {
            _tcpServer = new TCPServer();
			_tcpServer.DefaultPort = Config.InboundPort;
			_tcpServer.OnExecute += new TIdServerThreadEvent(OnServerExecute);

			_tcpServer.OnConnect += new TIdServerThreadEvent(OnServerConnect);
			_tcpServer.OnDisconnect += new TIdServerThreadEvent(OnServerDisconnect);

			_tcpClient = new TCPClient();
			_tcpClient.Port = Config.OutboundPort;
			_tcpClient.Host = Config.OutboundHost;
			_tcpClient.OnDisconnected += new TIdNetNotifyEvent(OnClientDisconnected);
		}

		protected Indy.Sockets.TCPServer _tcpServer;
		protected Indy.Sockets.TCPClient _tcpClient;

		// ---------------- Private Events --------------------------------

		private void OnServerExecute(Context context)
		{
			InternalOnServerExecute(context);          
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
			Log.WriteLine("GameSocket: Receiving Message");

			byte[] data = ReadFromSocket(context.Connection.Socket);

			Log.WriteLine("GameSocket: Message Received: " +  Message.ToString(data));

			return data;
		}

		// -------------- protected Events ----------------------------------

		protected void OnServerConnect(Context context)
		{

		}

		protected void OnClientDisconnected(object sender)
		{
			;//_tcpClient.Disconnect(false);
		}


		protected void OnServerDisconnect(Context context)
		{
//			_tcpClient.Disconnect(false);
		}
	}
}
