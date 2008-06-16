using System;
using System.IO;
using System.Text;

namespace Laan.Utilities.IO
{
    public static class ConvertStream
    {
        public static string ToString(Stream stream)
        {
            return ToString(stream, true);
        }

        public static string ToString(Stream stream, bool resetPosition)
        {
            if (resetPosition)
                stream.Position = 0;

            using (StreamReader sr = new StreamReader(stream, Encoding.ASCII))
            {
                return sr.ReadToEnd();
            }
        }
    }

    public class Directory
    {
        // Copy directory structure recursively
        public static void Copy(string source, string destination)
        {
            destination = File.EnsureTrailingBackslash(destination);
            ForceDirectory(destination);

            // Retrieve list of files and directories
            String[] entries = System.IO.Directory.GetFileSystemEntries(source);

            foreach (string entry in entries)
            {
                // Copy sub directories
                if (System.IO.Directory.Exists(entry))
                    Copy(entry, destination + Path.GetFileName(entry));
                else
                    // Copy files in directory
                    System.IO.File.Copy(entry, destination + Path.GetFileName(entry), true);
            }
        }

        public static void ForceDirectory(string filePath)
        {
            string path = System.IO.Path.GetDirectoryName(filePath);

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
        }
    }

    static public class File
	{
        static public string EnsureTrailingBackslash(string filePath)
        {
            string seperator = new string(new char[] { System.IO.Path.DirectorySeparatorChar } );

            if (filePath != null && !filePath.EndsWith(seperator))
                filePath += System.IO.Path.DirectorySeparatorChar;

            return filePath;
        }
    }
}
