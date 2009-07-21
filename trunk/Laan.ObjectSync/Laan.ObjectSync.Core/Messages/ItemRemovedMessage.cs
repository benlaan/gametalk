using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Laan.ObjectSync
{
    public class ItemRemovedMessage : Message
    {
        public ItemRemovedMessage( BinaryReader reader ) : base( reader )
        {

        }

        /// <summary>
        /// Initializes a new instance of the ItemAddMessage class.
        /// </summary>
        public ItemRemovedMessage( int entityID )
        {
            EntityID = entityID;
        }

        public int EntityID { get; set; }

        protected override void Deserialise( BinaryReader reader )
        {
            EntityID = ReadInt32( reader );
        }

        protected override void Serialise( BinaryWriter writer )
        {
            writer.Write( EntityID );
        }

        public override void Execute( ISyncClient syncClient )
        {
            syncClient.RemoveEntity( EntityID );
        }
    }
}