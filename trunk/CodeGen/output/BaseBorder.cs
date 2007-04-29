using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Border
{
    class Fields
    {
        internal const int Economy = 2;
        internal const int Oil     = 3;
        internal const int Arms    = 4;
    }
    
    namespace Server
    {

        public class BorderList : ServerEntityList
        {
            public new Server.Border this[int index]
            {
                get {
                    return (Server.Border)base[index];
                }
            }
        }

        public abstract class BaseBorder : BaseEntityServer
        {
            // --------------- Private -------------------------------------------------

            
            internal Int32  _economy;
            internal Int32  _oil;
            internal Int32  _arms;

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
                writer.WriteInt32(this.Economy);
                writer.WriteInt32(this.Oil);
                writer.WriteInt32(this.Arms);
            }

            // --------------- Public -----------------------------------------------

            public BaseBorder() : base()
            {
            }

            public static implicit operator GameLibrary.Entity.Server(BaseBorder border)
            {
                // allows the class to be cast to an Entity.Server class
                return border.CommServer;
            }
            
            public Int32 Economy
            {
                get { return _economy; }
                set {
                    _economy = value;
                    
                    CommServer.Modify(this.ID, Fields.Economy, value);
                }
            }

            public Int32 Oil
            {
                get { return _oil; }
                set {
                    _oil = value;
                    
                    CommServer.Modify(this.ID, Fields.Oil, value);
                }
            }

            public Int32 Arms
            {
                get { return _arms; }
                set {
                    _arms = value;
                    
                    CommServer.Modify(this.ID, Fields.Arms, value);
                }
            }

        }
    }

    namespace Client
    {
    
        public class BorderList: ClientEntityList
        {
            public new Client.Border this[int index]
            {
                get {
                    return (Client.Border)base[index];
                }
            }
        }

        public abstract class BaseBorder : BaseEntityClient
        {

            // ------------ Private ---------------------------------------------------------

            internal Int32  _economy;
            internal Int32  _oil;
            internal Int32  _arms;

            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
                _economy = reader.ReadInt32();
                _oil     = reader.ReadInt32();
                _arms    = reader.ReadInt32();
            }

            // ------------ Public ----------------------------------------------------------

            public BaseBorder() : base()
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
                    case Fields.Economy:
                        _economy = reader.ReadInt32();
                        break;
                    case Fields.Oil:
                        _oil = reader.ReadInt32();
                        break;
                    case Fields.Arms:
                        _arms = reader.ReadInt32();
                        break;
                }
            }

            public Int32 Economy
            {
                get { return _economy; }
            }

            public Int32 Oil
            {
                get { return _oil; }
            }

            public Int32 Arms
            {
                get { return _arms; }
            }

        }
    }
}

