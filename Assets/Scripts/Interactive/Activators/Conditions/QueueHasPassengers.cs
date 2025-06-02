using System;
using Check.Queue;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Interactive.Activators.Conditions
{
    [Serializable]
    public class QueueHasPassengers : ICondition, Utils.Observable.IObserver<DequeuedPassenger>
    {
        public bool isSatisfied => !isInversed == queue.count > 0;

        [SerializeField] private PassengersQueue queue;
        [SerializeField] private bool isInversed;
        
        public bool isInitialized => _isInitialized;
        private bool _isInitialized;

        public event Action OnChanged;

        public void Initialize()
        {
            if (_isInitialized)
            {
                Debug.LogError($"The object {this} has already been initialized. Re-initialization is not possible.");
                return;
            }

            _isInitialized = true;

            var state = ServiceProvider.instance.Resolve<IObservableState<DequeuedPassenger>>();
            state.RegisterObserver(this, true);
        }

        public void Deinitialize()
        {
            _isInitialized = false;

            var state = ServiceProvider.instance.Resolve<IObservableState<DequeuedPassenger>>();
            state.UnregisterObserver(this);
        }

        public void HandleUpdate(DequeuedPassenger message)
        {
            OnChanged?.Invoke();
        }
    }
}