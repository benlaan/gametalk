using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

using Laan.Business.Risk.Region;
using Laan.Business.Risk.Unit;

namespace Laan.Business.Risk.Nation
{
    class Fields
    {
        internal const int Leader       = 1;
        internal const int Prestige     = 2;
        internal const int Technology   = 3;
        internal const int OwnedRegions = 4;
        internal const int OwnedUnits   = 5;
    }
    
    public interface INation : IBaseEntity
    {
        String      Leader { get; }
        Int32       Prestige { get; }
        Int32       Technology { get; }
        IRegionList OwnedRegions { get; }
        IUnitList   OwnedUnits { get; }
    }
 
    public interface INationList : IBaseEntity
    {
        INation this [int index] { get; }
    }

    namespace Server
    {
        public class NationList : ServerEntityList, INationList
        {
            public new INation this[int index]
            {
                get {
                    return (INation)base[index];
                }
            }
        }

        public abstract class BaseNation : BaseEntityServer, INation
        {
            // --------------- Private -------------------------------------------------

            internal Int32 _ownedRegionsID;
            internal Int32 _ownedUnitsID;
            
            internal String     _leader;
            internal Int32      _prestige;
            internal Int32      _technology;
            internal IRegionList _ownedRegions;
            internal IUnitList  _ownedUnits;

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

            public IRegionList OwnedRegions
            {
                get { return _ownedRegions; }
                set {
                    _ownedRegions = value;
                    _ownedRegionsID = value.ID;
                    CommServer.Modify(this.ID, Fields.OwnedRegions, _ownedRegionsID);
                }
            }

            public IUnitList OwnedUnits
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
        public class NationList: ClientEntityList, INationList
        {
            public new INation this[int index]
            {
                get {
                    return (INation)base[index];
                }
            }
        }

        public abstract class BaseNation : BaseEntityClient, INation
        {

            // ------------ Private ---------------------------------------------------------

            internal String       _leader;
            internal Int32        _prestige;
            internal Int32        _technology;
            internal IRegionList  _ownedRegions = null;
            internal IUnitList    _ownedUnits = null;

            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
                _leader     = reader.ReadString();
                _prestige   = reader.ReadInt32();
                _technology = reader.ReadInt32();
            }

            // ------------ Public ----------------------------------------------------------

            public BaseNation() : base()
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

            public IRegionList OwnedRegions
            {
                get { return _ownedRegions; }
            }

            public IUnitList OwnedUnits
            {
                get { return _ownedUnits; }
            }

        }
    }
}

