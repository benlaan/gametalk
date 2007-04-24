using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Unit.Armour
{
    class Fields
    {
    }
    
    namespace Server
    {

        public class ArmourList : ServerEntityList
        {
            public new Server.Armour this[int index]
            {
                get {
                    return (Server.Armour)base[index];
                }
            }
        }

        public abstract class BaseArmour : Unit.Server.Unit
        {
            // --------------- Private -------------------------------------------------

            

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
            }

            // --------------- Public -----------------------------------------------

            public BaseArmour() : base()
            {
            }

            public static implicit operator GameLibrary.Entity.Server(BaseArmour armour)
            {
                // allows the class to be cast to an Entity.Server class
                return armour.CommServer;
            }
            
        }
    }

    namespace Client
    {
    
        public class ArmourList: ClientEntityList
        {
            public new Client.Armour this[int index]
            {
                get {
                    return (Client.Armour)base[index];
                }
            }
        }

        public abstract class BaseArmour : Unit.Client.Unit
        {

            // ------------ Private ---------------------------------------------------------


            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
            }

            // ------------ Public ----------------------------------------------------------

            public BaseArmour() : base()
            {
            }

            // when a change is caught (by the client), ensure the correct field is updated
            public new void OnModify(byte field, BinaryStreamReader reader)
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

