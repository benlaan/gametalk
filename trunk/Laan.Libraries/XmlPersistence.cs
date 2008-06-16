using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace Laan.Utilities.Xml
{
    public class XmlPersistence<T>
    {

        public static T LoadFromFile(string fileName)
        {
            T entity = default(T);

            using (Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                entity = LoadFromStream(stream);
                stream.Close();
            }

            return entity;
        }

        public static T LoadFromStream(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T entity = (T)(serializer.Deserialize(stream));
            return entity;
        }

        public static T LoadFromString(string xml)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(xml);

            using(MemoryStream ms = new MemoryStream())
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;

                return LoadFromStream(ms);
            }
        }

        public static void SaveToFile(T entity, string fileName)
        {
            using (Stream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                SaveToStream(entity, stream);
                stream.Close();
            }
        }

        public static void SaveToStream(T macro, Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, macro);
        }

        public static string SaveToString(T entity)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                SaveToStream(entity, ms);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms, Encoding.ASCII))
                {
                    string result = sr.ReadToEnd();
                    return result.Replace("\r", "");
                }
            }
        }
    }
}
