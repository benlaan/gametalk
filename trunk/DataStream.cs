using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Diagnostics;

using Laan.Library.Logging;

namespace Laan.GameLibrary.Data
{
	public class BinaryStreamReader : IDisposable
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
			bool data = (_reader.ReadByte() == 1);
			Log.WriteLine("ReadBoolean: " + data.ToString());
			return data;
		}

		public Color ReadColor()
		{
			Color data = Color.FromArgb(_reader.ReadInt32());
			Log.WriteLine("ReadColor: " + data.ToString());
			return data;
		}

		public DateTime ReadDateTime()
		{
			DateTime data = System.Convert.ToDateTime(_reader.ReadString());
			Log.WriteLine("ReadDateTime: " + data.ToString());
			return data;
		}

		public byte[] DataStream
        {
            get {
                return _stream.GetBuffer();
            }
        }
    }

    public class BinaryStreamWriter : IDisposable
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

		public void WriteDateTime(DateTime value)
		{
			_writer.Write(value.ToString());
			Log.WriteLine("WriteDateTime: " + value);
		}

		public void WriteBoolean(bool value)
		{
			_writer.Write((byte)(value ? 1 : 0));
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

	public enum BinaryType
	{
		String,
		Int,
		Boolean,
		Date
	}

	public struct TypeValue
	{

		public TypeValue(BinaryType type, object value)
		{
			Type = type;
			Value = value;
		}

		public BinaryType Type;
		public object Value;
	}

	public class BinaryHelper
    {
    
		public static object[] Read(byte[] message, params TypeCode[] types)
        {
            object[] result = new object[types.Length];

            int index = 0;
            using (BinaryStreamReader reader = new BinaryStreamReader(message))
			{
				foreach(TypeCode type in types)
				{
					switch(type)
					{
						case TypeCode.Int32:
							result[index] = reader.ReadInt32();
							break;
						case TypeCode.String:
							result[index] = reader.ReadString();
							break;
						case TypeCode.Boolean:
							result[index] = reader.ReadBoolean();
							break;
						case TypeCode.DateTime:
							result[index] = reader.ReadDateTime();
							break;
					}
					index++;
				}
				return result;
            }
        }

        public static byte[] Write(params object[] values)
        {
			int index = 0;

            using (BinaryStreamWriter writer = new BinaryStreamWriter(3))
			{
				foreach(object value in values)
				{
					switch(Type.GetTypeCode(value.GetType()))
					{
						case TypeCode.Int32:
							writer.WriteInt32((int)values[index]);
							break;
						case TypeCode.String:
							writer.WriteString((string)values[index]);
							break;
						case TypeCode.Boolean:
							writer.WriteBoolean((bool)values[index]);
							break;
						case TypeCode.DateTime:
							writer.WriteDateTime((DateTime)values[index]);
							break;
					}
                    index++;
                }
                return writer.DataStream;
            }
        }
    }

}