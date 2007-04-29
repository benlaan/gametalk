using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Border
{

    namespace Server
    {
        public class Border : BaseBorder
        {

            // --------------- Protected --------------------------------------------

            protected override byte[] ProcessCommand(BinaryStreamReader reader)
            {
                return null;                
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

