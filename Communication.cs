using System;
using System.Collections;
using System.Diagnostics;

using Laan.GameLibrary.Data;

namespace Laan.GameLibrary.Entity
{
    public class Communication
    {
    }

    // Responsible for receiving messages from Laan.GameLibrary.GameServer
    // and updating properties
    public class Client: Communication
    {

        private System.Collections.Hashtable _recency = null;

        public Client()
        {
            _recency = new Hashtable();
        }

        internal void Modify(BinaryStreamReader reader)
        {
            Debug.WriteLine("Client.Modify()");
            byte field = reader.ReadByte();
            if (OnModify != null)
                OnModify(field, reader);
        }

        public event OnServerMessageEventHandler OnModify;

        public TimeSpan LastUpdated(byte field)
        {
            return System.DateTime.Now - (System.DateTime)_recency[field];
        }

        public void UpdateRecency(byte field)
        {
            _recency[field] = System.DateTime.Now;
        }
    }

    // base class for all server-side objects, used by the server
    // for processing purposes.
    public class Server: Communication
    {
        static int uniqueID = 10000;

        private BaseEntity _entity;

        public event OnProcessCommandEventHandler OnProcessCommand;

        public Server(BaseEntity entity)
        {
			_entity = entity;
			Size = _entity.StreamSize;

            entity.ID = uniqueID++;
            Debug.WriteLine(String.Format("Server({0}): {1}", entity.Name, entity.ID));

            Insert();
		}

        ~Server()
		{
			Debug.WriteLine("~Server()");
            Delete();
		}
		private int _size;

        internal void Insert()
        {
			Debug.WriteLine("Server.Insert()");
			BinaryStreamWriter writer = new BinaryStreamWriter(Size);
            // call the virtual method Serialise
            Serialise(writer);
            // send as message
            GameServer.Instance.AddUpdateMessage(writer.DataStream);
        }

        internal void Delete()
        {
            Debug.WriteLine("Server.Delete()");
			BinaryStreamWriter writer = new BinaryStreamWriter(Size);
			writer.WriteByte(MessageCode.Delete);
			writer.WriteInt32(_entity.ID);
			GameServer.Instance.AddUpdateMessage(writer.DataStream);
		}

		public void Modify(int ID, byte field, object value)
		{
			Debug.WriteLine(String.Format("Server.Modify({0},{1},{2})", ID, field, value));

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

        private void Serialise(BinaryStreamWriter writer)
		{
			writer.WriteByte(MessageCode.Create);
			 _entity.Serialise(writer);

			 int length = writer.DataStream.Length;
		}

        public void ProcessCommand(BinaryStreamReader reader)
        {
            if (OnProcessCommand != null)
                OnProcessCommand(reader);
        }

		public int Size
		{
			get { return _size; }
			set { _size = value; }
		}
	}
    
}
