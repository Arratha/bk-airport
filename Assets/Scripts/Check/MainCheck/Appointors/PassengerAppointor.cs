using Check.Queue;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.MainCheck.Appointors
{
    //Sets dequeued passenger as new processed passenger
    //Initiates check
    [RequireComponent(typeof(CheckProcessor))]
    public class PassengerAppointor : MonoBehaviour, IObserver<DequeuedPassenger>
    {
        private CheckProcessor _processor;

        private IObservableState<DequeuedPassenger> _passengerState;

        public void HandleUpdate(DequeuedPassenger message)
        {
            _processor.AppointPassenger(message.passenger);

            _processor.AppointCommand(CheckStage.PlaceBaggage);
            _processor.AppointCommand(CheckStage.PlaceMetallic);
            _processor.AppointCommand(CheckStage.PassDetector);
        }

        private void Awake()
        {
            _processor = GetComponent<CheckProcessor>();
            
            _passengerState = ServiceProvider.instance.Resolve<IObservableState<DequeuedPassenger>>();
            _passengerState.RegisterObserver(this, true);
        }

        private void OnDestroy()
        {
            _passengerState.UnregisterObserver(this);
        }
    }
}