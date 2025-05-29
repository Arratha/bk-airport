using System.Collections.Generic;
using Items.Base;
using Passenger;

namespace Check.MainCheck
{
    public class ProcessedPassenger
    {
        public PassengerController passenger { get; set; }
        public List<ItemIdentifier> bags { get; set; }
        public List<ItemIdentifier> items { get; set; }
    }
}