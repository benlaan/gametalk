using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Business.Risk.Nation
{

    namespace Server
    {
        public class Nation : BaseNation
        {

            // --------------- Protected --------------------------------------------

            protected override void ProcessCommand(BinaryStreamReader reader) 
            {
                
            }
            
            // --------------- Public -----------------------------------------------

            public Nation() : base()
            {

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

