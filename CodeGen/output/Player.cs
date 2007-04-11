using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Business.Risk.Player
{

    namespace Server
    {
        public class Player : BasePlayer
        {

            // --------------- Protected --------------------------------------------

            protected override void ProcessCommand(BinaryStreamReader reader) 
            {
                
            }
            
            // --------------- Public -----------------------------------------------

            public Player() : base()
            {

            }
        }
    }

    namespace Client
    {
        public class Player : BasePlayer
        {
            // --------------- Public -----------------------------------------------

            public Player() : base()
            {

            }
		}
    }
}

