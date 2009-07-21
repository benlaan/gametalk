using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

using Laan.ObjectSync.Client;

namespace Laan.ObjectSync
{
    public class SyncClient : ISyncClient
    {
        private ClientEndPoint _clientEndPoint;
        private IMessageClient _client;
        private Dictionary<int, IEntity> _instances;

        /// <summary>
        /// Initializes a new instance of the SyncClient class.
        /// </summary>
        public SyncClient( IMessageClient client )
        {
            _clientEndPoint = new ClientEndPoint( Environment.MachineName, Environment.UserName );

            _client = client;
            _instances = new Dictionary<int, IEntity>();
        }

        public void ProcessMessages()
        {
            List<Message> messages;
            do
            {
                messages = _client.GetMessages();
                Active = ( messages != null );

                if ( Active )
                    foreach ( Message message in messages )
                        message.Execute( this );

            }
            while ( Active );
        }

        #region ISyncClient Members

        public void Connect()
        {
            try
            {
                _client.Connect();
            }
            catch ( Exception ex )
            {
                throw new ConnectionFailedException( ex );
            }
        }

        public void AddEntity( IEntity entity )
        {
            _instances.Add( entity.ID, entity );
        }

        public void RemoveEntity( int entityID )
        {
            _instances.Remove( entityID );
        }

        public IEntity FindEntity( int id )
        {
            return _instances[ id ];
        }

        public bool Active { get; set; }
        public IEntity Root { get; set; }

        #endregion
    }
}
