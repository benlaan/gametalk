using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Laan.ObjectSync
{
    public class ItemAddedMessage : Message
    {
        public ItemAddedMessage( BinaryReader reader ) : base( reader )
        {

        }

        /// <summary>
        /// Initializes a new instance of the ItemAddMessage class.
        /// </summary>
        public ItemAddedMessage( Type type, int entityID, int parentID )
        {
            Type = type;
            EntityID = entityID;
            ParentID = parentID;
        }

        public Type Type { get; set; }
        public int EntityID { get; set; }
        public int ParentID { get; set; }

        protected override void Deserialise( BinaryReader reader )
        {
            EntityID = ReadInt32( reader );
            ParentID = ReadInt32( reader );
            Type = RetrieveType( reader );
        }

        protected override void Serialise( BinaryWriter writer )
        {
            writer.Write( EntityID );
            writer.Write( ParentID );
            WriteType( writer, Type );
        }

        public override void Execute( ISyncClient syncClient )
        {
            var item = Activator.CreateInstance( Type ) as IEntity;
            item.ID = EntityID;
            
            syncClient.AddEntity( item );
        }
    }
}