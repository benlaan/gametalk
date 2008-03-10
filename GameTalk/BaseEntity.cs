using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

using Laan.GameLibrary.Data;
using log4net;

namespace Laan.GameLibrary.Entity
{

    public delegate void OnServerMessageEventHandler(Byte field, BinaryStreamReader reader);
    public delegate void OnSerialiseEventHandler(BinaryStreamWriter writer);

    public class Fields
    {
        internal const int Name = 0;
    }

	public class MessageCode
	{
		public const byte Create = 0;
		public const byte Update = 1;
		public const byte Delete = 2;
	}

    public class EntityProperty
    {
        public BaseEntity Entity;
        public byte       Field;
    }

	// generic class to contain all transferable objects
	public abstract class BaseEntity
	{
        private const int cNOT_ASSIGNED = -1;

        // ------------ Private ----------------------------------------------------------

        string  _name = "";
        int     _id;

        // ------------ Protected --------------------------------------------------------

        protected virtual void SetName(string value)
        {
            _name = value;
        }

        protected ILog Log = log4net.LogManager.GetLogger(Assembly.GetEntryAssembly().ManifestModule.Name);

        // ------------ Public ------------------------------------------------------------
        
        public abstract Communication Communication();

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

		public BaseEntity()
        {
            _name = "";
			_streamSize = 32;
        }
		private int _streamSize;

		public BaseEntity Entity()
		{
        	return this;
		}

//		public static implicit operator GameLibrary.Entity.Client(BaseEntity entity)
//        {
//            // allows the class to be cast to an Entity.Client class
//            return (Client)entity.GetComms();
//        }
//
//        public static implicit operator GameLibrary.Entity.Server(BaseEntity entity)
//		{
//			// allows the class to be cast to an Entity.Server class
//            return (Server)entity.GetComms();
//        }

        protected void WriteID(BinaryStreamWriter writer, BaseEntity entity, byte field)
        {
            writer.WriteByte(MessageCode.Update);
            writer.WriteInt32(this.ID);
            writer.WriteByte(field);

            if (entity != null)
                writer.WriteInt32(entity.ID);
            else
                writer.WriteInt32(cNOT_ASSIGNED);
        }

        public string Name
        {
            get { return _name; }
            set { SetName(value); }
        }

        public virtual void Serialise(BinaryStreamWriter writer)
        {
            Type t = this.GetType();
            writer.WriteString(t.FullName);
            
            writer.WriteInt32(this.ID);
            writer.WriteString(this.Name);
        }

        protected virtual List<EntityProperty> GetEntityProperties()
        {
            return null;
        }

        public void SerialiseChildEntities(IClientNode client)
        {
            List<EntityProperty> properties = GetEntityProperties();

            if (properties != null)
                foreach (EntityProperty property in properties)
                {
                    BinaryStreamWriter writer = new BinaryStreamWriter(10); 
                    WriteID(writer, property.Entity, property.Field);
                    client.AddMessage(new PendingMessage(writer));
                }
        }

        public virtual void Initialise()
        {
        }

        public virtual void Deserialise(BinaryStreamReader reader)
        {
            _id = reader.ReadInt32();
            _name = reader.ReadString();
        }

		internal int StreamSize
		{
			get { return _streamSize; }
			set { _streamSize = value; }
		}
    }

}
