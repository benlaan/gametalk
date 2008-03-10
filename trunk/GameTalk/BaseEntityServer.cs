using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Laan.GameLibrary.Data;
using log4net;
using System.Collections.Generic;
using System.ComponentModel;

namespace Laan.GameLibrary.Entity
{
    public abstract class BaseEntityServer : BaseEntity
    {
        // --------------- Private -------------------------------------------------

        private Laan.GameLibrary.Entity.Server _server = null;

        // --------------- Protected -----------------------------------------------

        protected override void SetName(string value)
        {
            base.SetName(value);
            CommServer.Modify(this.ID, Fields.Name, value);
        }

        public override Communication Communication()
        {
            return _server;
        }

        protected abstract byte[] ProcessCommand(BinaryStreamReader reader);

        // --------------- Public --------------------------------------------------

        public BaseEntityServer() : base()
        {
            _server = new Laan.GameLibrary.Entity.Server(this);
            _server.OnProcessCommand += new OnProcessCommandEventHandler(ProcessCommand);
        }

        [Browsable(false)]
        public GameLibrary.Entity.Server CommServer
        {
            get { return _server; }
            set { _server = value; }
        }
    }
}
