using System;
using System.IO;
using System.Diagnostics;

using Laan.GameLibrary;
using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;


namespace Laan.Business.Risk.Unit.Infantry
{
    class Fields
    {
    }
    
    public interface IInfantry : IBaseEntity
    {
    }
 
    public interface IInfantryList : IBaseEntity
    {
        IInfantry this [int index] { get; }
    }

    namespace Server
    {
        public class InfantryList : ServerEntityList, IInfantryList
        {
            public new IInfantry this[int index]
            {
                get {
                    return (IInfantry)base[index];
                }
            }
        }

        public abstract class BaseInfantry : Unit.Server.Unit, IInfantry
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
        public class InfantryList: ClientEntityList, IInfantryList
        {
            public new IInfantry this[int index]
            {
                get {
                    return (IInfantry)base[index];
                }
            }
        }

        public abstract class BaseInfantry : Unit.Client.Unit, IInfantry
        {

            // ------------ Private ---------------------------------------------------------


            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
            }

            // ------------ Public ----------------------------------------------------------

            public BaseInfantry() : base()
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

