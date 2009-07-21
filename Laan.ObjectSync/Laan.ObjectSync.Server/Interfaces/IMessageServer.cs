using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Laan.ObjectSync.Server
{
    [ServiceContract]
    public interface IMessageServer
    {
        [OperationContract]
        List<TypeDefinition> RegisterClient( Guid clientID );

        [OperationContract]
        List<byte[]> GetMessages( Guid clientID );
    }
}
