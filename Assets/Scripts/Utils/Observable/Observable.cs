using System.Collections.Generic;

namespace Utils.Observable
{
    public class Observable<T> : IObservable<T>
    {
        protected T CurrentState;

        private HashSet<IObserver<T>> _observers = new();

        protected bool HasState;

        public void RegisterObserver(IObserver<T> observer, bool immediatelyUpdate = false)
        {
            _observers.Add(observer);

            if (HasState && immediatelyUpdate)
            {
                observer.HandleUpdate(CurrentState);
            }
        }

        public void UnregisterObserver(IObserver<T> observer)
        {
            _observers.Remove(observer);
        }

        public bool TryGetState(out T state)
        {
            state = HasState ? CurrentState : default;

            return HasState;
        }

        protected void UpdateObservers()
        {
            foreach (var observer in _observers)
            {
                observer.HandleUpdate(CurrentState);
            }
        }
    }
}