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

    public class Commands
    {
        internal const int Add    = 0;
        internal const int Remove = 1;
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
			_streamSize = 4;
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

	public abstract class EntityList : BaseEntity, IEnumerable
	{

        private ArrayList _list;

        public EntityList() : base()
        {
            _list = new ArrayList(10);
		}

		public IEnumerator GetEnumerator()
		{
        	return _list.GetEnumerator();
		}

        public virtual void Add(BaseEntity entity)
        {
            _list.Add(entity);
        }

        public virtual void Remove(BaseEntity entity)
        {
            _list.Remove(entity);
        }

        public bool IsEmpty
        {
            get { return (_list.Count == 0); }
        }

        public BaseEntity Find(int identity)
        {
            foreach(BaseEntity e in _list)
            {
                if(e.ID == identity)
                {
                    return e;
                }
            }
            return null;
		}

		public override Communication Communication()
        {
            return null;
        }

//        public IList DataSource
//        {
//            get {
//                return _list;
//            }
//        }

        public BaseEntity this[int index]
        {
            get {
                return (BaseEntity)_list[index];
            }
        }
     }

    public delegate byte[] OnProcessCommandEventHandler(BinaryStreamReader reader);

	public delegate void OnNewEntityEventHandler(BaseEntity instance);
	public delegate void OnRootEntityEventHandler(BaseEntity rootEntity);
	public delegate void OnModifyEntityEventHandler(BaseEntity entity);

	public class ServerEntityList : EntityList
	{
		Server _server;

		public ServerEntityList()
		{
			_server = new GameLibrary.Entity.Server(this);
		}

		public override Communication Communication()
		{
			return _server;
		}

		public override void Add(BaseEntity entity)
		{
			base.Add(entity);
			_server.Modify(this.ID, Commands.Add, entity.ID);
		}

		public override void Remove(BaseEntity entity)
		{
			base.Remove(entity);
			_server.Modify(this.ID, Commands.Remove, entity.ID);
		}
	}

	public class ClientEntityList : EntityList
	{
		Client _client;

		public ClientEntityList()
		{
			_client = new GameLibrary.Entity.Client();
			_client.OnModify += new OnServerMessageEventHandler(OnModify);
		}

        private void OnModify(Byte field, BinaryStreamReader reader)
        {
            DoModify(field, reader);
        }

		public override Communication Communication()
		{
			return _client;
		}

        protected virtual void DoModify(Byte field, BinaryStreamReader reader)
		{
			int id = reader.ReadInt32();
			BaseEntity e = ClientDataStore.Instance.Find(id);

			Debug.Assert(e != null, String.Format("ClientDataStore.Find({0}): not found!", id));
			switch (field)
			{
				case Commands.Add:
					this.Add(e);
					break;
				case Commands.Remove:
					this.Remove(e);
					break;
				default:
					throw new Exception("ClientEntityList: Command must be Add or Remove");
			}
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
