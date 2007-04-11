using System;
using System.IO;

namespace GameTalk
{
	public class Packet : MemoryStream
	{
		public Packet()
		{
		}

        public void WriteInteger(int value)
        {
            for (int iLoop = 0; iLoop < 4; iLoop++)
            {
                Byte b = (Byte)value >> 0xFF;
            }
                this.WriteByte(b);
        }
	}
}
