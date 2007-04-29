using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Region
{
    class Fields
    {
        internal const int Economy   = 2;
        internal const int Oil       = 3;
        internal const int Arms      = 4;
        internal const int Defenders = 5;
        internal const int Attackers = 6;
    }
    
    namespace Server
    {
        using Units = Laan.Risk.Unit.Server;

        public class RegionList : ServerEntityList
        {
            public new Server.Region this[int index]
            {
                get {
                    return (Server.Region)base[index];
                }
            }
        }

        public abstract class BaseRegion : BaseEntityServer
        {
            // --------------- Private -------------------------------------------------

            internal Int32          _defendersID;
            internal Int32          _attackersID;
            
            internal Int32          _economy;
            internal Int32          _oil;
            internal Int32          _arms;
            internal Units.UnitList _defenders;
            internal Units.UnitList _attackers;

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
                writer.WriteInt32(this.Economy);
                writer.WriteInt32(this.Oil);
                writer.WriteInt32(this.Arms);
            }

            // --------------- Public -----------------------------------------------

            public BaseRegion() : base()
            {
                Defenders = new Unit.Server.UnitList();
                Attackers = new Unit.Server.UnitList();
            }

            public static implicit operator GameLibrary.Entity.Server(BaseRegion region)
            {
                // allows the class to be cast to an Entity.Server class
                return region.CommServer;
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

            public Units.UnitList Defenders
            {
                get { return _defenders; }
                set {
                    _defenders = value;
                    _defendersID = value.ID;
                    CommServer.Modify(this.ID, Fields.Defenders, _defendersID);
                }
            }

            public Units.UnitList Attackers
            {
                get { return _attackers; }
                set {
                    _attackers = value;
                    _attackersID = value.ID;
                    CommServer.Modify(this.ID, Fields.Attackers, _attackersID);
                }
            }

        }
    }

    namespace Client
    {
        using Units = Laan.Risk.Unit.Client;
    
        public class RegionList: ClientEntityList
        {
            public new Client.Region this[int index]
            {
                get {
                    return (Client.Region)base[index];
                }
            }
        }

        public abstract class BaseRegion : BaseEntityClient
        {

            // ------------ Private ---------------------------------------------------------

            internal Int32          _economy;
            internal Int32          _oil;
            internal Int32          _arms;
            internal Units.UnitList _defenders;
            internal Units.UnitList _attackers;

            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
                _economy   = reader.ReadInt32();
                _oil       = reader.ReadInt32();
                _arms      = reader.ReadInt32();
            }

            // ------------ Public ----------------------------------------------------------

            public BaseRegion() : base()
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
                    case Fields.Defenders:
                        _defenders = (Units.UnitList)(ClientDataStore.Instance.Find(reader.ReadInt32()));
                        break;
                    case Fields.Attackers:
                        _attackers = (Units.UnitList)(ClientDataStore.Instance.Find(reader.ReadInt32()));
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

            public Units.UnitList Defenders
            {
                get { return _defenders; }
            }

            public Units.UnitList Attackers
            {
                get { return _attackers; }
            }

        }
    }
}

