using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Business.Risk.Border
{

    namespace Server
    {
        public class Border : BaseBorder
        {

            // --------------- Protected --------------------------------------------

            protected override void ProcessCommand(BinaryStreamReader reader) 
            {
                
            }
            
            // --------------- Public -----------------------------------------------

            public Border() : base()
            {

            }
        }
    }

    namespace Client
    {
        public class Border : BaseBorder
        {
            // --------------- Public -----------------------------------------------

            public Border() : base()
            {

            }
		}
    }
}

