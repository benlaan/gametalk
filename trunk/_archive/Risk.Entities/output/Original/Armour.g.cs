using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Unit.Armour
{
    class Fields
    {
    }
    
    namespace Server
    {

        public class ArmourList : ServerEntityList<Armour> { }

        public partial class Armour : Unit.Server.Unit
        {
            // --------------- Private -------------------------------------------------

            

            public override void Serialise(BinaryStreamWriter writer)
            {
                base.Serialise(writer);
            }

            protected override List<EntityProperty> GetEntityProperties()
            {
                return new List<EntityProperty>()
                {
                };
            }
        
            // --------------- Public -----------------------------------------------

            public Armour() : base()
            {

                Initialise();
            }

            public static implicit operator GameLibrary.Entity.Server(Armour armour)
            {
                // allows the class to be cast to an Entity.Server class
                return armour.CommServer;
            }
            
        }
    }

    namespace Client
    {
    
        public class ArmourList : ClientEntityList<Armour> { }

        public partial class Armour : Unit.Client.Unit
        {

            // ------------ Private ---------------------------------------------------------


            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);
            }

            // ------------ Public ----------------------------------------------------------

            public Armour() : base()
            {
            }

            // when a change is caught (by the client), ensure the correct field is updated
            protected override void DoModify(byte field, BinaryStreamReader reader)
            {

                base.DoModify(field, reader);
                
                // move this to the call site of the delegate that calls this (OnUpdate) event
                CommClient.UpdateRecency(field);

            }

        }
    }
}

