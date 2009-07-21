using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using log4net;

namespace Laan.ObjectSync.Server
{
    public class Host : IDisposable
    {

        private ILog _logger;
        private ServiceHost _host;

        public Host( ILog logger )
        {
            _logger = logger;
            var server = new MessageServer();

            var types = new List<Type>();
            RegisterTypes( types );
            foreach ( var type in types )
                server.RegisterType( type );

            _host = new ServiceHost( server );
            _host.UnknownMessageReceived += ( sender, e ) => { _logger.Error( "Unknown Message Received" ); };
        }

        protected virtual void RegisterTypes( List<Type> list )
        {
            list.AddRange( 
                new[] 
                { 
                    typeof( ItemAddedMessage ),
                    typeof( PropertyChangedMessage ),
                    typeof( TypeDefinition )
                }
            );
        }

        #region IDisposable Members

        public void Dispose()
        {
            _host.Close();
        }

        #endregion

        public void Listen()
        {
            _host.Open();
        }

    }
}
