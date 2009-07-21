using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Laan.GameLibrary.Data;
using log4net;

namespace Laan.GameLibrary.Entity
{
    public class ClientEntityList<T> : EntityList where T : BaseEntity
    {
        Client _client;

        public ClientEntityList()
        {
            _client = new GameLibrary.Entity.Client();
            _client.OnModify += new OnServerMessageEventHandler(OnModify);
        }

        private void OnModify(Byte field, BinaryStreamReader reader)
        {
            DoModify(field, reader);
        }

        public override Communication Communication()
        {
            return _client;
        }

        protected virtual void DoModify(Byte field, BinaryStreamReader reader)
        {
            int id = reader.ReadInt32();
            BaseEntity e = ClientDataStore.Instance.Find(id);

            Debug.Assert(e != null, String.Format("ClientDataStore.Find({0}): not found!", id));
            switch (field)
            {
                case Commands.Add:
                    this.Add(e);
                    break;
                case Commands.Remove:
                    this.Remove(e);
                    break;
                default:
                    throw new Exception("ClientEntityList: Command must be Add or Remove");
            }
        }
    }
}
