using Passenger;

namespace Check.Queue
{
    public class DequeuedPassenger
    {
        public PassengerController passenger { get; set; }

        public DequeuedPassenger()
        {
            
        }

        public DequeuedPassenger(PassengerController passenger)
        {
            this.passenger = passenger;
        }
    }
}