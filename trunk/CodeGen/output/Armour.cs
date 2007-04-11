using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Business.Risk.Unit.Armour
{

    namespace Server
    {
        public class Armour : BaseArmour
        {

            // --------------- Protected --------------------------------------------

            protected override void ProcessCommand(BinaryStreamReader reader) 
            {
                
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

