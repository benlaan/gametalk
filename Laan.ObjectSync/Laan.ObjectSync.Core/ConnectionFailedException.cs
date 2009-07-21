using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Laan.ObjectSync
{
    public class ConnectionFailedException : Exception
    {
        public ConnectionFailedException( Exception ex ) : base( "Connection Failed", ex )
        {

        }
    }
}
