using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Region
{
    class Fields
    {
        internal const int Economy        = 2;
        internal const int EconomicWeight = 3;
        internal const int Oil            = 4;
        internal const int Arms           = 5;
        internal const int Defenders      = 6;
        internal const int Attackers      = 7;
    }
    
    namespace Server
    {
        using Units = Laan.Risk.Unit.Server;

        public partial class RegionList : ServerEntityList<Region> { }

        public partial class Region : BaseEntityServer
        {
            // --------------- Private -------------------------------------------------

            internal Int32          _defendersID;
            internal Int32          _attackersID;

            internal Int32          _economy;
            internal Int32          _economicWeight;
            internal Int32          _oil;
            internal Int32          _arms;
            internal Units.UnitList _defenders;
            internal Units.UnitList _attackers;

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
                writer.WriteInt32(this.Economy);
                writer.WriteInt32(this.EconomicWeight);
                writer.WriteInt32(this.Oil);
                writer.WriteInt32(this.Arms);
            }

            protected override List<EntityProperty> GetEntityProperties()
            {
                return new List<EntityProperty>()
                {
                    new EntityProperty() { Entity = _defenders, Field = Fields.Defenders },
                    new EntityProperty() { Entity = _attackers, Field = Fields.Attackers },
                };
            }
        
            // --------------- Public -----------------------------------------------

            public Region () : base()
            {
                Defenders = new Unit.Server.UnitList();
                Attackers = new Unit.Server.UnitList();

                Initialise();
            }

            public static implicit operator GameLibrary.Entity.Server(Region region)
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

            public Int32 EconomicWeight
            {
                get { return _economicWeight; }
                set {
                    _economicWeight = value;

                    CommServer.Modify(this.ID, Fields.EconomicWeight, value);
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

        using Units = Laan.Risk.Unit.Server;
    
        public partial class RegionList : ClientEntityList<Region> { }

        public partial class Region : BaseEntityClient
        {

            // ------------ Private ---------------------------------------------------------

            internal Int32          _economy;
            internal Int32          _economicWeight;
            internal Int32          _oil;
            internal Int32          _arms;
            internal Units.UnitList _defenders;
            internal Units.UnitList _attackers;

            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);

                _economy = reader.ReadInt32();
                _economicWeight = reader.ReadInt32();
                _oil = reader.ReadInt32();
                _arms = reader.ReadInt32();
            }

            // ------------ Public ----------------------------------------------------------

            public Region () : base()
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
                        Economy = reader.ReadInt32();
                        break;
                        
                    case Fields.EconomicWeight:
                        EconomicWeight = reader.ReadInt32();
                        break;
                        
                    case Fields.Oil:
                        Oil = reader.ReadInt32();
                        break;
                        
                    case Fields.Arms:
                        Arms = reader.ReadInt32();
                        break;
                        
                    case Fields.Defenders:
                        Defenders = (Units.UnitList)(ClientDataStore.Instance.Find(reader.ReadInt32()));
                        break;
                        
                    case Fields.Attackers:
                        Attackers = (Units.UnitList)(ClientDataStore.Instance.Find(reader.ReadInt32()));
                        break;
                        
//                  default:
//                      throw new Exception("Illegal field value");
                }
            }

            public Int32 Economy
            {
                get { return _economy; }
                set { _economy = value; }
            }

            public Int32 EconomicWeight
            {
                get { return _economicWeight; }
                set { _economicWeight = value; }
            }

            public Int32 Oil
            {
                get { return _oil; }
                set { _oil = value; }
            }

            public Int32 Arms
            {
                get { return _arms; }
                set { _arms = value; }
            }

            public Units.UnitList Defenders
            {
                get { return _defenders; }
                set { _defenders = value; }
            }

            public Units.UnitList Attackers
            {
                get { return _attackers; }
                set { _attackers = value; }
            }
        }
    }
}
