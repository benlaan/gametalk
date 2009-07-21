using System;
using System.Configuration;

namespace Laan.GameLibrary
{
    public class Config
    {
        private static string ReadString(string section, string defaultValue)
        {
            string read = ConfigurationSettings.AppSettings[section];
            return read != null ? read : defaultValue;
        }

        private static int ReadInt(string section, int defaultValue)
        {
            string read = ConfigurationSettings.AppSettings[section];
            return read != null ? Int32.Parse(read) : defaultValue;
        }

        public static bool StreamUsingXML
        {
            get { return true; }
        }

        public static int ThreadWait
        {
            get { return ReadInt("ThreadWait", 500); }
        }

        public static int OutboundPort
        {
            get { return ReadInt("OutboundPort", 50000); }
        }

        public static int InboundPort
        {
            get { return ReadInt("InboundPort", 50001); }
        }

        public static int RendezvousPort
        {
            get { return ReadInt("RendezvousPort", 50002); }
        }

        public static int RendezvousFrequency
        {
            get { return ReadInt("RendezvousFrequency", 3000); }
        }

        public static string OutboundHost
        {
            get { return ReadString("OutboundHost", "localhost"); }
        }

        public static string UserName
        {
            get { return ReadString("UserName", Environment.UserName); }
        }
    }
}
