using System;
using System.Text;

namespace Laan.GameLibrary
{
    internal class Message
    {

        internal static string ToString(byte[] data)
        {
            if (data == null)
                return "";

            StringBuilder resultBuilder = new StringBuilder(data.Length.ToString() + ": ");
            for (int index = 0; index < data.Length; index++)
                resultBuilder.Append((index > 0 ? "-" : "") + data[index].ToString());

            return resultBuilder.ToString();
            //return data.Length.ToString() + ": " + Convert.ToBase64String(data);
        }
    }
}
