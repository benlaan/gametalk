using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laan.ObjectSync
{
    public interface ISyncServer
    {
        int Port { get; }

        Entity Connect( IClientEndPoint client );

        List<Message> Receive();
    }
}
