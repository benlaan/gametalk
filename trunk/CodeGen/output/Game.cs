using System;
using System.IO;

using Laan.Library.Logging;
using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Game
{

    class Command
	{
		internal const int PlayerJoin  = 0;
//		internal const int PlayerLeave = 1;
//		internal const int Start       = 2;
//		internal const int Quit        = 3;
	}

	namespace Server
	{
		public class Game : BaseGame
        {

            // --------------- Protected --------------------------------------------

            protected override byte[] ProcessCommand(BinaryStreamReader reader) 
            {
				int command = reader.ReadInt32();
                switch (command)
                {
				  case Command.PlayerJoin:
					return PlayerJoin(reader);
				  default:
					return null;
				}
			}
            
            // --------------- Public -----------------------------------------------

            public Game() : base()
			{

            }

			public byte[] PlayerJoin(BinaryStreamReader reader)
			{
				Laan.Risk.Player.Server.Player p = new Laan.Risk.Player.Server.Player();

				p.Nation.Name = reader.ReadString();
				p.Nation.ShortName = reader.ReadString();
                p.Nation.Leader = reader.ReadString();
				p.Colour = reader.ReadInt32();
				Players.Add(p);

				Log.WriteLine("MessageReceived(PlayerJoin)");

				return BinaryHelper.Write(p.ID);
			}
		}
    }

    namespace Client
    {
        public class Game : BaseGame
        {
            // --------------- Public -----------------------------------------------

            public Game() : base()
            {

			}

			public int AddPlayer(string name, string shortName, string leader, int colour)
			{
				byte[] message = BinaryHelper.Write(
					this.ID, Command.PlayerJoin, name, shortName, leader, colour
                );
                byte[] response = GameClient.Instance.SendMessage(message, true);

				using (BinaryStreamReader reader = new BinaryStreamReader(response))
					return reader.ReadInt32();
			}
		}
	}
}

