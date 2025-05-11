using System.Collections.Generic;
using Utils.Observable;

namespace Items.Trackable
{
    public class TrackableMediator<T> : IObserverMediator<T>
    {
        private HashSet<IObservableBehaviour<T>> _observables = new();
        private HashSet<IObserverBehaviour<T>> _observers = new();

        public void RegisterObserver(IObserverBehaviour<T> observer)
        {
            if (_observers.Contains(observer))
            {
                return;
            }

            _observers.Add(observer);

            foreach (var observable in _observables)
            {
                observer.OnAdd(observable);
            }
        }

        public void UnregisterObserver(IObserverBehaviour<T> observer)
        {
            _observers.Remove(observer);
        }

        public void RegisterObservable(IObservableBehaviour<T> observable)
        {
            if (_observables.Contains(observable))
            {
                return;
            }

            observable.OnUpdate += OnUpdate;
            _observables.Add(observable);

            foreach (var observer in _observers)
            {
                observer.OnAdd(observable);
            }
        }

        public void UnregisterObservable(IObservableBehaviour<T> observable)
        {
            if (!_observables.Contains(observable))
            {
                return;
            }

            observable.OnUpdate -= OnUpdate;
            _observables.Remove(observable);

            foreach (var observer in _observers)
            {
                observer.OnRemove(observable);
            }
        }

        private void OnUpdate(T message)
        {
            foreach (var observer in _observers)
            {
                observer.OnUpdate(message);
            }
        }
    }
}