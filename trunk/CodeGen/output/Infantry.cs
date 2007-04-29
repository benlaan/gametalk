using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Unit.Infantry
{

    namespace Server
    {
        public class Infantry : BaseInfantry
        {

            // --------------- Protected --------------------------------------------

            protected override byte[] ProcessCommand(BinaryStreamReader reader)
            {
                return null;                
            }
            
            // --------------- Public -----------------------------------------------

            public Infantry() : base()
            {

            }
        }
    }

    namespace Client
    {
        public class Infantry : BaseInfantry
        {
            // --------------- Public -----------------------------------------------

            public Infantry() : base()
            {

            }
		}
    }
}

