using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Player
{
    namespace Server
    {
        public partial class PlayerList { }

        public partial class Player
        {

            // --------------- Protected --------------------------------------------

            protected override byte[] ProcessCommand(BinaryStreamReader reader)
            { 
                return null;
            }
            
            // --------------- Public -----------------------------------------------

            public override void Initialise()
            {
                base.Initialise();
            }
        }
    }

    namespace Client
    {
        public partial class PlayerList { }

        public partial class Player
        {
            // --------------- Public -----------------------------------------------

            public override void Initialise()
            {
                base.Initialise();
            }
		}
    }
}

