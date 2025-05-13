using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Trackables
{
    public abstract class TrackerBehaviour<T> : MonoBehaviour, IObserver<T>
    {
        private IObservable<T> _state;
        
        public abstract void HandleUpdate(T message);

        private void Awake()
        {
            var serviceProvider = ServiceProvider.instance;
            _state = serviceProvider.Resolve<IObservable<T>>();

            OnInit();
        }

        protected virtual void OnInit()
        {
            
        }

        private void OnEnable()
        {
            _state.RegisterObserver(this);
            HandleUpdate(_state.GetState());
            
            HandleEnable();
        }

        private void OnDisable()
        {
            _state.UnregisterObserver(this);

            HandleDisable();
        }

        protected virtual void HandleEnable()
        {

        }

        protected virtual void HandleDisable()
        {

        }
    }
}