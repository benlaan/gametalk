using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;


namespace Laan.Business.Risk.Border
{
    class Fields
    {
        internal const int Economy = 1;
        internal const int Oil     = 2;
        internal const int Arms    = 3;
    }
    
    public interface IBorder : IBaseEntity
    {
        Int32   Economy { get; }
        Int32   Oil { get; }
        Int32   Arms { get; }
    }
 
    public interface IBorderList : IBaseEntity
    {
        IBorder this [int index] { get; }
    }

    namespace Server
    {
        public class BorderList : ServerEntityList, IBorderList
        {
            public new IBorder this[int index]
            {
                get {
                    return (IBorder)base[index];
                }
            }
        }

        public abstract class BaseBorder : BaseEntityServer, IBorder
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
        public class BorderList: ClientEntityList, IBorderList
        {
            public new IBorder this[int index]
            {
                get {
                    return (IBorder)base[index];
                }
            }
        }

        public abstract class BaseBorder : BaseEntityClient, IBorder
        {

            // ------------ Private ---------------------------------------------------------

            internal Int32   _economy;
            internal Int32   _oil;
            internal Int32   _arms;

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
            public void OnModify(byte field, BinaryStreamReader reader)
            {

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
                    default:
                        throw new Exception("Illegal field value");
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

