using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;


namespace Laan.Business.Risk.Unit.Armour
{
    class Fields
    {
    }
    
    public interface IArmour : IBaseEntity
    {
    }
 
    public interface IArmourList : IBaseEntity
    {
        IArmour this [int index] { get; }
    }

    namespace Server
    {
        public class ArmourList : ServerEntityList, IArmourList
        {
            public new IArmour this[int index]
            {
                get {
                    return (IArmour)base[index];
                }
            }
        }

        public abstract class BaseArmour : Unit.Server.Unit, IArmour
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
        public class ArmourList: ClientEntityList, IArmourList
        {
            public new IArmour this[int index]
            {
                get {
                    return (IArmour)base[index];
                }
            }
        }

        public abstract class BaseArmour : Unit.Client.Unit, IArmour
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

