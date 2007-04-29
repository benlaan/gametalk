using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Region
{

    namespace Server
    {
        public class Region : BaseRegion
        {

            // --------------- Protected --------------------------------------------

            protected override byte[] ProcessCommand(BinaryStreamReader reader)
            {
                return null;                
            }
            
            // --------------- Public -----------------------------------------------

            public Region() : base()
            {

            }
        }
    }

    namespace Client
    {
        public class Region : BaseRegion
        {
            // --------------- Public -----------------------------------------------

            public Region() : base()
            {

            }
		}
    }
}

