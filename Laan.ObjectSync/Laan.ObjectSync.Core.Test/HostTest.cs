using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MbUnit.Framework;

using log4net;

using Laan.ObjectSync.Server;
using Laan.ObjectSync.Client.MessageServer;

namespace Laan.ObjectSync.Test
{
    [TestFixture]
    public class HostTest
    {
        [Test]
        public void Can_Create_ServiceHost()
        {
            ILog log = LogManager.GetLogger( typeof( Host ) );
            
            using ( var host = new Host( log ) )
            {
                host.Listen();

                Assert.IsNotNull( host );
            }
        }

        [Test]
        public void Can_Register_Client_To_Server()
        {
            ILog log = LogManager.GetLogger( typeof( Host ) );

            using ( var host = new Host( log ) )
            {
                host.Listen();

                var client = new MessageServerClient();
                Guid id = Guid.NewGuid();
                var types = client.RegisterClient( id );

                Assert.IsNotNull( types );
                Assert.IsTrue( types.Any() );
            }
        }

        [Test]
        public void Can_Get_Messages_For_Server()
        {
            ILog log = LogManager.GetLogger( typeof( Host ) );

            using ( var host = new Host( log ) )
            {
                host.Listen();

                var client = new MessageServerClient();
                Guid id = Guid.NewGuid();
                client.RegisterClient( id );

                var messages = client.GetMessages( id );

                Assert.IsNotNull( messages );
            }
        }
    }
}
