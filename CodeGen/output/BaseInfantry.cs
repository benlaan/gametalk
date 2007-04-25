using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Unit.Infantry
{
    class Fields
    {
    }
    
    namespace Server
    {

        public class InfantryList : ServerEntityList
        {
            public new Server.Infantry this[int index]
            {
                get {
                    return (Server.Infantry)base[index];
                }
            }
        }

        public abstract class BaseInfantry : Unit.Server.Unit
        {
            // --------------- Private -------------------------------------------------

            

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
            }

            // --------------- Public -----------------------------------------------

            public BaseInfantry() : base()
            {
            }

            public static implicit operator GameLibrary.Entity.Server(BaseInfantry infantry)
            {
                // allows the class to be cast to an Entity.Server class
                return infantry.CommServer;
            }
            
        }
    }

    namespace Client
    {
    
        public class InfantryList: ClientEntityList
        {
            public new Client.Infantry this[int index]
            {
                get {
                    return (Client.Infantry)base[index];
                }
            }
        }

        public abstract class BaseInfantry : Unit.Client.Unit
        {

            // ------------ Private ---------------------------------------------------------


            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
            }

            // ------------ Public ----------------------------------------------------------

            public BaseInfantry() : base()
            {
                /*
                */
            }

            // when a change is caught (by the client), ensure the correct field is updated
            protected override void OnModify(byte field, BinaryStreamReader reader)
            {

                // move this to the call site of the delegate that calls this (OnUpdate) event
                CommClient.UpdateRecency(field);

                // update the appropriate field
                switch (field)
                {
                    default:
                        throw new Exception("Illegal field value");
                }
            }

        }
    }
}

