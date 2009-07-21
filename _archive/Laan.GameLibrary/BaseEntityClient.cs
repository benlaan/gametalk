using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;

using Laan.GameLibrary.Data;
using log4net;
using System.ComponentModel;

namespace Laan.GameLibrary.Entity
{
    public abstract class BaseEntityClient : BaseEntity
    {

        public BaseEntityClient()
        {
            _client = new GameLibrary.Entity.Client();
            _client.OnModify += new OnServerMessageEventHandler(OnModify);
        }

        // --------------- Private -------------------------------------------------

        private GameLibrary.Entity.Client _client = null;

        private void OnModify(Byte field, BinaryStreamReader reader)
        {
            DoModify(field, reader);
        }

        protected virtual void DoModify(Byte field, BinaryStreamReader reader)
        {
            switch (field)
            {
                case Fields.Name:
                    Name = reader.ReadString();
                    break;
            }
        }

        protected override void SetName(string value)
        {
            base.SetName(value);

            //byte[] message = BinaryHelper.Write(
            //    new string[] { "int", "int", "string" },
            //    new object[] { this.ID, Command.ChangeName, value }
            //);
            //GameClient.Instance.SendMessage(message);
        }

        public override Communication Communication()
        {
            return _client;
        }

        // --------------- Public --------------------------------------------------

        [Browsable(false)]
        public GameLibrary.Entity.Client CommClient
        {
            get { return _client; }
            set { _client = value; }
        }
    }
}
