using System;

namespace Laan.ObjectSync.Test
{
    public class Root : Entity
    {
        /// <summary>
        /// Initializes a new instance of the RootObject class.
        /// </summary>
        public Root() : base()
        {
            Children = new EntityList<Child>();
        }

        public EntityList<Child> Children { get; set; }

        public override void SetValue( string Name, object Value )
        {
            switch ( Name )
            {
                case "Children":
                    Children = Value as EntityList<Child>;
                    break;
            }
        }
    }
}
