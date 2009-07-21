using System;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

using log4net;

namespace Laan.GameLibrary.Data
{
    public enum FieldType
    {
        String,
        Integer,
        Boolean,
        Date,
        Binary
    }

    public class BinaryStreamReader : System.IDisposable
    {
        // --------------- Private -------------------------------------------------------

        private MemoryStream _stream;
        private BinaryReader _reader;

        private ILog Log = log4net.LogManager.GetLogger(Assembly.GetEntryAssembly().ManifestModule.Name);

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
			Log.Debug("ReadByte: " + data.ToString());
			return data;
		}

		public int ReadInt32()
		{
			int data = _reader.ReadInt32();
			Log.Debug("ReadInt32: " + data.ToString());
			return data;
		}


		public string ReadString()
		{
			string data = _reader.ReadString();
			Log.Debug("ReadString: " + data.ToString());
			return data;
		}

		public bool ReadBoolean()
		{
			bool data = (_reader.ReadByte() == 1);
			Log.Debug("ReadBoolean: " + data.ToString());
			return data;
		}

		public Color ReadColor()
		{
			Color data = Color.FromArgb(_reader.ReadInt32());
			Log.Debug("ReadColor: " + data.ToString());
			return data;
		}

        public byte[] ReadBinary()
        {
            int count = _reader.ReadInt32();
            byte[] data = _reader.ReadBytes(count);
            Log.Debug(String.Format("ReadBinary({0}): {1}", count, data));
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
        private int             _length;

        private ILog Log = log4net.LogManager.GetLogger(Assembly.GetEntryAssembly().ManifestModule.Name);

        // --------------- Public --------------------------------------------------------

		public BinaryStreamWriter(int size)
		{
			_stream = new MemoryStream(size);
            _writer = new BinaryWriter(_stream);
            _length = 0;
        }

        public void WriteByte(byte value)
        {
			_writer.Write((byte)value);
            _length += sizeof(byte);
            Log.Debug("WriteByte: " + value);
		}

		public void WriteInt32(int value)
		{
			_writer.Write((int)value);
            _length += sizeof(int);
			Log.Debug("WriteInt32: " + value);
		}

		public void WriteString(string value)
		{
			_writer.Write((string)value);
            _length += value.Length + 1;
            Log.Debug("WriteString: " + value);
		}

		public void WriteDateTime(System.DateTime value)
		{
			_writer.Write(value.ToString());
            _length += value.ToString().Length;
            Log.Debug("WriteDateTime: " + value);
		}

		public void WriteBoolean(bool value)
		{
			_writer.Write((byte)(value ? 1 : 0));
            _length += sizeof(byte);
            Log.Debug("WriteBoolean: " + value);
		}

		public void WriteColor(Color value)
		{
			_writer.Write(value.ToArgb());
            _length += sizeof(int);
            Log.Debug("WriteColor: " + value);
		}

        public void WriteBinary(byte[] value)
        {
            _writer.Write(value.Length);
            _writer.Write(value);
            _length += value.Length + sizeof(int);
            Log.Debug(String.Format("WriteBinary({0}): {1}", value.Length, value));
        }

		public byte[] DataStream
        {
            get { return _stream.GetBuffer(); }
        }

        public void Dispose()
        {
            _stream.Close();
            _writer.Close();
        }

        public int Length
        {
            get { return _length; }
        }
    }

    public class BinaryHelper
    {
    
        public static object[] Read(byte[] message, FieldType[] types)
        {
            object[] result = new object[types.Length];

            int index = 0;
            using (BinaryStreamReader reader = new BinaryStreamReader(message))
            {
                foreach(FieldType type in types)
                {
                    switch(type)
                    {
                        case FieldType.String:
                            result[index] = reader.ReadString();
                            break;
                        case FieldType.Integer:
                            result[index] = reader.ReadInt32();
                            break;
                        case FieldType.Boolean:
                            result[index] = reader.ReadBoolean();
                            break;
//                        case FieldType.Date:
//                            result[index] = reader.Read();
//                            break;
                        case FieldType.Binary:
                            result[index] = reader.ReadBinary();
                            break;
                    }
                    index++;
                }
                return result;
            }
        }

        public static byte[] Write(FieldType[] types, object[] values)
        {
            int index = 0;
            Debug.Assert(types.Length == values.Length);

            using (BinaryStreamWriter writer = new BinaryStreamWriter(3))
            {
                foreach(FieldType type in types)
                {
                    switch(type)
                    {
                        case FieldType.String:
                            writer.WriteString((string)values[index]);
                            break;
                        case FieldType.Integer:
                            writer.WriteInt32((int)values[index]);
                            break;
                        case FieldType.Boolean:
                            writer.WriteBoolean((bool)values[index]);
                            break;
                        case FieldType.Date:
                            writer.WriteDateTime((System.DateTime)values[index]);
                            break;
                        case FieldType.Binary:
                            writer.WriteBinary((byte[])values[index]);
                            break;
                    }
                    index++;
                }
                return writer.DataStream;
            }
        }
    }

}