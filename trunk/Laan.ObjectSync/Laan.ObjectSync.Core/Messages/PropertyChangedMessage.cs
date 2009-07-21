using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Linq.Expressions;

namespace Laan.ObjectSync
{
    public class PropertyChangedMessage<T,V> : PropertyChangedMessage 
    {
        public PropertyChangedMessage( int entityID, Expression<Func<T, V>> name, V value )
            : base( entityID, ( (MemberExpression) name.Body ).Member.Name, value )
        {
        }
    }

    public class PropertyChangedMessage : Message
    {
        public PropertyChangedMessage( BinaryReader reader ) : base( reader )
        {
        }

        /// <summary>
        /// Initializes a new instance of the PropertyChangedMessage class.
        /// </summary>
        public PropertyChangedMessage( int entityID, string name, object value )
        {
            EntityID = entityID;
            Name = name;
            Type = value.GetType();
            Value = value;
        }

        protected override void Deserialise( BinaryReader reader )
        {
            EntityID = ReadInt32( reader );
            Name = ReadString( reader );
            Type = RetrieveType( reader );
            Value = ReadValue( reader, Type );
        }

        protected override void Serialise( BinaryWriter writer )
        {
            writer.Write( EntityID );
            writer.Write( Name );
            WriteType( writer, Type );
            WriteValue( writer, Type, Value );
        }

        public int EntityID { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }

        public override void Execute( ISyncClient syncClient )
        {
            var entity = syncClient.FindEntity( EntityID );
            if ( Value is Int32 )
                Value = syncClient.FindEntity( (int)Value );

            entity.SetValue( Name, Value );
        }
    }
}
