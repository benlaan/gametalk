using System;
using System.Collections.Generic;

namespace Laan.ObjectSync.Client
{
    public interface IMessageClient
    {
        List<Message> GetMessages();

        void Connect();
    }
}
