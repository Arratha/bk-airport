using System.Linq;
using Check.MainCheck;
using Items.Storages;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.AdditionalCheck
{
    //Receives and places in the passenger's room for additional checking
    //Hides the passenger when current check type changed to main check
    public class ProcessedPassengerHandler : MonoBehaviour, IObserver<ProcessedPassenger>, IObserver<CheckType>
    {
        [SerializeField] private Transform passengerPoint;
        [SerializeField] private StorageAbstract bagStorage;
        [SerializeField] private StorageAbstract itemStorage;

        private IObservableState<ProcessedPassenger> _processedState;
        private IObservableState<CheckType> _checkState;

        private GameObject _passenger;

        public void HandleUpdate(ProcessedPassenger message)
        {
            _passenger = message.passenger.gameObject;
            var passengerTransform = _passenger.transform;

            passengerTransform.position = passengerPoint.position;
            passengerTransform.rotation = passengerPoint.rotation;

            message.bags.ForEach(x => bagStorage.TryAddItem(x));
            message.items.ForEach(x => itemStorage.TryAddItem(x));
        }

        public void HandleUpdate(CheckType message)
        {
            if (_passenger != null)
            {
                _passenger.SetActive(message == CheckType.AdditionalCheck);
            }

            if (message != CheckType.AdditionalCheck)
            {
                _passenger = null;
                bagStorage.items.ToList().ForEach(x => bagStorage.TryRemoveItem(x));
                itemStorage.items.ToList().ForEach(x => itemStorage.TryRemoveItem(x));
            }
        }

        private void Awake()
        {
            _processedState = ServiceProvider.instance.Resolve<IObservableState<ProcessedPassenger>>();
            _processedState.RegisterObserver(this, true);
            
            _checkState = ServiceProvider.instance.Resolve<IObservableState<CheckType>>();
            _checkState.RegisterObserver(this, true);
        }

        private void OnDestroy()
        {
            _checkState.UnregisterObserver(this);
            _processedState.UnregisterObserver(this);
        }
    }
}