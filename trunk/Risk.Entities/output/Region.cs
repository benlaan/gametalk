using System;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Region
{

    namespace Server
    {
        using Borders = Laan.Risk.Border.Server;

        public partial class Region
        {

            // --------------- Private ----------------------------------------------

            private Laan.Risk.Border.Server.BorderList _borders;

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
            public Game.Server.Game Game { get; set; }

            [Browsable(false)]
            public Borders.BorderList Borders
            {
                get
                {
                    Debug.Assert(Game != null, "require a valid Game instance");

                    // check cache first - assumes that the region borders will be static
                    // is this a good assumption?
                    if(_borders == null)
                        _borders = Game.Borders.FindByRegion(this);

                    return _borders;
                }
            }

            public override string ToString()
            {
                return String.Format("({0}) {1}\n\n Border: {2}", ID, Name, Borders.ToString());
            }

        }
    }

    namespace Client
    {
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
