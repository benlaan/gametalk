using System;
using System.Runtime.Serialization;

namespace Laan.ObjectSync
{
    [DataContract]
    public class TypeDefinition
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string FullName { get; set; }

        /// <summary>
        /// Initializes a new instance of the TypeOptimisationMessage class.
        /// </summary>
        public TypeDefinition( int id, string fullName )
        {
            ID = id;
            FullName = fullName;
        }

        public void Execute( ISyncClient syncClient )
        {
            Message.StoreType( ID, FullName );
        }
    }
}
