using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Unit
{
    class Fields
    {
        internal const int Size       = 2;
        internal const int Experience = 3;
        internal const int Location   = 4;
    }
    
    namespace Server
    {

        public class UnitList : ServerEntityList
        {
            public new Server.Unit this[int index]
            {
                get {
                    return (Server.Unit)base[index];
                }
            }
        }

        public abstract class BaseUnit : BaseEntityServer
        {
            // --------------- Private -------------------------------------------------

            
            internal Int32  _size;
            internal Int32  _experience;
            internal Int32  _location;

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
                writer.WriteInt32(this.Size);
                writer.WriteInt32(this.Experience);
                writer.WriteInt32(this.Location);
            }

            // --------------- Public -----------------------------------------------

            public BaseUnit() : base()
            {
            }

            public static implicit operator GameLibrary.Entity.Server(BaseUnit unit)
            {
                // allows the class to be cast to an Entity.Server class
                return unit.CommServer;
            }
            
            public Int32 Size
            {
                get { return _size; }
                set {
                    _size = value;
                    
                    CommServer.Modify(this.ID, Fields.Size, value);
                }
            }

            public Int32 Experience
            {
                get { return _experience; }
                set {
                    _experience = value;
                    
                    CommServer.Modify(this.ID, Fields.Experience, value);
                }
            }

            public Int32 Location
            {
                get { return _location; }
                set {
                    _location = value;
                    
                    CommServer.Modify(this.ID, Fields.Location, value);
                }
            }

        }
    }

    namespace Client
    {
    
        public class UnitList: ClientEntityList
        {
            public new Client.Unit this[int index]
            {
                get {
                    return (Client.Unit)base[index];
                }
            }
        }

        public abstract class BaseUnit : BaseEntityClient
        {

            // ------------ Private ---------------------------------------------------------

            internal Int32      _size;
            internal Int32      _experience;
            internal Int32      _location;

            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
                _size       = reader.ReadInt32();
                _experience = reader.ReadInt32();
                _location   = reader.ReadInt32();
            }

            // ------------ Public ----------------------------------------------------------

            public BaseUnit() : base()
            {
                /*
                */
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
                    case Fields.Size:
                        _size = reader.ReadInt32();
                        break;
                    case Fields.Experience:
                        _experience = reader.ReadInt32();
                        break;
                    case Fields.Location:
                        _location = reader.ReadInt32();
                        break;
//                    default:
//                        throw new Exception("Illegal field value");
                }
            }

            public Int32 Size
            {
                get { return _size; }
            }

            public Int32 Experience
            {
                get { return _experience; }
            }

            public Int32 Location
            {
                get { return _location; }
            }

        }
    }
}

