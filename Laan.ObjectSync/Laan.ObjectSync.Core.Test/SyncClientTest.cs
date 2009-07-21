using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using MbUnit.Framework;

using Rhino.Mocks;
using Laan.ObjectSync.Client;

namespace Laan.ObjectSync.Test
{
    [TestFixture]
    public class SyncClientTest
    {
        private MockRepository _mock;
        private IMessageClient _client;

        [SetUp]
        public void Setup()
        {
            _mock = new MockRepository();
            _client = _mock.DynamicMock<IMessageClient>();
        }

        [Test]
        public void Can_Create_Sync_Client_Instance()
        {
            var syncClient = new SyncClient( _client );

            Assert.IsNotNull( syncClient );
        }

        [Test]
        public void Can_Connect()
        {
            // Setup
            var syncClient = new SyncClient( _client );

            // Exercise
            Root root = new Root();

            using ( _mock.Record() )
            {
                Expect.Call( () => _client.Connect() ).Repeat.Once();
            }

            using ( _mock.Playback() )
            {
                syncClient.Connect();
                
                // Verify
                Assert.IsTrue( true );
            }
        }

        [Test]
        [ExpectedException( typeof( ConnectionFailedException ) )]
        public void Will_Fail_If_Connect_Doesnt_Return_Root()
        {
            var syncClient = new SyncClient( _client );

            // Exercise
            using ( _mock.Record() )
            {
                Expect.Call( () => _client.Connect() ).Throw( new Exception() ).IgnoreArguments();
            }

            using ( _mock.Playback() )
            {
                syncClient.Connect();
            }
        }

        [Test]
        public void Can_Receive_Root_And_Children()
        {
            // Setup
            var syncClient = new SyncClient( _client );

            int rootID = 1;
            Root root = new Root() { ID = rootID };
            int childListID = 2;
            int child1ID = 3;
            int child2ID = 4;

            using ( _mock.Record() )
            {
                // Send Root Object
                Expect.Call( () => _client.Connect() ).Repeat.Once();

                // Send New Entity (child) Object
                Expect.Call( _client.GetMessages() ).Return(
                    new List<Message>()
                    {
                        new RootEntityMessage( typeof( Root ), root.ID ),
                        new ItemAddedMessage( typeof( EntityList<Child> ), childListID, root.ID ),

                        // Link children list to root
                        new PropertyChangedMessage( root.ID, "Children", childListID ),

                        // Send New Entity (child) Object, linking it to the parent
                        new ItemAddedMessage( typeof( Child ), child1ID, childListID ),

                        // Send New Entity (child) Object, linking it to the parent
                        new ItemAddedMessage( typeof( Child ), child2ID, childListID ),

                        // Link child to children list
                        new PropertyChangedMessage( childListID, "Add", child1ID ),
                        new PropertyChangedMessage( childListID, "Add", child2ID ),

                        // Set Field to 'Value' on child
                        new PropertyChangedMessage( child1ID, "StringField", "Value" ),

                        //// Set child objects to be siblings of each other
                        new PropertyChangedMessage( child2ID, "Sibling", child1ID ),
                        new PropertyChangedMessage( child1ID, "Sibling", child2ID ),
                    }
                );

                // Empty message indicates disconnect
                Expect.Call( _client.GetMessages() ).Return( null ).Repeat.Once();
            }

            // Exercise
            using ( _mock.Playback() )
            {
                syncClient.Connect();
                syncClient.ProcessMessages();
            }

            // Verify
            IEntity retrievedInstance = syncClient.Root;
            Assert.IsNotNull( retrievedInstance );

            Assert.IsTrue( retrievedInstance is Root );
            var rootInstance = retrievedInstance as Root;

            Assert.IsNotNull( rootInstance );
            Assert.AreEqual( rootID, rootInstance.ID );

            Assert.IsNotNull( rootInstance.Children );
            Assert.AreEqual( childListID, rootInstance.Children.ID );
            Assert.AreEqual( 2, rootInstance.Children.Count );

            var child1 = rootInstance.Children.First();
            var child2 = rootInstance.Children.Last();

            Assert.AreEqual( "Value", child1.StringField );
            Assert.AreEqual( child2, child1.Sibling );
            Assert.AreEqual( child1, child2.Sibling );

        }
    }
}
