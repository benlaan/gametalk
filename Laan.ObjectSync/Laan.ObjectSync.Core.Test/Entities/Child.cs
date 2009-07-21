using System;

namespace Laan.ObjectSync.Test
{
    public class Child : Entity
    {
        public string StringField { get; set; }
        public Child Sibling { get; set; }

        public override void SetValue( string Name, object Value )
        {
            switch ( Name )
            {
                case "StringField":
                    StringField = ( string )Value;
                    break;
                case "Sibling":
                    Sibling = ( Child )Value;
                    break;
            }
        }
    }
}
