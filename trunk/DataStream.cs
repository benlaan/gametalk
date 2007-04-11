using System.IO;
using System.Drawing;

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
            return _reader.ReadByte();
        }

        public int ReadInt32()
        {
            return _reader.ReadInt32();
        }

        public string ReadString()
        {
            return _reader.ReadString();
        }

        public bool ReadBoolean()
        {
            return _reader.ReadBoolean();
        }

        public Color ReadColor()
        {
            return Color.FromArgb(_reader.ReadInt32());
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
        }

        public void WriteInt32(int value)
        {
            _writer.Write((int)value);
        }

        public void WriteString(string value)
        {
            _writer.Write((string)value);
        }

        public void WriteDateTime(System.DateTime value)
        {
            _writer.Write(value.ToString());
        }

        public void WriteBoolean(bool value)
        {
            _writer.Write((bool)value);
        }

        public void WriteColor(Color value)
        {
            _writer.Write(value.ToArgb());
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