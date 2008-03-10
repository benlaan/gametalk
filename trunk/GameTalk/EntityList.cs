using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

using Laan.GameLibrary.Data;
using log4net;
using System.Text;
using System.ComponentModel;

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

	public abstract class EntityList : BaseEntity, IEnumerable
	{

		private List<BaseEntity> _list;

		public EntityList() : base()
		{
            _list = new List<BaseEntity>();
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

        protected override List<EntityProperty> GetEntityProperties()
        {
            List<EntityProperty> result = new List<EntityProperty>();
            foreach (BaseEntity entity in this)
                result.Add(new EntityProperty() { Entity = entity, Field = Commands.Add });
            return result;
        }

        [Browsable(false)]
        public bool IsEmpty
		{
			get { return (_list.Count == 0); }
		}

        public BaseEntity FindByName(string name)
        {
            return _list.Find(entity => entity.Name == name);
        }

		public BaseEntity Find(int identity)
		{
            return _list.Find(entity => entity.ID == identity);
		}

		public override Communication Communication()
		{
			return null;
		}

		public BaseEntity this[int index]
		{
            get { return _list[index]; }
		}

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (BaseEntity entity in _list)
                result.Append((result.Length > 0 ? ", " : "") + entity.ToString());
            
            return result.ToString();
        }

	 }

}
