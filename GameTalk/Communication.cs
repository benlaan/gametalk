using System;
using System.Collections;
using System.Drawing;
using System.Reflection;

using log4net;

using Laan.GameLibrary.Data;

namespace Laan.GameLibrary.Entity
{
    public class Communication
    {
        protected ILog Log = log4net.LogManager.GetLogger(Assembly.GetEntryAssembly().ManifestModule.Name);
    }

    // Responsible for receiving messages from Laan.GameLibrary.GameServer
	// and updating properties
	public class Client : Communication
    {
		public Client()
		{
			_recency = new Hashtable();
		}

		private System.Collections.Hashtable _recency = null;

		public TimeSpan LastUpdated(byte field)
		{
			return System.DateTime.Now - (System.DateTime)_recency[field];
		}

		public void UpdateRecency(byte field)
		{
			_recency[field] = System.DateTime.Now;
		}

		internal void Modify(BinaryStreamReader reader)
		{
            Log.Info("Client.Modify()");
			byte field = reader.ReadByte();
			if (OnModify != null)
				OnModify(field, reader);
		}

		public event OnServerMessageEventHandler OnModify;
    }

    // base class for all server-side objects, used by the server
	// for processing purposes.
	public class Server : Communication
    {
		public Server(BaseEntity entity)
		{
			_entity = entity;
			Size = _entity.StreamSize;

			entity.ID = uniqueID++;
            Log.Debug(String.Format("Server({0}): {1}", entity.Name, entity.ID));

			Insert();
		}

		~Server()
		{
            Log.Debug("~Server()");
			Delete();
		}

		private BaseEntity _entity;
		static int uniqueID = 10000;

		public void Modify(int ID, byte field, object value)
		{
            Log.Debug(String.Format("Server.Modify({0},{1},{2})", ID, field, value));

			BinaryStreamWriter writer = new BinaryStreamWriter(Size);

			writer.WriteByte(MessageCode.Update);
			writer.WriteInt32(ID);
			writer.WriteByte(field);

			if (value is Int32)
				writer.WriteInt32((int)value);

			if (value is String)
				writer.WriteString((string)value);

			if (value is System.DateTime)
				writer.WriteDateTime((System.DateTime)value);

			if (value is Boolean)
				writer.WriteBoolean((bool)value);

			GameServer.Instance.AddUpdateMessage(writer);
		}

		public byte[] ProcessCommand(BinaryStreamReader reader)
		{
            return OnProcessCommand != null ? OnProcessCommand(reader) : null;
		}

		internal void Delete()
		{
			// register the entity with the data store
			ServerDataStore.Instance.Remove(_entity);

            Log.Info("Server.Delete()");
			BinaryStreamWriter writer = new BinaryStreamWriter(Size);
			writer.WriteByte(MessageCode.Delete);
			writer.WriteInt32(_entity.ID);

			// send as message
			GameServer.Instance.AddUpdateMessage(writer);
		}

		internal void Insert()
		{
            Log.Info("Server.Insert()");
			BinaryStreamWriter writer = new BinaryStreamWriter(Size);
			// call the virtual method Serialise
			Serialise(writer);

			// send as message
			GameServer.Instance.AddUpdateMessage(writer);

			// register the entity with the data store
			ServerDataStore.Instance.Add(_entity);
		}

		private void Serialise(BinaryStreamWriter writer)
		{
            writer.WriteByte(MessageCode.Create);
			_entity.Serialise(writer);

			int length = writer.DataStream.Length;
		}

        public int Size { get; set; }

		public event OnProcessCommandEventHandler OnProcessCommand;
	}
    
}
