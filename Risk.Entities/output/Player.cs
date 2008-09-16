using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

using log4net;

namespace Laan.Risk.Player
{

    class Command
    {
        internal const int Ready = 0;
    }

    namespace Server
    {
        public partial class Player
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

            public override void Initialise()
            {
                base.Initialise();
            }
        }
    }

    namespace Client
    {
        public partial class Player
        {
            // --------------- Public -----------------------------------------------

            public override void Initialise()
            {
                base.Initialise();
            }

            public void SetReady(bool value)
            {
                using (BinaryStreamWriter writer = new BinaryStreamWriter(3))
                {
                    writer.WriteInt32(this.ID);
                    writer.WriteInt32(Command.Ready);
                    writer.WriteBoolean(value);

                    GameClient.Instance.SendMessage(writer.DataStream);

                    Log.Debug(String.Format("MessageSent(Player.Ready = {0})", value));
                }
            }
        }
    }
}

