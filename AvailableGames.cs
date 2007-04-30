using System;
using System.Collections;

namespace Laan.GameLibrary
{

	public class AvailableGame
	{

		private string _host;
		private string _name;
		private int _port;

		public AvailableGame(string name, string host, int port)
		{
			_host = host;
			_name = name;
			_port = port;
		}

		public string Host
		{
			get { return _host; }
			set { _host = value; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public int Port
		{
			get { return _port; }
			set { _port = value; }
		}

	}

	public class AvailableGameList: ArrayList
	{

		public bool Add(AvailableGame game)
		{
			foreach(AvailableGame g in this)
				if (g.Name == game.Name)
				   return false;

			base.Add(game);
			return true;
		}

		public new AvailableGame this[int Index]
		{
			get { return (AvailableGame)base[Index]; }
			set { base[Index] = value; }
		}

	}
}
