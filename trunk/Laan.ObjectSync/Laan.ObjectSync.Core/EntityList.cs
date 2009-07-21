using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laan.ObjectSync
{
    public class EntityList<T> : List<T>, IEntity
    {

        public EntityList()
        {
        }

        #region IEntity Members

        public int ID { set; get; }

        public virtual void SetValue( string name, object value )
        {
            switch ( name )
            {
                case "Add":
                    Add( ( T )value );
                    break;
                case "Remove":
                    Remove( ( T )value );
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
