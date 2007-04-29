using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Nation
{

    namespace Server
    {
        public class Nation : BaseNation
        {

            // --------------- Protected --------------------------------------------

            protected override byte[] ProcessCommand(BinaryStreamReader reader) 
            {
                return null;                
            }
            
            // --------------- Public -----------------------------------------------

            public Nation() : base()
            {
                Prestige = 100;
                Technology = 100;
            }
        }
    }

    namespace Client
    {
        public class Nation : BaseNation
        {
            // --------------- Public -----------------------------------------------

            public Nation() : base()
            {

            }
		}
    }
}

