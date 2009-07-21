using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using MbUnit.Framework;
using Rhino.Mocks;
using Laan.ObjectSync.Client;
using System.IO;

namespace Laan.ObjectSync.Test
{
    [TestFixture]
    public class MessageTest
    {
        [FixtureSetUp]
        public void Setup()
        {
            var types = new[] 
            {
                typeof( Root ), typeof( Child ),
                typeof( string ), typeof( Int32 ), typeof( DateTime ), typeof( Byte[] ), typeof( Guid )
            };

            for ( int index = 0; index < types.Length; index++ )
                Message.StoreType( index, types[ index ].AssemblyQualifiedName );
        }

        [Test]
        public void Can_Serialise_RootEntityMessages()
        {
            // Exercise
            int id = 0;
            var message = new RootEntityMessage( typeof( Root ), id );
            var bytes = message.ToBytes();

            // Verify outcome
            Assert.IsNotNull( bytes );

            using ( BinaryReader reader = new BinaryReader( new MemoryStream( bytes ) ) )
            {
                var deserialisedMessage = new RootEntityMessage( reader );

                Assert.IsNotNull( deserialisedMessage );
                Assert.AreEqual( typeof( Root ), deserialisedMessage.Type );
                Assert.AreEqual( id, deserialisedMessage.EntityID );
                Assert.AreEqual( -1, deserialisedMessage.ParentID );
            }
        }

        [Test]
        public void Can_Serialise_ItemAddedMessage()
        {
            // Exercise
            int rootID = 0;
            int id = 1;
            var message = new ItemAddedMessage( typeof( Child ), id, rootID );
            var bytes = message.ToBytes();

            // Verify outcome
            Assert.IsNotNull( bytes );

            using ( BinaryReader reader = new BinaryReader( new MemoryStream( bytes ) ) )
            {
                var deserialisedMessage = new ItemAddedMessage( reader );

                Assert.IsNotNull( deserialisedMessage );
                Assert.AreEqual( typeof( Child ), deserialisedMessage.Type );
                Assert.AreEqual( id, deserialisedMessage.EntityID );
                Assert.AreEqual( rootID, deserialisedMessage.ParentID );
            }
        }

        [Test]
        public void Can_Serialise_String_PropertyChangedMessage()
        {
            // Exercise
            int id = 1;
            var message = new PropertyChangedMessage( id, "StringField", "SomeValue" );
            var bytes = message.ToBytes();

            // Verify outcome
            Assert.IsNotNull( bytes );

            using ( BinaryReader reader = new BinaryReader( new MemoryStream( bytes ) ) )
            {
                var deserialisedMessage = new PropertyChangedMessage( reader );

                Assert.IsNotNull( deserialisedMessage );
                Assert.AreEqual( typeof( string ), deserialisedMessage.Type );
                Assert.AreEqual( id, deserialisedMessage.EntityID );
                Assert.AreEqual( "StringField", deserialisedMessage.Name );
                Assert.AreEqual( "SomeValue", deserialisedMessage.Value );
            }
        }

        [Test]
        public void Can_Serialise_Int_PropertyChangedMessage()
        {
            // Exercise
            int id = 1;
            var message = new PropertyChangedMessage( id, "IntField", 22 );
            var bytes = message.ToBytes();

            // Verify outcome
            Assert.IsNotNull( bytes );

            using ( BinaryReader reader = new BinaryReader( new MemoryStream( bytes ) ) )
            {
                var deserialisedMessage = new PropertyChangedMessage( reader );

                Assert.IsNotNull( deserialisedMessage );
                Assert.AreEqual( typeof( Int32 ), deserialisedMessage.Type );
                Assert.AreEqual( id, deserialisedMessage.EntityID );
                Assert.AreEqual( "IntField", deserialisedMessage.Name );
                Assert.AreEqual( 22, deserialisedMessage.Value );
            }
        }

        [Test]
        public void Can_Serialise_DateTime_PropertyChangedMessage()
        {
            // Exercise
            int id = 1;
            DateTime now = DateTime.Now;
            var message = new PropertyChangedMessage( id, "DateTimeField", now );
            var bytes = message.ToBytes();

            // Verify outcome
            Assert.IsNotNull( bytes );

            using ( BinaryReader reader = new BinaryReader( new MemoryStream( bytes ) ) )
            {
                var deserialisedMessage = new PropertyChangedMessage( reader );

                Assert.IsNotNull( deserialisedMessage );
                Assert.AreEqual( typeof( DateTime ), deserialisedMessage.Type );
                Assert.AreEqual( id, deserialisedMessage.EntityID );
                Assert.AreEqual( "DateTimeField", deserialisedMessage.Name );
                Assert.IsTrue( ( now - (DateTime) deserialisedMessage.Value ).TotalMilliseconds < 1000 );
            }
        }

        [Test]
        public void Can_Serialise_Guid_PropertyChangedMessage()
        {
            // Exercise
            int id = 1;
            Guid value = Guid.NewGuid();
            var message = new PropertyChangedMessage( id, "GuidField", value );
            var bytes = message.ToBytes();

            // Verify outcome
            Assert.IsNotNull( bytes );

            using ( BinaryReader reader = new BinaryReader( new MemoryStream( bytes ) ) )
            {
                var deserialisedMessage = new PropertyChangedMessage( reader );

                Assert.IsNotNull( deserialisedMessage );
                Assert.AreEqual( typeof( Guid ), deserialisedMessage.Type );
                Assert.AreEqual( id, deserialisedMessage.EntityID );
                Assert.AreEqual( "GuidField", deserialisedMessage.Name );
                Assert.AreEqual( value, deserialisedMessage.Value );
            }
        }

        [Test]
        public void Can_Serialise_Bytes_PropertyChangedMessage()
        {
            // Exercise
            int id = 1;
            Byte[] value = new Byte[ 2 ] { 0x12, 0xFF };
            var message = new PropertyChangedMessage( id, "BytesField", value );
            var bytes = message.ToBytes();

            // Verify outcome
            Assert.IsNotNull( bytes );

            using ( BinaryReader reader = new BinaryReader( new MemoryStream( bytes ) ) )
            {
                var deserialisedMessage = new PropertyChangedMessage( reader );

                Assert.IsNotNull( deserialisedMessage );
                Assert.AreEqual( typeof( Byte[] ), deserialisedMessage.Type );
                Assert.AreEqual( id, deserialisedMessage.EntityID );
                Assert.AreEqual( "BytesField", deserialisedMessage.Name );
                Assert.AreEqual( value, deserialisedMessage.Value );
            }
        }

        [Test]
        public void Can_Serialise_Entity_PropertyChangedMessage()
        {
            // Exercise
            int id = 1;
            Child value = new Child() { ID = 22 };
            var message = new PropertyChangedMessage( id, "Sibling", value );
            var bytes = message.ToBytes();

            // Verify outcome
            Assert.IsNotNull( bytes );

            using ( BinaryReader reader = new BinaryReader( new MemoryStream( bytes ) ) )
            {
                var deserialisedMessage = new PropertyChangedMessage( reader );

                Assert.IsNotNull( deserialisedMessage );
                Assert.AreEqual( typeof( Child ), deserialisedMessage.Type );
                Assert.AreEqual( id, deserialisedMessage.EntityID );
                Assert.AreEqual( "Sibling", deserialisedMessage.Name );
                Assert.AreEqual( value.ID, deserialisedMessage.Value );
            }
        }
    }
}
