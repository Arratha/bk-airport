using System;
using Check;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Interactive.Activators.Conditions
{
    [Serializable]
    public class RequiredCheckType : ICondition, Utils.Observable.IObserver<CheckType>
    {
        public bool isSatisfied => _currentState == requiredState;

        [SerializeField] private CheckType requiredState;
        private CheckType _currentState;

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

            var state = ServiceProvider.instance.Resolve<IObservableState<CheckType>>();
            state.RegisterObserver(this, true);
        }

        public void Deinitialize()
        {
            if (!_isInitialized)
            {
                Debug.LogError($"The object {this} is not initialized.");
                return;
            }

            _isInitialized = false;

            var state = ServiceProvider.instance.Resolve<IObservableState<CheckType>>();
            state.UnregisterObserver(this);
        }

        public void HandleUpdate(CheckType message)
        {
            _currentState = message;
            OnChanged?.Invoke();
        }
    }
}