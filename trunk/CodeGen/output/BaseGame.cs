using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

using Laan.Business.Risk.Player;
using Laan.Business.Risk.Region;

namespace Laan.Business.Risk.Game
{
    class Fields
    {
        internal const int Players = 1;
        internal const int Regions = 2;
    }
    
    public interface IGame : IBaseEntity
    {
        IPlayerList Players { get; }
        IRegionList Regions { get; }
    }
 
    public interface IGameList : IBaseEntity
    {
        IGame this [int index] { get; }
    }

    namespace Server
    {
        public class GameList : ServerEntityList, IGameList
        {
            public new IGame this[int index]
            {
                get {
                    return (IGame)base[index];
                }
            }
        }

        public abstract class BaseGame : BaseEntityServer, IGame
        {
            // --------------- Private -------------------------------------------------

            internal Int32 _playersID;
            internal Int32 _regionsID;
            
            internal IPlayerList _players;
            internal IRegionList _regions;

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
            }

            // --------------- Public -----------------------------------------------

            public BaseGame() : base()
            {
                _players = new Player.Server.PlayerList();
                _regions = new Region.Server.RegionList();
            }

            public static implicit operator GameLibrary.Entity.Server(BaseGame game)
            {
                // allows the class to be cast to an Entity.Server class
                return game.CommServer;
            }
            
            public IPlayerList Players
            {
                get { return _players; }
                set {
                    _players = value;
                    _playersID = value.ID;
                    CommServer.Modify(this.ID, Fields.Players, _playersID);
                }
            }

            public IRegionList Regions
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
        public class GameList: ClientEntityList, IGameList
        {
            public new IGame this[int index]
            {
                get {
                    return (IGame)base[index];
                }
            }
        }

        public abstract class BaseGame : BaseEntityClient, IGame
        {

            // ------------ Private ---------------------------------------------------------

            internal IPlayerList _players = null;
            internal IRegionList _regions = null;

            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
            }

            // ------------ Public ----------------------------------------------------------

            public BaseGame() : base()
            {
            }

            // when a change is caught (by the client), ensure the correct field is updated
            public void OnModify(byte field, BinaryStreamReader reader)
            {

                // move this to the call site of the delegate that calls this (OnUpdate) event
                CommClient.UpdateRecency(field);

                // update the appropriate field
                switch (field)
                {
                    default:
                        throw new Exception("Illegal field value");
                }
            }

            public IPlayerList Players
            {
                get { return _players; }
            }

            public IRegionList Regions
            {
                get { return _regions; }
            }

        }
    }
}

