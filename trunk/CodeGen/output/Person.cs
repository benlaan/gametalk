using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Test.Business.Person
{

    class Fields
    {
        internal const int Age    = 1;
        internal const int Name   = 2;
        internal const int Weight = 3;
    }

    class Command
    {
		internal const int GrowFatter = 0;
		internal const int GetOlder = 1;
	}

    public class BasePerson: Laan.GameLibrary.Entity.BaseEntity
    {

        // ------------ Private ----------------------------------------------------------

        int     _age  = 0;
        int     _weight = 0;

        // ------------ Protected --------------------------------------------------------

        protected virtual void SetAge(int value)
        {
            _age = value;
        }

        protected virtual void SetWeight(int value)
        {
            _weight = value;
        }

        protected override Communication GetComms()
        {
            return null;
        }

        // ------------ Public ------------------------------------------------------------

        public BasePerson()
        {
        
        }

        public BasePerson(string name, int age, int weight): base()
        {
            Name = name;
            _age = age;
            _weight = weight;
        }

        public int Age {

        get { return _age; }
        set { SetAge(value); }
        }

        public int Weight {

            get { return _weight; }
            set { SetWeight(value); }
        }
    }

    namespace Server
    {

        public class PersonList: ServerEntityList
        {
            public new Person this[int index]
            {
                get {
                    return (Person)base[index];
                }
            }
        }

        public class Person : BasePerson
        {

            public static implicit operator GameLibrary.Entity.Server(Person person)
            {
                // allows the class to be cast to an Entity.Server class
                return person._server;
            }

            // --------------- Private -------------------------------------------------------

            GameLibrary.Entity.Server _server = null;

            private void ProcessCommand(BinaryStreamReader reader)
            {
                int command = reader.ReadInt32();
                switch (command)
                {
				  case Command.GrowFatter:
					Weight += reader.ReadInt32();
					Debug.WriteLine("MessageReceived(GrowFatter)");
					break;
				  case Command.GetOlder:
					Age = reader.ReadInt32();
					Debug.WriteLine("MessageReceived(GetOlder)");
					break;
				  default:
                    break;
                }
            }

            // --------------- Protected Serialization ---------------------------------------

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
                writer.WriteInt32(this.Age);
                writer.WriteInt32(this.Weight);
            }

            protected override Communication GetComms()
            {
                return _server;
            }

            // --------------- Protected Setters ----------------------------------------------

            protected override void SetAge(int value)
            {
                base.SetAge(value);
                _server.Modify(this.ID, Fields.Age, value);
            }

            protected override void SetName(string value)
            {
                base.SetName(value);
                _server.Modify(this.ID, Fields.Name, value);
            }

            protected override void SetWeight(int value)
            {
                base.SetWeight(value);
                _server.Modify(this.ID, Fields.Weight, value);
            }

            // --------------- Public --------------------------------------------------------

            public Person(string name, int age, int weight)
			{
				_server = new GameLibrary.Entity.Server(this);
				_server.Size = 16;
                _server.OnProcessCommand += new OnProcessCommandEventHandler(ProcessCommand);

                Name = name;
                Age = age;
                Weight = weight;
            }
        }
    }

    namespace Client
    {

        public class PersonList: ClientEntityList
        {
            public new Person this[int index]
            {
                get {
                    return (Person)base[index];
                }
            }
        }

        public class Person : BasePerson
        {

            // ------------ Private ---------------------------------------------------------

            GameLibrary.Entity.Client _client = null;

            protected override Communication GetComms()
            {
                return _client;
            }

            // ------------ Public ----------------------------------------------------------

            public Person(): base("", 0, 0)
            {
                Initialise();
            }

            public Person(string name, int age, int weight): base(name, age, weight)
            {
                Initialise();
            }

            private void Initialise()
            {
                _client = new GameLibrary.Entity.Client();
                _client.OnModify += new GameLibrary.Entity.OnServerMessageEventHandler(OnModify);
            }

            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
                base.SetAge(reader.ReadInt32());
                base.SetWeight(reader.ReadInt32());
            }

            // when a change is caught (by the client), ensure the correct field is updated
            public void OnModify(byte field, BinaryStreamReader reader)
            {

                // move this to the call site of the delegate that calls this (OnUpdate) event
                _client.UpdateRecency(field);

                // update the appropriate field
                switch (field)
                {
                    case Fields.Age:
                        Age = reader.ReadInt32();
                        break;

                    case Fields.Name:
                        Name = reader.ReadString();
                        break;

                    case Fields.Weight:
                        Weight = reader.ReadInt32();
                        break;

                    default:
                        throw new Exception("Illegal field value");
                }
			}

			public void GetOlder()
			{
				using (BinaryStreamWriter writer = new BinaryStreamWriter(12))
				{
					writer.WriteInt32(this.ID);
					writer.WriteInt32(Command.GetOlder);
					writer.WriteInt32(Age + 1);

					GameClient.Instance.SendMessage(writer.DataStream);
					Debug.WriteLine("MessageSent(GetOlder)");
				}
			}

			public void GrowFatter()
            {
                Random r = new Random();
                int weight = r.Next(10);

				using (BinaryStreamWriter writer = new BinaryStreamWriter(12))
                {
                    writer.WriteInt32(this.ID);
                    writer.WriteInt32(Command.GrowFatter);
                    writer.WriteInt32(weight);

					GameClient.Instance.SendMessage(writer.DataStream);

					Debug.WriteLine("MessageSent(GrowFatter)");
                }
            }
        }
    }
}

