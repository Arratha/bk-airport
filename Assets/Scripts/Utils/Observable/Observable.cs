using System.Collections.Generic;

namespace Utils.Observable
{
    public class Observable<T> : IObservable<T>
    {
        protected T CurrentState;

        private HashSet<IObserver<T>> _observers = new();

        public void RegisterObserver(IObserver<T> observer)
        {
            _observers.Add(observer);
        }

        public void UnregisterObserver(IObserver<T> observer)
        {
            _observers.Remove(observer);
        }

        public T GetState()
        {
            return CurrentState;
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