using System.Collections.Generic;
using Commands.Contexts;
using Passenger;
using UnityEngine;

namespace Check.Queue
{
    //Creates queue of passengers and assigns random baggage preset to them
    public class PassengersQueue : MonoBehaviour
    {
        [SerializeField] private int initialCount;
        [SerializeField] private Vector3 pointOffset;
        [SerializeField] private Transform startingPoint;

        [Space, SerializeField] private PassengerCreator creator;
        
        public int count => _passengers.Count;
        private Queue<PassengerController> _passengers = new();
        
        public PassengerController Dequeue()
        {
            var result = _passengers.Dequeue();
            result.StopCommands();

            var position = startingPoint.position;

            foreach (var passenger in _passengers)
            {
                passenger.EnqueueCommand(new MoveToContext(position));
                position += pointOffset;
            }
            
            return result;
        }

        private void Awake()
        {
            InitializePassengers();
        }

        private void InitializePassengers()
        {
            var position = startingPoint.position;

            for (var i = 0; i < initialCount; i++)
            {
                var passenger = creator.InstantiatePlayer();

                var passengerTransform = passenger.transform;
                passengerTransform.SetParent(transform);
                passenger.transform.position = position;

                position += pointOffset;

                _passengers.Enqueue(passenger);
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (startingPoint == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;

            var position = startingPoint.position;
            
            for (var i = 0; i < initialCount; i++)
            {
                Gizmos.DrawWireSphere(position, 0.2f);

                position += pointOffset;
            }
        }
    }
}