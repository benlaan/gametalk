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
//		internal const int PlayerReady = 1;
	}

	namespace Server
	{
		public class Game : BaseGame
        {

            // --------------- Protected --------------------------------------------

            protected override void ProcessCommand(BinaryStreamReader reader) 
            {
				int command = reader.ReadInt32();
                switch (command)
                {
				  case Command.PlayerJoin:
					PlayerJoin(reader);
					break;
//				  case Command.PlayerReady:
//					PlayerReady(reader);
//					break;
				  default:
					break;
				}
			}
            
            // --------------- Public -----------------------------------------------

            public Game() : base()
			{

            }

			public void PlayerJoin(BinaryStreamReader reader)
			{
				Laan.Risk.Player.Server.Player p = new Laan.Risk.Player.Server.Player();

				p.Nation.Name = reader.ReadString();
				string shortName = reader.ReadString();
				p.Colour = reader.ReadInt32();
				Players.Add(p);

				Log.WriteLine("MessageReceived(PlayerJoin)");
			}

//			public void PlayerReady(BinaryStreamReader reader)
//			{
//				Laan.Business.Risk.Player.Server.Player p = new Laan.Business.Risk.Player.Server.Player();
//				p.Nation.Name = reader.ReadBoolean();
//				string shortName = reader.ReadString();
//				p.Colour = System.Drawing.Color.FromArgb(reader.ReadInt32());
//				Players.Add(p);
//
//				Log.WriteLine("MessageReceived(PlayerReady)");
//			}
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

			public void AddPlayer(string name, string shortName, int colour)
			{
				using (BinaryStreamWriter writer = new BinaryStreamWriter(12))
				{
					writer.WriteInt32(this.ID);
					writer.WriteInt32(Command.PlayerJoin);
					writer.WriteString(name);
					writer.WriteString(shortName);
					writer.WriteInt32(colour);

					GameClient.Instance.SendMessage(writer.DataStream);

					Log.WriteLine("MessageSent(AddPlayer)");
				}
			}

//			public void PlayerReady(bool ready)
//			{
//				using (BinaryStreamWriter writer = new BinaryStreamWriter(12))
//				{
//					writer.WriteInt32(this.ID);
//					writer.WriteInt32(Command.PlayerReady);
//					writer.WriteInt32(c);
//					writer.WriteBoolean(ready);
//
//					GameClient.Instance.SendMessage(writer.DataStream);
//
//					Debug.WriteLine("MessageSent(PlayerReady)");
//				}
//			}
		}
	}
}

