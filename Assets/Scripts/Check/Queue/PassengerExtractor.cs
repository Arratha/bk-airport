using UnityEngine;
using Usables;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.Queue
{
    //Removes first passenger for queue and updates dequeued passenger state
    [RequireComponent(typeof(PassengersQueue))]
    [RequireComponent(typeof(IUsable))]
    public class PassengerExtractor : MonoBehaviour
    {
        private PassengersQueue _queue;
        private IUsable _usable;

        private void Awake()
        {
            _queue = GetComponent<PassengersQueue>();
            _usable = GetComponent<IUsable>();

            _usable.OnUsed += HandleUsed;
        }

        private void HandleUsed()
        {
            var dequeuedPassenger = _queue.Dequeue();
            var dequeueState = ServiceProvider.instance.Resolve<IObservableState<DequeuedPassenger>>();

            dequeueState.HandleUpdate(new DequeuedPassenger(dequeuedPassenger));
        }

        private void OnDestroy()
        {
            if (_usable == null)
            {
                return;
            }

            _usable.OnUsed -= HandleUsed;
        }
    }
}