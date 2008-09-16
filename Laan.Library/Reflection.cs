using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;

namespace Laan.Utilities
{
    public class Reflection
    {
        private const string cDLL = "*.DLL";
        public static IList<T> FindByAssembly<T>(string path)
        {
            List<T> result = new List<T>();

            foreach (string file in Directory.GetFiles(path, cDLL))
            {
                Assembly assembly = Assembly.LoadFile(file);
                foreach (Type type in assembly.GetTypes())
                    if (type.GetInterface(typeof(T).FullName) != null)
                        result.Add((T)Activator.CreateInstance(type));
            }
            return result;
        }

        public static IList<T> FindByAssemblyByLing<T>(string path) where T : class
        {
            var items =
                from assembly in Directory.GetFiles(path, cDLL)
                from type in Assembly.LoadFile(assembly).GetTypes()
                where type.GetInterface(typeof(T).FullName) != null
                select (T)Activator.CreateInstance(type);

            return items.ToList<T>();
        }
    }
}
