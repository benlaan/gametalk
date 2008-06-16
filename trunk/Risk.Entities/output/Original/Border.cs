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
        using Regions = Laan.Risk.Region.Server;
        using System.ComponentModel;

        public enum Connection
        {
            Sea, Road, Rail
        }

        public partial class BorderList 
        {
            internal BorderList FindByRegion(Regions.Region region)
            {
                BorderList result = new BorderList();

                foreach (Border border in this)
                    if (border.BoundedBy(region))
                        result.Add(border);

                return result;
            }
        }

        public partial class Border
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

            [Browsable(false)]
            public Connection[] Connections
            {
                get { return _connections; }
                set { _connections = value; }
            }

            internal bool BoundedBy(Regions.Region region)
            {
                return this.FromRegion == region || this.ToRegion == region;
            }

            public override string ToString()
            {
                return String.Format("({0}) {1}", ID, Name);
            }
        }
    }

    namespace Client
    {
        public partial class Border
        {
            // --------------- Public -----------------------------------------------

            public override void Initialise()
            {
                base.Initialise();
            }
		}
    }
}

