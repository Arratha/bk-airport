using Check.Queue;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.MainCheck.Appointors
{
    //Sets dequeued passenger as new processed passenger
    //Initiates check
    public class PassengerAppointor : MonoBehaviour, IObserver<DequeuedPassenger>
    {
        [SerializeField] private CheckProcessor processor;

        private IObservableState<DequeuedPassenger> _passengerState;

        public void HandleUpdate(DequeuedPassenger message)
        {
            processor.AppointPassenger(message.passenger);

            processor.AppointCommand(CheckStage.PlaceBaggage);
            processor.AppointCommand(CheckStage.PlaceMetallic);
            processor.AppointCommand(CheckStage.PassDetector);
        }

        private void Awake()
        {
            _passengerState = ServiceProvider.instance.Resolve<IObservableState<DequeuedPassenger>>();
            _passengerState.RegisterObserver(this, true);
        }

        private void OnDestroy()
        {
            _passengerState.UnregisterObserver(this);
        }
    }
}