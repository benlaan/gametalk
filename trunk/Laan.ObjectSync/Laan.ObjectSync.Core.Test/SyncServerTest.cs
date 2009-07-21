using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using MbUnit.Framework;

using Rhino.Mocks;

namespace Laan.ObjectSync.Test
{
    [TestFixture]
    public class SyncServerTest
    {
        private MockRepository _mock;

        [SetUp]
        public void Setup()
        {
            _mock = new MockRepository();
        }

        [Test]
        public void Can_Create_Sync_Server_Instance()
        {
            var syncServer = new SyncServer( 9999 );

            Assert.IsNotNull( syncServer );
            Assert.AreEqual( 9999, syncServer.Port );
        }

        [Test]
        public void Can_Send_Root_Message()
        {
            var syncServer = new SyncServer( 9999 );
            Entity root = new Root();
            syncServer.Root = root;

            IClientEndPoint client = _mock.DynamicMock<IClientEndPoint>();
            syncServer.Clients.Add( client );

            using ( _mock.Record() )
            {
                Expect.Call( () => client.SendMessages( null ) ).IgnoreArguments().Repeat.Any();
            }

            using ( _mock.Playback() )
            {
                syncServer.UpdateClients();
            } 
        }

        [Test]
        public void Can_Send_New_Instance_Message()
        {
            var syncServer = new SyncServer( 9999 );
            Entity root = new Root();
            syncServer.Root = root;
        }
    }
}
