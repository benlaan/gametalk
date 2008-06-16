using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Border
{
    class Fields
    {
        internal const int FromRegion      = 2;
        internal const int ToRegion        = 3;
        internal const int ConnectionTypes = 4;
        internal const int Economy         = 5;
        internal const int Oil             = 6;
        internal const int Arms            = 7;
    }
    
    namespace Server
    {
        using Regions = Laan.Risk.Region.Server;

        public partial class BorderList : ServerEntityList<Border> { }

        public partial class Border : BaseEntityServer
        {
            // --------------- Private -------------------------------------------------

            internal Int32                              _fromRegionID;
            internal Int32                              _toRegionID;
            internal Int32                              _connectionTypesID;

            internal Regions.Region                     _fromRegion;
            internal Regions.Region                     _toRegion;
            internal ConnectionTypes.ConnectionTypeList _connectionTypes;
            internal Int32                              _economy;
            internal Int32                              _oil;
            internal Int32                              _arms;

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
                writer.WriteRegion(this.FromRegion);
                writer.WriteRegion(this.ToRegion);
                writer.WriteInt32(this.Economy);
                writer.WriteInt32(this.Oil);
                writer.WriteInt32(this.Arms);
            }

            protected override List<EntityProperty> GetEntityProperties()
            {
                return new List<EntityProperty>()
                {
                    new EntityProperty() { Entity = _connectionTypes, Field = Fields.ConnectionTypes },
                };
            }
        
            // --------------- Public -----------------------------------------------

            public Border () : base()
            {
                FromRegion = new Region.Server.Region();
                ToRegion = new Region.Server.Region();
                ConnectionTypes = new ConnectionType.Server.ConnectionTypeList();

                Initialise();
            }

            public static implicit operator GameLibrary.Entity.Server(Border border)
            {
                // allows the class to be cast to an Entity.Server class
                return border.CommServer;
            }

            public Regions.Region FromRegion
            {
                get { return _fromRegion; }
                set {
                    _fromRegion = value;
                    _fromRegionID = value.ID;

                    CommServer.Modify(this.ID, Fields.FromRegion, _fromRegionID);
                }
            }

            public Regions.Region ToRegion
            {
                get { return _toRegion; }
                set {
                    _toRegion = value;
                    _toRegionID = value.ID;

                    CommServer.Modify(this.ID, Fields.ToRegion, _toRegionID);
                }
            }

            public ConnectionTypes.ConnectionTypeList ConnectionTypes
            {
                get { return _connectionTypes; }
                set {
                    _connectionTypes = value;
                    _connectionTypesID = value.ID;

                    CommServer.Modify(this.ID, Fields.ConnectionTypes, _connectionTypesID);
                }
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

        using Regions = Laan.Risk.Region.Server;
    
        public partial class BorderList : ClientEntityList<Border> { }

        public partial class Border : BaseEntityClient
        {

            // ------------ Private ---------------------------------------------------------

            internal Regions.Region                     _fromRegion;
            internal Regions.Region                     _toRegion;
            internal ConnectionTypes.ConnectionTypeList _connectionTypes;
            internal Int32                              _economy;
            internal Int32                              _oil;
            internal Int32                              _arms;

            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);

                _fromRegion = reader.ReadRegion();
                _toRegion = reader.ReadRegion();
                _economy = reader.ReadInt32();
                _oil = reader.ReadInt32();
                _arms = reader.ReadInt32();
            }

            // ------------ Public ----------------------------------------------------------

            public Border () : base()
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
                    case Fields.FromRegion:
                        FromRegion = (Regions.Region)(ClientDataStore.Instance.Find(reader.ReadInt32()));
                        break;
                        
                    case Fields.ToRegion:
                        ToRegion = (Regions.Region)(ClientDataStore.Instance.Find(reader.ReadInt32()));
                        break;
                        
                    case Fields.ConnectionTypes:
                        ConnectionTypes = (ConnectionTypes.ConnectionTypeList)(ClientDataStore.Instance.Find(reader.ReadInt32()));
                        break;
                        
                    case Fields.Economy:
                        Economy = reader.ReadInt32();
                        break;
                        
                    case Fields.Oil:
                        Oil = reader.ReadInt32();
                        break;
                        
                    case Fields.Arms:
                        Arms = reader.ReadInt32();
                        break;
                        
//                  default:
//                      throw new Exception("Illegal field value");
                }
            }

            public Regions.Region FromRegion
            {
                get { return _fromRegion; }
                set { _fromRegion = value; }
            }

            public Regions.Region ToRegion
            {
                get { return _toRegion; }
                set { _toRegion = value; }
            }

            public ConnectionTypes.ConnectionTypeList ConnectionTypes
            {
                get { return _connectionTypes; }
                set { _connectionTypes = value; }
            }

            public Int32 Economy
            {
                get { return _economy; }
                set { _economy = value; }
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
        }
    }
}
