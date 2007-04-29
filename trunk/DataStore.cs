using System;

using Laan.GameLibrary.Data;
using Laan.Library.Logging;

namespace Laan.GameLibrary.Entity
{

        public class ClientDataStore : EntityList
        {

            static ClientDataStore _entities = new ClientDataStore();

            private bool _isRoot = true;

            private ClientDataStore() { }
            private string _assemblyName;

            public static ClientDataStore Instance
            {
                get { return _entities; }
            }

            public event OnRootEntityEventHandler   OnRootEntityEvent;
            public event OnNewEntityEventHandler    OnNewEntityEvent;
            public event OnModifyEntityEventHandler OnModifyEntityEvent;

            public byte[] ProcessMessage(byte[] data)
            {
                try
                {
                    BinaryStreamReader reader = new BinaryStreamReader(data);

                    Byte code = reader.ReadByte();
                    switch (code)
                    {
                        case MessageCode.Create:
                            return ProcessInsert(reader);
                        case MessageCode.Delete:
                            return ProcessDelete(reader);
                        case MessageCode.Update:
                            return ProcessModify(reader);
                        default:
                            throw new Exception("Invalid MesssageCode");
                    }
                }
                catch (System.Exception e)
                {
                    Log.WriteLine("Error: " + e.ToString());
                    throw;
                }
            }

            internal byte[] ProcessInsert(BinaryStreamReader reader)
            {
                // create an instance of the given type, and attach
                // it to the entities list
                string typeName = reader.ReadString().Replace(".Server.", ".Client.");

                Log.WriteLine("ClientDataStore.ProcessInsert: {0}", typeName);


                BaseEntity e = (Activator.CreateInstanceFrom(AssemblyName + ".DLL", typeName).Unwrap()) as BaseEntity;
                if (e == null)
                    throw new Exception("Invalid Type: " + typeName);

                // this is only required for the 'root' object within the data store
                if(_isRoot && OnRootEntityEvent != null)
                    OnRootEntityEvent(e);
                _isRoot = false;

                if (OnNewEntityEvent != null)
                    OnNewEntityEvent(e);

                _entities.Add(e);

                if (e != null)
                    e.Deserialise(reader);

                Log.WriteLine("Created BaseEntity {0}:{1}", e.ID, e.Name);

                // return new ID to client
                using (BinaryStreamWriter writer = new BinaryStreamWriter(1))
                {
                    writer.WriteInt32(e.ID);
                    return writer.DataStream;
                }
            }

            internal byte[] ProcessDelete(BinaryStreamReader reader)
            {
                Log.WriteLine("ClientDataStore.ProcessDelete");

                int ID = reader.ReadInt32();
                BaseEntity e = _entities.Find(ID);

                // remove the entity from the main list
                _entities.Remove(e);

                return null;
            }

            internal byte[] ProcessModify(BinaryStreamReader reader)
            {
                Log.WriteLine("ClientDataStore.ProcessModify");

                int ID = reader.ReadInt32();

                BaseEntity e = _entities.Find(ID);
    //            ((Client)e).Modify(reader);
                (e.Communication() as Client).Modify(reader);

                if(OnModifyEntityEvent != null)
                    OnModifyEntityEvent(e);

                return null;
            }

            public string AssemblyName
            {
                get { return _assemblyName; }
                set { _assemblyName = value; }
            }

        }

        public class ServerDataStore: EntityList
        {
            static ServerDataStore _entities = new ServerDataStore();

            public static ServerDataStore Instance
            {
                get { return _entities; }
            }

        }
}
