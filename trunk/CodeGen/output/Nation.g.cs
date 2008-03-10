using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Nation
{
    class Fields
    {
        internal const int ShortName    = 2;
        internal const int Leader       = 3;
        internal const int Prestige     = 4;
        internal const int Technology   = 5;
        internal const int OwnedRegions = 6;
        internal const int OwnedUnits   = 7;
    }
    
    namespace Server
    {
        using Regions = Laan.Risk.Region.Server;
        using Units = Laan.Risk.Unit.Server;

        public class NationList : ServerEntityList<Nation> { }

        public partial class Nation : BaseEntityServer
        {
            // --------------- Private -------------------------------------------------

            internal Int32              _ownedRegionsID;
            internal Int32              _ownedUnitsID;
            
            internal String             _shortName = "";
            internal String             _leader = "";
            internal Int32              _prestige;
            internal Int32              _technology;
            internal Regions.RegionList _ownedRegions;
            internal Units.UnitList     _ownedUnits;

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
                writer.WriteString(this.ShortName);
                writer.WriteString(this.Leader);
                writer.WriteInt32(this.Prestige);
                writer.WriteInt32(this.Technology);
            }

            protected override List<EntityProperty> GetEntityProperties()
            {
                return new List<EntityProperty>()
                {
                    new EntityProperty() { Entity = _ownedRegions, Field = Fields.OwnedRegions },
                    new EntityProperty() { Entity = _ownedUnits, Field = Fields.OwnedUnits },
                };
            }
        
            // --------------- Public -----------------------------------------------

            public Nation() : base()
            {
                OwnedRegions = new Region.Server.RegionList();
                OwnedUnits = new Unit.Server.UnitList();

                Initialise();
            }

            public static implicit operator GameLibrary.Entity.Server(Nation nation)
            {
                // allows the class to be cast to an Entity.Server class
                return nation.CommServer;
            }
            
            public String ShortName
            {
                get { return _shortName; }
                set {
                    _shortName = value;
                    
                    CommServer.Modify(this.ID, Fields.ShortName, value);
                }
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
    
        public class NationList : ClientEntityList<Nation> { }

        public partial class Nation : BaseEntityClient
        {

            // ------------ Private ---------------------------------------------------------

            internal String             _shortName = "";
            internal String             _leader = "";
            internal Int32              _prestige;
            internal Int32              _technology;
            internal Regions.RegionList _ownedRegions;
            internal Units.UnitList     _ownedUnits;

            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
                _shortName    = reader.ReadString();
                _leader       = reader.ReadString();
                _prestige     = reader.ReadInt32();
                _technology   = reader.ReadInt32();
            }

            // ------------ Public ----------------------------------------------------------

            public Nation() : base()
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
                    case Fields.ShortName:
                        _shortName = reader.ReadString();
                        break;
                    case Fields.Leader:
                        _leader = reader.ReadString();
                        break;
                    case Fields.Prestige:
                        _prestige = reader.ReadInt32();
                        break;
                    case Fields.Technology:
                        _technology = reader.ReadInt32();
                        break;
                    case Fields.OwnedRegions:
                        _ownedRegions = (Regions.RegionList)(ClientDataStore.Instance.Find(reader.ReadInt32()));
                        break;
                    case Fields.OwnedUnits:
                        _ownedUnits = (Units.UnitList)(ClientDataStore.Instance.Find(reader.ReadInt32()));
                        break;
                }
            }

            public String ShortName
            {
                get { return _shortName; }
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

