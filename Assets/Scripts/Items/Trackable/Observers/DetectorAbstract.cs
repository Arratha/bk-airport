using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Items.Trackable.Observers
{
    public abstract class DetectorAbstract<T> : MonoBehaviour, IObserverBehaviour<T>
    {
        private IObserverMediator<T> _mediator;

        public abstract void OnAdd(IObservableBehaviour<T> observable);

        public abstract void OnRemove(IObservableBehaviour<T> observable);

        public abstract void OnUpdate(T message);

        private void Awake()
        {
            var serviceProvider = ServiceProvider.instance;
            _mediator = serviceProvider.Resolve<IObserverMediator<T>>();

            OnInit();
        }

        protected abstract void OnInit();

        private void OnEnable()
        {
            _mediator.RegisterObserver(this);
        }

        private void OnDisable()
        {
            _mediator.UnregisterObserver(this);
        }
    }
}