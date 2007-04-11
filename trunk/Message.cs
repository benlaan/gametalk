using System;

namespace Laan.GameLibrary
{
    internal class Message
    {

        internal static string ToString(byte[] data)
        {
            return data.Length.ToString() + ": " + Convert.ToBase64String(data);
        }
    }
}
