using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Laan.ObjectSync
{
    public abstract class Message
    {
        private byte[] _data;
        private const int MaxMessageLength = 100;
        protected static Dictionary<int, Type> _types;

        static Message()
        {
            _types = new Dictionary<int, Type>();
        }

        public Message()
        {
            Recipient = Guid.Empty;
        }

        protected Message( BinaryReader reader )
        {
            Deserialise( reader );
        }

        public static Type RetrieveType( BinaryReader reader )
        {
            int id = reader.ReadInt32();
            Type type;

            if ( id > _types.Count )
                throw new ArgumentException( String.Format( "Type with key {0} not found", id ) );
            else
                type = _types[ id ];

            return type;
        }

        public static void StoreType( int id, string type )
        {
            _types.Add( id, Type.GetType( type, true ) );
            if ( id != _types.Count - 1 )
                throw new Exception( "Index Mismatch" );
        }

        protected virtual void Deserialise( BinaryReader reader )
        {
            // DO NOT inherit this code - use only this (and it's opposite [below]) or none of it!!
            PropertyInfo[] properties = GetType().GetProperties( BindingFlags.Instance | BindingFlags.Public );
            foreach ( var property in properties )
            {
                object value = ReadValue( reader, RetrieveType( reader ) );
                property.SetValue( this, value, null );
            }
        }

        protected virtual void Serialise( BinaryWriter writer )
        {
            // DO NOT inherit this code - use only this (and it's opposite [above]) or none of it!!
            PropertyInfo[] properties = GetType().GetProperties( BindingFlags.Instance | BindingFlags.Public );
            foreach ( var property in properties )
            {
                WriteValue( writer, property.PropertyType, property.GetValue( this, null ) );
            }
        }

        #region Read Methods

        protected string ReadString( BinaryReader reader )
        {
            return reader.ReadString();
        }

        protected Guid ReadGuid( BinaryReader reader )
        {
            return new Guid( reader.ReadBytes( 16 ) );
        }

        protected Int32 ReadInt32( BinaryReader reader )
        {
            return reader.ReadInt32();
        }

        protected DateTime ReadDateTime( BinaryReader reader )
        {
            return DateTime.Parse( reader.ReadString() );
        }

        protected Byte[] ReadBytes( BinaryReader reader )
        {
            var length = reader.ReadInt32();
            return reader.ReadBytes( length );
        }

        protected object ReadValue( BinaryReader reader, Type type )
        {
            var map = new Dictionary<Type, Func<BinaryReader, object>>()
            {
                { typeof(Int32),    r => ReadInt32( r ) },
                { typeof(String),   r => ReadString( r ) },
                { typeof(Guid),     r => ReadGuid( r ) },
                { typeof(DateTime), r => ReadDateTime( r ) },
                { typeof(Byte[]),   r => ReadBytes( r ) },
            };

            if ( typeof( IEntity ).IsAssignableFrom( type ) )
                return ReadInt32( reader );
            else
            {
                Func<BinaryReader, object> action = null;
                if ( map.TryGetValue( type, out action ) )
                    return action( reader );
                else
                    throw new Exception( String.Format( "Type Not Implemented: {0}", type ) );
            }
        }

        #endregion

        #region Write Methods

        protected void WriteValue( BinaryWriter writer, Type type, object value )
        {
            var map = new Dictionary<Type, Action>()
            {
                { typeof(Int32),    () => writer.Write( (Int32) value ) },
                { typeof(String),   () => writer.Write( (String) value ) },
                { typeof(Guid),     () => WriteGuid(writer, (Guid) value ) },
                { typeof(DateTime), () => writer.Write( (string) ((DateTime)value).ToString() ) },
                { typeof(Byte[]),   () => WriteBytes( writer, (Byte[]) value ) },
            };

            Action action = null;
            if ( value is IEntity )
            {
                IEntity entity = (IEntity) value;
                writer.Write( entity.ID );
            }
            else
                if ( map.TryGetValue( type, out action ) )
                    action();
                else
                    throw new Exception( String.Format( "Type Not Implemented: {0}", type.Name ) );
        }

        protected void WriteGuid( BinaryWriter writer, Guid value )
        {
            writer.Write( value.ToByteArray() );
        }

        protected void WriteBytes( BinaryWriter writer, byte[] data )
        {
            writer.Write( data.Length );
            writer.Write( data );
        }

        protected void WriteType( BinaryWriter writer, Type type )
        {
            int id = _types.Where( tp => tp.Value == type ).First().Key;
            if (id == -1 )
                throw new ArgumentOutOfRangeException();
            writer.Write( id );
        }

        #endregion

        public DateTime Date { get; private set; }
        public Guid Recipient { get; set; }

        public byte[] ToBytes()
        {
            if ( _data != null )
                return _data;

            var buffer = new byte[ MaxMessageLength ];
            using ( Stream stream = new MemoryStream( buffer ) )
            {
                using ( var writer = new BinaryWriter( stream ) )
                {
                    Serialise( writer );

                    _data = new byte[ stream.Position ];
                    Array.Copy( buffer, _data, stream.Position );
                }
            }

            return _data;
        }

        public abstract void Execute( ISyncClient syncClient );

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}