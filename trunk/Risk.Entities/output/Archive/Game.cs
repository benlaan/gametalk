using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Game
{

    class Command
    {
        internal const int PlayerJoin = 0;
        //		internal const int PlayerLeave = 1;
        //		internal const int Start = 2;
        //		internal const int Quit = 3;
        internal const int PlayerReady = 1;
    }

    namespace Server
    {
        using Laan.Risk.Region.Server;
        using Laan.Risk.Player.Server;
        using Laan.Risk.Border.Server;

        public partial class Game
        {

            // --------------- Protected --------------------------------------------

            protected override byte[] ProcessCommand(BinaryStreamReader reader)
            {
                int command = reader.ReadInt32();
                switch (command)
                {
                    case Command.PlayerJoin:
                        return PlayerJoin(reader);
                    default:
                        return null;
                }
            }
            
            // --------------- Public -----------------------------------------------

            public override void Initialise()
            {
                base.Initialise();
            }

            public byte[] PlayerJoin(BinaryStreamReader reader)
            {
                Laan.Risk.Player.Server.Player p = new Laan.Risk.Player.Server.Player();

                p.Nation.Name = reader.ReadString();
                p.Nation.ShortName = reader.ReadString();
                p.Nation.Leader = reader.ReadString();
                p.Colour = reader.ReadInt32();
                Players.Add(p);

                Log.Info("MessageReceived(PlayerJoin)");

                return BinaryHelper.Write(new FieldType[] { FieldType.Integer }, new object[] { p.ID });
            }

            public void Start()
            {
                // Build Regions
                var regions = new[]
                {
                    new { Name = "SA",  Economy = 25, Oil = 10 },
                    new { Name = "WA",  Economy = 30, Oil = 35 },
                    new { Name = "NSW", Economy = 65, Oil =  0 },
                    new { Name = "VIC", Economy = 40, Oil = 30 },
                    new { Name = "NT",  Economy = 10, Oil =  0 },
                    new { Name = "QLD", Economy = 45, Oil = 15 },
                    new { Name = "TAS", Economy = 10, Oil = 30 }
                };

                foreach (var region in regions)
                {
                    _regions.Add(
                        new Region()
                        {
                            Name = region.Name,
                            Economy = region.Economy,
                            EconomicWeight = 1,
                            Oil = region.Oil,
                            Game = this
                        }
                    );
                }

                var borders = new[]
                {
                    new { From = "SA", To = "WA", ConnectionTypes = new Connection[] { Connection.Road, Connection.Rail } },
                    new { From = "SA", To = "NSW", ConnectionTypes = new Connection[] { Connection.Road, Connection.Rail } },
                    new { From = "SA", To = "VIC", ConnectionTypes = new Connection[] { Connection.Road, Connection.Rail } },
                    new { From = "SA", To = "NT", ConnectionTypes = new Connection[] { Connection.Road, Connection.Rail } },
                    new { From = "SA", To = "QLD", ConnectionTypes = new Connection[] { Connection.Road } },
                    new { From = "WA", To = "NT", ConnectionTypes = new Connection[] { Connection.Road, Connection.Rail } },
                    new { From = "VIC", To = "TAS", ConnectionTypes = new Connection[] { Connection.Sea } },
                    new { From = "VIC", To = "NSW", ConnectionTypes = new Connection[] { Connection.Road, Connection.Rail } },
                    new { From = "QLD", To = "NSW", ConnectionTypes = new Connection[] { Connection.Road, Connection.Rail } },
                    new { From = "QLD", To = "NT" , ConnectionTypes = new Connection[] { Connection.Road } }
                };

                foreach (var border in borders)
                {
                    Region from = _regions.FindByName(border.From) as Region;
                    Region to = _regions.FindByName(border.To) as Region;

                    _borders.Add(
                        new Border()
                        {
                            Name = String.Format("{0}-{1}", from.Name, to.Name),
                            FromRegion = from,
                            ToRegion = to,
                            Connections = border.ConnectionTypes
                        }
                    );
                }

                BorderList list = ((Region)_regions.FindByName("SA")).Borders;

                this.Started = true;
            }

            public bool AllPlayersReady
            {
                get
                {
                    bool result = true;
                    foreach (Player player in _players)
                        result &= player.Ready;

                    return result;
                }
            }
        }
    }

    namespace Client
    {
        public delegate void OnGameStarted(object sender, bool started);

        public partial class Game
        {
            // --------------- Public -----------------------------------------------

            public override void Initialise()
            {
                base.Initialise();
            }

            public bool Started
            {
                get { return _started; }
                set
                {
                    if (_started != value)
                    {
                        _started = value;
                        if (GameStarted != null)
                            GameStarted(this, value);
                    }
                }
            }

            public event OnGameStarted GameStarted;

            public int AddPlayer(string name, string shortName, string leader, int colour)
            {
                byte[] message = BinaryHelper.Write(
                    new FieldType[] { FieldType.Integer, FieldType.Integer, FieldType.String, FieldType.String, FieldType.String, FieldType.Integer },
                    new object[] { this.ID, Command.PlayerJoin, name, shortName, leader, colour }
                );
                byte[] response = GameClient.Instance.SendMessage(message, true);

                using (BinaryStreamReader reader = new BinaryStreamReader(response))
                    return reader.ReadInt32();
            }
        }
    }
}

