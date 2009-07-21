using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Laan.ObjectSync
{
    public interface IClientEndPoint
    {
        Guid ID { get; }
        void SendMessages( IList<Message> messages );
        string User { get; set; }
        string MachineName { get; set; }
    }
}
