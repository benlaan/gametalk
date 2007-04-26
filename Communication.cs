using System;
using System.Collections;

using Laan.Library.Logging;
using Laan.GameLibrary.Data;
using System.Drawing;

namespace Laan.GameLibrary.Entity
{
    public class Communication
    {
    }

    // Responsible for receiving messages from Laan.GameLibrary.GameServer
	// and updating properties
	public class Client: Communication
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
			Log.WriteLine("Client.Modify()");
			byte field = reader.ReadByte();
			if (OnModify != null)
				OnModify(field, reader);
		}

		public event OnServerMessageEventHandler OnModify;
    }

    // base class for all server-side objects, used by the server
	// for processing purposes.
	public class Server: Communication
    {
		public Server(BaseEntity entity)
		{
			_entity = entity;
			Size = _entity.StreamSize;

			entity.ID = uniqueID++;
			Log.WriteLine("Server({0}): {1}", entity.Name, entity.ID);

			Insert();
		}

		~Server()
		{
			Log.WriteLine("~Server()");
			Delete();
		}

		private BaseEntity _entity;
		private int _size;
		static int uniqueID = 10000;

		public void Modify(int ID, byte field, object value)
		{
			Log.WriteLine("Server.Modify({0},{1},{2})", ID, field, value);

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

			byte[] data = writer.DataStream;
			GameServer.Instance.AddUpdateMessage(data);
		}

		public void ProcessCommand(BinaryStreamReader reader)
		{
			if (OnProcessCommand != null)
				OnProcessCommand(reader);
		}

		internal void Delete()
		{
			// register the entity with the data store
			ServerDataStore.Instance.Remove(this._entity);

			Log.WriteLine("Server.Delete()");
			BinaryStreamWriter writer = new BinaryStreamWriter(Size);
			writer.WriteByte(MessageCode.Delete);
			writer.WriteInt32(_entity.ID);

			// send as message
			GameServer.Instance.AddUpdateMessage(writer.DataStream);
		}

		internal void Insert()
		{
			Log.WriteLine("Server.Insert()");
			BinaryStreamWriter writer = new BinaryStreamWriter(Size);
			// call the virtual method Serialise
			Serialise(writer);

			// send as message
			GameServer.Instance.AddUpdateMessage(writer.DataStream);

			// register the entity with the data store
			ServerDataStore.Instance.Add(this._entity);
		}

		private void Serialise(BinaryStreamWriter writer)
		{
            writer.WriteByte(MessageCode.Create);
			_entity.Serialise(writer);

			int length = writer.DataStream.Length;
		}

		public int Size
		{
			get { return _size; }
			set { _size = value; }
		}

		public event OnProcessCommandEventHandler OnProcessCommand;
	}
    
}
