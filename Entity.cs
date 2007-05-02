using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using Laan.GameLibrary.Data;
using Laan.Library.Logging;

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

	// generic class to contain all transferable objects
	public abstract class BaseEntity
	{

        // ------------ Private ----------------------------------------------------------

        string  _name = "";
        int     _id;

        // ------------ Protected --------------------------------------------------------

        protected virtual void SetName(string value)
        {
            _name = value;
        }

		public abstract Communication Communication();

        // ------------ Public ------------------------------------------------------------
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

		public BaseEntity()
        {
            _name = "";
			_streamSize = 1;
        }
		private int _streamSize;

		public BaseEntity Entity()
		{
        	return this;
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

	public abstract class BaseEntityClient: BaseEntity
	{

		public BaseEntityClient()
		{
			_client = new GameLibrary.Entity.Client();
			_client.OnModify += new OnServerMessageEventHandler(OnModify);
		}

		// --------------- Private -------------------------------------------------

		private GameLibrary.Entity.Client _client = null;

		private void OnModify(Byte field, BinaryStreamReader reader)
		{
			DoModify(field, reader);
		}

		protected virtual void DoModify(Byte field, BinaryStreamReader reader)
		{
				switch (field)
				{
					case Fields.Name:
						Name = reader.ReadString();
						break;
				}
		}

		public override Communication Communication()
		{
			return _client;
		}

		// --------------- Public --------------------------------------------------

		public GameLibrary.Entity.Client CommClient
		{
			get {
				return _client;
			}
			set {
				_client = value;
			}
		}
	}

	public abstract class BaseEntityServer: BaseEntity
	{
		// --------------- Private -------------------------------------------------

		private Laan.GameLibrary.Entity.Server _server = null;

		// --------------- Protected -----------------------------------------------

		protected override void SetName(string value)
		{
			base.SetName(value);
			CommServer.Modify(this.ID, Fields.Name, value);
		}

		public override Communication Communication()
		{
			return _server;
		}

		protected abstract byte[] ProcessCommand(BinaryStreamReader reader);
		
		// --------------- Public --------------------------------------------------

		public BaseEntityServer() : base()
		{
			_server = new Laan.GameLibrary.Entity.Server(this);
			_server.OnProcessCommand += new OnProcessCommandEventHandler(ProcessCommand);
		}

		public GameLibrary.Entity.Server CommServer
		{
			get {
				return _server;
			}
			set {
				_server = value;
			}
		}
	}

}
