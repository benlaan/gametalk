using System;
using System.IO;
using Indy.Sockets;
using System.Text;
using System.Runtime.Serialization;

using log4net;
using System.Reflection;

namespace Laan.GameLibrary
{

	public delegate void OnRendezvousReceivedEventHandler(object sender, string data, ref bool isValid);
	public delegate void OnBroadcastFoundEventHandler(object sender, string Name, string Host, int Port);

	public delegate void OnProcessMessageEventHandler(object sender, ClientMessage message);
	public delegate void OnAllowClientUpdateEventHandler(object sender, PendingMessage message, ref bool isValid);
	public delegate void OnNewClientConnectionEventHandler(object sender, ClientNodeList clients);
	public delegate void OnClientDisconnectionEventHandler(object sender, ClientNodeList clients);

    class Command
	{
		internal const int Login  = 0;
		internal const int Logout = 1;
	}

    public abstract class GameSocket
    {
        
        /// Public Constructor

        protected ILog Log = log4net.LogManager.GetLogger(Assembly.GetEntryAssembly().ManifestModule.Name);

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
		}

		protected Indy.Sockets.TCPServer _tcpServer;
		protected Indy.Sockets.TCPClient _tcpClient;

		// ---------------- Private Events --------------------------------

		private void OnServerExecute(Context context)
		{
			InternalOnServerExecute(context);          
		}

		// --------------- Protected Methods ------------------------------

        public void WriteToSocket(IOHandlerSocket socket, PendingMessage message)
        {
            //socket.Write(message.Length, false);

            byte[] data = new byte[message.Length];
            for (int index = 0; index < message.Length; index++)
			    data[index] = message.Data[index];

            //socket.WriteDirect(ref data);
            WriteToSocket(socket, data);
        }

        public void WriteToSocket(IOHandlerSocket socket, byte[] message)
        {
            socket.Write(message.Length, false);
            socket.WriteDirect(ref message);
        }

        protected byte[] ReadFromSocket(IOHandlerSocket socket)
        {
            byte[] data = null;
            try
            {
                int iSize = socket.ReadInteger(false);
                socket.ReadBytes(ref data, iSize, false);
            }
            catch (EIdConnClosedGracefully) 
            {
                socket.CloseGracefully();
                return null;
            } 
            return data;
		}

		protected virtual byte[] InternalOnServerExecute(Context context)
		{
			Log.Info("GameSocket: Receiving Message");

			byte[] data = ReadFromSocket(context.Connection.Socket);

			Log.Debug("GameSocket: Message Received: " +  Message.ToString(data));

			return data;
		}

		// -------------- protected Events ----------------------------------

		protected void OnServerConnect(Context context)
		{

		}

		protected void OnServerDisconnect(Context context)
		{
//			_tcpClient.Disconnect(false);
		}
	}
}
