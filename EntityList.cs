using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using Laan.GameLibrary.Data;
using Laan.Library.Logging;

namespace Laan.GameLibrary.Entity
{

	public class Commands
	{
		internal const int Add    = 0;
		internal const int Remove = 1;
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

	public abstract class EntityList : BaseEntity, IEnumerable
	{

		private ArrayList _list;

		public EntityList() : base()
		{
			_list = new ArrayList();
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

		public BaseEntity this[int index]
		{
			get {
				return (BaseEntity)_list[index];
			}
		}
	 }

}
