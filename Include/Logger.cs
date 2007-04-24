#define DEBUG

using System;
using System.IO;
using System.Diagnostics;

namespace Laan.Library.Logging
{

	public class Log
	{
		static Log()
		{
			_log = new MemoryStream();
			_writer = new BinaryWriter(_log);
			_capacity = 0;
			_lineCount = 0;
			_fileName = Process.GetCurrentProcess().MainModule.FileName.Replace(".exe", ".log");
		}

		const string conditionalText = "LOGGING";

		private static int          _capacity;
		private static string       _fileName;
		private static int          _lineCount;
		private static MemoryStream _log;
		private static BinaryWriter _writer;

		static public void SaveToFile()
		{
			using (FileStream fs = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Write))
			{
				_log.WriteTo(fs);
				_log.Flush();
			}
			_lineCount = 0;
		}

		[Conditional(conditionalText)]
		static public void WriteLine(string message)
		{
			InternalWriteLine(message);
		}

		[Conditional(conditionalText)]
		static public void WriteLine(string message, params object[] args)
		{
			InternalWriteLine(String.Format(message, args));
		}

		private static void InternalWriteLine(string message)
		{
			Debug.WriteLine(message);
			_writer.Write(message + Environment.NewLine);

			_lineCount++;
			if(_lineCount > _capacity)
				SaveToFile();
		}

		public static int Capacity
		{
			get { return _capacity; }
			set { _capacity = value; }
		}

		public static string FileName
		{
			get { return _fileName; }
			set {
				if(_fileName != "")
					_fileName = value;
			}
		}
	}
}
