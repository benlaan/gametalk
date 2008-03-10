using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Laan.GameLibrary.Data;
using log4net;

namespace Laan.GameLibrary.Entity
{
    public class ServerEntityList<T> : EntityList where T : BaseEntity
    {
        Server _server;

        public ServerEntityList()
        {
            _server = new GameLibrary.Entity.Server(this);
        }

        public override Communication Communication()
        {
            return _server;
        }

        public void Add(T entity)
        {
            base.Add(entity);
            _server.Modify(this.ID, Commands.Add, entity.ID);
        }

        public void Remove(T entity)
        {
            base.Remove(entity);
            _server.Modify(this.ID, Commands.Remove, entity.ID);
        }
    }
}
