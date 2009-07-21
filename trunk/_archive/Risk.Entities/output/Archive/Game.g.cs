using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Game
{
    class Fields
    {
        internal const int Started = 2;
        internal const int Players = 3;
        internal const int Regions = 4;
        internal const int Borders = 5;
    }
    
    namespace Server
    {
        using Players = Laan.Risk.Player.Server;
        using Regions = Laan.Risk.Region.Server;
        using Borders = Laan.Risk.Border.Server;

        public class GameList : ServerEntityList<Game> { }

        public partial class Game : BaseEntityServer
        {
            // --------------- Private -------------------------------------------------

            internal Int32              _playersID;
            internal Int32              _regionsID;
            internal Int32              _bordersID;
            
            internal bool               _started;
            internal Players.PlayerList _players;
            internal Regions.RegionList _regions;
            internal Borders.BorderList _borders;

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
            }

            protected override List<EntityProperty> GetEntityProperties()
            {
                return new List<EntityProperty>()
                {
                    new EntityProperty() { Entity = _players, Field = Fields.Players },
                    new EntityProperty() { Entity = _regions, Field = Fields.Regions },
                    new EntityProperty() { Entity = _borders, Field = Fields.Borders },
                };
            }
        
            // --------------- Public -----------------------------------------------

            public Game() : base()
            {
                Players = new Player.Server.PlayerList();
                Regions = new Region.Server.RegionList();
                Borders = new Border.Server.BorderList();

                Initialise();
            }

            public static implicit operator GameLibrary.Entity.Server(Game game)
            {
                // allows the class to be cast to an Entity.Server class
                return game.CommServer;
            }

            public bool Started
            {
                get { return _started; }
                set
                {
                    _started = value;
                    CommServer.Modify(this.ID, Fields.Started, _started);
                }
            }

            public Players.PlayerList Players
            {
                get { return _players; }
                set
                {
                    _players = value;
                    _playersID = value.ID;
                    CommServer.Modify(this.ID, Fields.Players, _playersID);
                }
            }

            public Regions.RegionList Regions
            {
                get { return _regions; }
                set
                {
                    _regions = value;
                    _regionsID = value.ID;
                    CommServer.Modify(this.ID, Fields.Regions, _regionsID);
                }
            }

            public Borders.BorderList Borders
            {
                get { return _borders; }
                set
                {
                    _borders = value;
                    _bordersID = value.ID;
                    CommServer.Modify(this.ID, Fields.Borders, _bordersID);
                }
            }

        }
    }

    namespace Client
    {
        using Players = Laan.Risk.Player.Client;
        using Regions = Laan.Risk.Region.Client;
        using Borders = Laan.Risk.Border.Client;
    
        public class GameList : ClientEntityList<Game> { }

        public partial class Game : BaseEntityClient
        {

            // ------------ Private ---------------------------------------------------------

            internal Players.PlayerList _players;
            internal Regions.RegionList _regions;
            internal Borders.BorderList _borders;

            // ------------ Protected -------------------------------------------------------

            protected bool _started;

            // ------------ Public ----------------------------------------------------------

            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
            }

            public Game() : base()
            {
            }

            // when a change is caught (by the client), ensure the correct field is updated
            protected override void DoModify(byte field, BinaryStreamReader reader)
            {

                base.DoModify(field, reader);
                
                // move this to the call site of the delegate that calls this (OnUpdate) event
                CommClient.UpdateRecency(field);

                // update the appropriate field
                switch (field)
                {
                    case Fields.Started:
                        Started = reader.ReadBoolean();
                        break;
                    case Fields.Players:
                        _players = (Players.PlayerList)(ClientDataStore.Instance.Find(reader.ReadInt32()));
                        break;
                    case Fields.Regions:
                        _regions = (Regions.RegionList)(ClientDataStore.Instance.Find(reader.ReadInt32()));
                        break;
                    case Fields.Borders:
                        _borders = (Borders.BorderList)(ClientDataStore.Instance.Find(reader.ReadInt32()));
                        break;
                }
            }

            public Players.PlayerList Players
            {
                get { return _players; }
            }

            public Regions.RegionList Regions
            {
                get { return _regions; }
            }

            public Borders.BorderList Borders
            {
                get { return _borders; }
            }

        }
    }
}

