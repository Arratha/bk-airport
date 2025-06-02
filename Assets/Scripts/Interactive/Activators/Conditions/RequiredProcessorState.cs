using System;
using Check.MainCheck;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Interactive.Activators.Conditions
{
    [Serializable]
    public class RequiredProcessorState : ICondition, Utils.Observable.IObserver<ProcessorState>
    {
        public bool isSatisfied => !isInversed == (_currentState == requiredState);

        [SerializeField] private ProcessorState requiredState;
        private ProcessorState _currentState;

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

            var state = ServiceProvider.instance.Resolve<IObservableState<ProcessorState>>();
            state.RegisterObserver(this, true);
        }

        public void Deinitialize()
        {
            _isInitialized = false;

            var state = ServiceProvider.instance.Resolve<IObservableState<ProcessorState>>();
            state.UnregisterObserver(this);
        }

        public void HandleUpdate(ProcessorState message)
        {
            _currentState = message;
            OnChanged?.Invoke();
        }
    }
}