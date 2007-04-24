using System.IO;
using System.Drawing;

using Laan.Library.Logging;

namespace Laan.GameLibrary.Data
{
    public class BinaryStreamReader : System.IDisposable
    {
        // --------------- Private -------------------------------------------------------

        private MemoryStream _stream;
        private BinaryReader _reader;

        // --------------- Public --------------------------------------------------------

        public BinaryStreamReader(byte[] data)
        {
            _stream = new MemoryStream();
            _reader = new BinaryReader(_stream);

            _stream.Write(data, 0, data.Length);
            _stream.Position = 0;
        }

        public void Dispose()
        {
            _stream.Close();
            _reader.Close();
        }

        public byte ReadByte()
        {
			byte data = _reader.ReadByte();
			Log.WriteLine("ReadByte: " + data.ToString());
			return data;
		}

		public int ReadInt32()
		{
			int data = _reader.ReadInt32();
			Log.WriteLine("ReadInt32: " + data.ToString());
			return data;
		}

		public string ReadString()
		{
			string data = _reader.ReadString();
			Log.WriteLine("ReadString: " + data.ToString());
			return data;
		}

		public bool ReadBoolean()
		{
			bool data = _reader.ReadBoolean();
			Log.WriteLine("ReadBoolean: " + data.ToString());
			return data;
		}

		public Color ReadColor()
		{
			Color data = Color.FromArgb(_reader.ReadInt32());
			Log.WriteLine("ReadColor: " + data.ToString());
			return data;
		}

		public byte[] DataStream
        {
            get {
                return _stream.GetBuffer();
            }
        }
    }

    public class BinaryStreamWriter : System.IDisposable
    {
        // --------------- Private -------------------------------------------------------

        private MemoryStream    _stream;
        private BinaryWriter    _writer;

        // --------------- Public --------------------------------------------------------

		public BinaryStreamWriter(int size)
		{
			_stream = new MemoryStream(size);
            _writer = new BinaryWriter(_stream);
        }

        public void WriteByte(byte value)
        {
			_writer.Write((byte)value);
			Log.WriteLine("WriteByte: " + value);
		}

		public void WriteInt32(int value)
		{
			_writer.Write((int)value);
			Log.WriteLine("WriteInt32: " + value);
		}

		public void WriteString(string value)
		{
			_writer.Write((string)value);
			Log.WriteLine("WriteString: " + value);
		}

		public void WriteDateTime(System.DateTime value)
		{
			_writer.Write(value.ToString());
			Log.WriteLine("WriteDateTime: " + value);
		}

		public void WriteBoolean(bool value)
		{
			_writer.Write((bool)value);
			Log.WriteLine("WriteBoolean: " + value);
		}

		public void WriteColor(Color value)
		{
			_writer.Write(value.ToArgb());
			Log.WriteLine("WriteColor: " + value);
		}

		public byte[] DataStream
        {
            get {
                return _stream.GetBuffer();
            }
        }

        public void Dispose()
        {
            _stream.Close();
            _writer.Close();
        }
    }
}