using System;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Items.Trackable.Observables
{
    public abstract class TrackableAbstract<T> : MonoBehaviour, IObservableBehaviour<T>
    {
        private IObserverMediator<T> _mediator;
        
#pragma warning disable 0067
        public event Action<T> OnUpdate;
#pragma warning restore 0067
        
        private void Awake()
        {
            var serviceProvider = ServiceProvider.instance;
            _mediator = serviceProvider.Resolve<IObserverMediator<T>>();

            OnInit();
        }

        protected abstract void OnInit();

        private void OnEnable()
        {
            _mediator.RegisterObservable(this);
        }

        private void OnDisable()
        {
            _mediator.UnregisterObservable(this);
        }
    }
}