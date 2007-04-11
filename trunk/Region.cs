using System;

namespace Laan.Test.Business.Region
{

    using Laan.GameLibrary;

    class Fields
    {
        internal const int Economy  = 0;
        internal const int Name = 1;
    }

    class BaseRegion : Laan.GameLibrary.Entity.BaseEntity
    {

        // ------------ Private ----------------------------------------------------------

        int     _economy  = 0;

        // ------------ Protected --------------------------------------------------------

        protected virtual void SetEconomy(int value)
        {
            _economy = value;
        }

        // ------------ Public ------------------------------------------------------------

        public BaseRegion(string name, int economy) : base(name)
        {
            _economy = economy;
        }

        public int Economy {

            get { return _economy; }
            set { SetEconomy(value); }
        }
    }

    namespace Server
    {

        class Region : BaseRegion
        {
            public static implicit operator GameLibrary.Entity.Server(Region region)
            {
                // allows the class to be cast to an Entity.Server class
                return Region._server;
            }

            // --------------- Private -------------------------------------------------------

            GameLibrary.Entity.Server _server = null;

            // --------------- Protected -----------------------------------------------------

            protected override void SetEconomy(int value)
            {
                base.SetEconomy(value);
                _server.Modify(this.ID, Fields.Economy, value);
            }

            protected override void SetName(string value)
            {
                base.SetName(value);
                _server.Modify(this.ID, Fields.Name, value);
            }

            // --------------- Public --------------------------------------------------------

            public Region(string name, int economy) : base (name, economy)
            {
                _server = new GameLibrary.Entity.Server(this);
            }
        }
    }

    namespace Client
    {

        class Region : BaseRegion
        {

            public static implicit operator GameLibrary.Entity.Client(Region region)
            {
                // allows the class to be cast to an Entity.Client class
                return Region._client;
            }

            // ------------ Private ---------------------------------------------------------

            GameLibrary.Entity.Client _client = null;

            // ------------ Public ----------------------------------------------------------

            public Region(string name, int economy) : base (name, economy)
            {
                _client = new GameLibrary.Entity.Client();
                _client.OnUpdate += new Laan.GameLibrary.Entity.OnUpdateEventHandler(OnUpdate);
            }

            // when a change is caught (by the client), ensure the correct field is updated
            public void OnUpdate(object sender, int field, object value)
            {
                // move this to the call site of the delegate that calls this (OnUpdate) event
                _client.UpdateRecency(field, value);

                // update the appropriate field
                switch (field)
                {
                    case Laan.Test.Business.Region.Fields.Economy:
                        Economy = (int)value;
                        break;

                    case Laan.Test.Business.Region.Fields.Name:
                        Name = (string)value;
                        break;

                    default:
                        throw new Exception("Illegal field value");
                }
            }
        }
    }
}