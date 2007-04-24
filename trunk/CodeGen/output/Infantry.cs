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

            protected override void ProcessCommand(BinaryStreamReader reader) 
            {
                
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

