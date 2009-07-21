using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Collections;

namespace Laan.ObjectSync.Server
{
    [ServiceBehavior( InstanceContextMode = InstanceContextMode.Single )]
    public class MessageServer : IMessageServer
    {
        #region IMessageServer Members

        private bool _started;
        private List<Guid> _clients;
        private List<Type> _types;
        private Dictionary<Guid, Queue<byte[]>> _messages;

        /// <summary>
        /// Initializes a new instance of the MessageServer class.
        /// </summary>
        public MessageServer()
        {
            _clients = new List<Guid>();
            _types = new List<Type>();
            _messages = new Dictionary<Guid, Queue<byte[]>>();
        }

        public int RegisterType( Type type )
        {
            _types.Add( type );
            return _types.Count;
        }

        public Type ResolveType( int id )
        {
            return _types[ id ];
        }

        public List<TypeDefinition> RegisterClient( Guid clientID )
        {
            _clients.Add( clientID );
            _messages[ clientID ] = new Queue<byte[]>();

            var list = new List<TypeDefinition>();
            for ( int index = 0; index < _types.Count; index++ )
                list.Add( new TypeDefinition( index, _types[ index ].AssemblyQualifiedName ) );
            return list;
        }

        public List<byte[]> GetMessages( Guid clientID )
        {
            var queue = _messages[ clientID ];
            lock ( queue )
            {
                var list = queue.ToList();

                queue.Clear();
                return list;
            }
        }

        #endregion
    }
}