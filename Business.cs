using System;

namespace Laan.Test.Business
{

    class Scale
    {

        internal const int Full = 100;
        internal const int None = 0;

        private int _value = Scale.None;

        public Scale(int value)
        {
            _value = value;
        }

        public static implicit operator int(Scale scale)
        {
            // allows the class to be cast to an int
            return scale.Value;
        }

        public int Scale Value {

            get { return _value; }
            set {

                if(value > Scale.None && value <= Scale.Full)
                    _value = value;
            }
        }
    }
}