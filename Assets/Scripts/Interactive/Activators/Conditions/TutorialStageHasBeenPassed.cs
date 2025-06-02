using System;
using Check.Tutorial;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Interactive.Activators.Conditions
{
    [Serializable]
    public class TutorialStageHasBeenPassed : ICondition, Utils.Observable.IObserver<TutorialStage>
    {
        public bool isSatisfied => !isInversed == _hasBeenPassed;

        [SerializeField] private TutorialStage requiredStage;
        [SerializeField] private bool isInversed;

        private bool _hasBeenPassed;

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

            var state = ServiceProvider.instance.Resolve<IObservableState<TutorialStage>>();
            state.RegisterObserver(this, true);
        }

        public void Deinitialize()
        {
            _isInitialized = false;

            var state = ServiceProvider.instance.Resolve<IObservableState<TutorialStage>>();
            state.UnregisterObserver(this);
        }

        public void HandleUpdate(TutorialStage message)
        {
            if (requiredStage == message || message == TutorialStage.None)
            {
                _hasBeenPassed = true;
            }

            OnChanged?.Invoke();
        }
    }
}