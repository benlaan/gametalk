using System;
using System.Collections.Generic;
using System.IO;

using Laan.ObjectSync.Client.MessageServer;

namespace Laan.ObjectSync.Client
{
    internal class MessageClient : IMessageClient
    {
        private Guid _id;
        private MessageServerClient _client;
        private Dictionary<int, Type> _storedTypes;

        public MessageClient( Guid clientID )
        {
            _id = clientID;
            _client = new MessageServerClient();
        }

        public void Connect()
        {
            _storedTypes = new Dictionary<int, Type>();
            var types = _client.RegisterClient( _id );

            foreach ( TypeDefinition type in types )
                _storedTypes[ type.ID ] = Type.GetType( type.FullName );
        }

        public List<Message> GetMessages()
        {
            byte[][] messageBytes = _client.GetMessages( _id );

            var result = new List<Message>( messageBytes.Length );
            foreach ( byte[] message in messageBytes )
            {
                using ( BinaryReader reader = new BinaryReader( new MemoryStream( message ) ) )
                {
                    Type type = _storedTypes[ reader.ReadInt32() ];
                    Message msg = ( Message )Activator.CreateInstance( type, reader );

                    result.Add( msg );
                }
            }
            return result;
        }
    }
}
