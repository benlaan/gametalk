using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Player
{
    class Fields
    {
        internal const int Nation = 2;
        internal const int Ready  = 3;
        internal const int Colour = 4;
    }
    
    namespace Server
    {
        using Nations = Laan.Risk.Nation.Server;

        public class PlayerList : ServerEntityList<Player> { }

        public partial class Player : BaseEntityServer
        {
            // --------------- Private -------------------------------------------------

            internal Int32          _nationID;
            
            internal Nations.Nation _nation;
            internal Boolean        _ready;
            internal Int32          _colour;

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
                writer.WriteBoolean(this.Ready);
                writer.WriteInt32(this.Colour);
            }

            protected override List<EntityProperty> GetEntityProperties()
            {
                return new List<EntityProperty>()
                {
                    new EntityProperty() { Entity = _nation, Field = Fields.Nation },
                };
            }
        
            // --------------- Public -----------------------------------------------

            public Player() : base()
            {
                Nation = new Nation.Server.Nation();

                Initialise();
            }

            public static implicit operator GameLibrary.Entity.Server(Player player)
            {
                // allows the class to be cast to an Entity.Server class
                return player.CommServer;
            }
            
            public Nations.Nation Nation
            {
                get { return _nation; }
                set {
                    _nation = value;
                    _nationID = value.ID;
                    CommServer.Modify(this.ID, Fields.Nation, _nationID);
                }
            }

            public Boolean Ready
            {
                get { return _ready; }
                set {
                    _ready = value;
                    
                    CommServer.Modify(this.ID, Fields.Ready, value);
                }
            }

            public Int32 Colour
            {
                get { return _colour; }
                set {
                    _colour = value;
                    
                    CommServer.Modify(this.ID, Fields.Colour, value);
                }
            }

        }
    }

    namespace Client
    {
        using Nations = Laan.Risk.Nation.Client;
    
        public class PlayerList : ClientEntityList<Player> { }

        public partial class Player : BaseEntityClient
        {

            // ------------ Private ---------------------------------------------------------

            internal Nations.Nation _nation;
            internal Boolean        _ready;
            internal Int32          _colour;

            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
                _ready  = reader.ReadBoolean();
                _colour = reader.ReadInt32();
            }

            // ------------ Public ----------------------------------------------------------

            public Player() : base()
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
                    case Fields.Nation:
                        _nation = (Nations.Nation)(ClientDataStore.Instance.Find(reader.ReadInt32()));
                        break;
                    case Fields.Ready:
                        _ready = reader.ReadBoolean();
                        break;
                    case Fields.Colour:
                        _colour = reader.ReadInt32();
                        break;
                }
            }

            public Nations.Nation Nation
            {
                get { return _nation; }
            }

            public Boolean Ready
            {
                get { return _ready; }
            }

            public Int32 Colour
            {
                get { return _colour; }
            }

        }
    }
}

