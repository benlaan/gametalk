using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Nation
{
    class Fields
    {
        internal const int Leader       = 1;
        internal const int Prestige     = 2;
        internal const int Technology   = 3;
        internal const int OwnedRegions = 4;
        internal const int OwnedUnits   = 5;
    }
    
    namespace Server
    {
        using Regions = Laan.Risk.Region.Server;
        using Units = Laan.Risk.Unit.Server;

        public class NationList : ServerEntityList
        {
            public new Server.Nation this[int index]
            {
                get {
                    return (Server.Nation)base[index];
                }
            }
        }

        public abstract class BaseNation : BaseEntityServer
        {
            // --------------- Private -------------------------------------------------

            internal Int32 _ownedRegionsID;
            internal Int32 _ownedUnitsID;
            
            internal String     _leader;
            internal Int32      _prestige;
            internal Int32      _technology;
            internal Regions.RegionList _ownedRegions;
            internal Units.UnitList _ownedUnits;

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
                writer.WriteString(this.Leader);
                writer.WriteInt32(this.Prestige);
                writer.WriteInt32(this.Technology);
            }

            // --------------- Public -----------------------------------------------

            public BaseNation() : base()
            {
                _ownedRegions = new Region.Server.RegionList();
                _ownedUnits = new Unit.Server.UnitList();
            }

            public static implicit operator GameLibrary.Entity.Server(BaseNation nation)
            {
                // allows the class to be cast to an Entity.Server class
                return nation.CommServer;
            }
            
            public String Leader
            {
                get { return _leader; }
                set {
                    _leader = value;
                    
                    CommServer.Modify(this.ID, Fields.Leader, value);
                }
            }

            public Int32 Prestige
            {
                get { return _prestige; }
                set {
                    _prestige = value;
                    
                    CommServer.Modify(this.ID, Fields.Prestige, value);
                }
            }

            public Int32 Technology
            {
                get { return _technology; }
                set {
                    _technology = value;
                    
                    CommServer.Modify(this.ID, Fields.Technology, value);
                }
            }

            public Regions.RegionList OwnedRegions
            {
                get { return _ownedRegions; }
                set {
                    _ownedRegions = value;
                    _ownedRegionsID = value.ID;
                    CommServer.Modify(this.ID, Fields.OwnedRegions, _ownedRegionsID);
                }
            }

            public Units.UnitList OwnedUnits
            {
                get { return _ownedUnits; }
                set {
                    _ownedUnits = value;
                    _ownedUnitsID = value.ID;
                    CommServer.Modify(this.ID, Fields.OwnedUnits, _ownedUnitsID);
                }
            }

        }
    }

    namespace Client
    {
        using Regions = Laan.Risk.Region.Client;
        using Units = Laan.Risk.Unit.Client;
    
        public class NationList: ClientEntityList
        {
            public new Client.Nation this[int index]
            {
                get {
                    return (Client.Nation)base[index];
                }
            }
        }

        public abstract class BaseNation : BaseEntityClient
        {

            // ------------ Private ---------------------------------------------------------

            internal String       _leader;
            internal Int32        _prestige;
            internal Int32        _technology;
            internal Regions.RegionList _ownedRegions = null;
            internal Units.UnitList _ownedUnits = null;

            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
                _leader       = reader.ReadString();
                _prestige     = reader.ReadInt32();
                _technology   = reader.ReadInt32();
            }

            // ------------ Public ----------------------------------------------------------

            public BaseNation() : base()
            {
                _ownedRegions = new Regions.RegionList();
                _ownedUnits = new Units.UnitList();
            }

            // when a change is caught (by the client), ensure the correct field is updated
            public void OnModify(byte field, BinaryStreamReader reader)
            {

                // move this to the call site of the delegate that calls this (OnUpdate) event
                CommClient.UpdateRecency(field);

                // update the appropriate field
                switch (field)
                {
                    case Fields.Leader:
                        _leader = reader.ReadString();
                        break;
                    case Fields.Prestige:
                        _prestige = reader.ReadInt32();
                        break;
                    case Fields.Technology:
                        _technology = reader.ReadInt32();
                        break;
                    default:
                        throw new Exception("Illegal field value");
                }
            }

            public String Leader
            {
                get { return _leader; }
            }

            public Int32 Prestige
            {
                get { return _prestige; }
            }

            public Int32 Technology
            {
                get { return _technology; }
            }

            public Regions.RegionList OwnedRegions
            {
                get { return _ownedRegions; }
            }

            public Units.UnitList OwnedUnits
            {
                get { return _ownedUnits; }
            }

        }
    }
}

