using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Unit.Armour
{

    namespace Server
    {
        public class Armour : BaseArmour
        {

            // --------------- Protected --------------------------------------------

            protected override byte[] ProcessCommand(BinaryStreamReader reader)
            {
                return null;                
            }
            
            // --------------- Public -----------------------------------------------

            public Armour() : base()
            {

            }
        }
    }

    namespace Client
    {
        public class Armour : BaseArmour
        {
            // --------------- Public -----------------------------------------------

            public Armour() : base()
            {

            }
		}
    }
}

