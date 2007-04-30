using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;
using Laan.Library.Logging;

namespace Laan.Risk.Player
{

    class Command
	{
		internal const int Ready = 0;
	}

    namespace Server
    {
        public class Player : BasePlayer
        {

            // --------------- Protected --------------------------------------------

			protected override byte[] ProcessCommand(BinaryStreamReader reader)
            {
				int command = reader.ReadInt32();
                switch (command)
				{
					case Command.Ready:
						Ready = reader.ReadBoolean();
						return null;
					default:
						return null;
				}
			}

			// --------------- Public -----------------------------------------------

			public Player() : base()
			{

			}

		}
    }

    namespace Client
    {
        public class Player : BasePlayer
        {
            // --------------- Public -----------------------------------------------

            public Player() : base()
            {

            }

            public new Boolean Ready
            {
                get { return base.Ready; }
                set {
                    using (BinaryStreamWriter writer = new BinaryStreamWriter(3))
                    {
                        writer.WriteInt32(this.ID);
                        writer.WriteInt32(Command.Ready);
                        writer.WriteBoolean(value);

                        GameClient.Instance.SendMessage(writer.DataStream);

                        Log.WriteLine("MessageSent(Player.Ready = {0})", value);
                    }
                }
            }

		}
    }
}

