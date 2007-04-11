using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Test.Business.Nation
{
    class Fields
    {
    
        internal const int Name       = 1
        internal const int Leader     = 2
        internal const int Prestige   = 3
        internal const int Technology = 4
    }
    
    class Command
    {
        internal const int Name = 0;
    }
    
    public abstract class BaseNation: Laan.GameLibrary.Entity.BaseEntity
    {

        // ------------ Private ----------------------------------------------------------

        internal const string     _leader;
        internal const int        _prestige;
        internal const int        _technology;

        // ------------ Protected --------------------------------------------------------

        protected virtual void SetLeader(string value)
        {
            _leader = value;
        }
        
        protected virtual void SetPrestige(int value)
        {
            _prestige = value;
        }
        
        protected virtual void SetTechnology(int value)
        {
            _technology = value;
        }
        

        // ------------ Public ------------------------------------------------------------

        public BaseNation()
        {
        
        }

        public BaseNation(string name, string leader, int prestige, int technology) : base(name)
        {
            _leader = leader;
            _prestige = prestige;
            _technology = technology;
        }

        public string Leader 
        {
            get { return _leader; }
            set { SetLeader(value); }
        }

        public int Prestige 
        {
            get { return _prestige; }
            set { SetPrestige(value); }
        }

        public int Technology 
        {
            get { return _technology; }
            set { SetTechnology(value); }
        }

    }  
    
    public class NationList: EntityList
    {
        GameLibrary.Entity.Server _server = null;

        public NationList(bool isServer)
        {
            if(isServer)
                _server = new GameLibrary.Entity.Server(this);
        }

        public NationList()
        {
        }

        public new BaseNation this[int index]
        {
            get {
                return (BaseNation)base[index];
            }
        }
    }

    namespace Server
    {

        public class NationList: ServerEntityList
        {
            public new Nation this[int index]
            {
                get {
                    return (Nation)base[index];
                }
            }
        }
        
        public class Nation : BaseNation
        {

            public static implicit operator GameLibrary.Entity.Server(Nation nation)
            {
                // allows the class to be cast to an Entity.Server class
                return nation._server;
            }

            // --------------- Private -------------------------------------------------------

            GameLibrary.Entity.Server _server = null;

            private void ProcessCommand(BinaryStreamReader reader)
            {
                int command = reader.ReadInteger();
                switch (command)
                {
                  case Command.GrowFatter:
                    Weight += reader.ReadInteger();
                    break;
                  default:
                    break;
                }
            }

            // --------------- Protected Serialization ---------------------------------------

            internal override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
                writer.WriteString(this.Leader);
                writer.WriteInteger(this.Prestige);
                writer.WriteInteger(this.Technology);
            }

            protected override Communication GetComms()
            {
                return _server;
            }

            // --------------- Protected Setters ----------------------------------------------

            protected virtual void SetLeader(string value)
            {
                base.SetLeader(value);
                _server.Modify(this.ID, Fields.Leader, value);
            }

            protected virtual void SetPrestige(int value)
            {
                base.SetPrestige(value);
                _server.Modify(this.ID, Fields.Prestige, value);
            }

            protected virtual void SetTechnology(int value)
            {
                base.SetTechnology(value);
                _server.Modify(this.ID, Fields.Technology, value);
            }

            // --------------- Public --------------------------------------------------------

            public Nation(string name, string leader, int prestige, int technology) : base(name, leader, prestige, technology)
            {
                _server = new GameLibrary.Entity.Server(this);
                _server.OnProcessCommand += new OnProcessCommandEventHandler(ProcessCommand);
            }
        }
    }
    
    namespace Client
    {

        public class NationList: ClientEntityList
        {
            public new Nation this[int index]
            {
                get {
                    return (Nation)base[index];
                }
            }
        }

        public class Nation : BaseNation
        {
        
            // ------------ Private ---------------------------------------------------------

            GameLibrary.Entity.Client _client = null;

            protected override Communication GetComms()
            {
                return _client;
            }

            // ------------ Public ----------------------------------------------------------

            public Nation() : base()
            {
                Initialise();
            }

            public Nation(string name, string leader, int prestige, int technology) : base(name, leader, prestige, technology)
            {
                Initialise();
            }

            private void Initialise()
            {
                _client = new GameLibrary.Entity.Client();
                _client.OnModify += new GameLibrary.Entity.OnServerMessageEventHandler(OnModify);
            }

            internal override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
                base.SetLeader(reader.ReadString());
                base.SetPrestige(reader.ReadInteger());
                base.SetTechnology(reader.ReadInteger());
            }

            // when a change is caught (by the client), ensure the correct field is updated
            public void OnModify(byte field, BinaryStreamReader reader)
            {

                // move this to the call site of the delegate that calls this (OnUpdate) event
                _client.UpdateRecency(field);

                // update the appropriate field
                switch (field)
                {
                    case Fields.Leader:
                        Leader = reader.ReadString();
                        break;
                    case Fields.Prestige:
                        Prestige = reader.ReadInteger();
                        break;
                    case Fields.Technology:
                        Technology = reader.ReadInteger();
                        break;
                    default:
                        throw new Exception("Illegal field value");
                }
            }

        }
    }
}



