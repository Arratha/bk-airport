using UnityEngine;
using Interactive.Usables;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.Queue
{
    //Removes first passenger for queue and updates dequeued passenger state
    public class PassengerExtractor : MonoBehaviour
    {
        [SerializeField] private PassengersQueue queue;
        [SerializeField] private UsableBehaviour usable;

        private void Awake() => usable.OnUsed += HandleUsed;

        private void HandleUsed()
        {
            var dequeuedPassenger = queue.Dequeue();
            var dequeueState = ServiceProvider.instance.Resolve<IObservableState<DequeuedPassenger>>();

            dequeueState.HandleUpdate(new DequeuedPassenger(dequeuedPassenger));
        }

        private void OnDestroy()
        {
            if (usable == null)
            {
                return;
            }

            usable.OnUsed -= HandleUsed;
        }
    }
}