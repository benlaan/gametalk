using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Laan.ObjectSync
{
    public class ClientEndPoint : IClientEndPoint
    {
        /// <summary>
        /// Initializes a new instance of the ClientEndPoint class.
        /// </summary>
        public ClientEndPoint( string machineName, string user )
        {
            MachineName = machineName;
            User = user;
        }

        public Guid ID { get; set; }
        public string MachineName { get; set; }
        public string User { get; set; }

        public void SendMessages( IList<Message> messages )
        {
        }
    }
}
