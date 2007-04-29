using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Game
{
    class Fields
    {
        internal const int Players = 2;
        internal const int Regions = 3;
    }
    
    namespace Server
    {
        using Players = Laan.Risk.Player.Server;
        using Regions = Laan.Risk.Region.Server;

        public class GameList : ServerEntityList
        {
            public new Server.Game this[int index]
            {
                get {
                    return (Server.Game)base[index];
                }
            }
        }

        public abstract class BaseGame : BaseEntityServer
        {
            // --------------- Private -------------------------------------------------

            internal Int32              _playersID;
            internal Int32              _regionsID;
            
            internal Players.PlayerList _players;
            internal Regions.RegionList _regions;

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
            }

            // --------------- Public -----------------------------------------------

            public BaseGame() : base()
            {
                Players = new Player.Server.PlayerList();
                Regions = new Region.Server.RegionList();
            }

            public static implicit operator GameLibrary.Entity.Server(BaseGame game)
            {
                // allows the class to be cast to an Entity.Server class
                return game.CommServer;
            }
            
            public Players.PlayerList Players
            {
                get { return _players; }
                set {
                    _players = value;
                    _playersID = value.ID;
                    CommServer.Modify(this.ID, Fields.Players, _playersID);
                }
            }

            public Regions.RegionList Regions
            {
                get { return _regions; }
                set {
                    _regions = value;
                    _regionsID = value.ID;
                    CommServer.Modify(this.ID, Fields.Regions, _regionsID);
                }
            }

        }
    }

    namespace Client
    {
        using Players = Laan.Risk.Player.Client;
        using Regions = Laan.Risk.Region.Client;
    
        public class GameList: ClientEntityList
        {
            public new Client.Game this[int index]
            {
                get {
                    return (Client.Game)base[index];
                }
            }
        }

        public abstract class BaseGame : BaseEntityClient
        {

            // ------------ Private ---------------------------------------------------------

            internal Players.PlayerList _players;
            internal Regions.RegionList _regions;

            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
            }

            // ------------ Public ----------------------------------------------------------

            public BaseGame() : base()
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
                    case Fields.Players:
                        _players = (Players.PlayerList)(ClientDataStore.Instance.Find(reader.ReadInt32()));
                        break;
                    case Fields.Regions:
                        _regions = (Regions.RegionList)(ClientDataStore.Instance.Find(reader.ReadInt32()));
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

        }
    }
}

