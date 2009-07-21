using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laan.ObjectSync
{
    public interface IEntity
    {
        int ID { get; set; }
        void SetValue( string Name, object Value );
    }

    public abstract class Entity : IEntity
    {
        public Entity() : this( null )
        {
        }

        public Entity( Entity parent )
        {
            Parent = parent;
        }

        public int ID { get; set; }
        public Entity Parent { get; set; }
        public string Name { get; set; }

        public abstract void SetValue( string Name, object Value );
    }
}