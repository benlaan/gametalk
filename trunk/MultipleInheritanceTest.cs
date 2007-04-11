namespace Laan.Test.Business.Person
{
	
	internal const Age = 0;
	internal const Name = 1;
	
	class Base : Laan.GameLibrary.Entity
	{
		
		int     _age  = 0;
		string  _name = "";
		
		protected virtual void SetAge(int value) {
			_age = value
		}
		protected virtual void SetName(string value) {
			_name = value;	
		}
		
		public Person(string name, int age)
		{
			_age = age;
			_name = name;
		}
		
		public int Age {
		
			get {
				return _age;
			}	
			set {
				SetAge(value);
			)
		}
		public int Name {
		
			get {
				return _name;
			}	
			set {
				SetName(value);
			)
		}
	}
}

namespace Laan.Test.Business.Person.Server
{

	class Person : Laan.Test.Business.Person.Base
	{
		GameLibrary.Entity.Server _server = null;
		
		public Person(GameServer gameServer, string name, int age) : this (name, age)
		{
			_server = new GameLibrary.Entity.Server();
		}

		protected override void SetAge(int value) : base(value)
		{
			_server.Changed(Laan.Test.Business.Shared.Age, value);
		}
		
		protected override void SetName(string value) : base(value)
		{
			_server.Changed(Laan.Test.Business.Shared.Name, value);
		}
	}
}

namespace Laan.Test.Business.Client
{

	class Person : Laan.Test.Business.Person.Base
	{

		GameLibrary.Entity.Client _client = null;
		
		public Person(GameClient gameClient)
		{
			_client = new GameLibrary.Entity.Client();
			_client.OnUpdate += new Laan.GameLibrary.Client.UpdateEvent(OnUpdate);
		}
		
		public void OnUpdate(object sender, int field, object value)
		{
			UpdateRecency(field, value);
			switch (field)
			{
				case Laan.Test.Business.Person.Age: 
					_age = (int)value);
					break; 
				case Laan.Test.Business.Person.Name: 
					_name = (string(value);
					break;
				default:
					throw new Exception("Illegal field value");					
			}
		}
		
	}
}
