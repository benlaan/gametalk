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
		internal const int PlayerJoin = 0;
//		internal const int PlayerLeave = 1;
//		internal const int Start = 2;
//		internal const int Quit = 3;
		internal const int PlayerReady = 1;
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

                return BinaryHelper.Write(new string[] {"int"}, new object[]{p.ID});
//                using (BinaryStreamWriter writer = new BinaryStreamWriter(3))
//                {
//                    writer.WriteInt32(p.ID);
//                    return writer.DataStream;
//                }
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
                    new string[] {"int", "int", "string", "string", "string", "int"},
                    new object[] {this.ID, Command.PlayerJoin, name, shortName, leader, colour}
                );
                byte[] response = GameClient.Instance.SendMessage(message, true);

                using (BinaryStreamReader reader = new BinaryStreamReader(response))
                {
                    return reader.ReadInt32();
                }
                
//				using (BinaryStreamWriter writer = new BinaryStreamWriter(12))
//				{
//					writer.WriteInt32(this.ID);
//					writer.WriteInt32(Command.PlayerJoin);
//					writer.WriteString(name);
//					writer.WriteString(shortName);
//					writer.WriteString(leader);
//					writer.WriteInt32(colour);
//
//					Log.WriteLine("MessageSent(AddPlayer)");
//
//				}
			}

		}
	}
}

