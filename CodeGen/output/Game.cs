using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Business.Risk.Game
{

    namespace Server
    {
        public class Game : BaseGame
        {

            // --------------- Protected --------------------------------------------

            protected override void ProcessCommand(BinaryStreamReader reader) 
            {
                
            }
            
            // --------------- Public -----------------------------------------------

            public Game() : base()
            {

            }
        }
    }

    namespace Client
    {
        public class Game : BaseGame
        {
            // --------------- Public -----------------------------------------------

            public Game() : base()
            {

            }
		}
    }
}

