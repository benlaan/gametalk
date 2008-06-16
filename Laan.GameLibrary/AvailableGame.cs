using System;
using System.Collections;
using System.Collections.Generic;

namespace Laan.GameLibrary
{

	public class AvailableGame
	{
        public string Host { get; set; }
        public string Name { get; set; }
        public int    Port { get; set; }
	}

	public class AvailableGameList : List<AvailableGame>
	{

		public new bool Add(AvailableGame game)
		{
            if (base.Exists(g => g.Name == game.Name))
                return false;

			base.Add(game);
			return true;
		}
	}
}
