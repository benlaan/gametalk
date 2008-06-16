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
        public partial class RegionList { }

        public partial class Region
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
        public partial class RegionList { }

        public partial class Region
        {
            // --------------- Public -----------------------------------------------

            public override void Initialise()
            {
                base.Initialise();
            }
		}
    }
}

