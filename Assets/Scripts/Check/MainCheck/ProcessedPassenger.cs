using Items.Base;
using Passenger;

namespace Check.MainCheck
{
    public class ProcessedPassenger
    {
        public PassengerController passenger { get; set; }
        public ItemIdentifier[] bags { get; set; }
        public ItemIdentifier[] items { get; set; }
    }
}