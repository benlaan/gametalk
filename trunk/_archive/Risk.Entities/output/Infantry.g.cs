using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;

using Laan.GameLibrary.Data;
using Laan.GameLibrary.Entity;

namespace Laan.Risk.Unit.Infantry
{
    class Fields
    {
    }
    
    namespace Server
    {

        public partial class InfantryList : ServerEntityList<Infantry> { }

        public partial class Infantry : Unit.Server.Unit
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

            public Infantry () : base()
            {

                Initialise();
            }

            public static implicit operator GameLibrary.Entity.Server(Infantry infantry)
            {
                // allows the class to be cast to an Entity.Server class
                return infantry.CommServer;
            }
        }
    }

    namespace Client
    {

    
        public partial class InfantryList : ClientEntityList<Infantry> { }

        public partial class Infantry : Unit.Client.Unit
        {

            // ------------ Private ---------------------------------------------------------


            public override void Deserialise(BinaryStreamReader reader)
            {
                base.Deserialise(reader);

            }

            // ------------ Public ----------------------------------------------------------

            public Infantry () : base()
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
