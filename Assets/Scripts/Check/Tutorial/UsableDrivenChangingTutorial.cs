using System.Collections.Generic;
using Check.Queue;
using Interactive.Usables;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.Tutorial
{
    public class UsableDrivenChangingTutorial : ChangingTutorialAbstract, IObserver<DequeuedPassenger>
    {
        [Space, SerializeField] private List<UsableBehaviour> usables;

        private IObservableState<DequeuedPassenger> _state;

        public void HandleUpdate(DequeuedPassenger message)
        {
            IsHidden = false;
            
            SetText();
        }

        protected override void HandleInit()
        {
            _state = ServiceProvider.instance.Resolve<IObservableState<DequeuedPassenger>>();
            _state.RegisterObserver(this, true);
        }

        protected override void HandleDestroy()
        {
            _state.UnregisterObserver(this);
        }

        private void HandleUsed()
        {
            IsHidden = true;

            if (!WasShown)
            {
                WasShown = true;
                TutorialState.HandleUpdate(stage + 1);
            }
            
            SetText();
        }

        private void OnEnable()
        {
            usables.ForEach(x =>
            {
                if (x != null)
                {
                    x.OnUsed += HandleUsed;
                }
            });
        }

        private void OnDisable()
        {
            usables.ForEach(x =>
            {
                if (x != null)
                {
                    x.OnUsed -= HandleUsed;
                }
            });
        }
    }
}