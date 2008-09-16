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
        internal const int FromRegion  = 2;
        internal const int ToRegion    = 3;
        internal const int Connections = 4;
        internal const int Economy     = 5;
        internal const int Oil         = 6;
        internal const int Arms        = 7;
    }
    
    namespace Server
    {
        using Regions = Laan.Risk.Region.Server;

        public partial class BorderList : ServerEntityList<Border> { }

        public partial class Border : BaseEntityServer
        {
            // --------------- Private -------------------------------------------------

            internal Int32          _fromRegionID;
            internal Int32          _toRegionID;

            internal Regions.Region _fromRegion;
            internal Regions.Region _toRegion;
            internal Connection[]   _connections;
            internal Int32          _economy;
            internal Int32          _oil;
            internal Int32          _arms;

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
            }

            protected override List<EntityProperty> GetEntityProperties()
            {
                return new List<EntityProperty>()
                {
                };
            }
        
            // --------------- Public -----------------------------------------------

            public Border () : base()
            {
                FromRegion = new Region.Server.Region();
                ToRegion = new Region.Server.Region();

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

            //public Connection[] Connections
            //{
            //    get { return _connections; }
            //    set {
            //        _connections = value;

            //        CommServer.Modify(this.ID, Fields.Connections, value);
            //    }
            //}

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

        using Regions = Laan.Risk.Region.Client;
    
        public partial class BorderList : ClientEntityList<Border> { }

        public partial class Border : BaseEntityClient
        {

            // ------------ Private ---------------------------------------------------------

            internal Regions.Region _fromRegion;
            internal Regions.Region _toRegion;
            //internal Connection[]   _connections;
            internal Int32          _economy;
            internal Int32          _oil;
            internal Int32          _arms;

            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);

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
                        
                    case Fields.Connections:
//                        Connections = reader.ReadConnection[]();
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


            //public Connection[] Connections
            //{
            //    get { return _connections; }
            //    set { _connections = value; }
            //}


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
