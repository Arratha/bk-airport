using System.Linq;
using Check.MainCheck;
using Items.Storages;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.AdditionalCheck
{
    public class ProcessedPassengerHandler : MonoBehaviour, IObserver<ProcessedPassenger>, IObserver<CheckType>
    {
        [SerializeField] private Transform passengerPoint;
        [SerializeField] private StorageAbstract bagStorage;

        private IObservableState<ProcessedPassenger> _processedState;
        private IObservableState<CheckType> _checkState;

        private GameObject _passenger;
        
        public void HandleUpdate(ProcessedPassenger message)
        {
            _passenger = message.passenger.gameObject;
            var passengerTransform = _passenger.transform;

            passengerTransform.position = passengerPoint.position;
            passengerTransform.rotation = passengerPoint.rotation;

            bagStorage.TryAddItem(message.bags);
        }

        public void HandleUpdate(CheckType message)
        {
            if (message == CheckType.AdditionalCheck)
            {
                return;
            }

            if (_passenger != null)
            {
                _passenger.SetActive(false);
                _passenger = null;
            }

            bagStorage.TryRemoveItem(bagStorage.items.ToArray());
        }

        private void Awake()
        {
            _processedState = ServiceProvider.instance.Resolve<IObservableState<ProcessedPassenger>>();
            _processedState.RegisterObserver(this);
            _checkState = ServiceProvider.instance.Resolve<IObservableState<CheckType>>();
            _checkState.RegisterObserver(this);
        }

        private void OnDestroy()
        {
            _checkState.UnregisterObserver(this);
            _processedState.UnregisterObserver(this);
        }
    }
}