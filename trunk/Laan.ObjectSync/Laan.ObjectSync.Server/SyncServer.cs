using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Laan.ObjectSync
{
    public class SyncServer : ISyncServer
    {

        public SyncServer( int port )
        {
            Clients = new List<IClientEndPoint>();
            OutboundMessages = new List<Message>();

            Port = port;
        }

        #region ISyncServer Members

        public int Port { get; private set; }

        public Entity Connect( IClientEndPoint client )
        {
            Clients.Add( client );
            return Root;
        }

        public virtual List<Message> Receive()
        {
            return null;
        }

        #endregion

        public List<IClientEndPoint> Clients { get; set; }
        public IList<Message> OutboundMessages { get; private set; }
        public Entity Root { get; set; }

        public void UpdateClients()
        {
            IList<Message> outbound;
            lock ( OutboundMessages )
            {
                outbound = new List<Message>( OutboundMessages );
                OutboundMessages.Clear();
            }
            foreach ( var client in Clients )
            {
                var messages = outbound
                    .Where( msg => msg.Recipient == client.ID || msg.Recipient == Guid.Empty )
                    .OrderBy( msg => msg.Date )
                    .ToList();

                client.SendMessages( messages );
            }
        }
    }
}
