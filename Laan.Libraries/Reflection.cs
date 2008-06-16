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
        public static IList<T> FindByAssembly<T>(string path)
        {
            List<T> result = new List<T>();

            foreach (string file in Directory.GetFiles(path, "*.DLL"))
            {
                Assembly assembly = Assembly.LoadFile(file);
                foreach (Type instance in assembly.GetTypes())
                {
                    foreach (Type item in instance.GetInterfaces())
                    {
                        if (item == typeof(T))
                            result.Add((T)Activator.CreateInstance(instance));
                    }
                }
            }
            return result;
        }

        public static IList<T> FindByAssemblyByLing<T>(string path) where T : class
        {
            var items =
                from assembly in Directory.GetFiles(path, "*.DLL")
                from instance in Assembly.LoadFile(assembly).GetTypes()
                from item in instance.GetInterfaces()
                where item == typeof(T)
                select (T)Activator.CreateInstance(instance);

            return items.ToList<T>();
        }
    }
}
