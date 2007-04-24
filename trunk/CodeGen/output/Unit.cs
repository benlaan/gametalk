using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Unit
{

    namespace Server
    {
        public class Unit : BaseUnit
        {

            // --------------- Protected --------------------------------------------

            protected override void ProcessCommand(BinaryStreamReader reader) 
            {
                
            }
            
            // --------------- Public -----------------------------------------------

            public Unit() : base()
            {

            }
        }
    }

    namespace Client
    {
        public class Unit : BaseUnit
        {
            // --------------- Public -----------------------------------------------

            public Unit() : base()
            {

            }
		}
    }
}

