using System;
using System.Collections.Generic;
using System.IO;

namespace Laan.ObjectSync
{
    public class RootEntityMessage : ItemAddedMessage
    {
        /// <summary>
        /// Initializes a new instance of the RootEntityMessage class.
        /// </summary>
        public RootEntityMessage( BinaryReader reader ) : base( reader )
        {
        }
                /// <summary>
        /// Initializes a new instance of the ItemAddMessage class.
        /// </summary>
        public RootEntityMessage( Type type, int entityID ) : base ( type, entityID, -1 )
        {
        }

        public override void Execute( ISyncClient syncClient )
        {
            RootInstance = Activator.CreateInstance( Type ) as IEntity;
            RootInstance.ID = EntityID;

            syncClient.AddEntity( RootInstance );
            syncClient.Root = RootInstance;
        }

        public IEntity RootInstance { get; set; }
    }
}
