using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Player
{
    class Fields
    {
        internal const int Colour = 1;
        internal const int Ready  = 2;
        internal const int Nation = 3;
    }
    
    namespace Server
    {
        using Nations = Laan.Risk.Nation.Server;

        public class PlayerList : ServerEntityList
        {
            public new Server.Player this[int index]
            {
                get {
                    return (Server.Player)base[index];
                }
            }
        }

        public abstract class BasePlayer : BaseEntityServer
        {
            // --------------- Private -------------------------------------------------

            internal Int32 _nationID;
            
            internal System.Drawing.Color _colour;
            internal Boolean              _ready;
            internal Nations.Nation       _nation;

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
                writer.WriteColor(this.Colour);
                writer.WriteBoolean(this.Ready);
            }

            // --------------- Public -----------------------------------------------

            public BasePlayer() : base()
            {
                _nation = new Nation.Server.Nation();
            }

            public static implicit operator GameLibrary.Entity.Server(BasePlayer player)
            {
                // allows the class to be cast to an Entity.Server class
                return player.CommServer;
            }
            
            public System.Drawing.Color Colour
            {
                get { return _colour; }
                set {
                    _colour = value;
                    
                    CommServer.Modify(this.ID, Fields.Colour, value);
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

            public Nations.Nation Nation
            {
                get { return _nation; }
                set {
                    _nation = value;
                    _nationID = value.ID;
                    CommServer.Modify(this.ID, Fields.Nation, _nationID);
                }
            }

        }
    }

    namespace Client
    {
        using Nations = Laan.Risk.Nation.Client;
    
        public class PlayerList: ClientEntityList
        {
            public new Client.Player this[int index]
            {
                get {
                    return (Client.Player)base[index];
                }
            }
        }

        public abstract class BasePlayer : BaseEntityClient
        {

            // ------------ Private ---------------------------------------------------------

            internal System.Drawing.Color _colour;
            internal Boolean _ready;
            internal Nations.Nation _nation = null;

            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
                _colour = reader.ReadColor();
                _ready  = reader.ReadBoolean();
            }

            // ------------ Public ----------------------------------------------------------

            public BasePlayer() : base()
            {
                /*
                _nation = new Nations.Nation();
                */
            }

            // when a change is caught (by the client), ensure the correct field is updated
            protected override void OnModify(byte field, BinaryStreamReader reader)
            {

                // move this to the call site of the delegate that calls this (OnUpdate) event
                CommClient.UpdateRecency(field);

                // update the appropriate field
                switch (field)
                {
                    case Fields.Colour:
                        _colour = reader.ReadColor();
                        break;
                    case Fields.Ready:
                        _ready = reader.ReadBoolean();
                        break;
                    case Fields.Nation:
                        _nation = (Nations.Nation)(ClientDataStore.Instance.Find(reader.ReadInt32()));
                        break;
                    default:
                        throw new Exception("Illegal field value");
                }
            }

            public System.Drawing.Color Colour
            {
                get { return _colour; }
            }

            public Boolean Ready
            {
                get { return _ready; }
            }

            public Nations.Nation Nation
            {
                get { return _nation; }
            }

        }
    }
}

