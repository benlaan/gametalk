using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

namespace Laan.ObjectSync
{
    public interface ISyncClient
    {
        bool Active { get; set; }
        void Connect();
        void AddEntity( IEntity entity );
        void RemoveEntity( int item );
        IEntity FindEntity( int id );
        IEntity Root { get; set; }
    }
}
