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

        public partial class PlayerList : ServerEntityList<Player> { }

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
            }

            protected override List<EntityProperty> GetEntityProperties()
            {
                return new List<EntityProperty>()
                {
                };
            }
        
            // --------------- Public -----------------------------------------------

            public Player () : base()
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
    
        public partial class PlayerList : ClientEntityList<Player> { }

        public partial class Player : BaseEntityClient
        {

            // ------------ Private ---------------------------------------------------------

            internal Nations.Nation _nation;
            internal Boolean        _ready;
            internal Int32          _colour;

            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);

            }

            // ------------ Public ----------------------------------------------------------

            public Player () : base()
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
                        Nation = (Nations.Nation)(ClientDataStore.Instance.Find(reader.ReadInt32()));
                        break;
                        
                    case Fields.Ready:
                        Ready = reader.ReadBoolean();
                        break;
                        
                    case Fields.Colour:
                        Colour = reader.ReadInt32();
                        break;
                        
//                  default:
//                      throw new Exception("Illegal field value");
                }
            }

            public Nations.Nation Nation
            {
                get { return _nation; }
                set { _nation = value; }
            }


            public Boolean Ready
            {
                get { return _ready; }
                set { _ready = value; }
            }


            public Int32 Colour
            {
                get { return _colour; }
                set { _colour = value; }
            }

        }
    }
}
