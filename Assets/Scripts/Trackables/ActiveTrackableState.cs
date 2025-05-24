using System.Collections.Generic;
using Utils;
using Utils.Observable;

namespace Trackables
{
    public class ActiveTrackableState<T> : IObservable<IReadOnlyCollection<T>>, IWriteOnlyCollection<T>
    {
        private HashSet<T> _currentTrackable = new();
        private HashSet<IObserver<IReadOnlyCollection<T>>> _observers = new();

        public void RegisterObserver(IObserver<IReadOnlyCollection<T>> observer, bool immediatelyUpdate)
        {
            _observers.Add(observer);

            if (immediatelyUpdate)
            {
                observer.HandleUpdate(_currentTrackable);
            }
        }

        public void UnregisterObserver(IObserver<IReadOnlyCollection<T>> observer)
        {
            _observers.Remove(observer);
        }

        public bool TryGetState(out IReadOnlyCollection<T> state)
        {
            state = _currentTrackable;
            
            return true;
        }
        
        public void Add(T obj)
        {
            _currentTrackable.Add(obj);

            foreach (var observer in _observers)
            {
                observer.HandleUpdate(_currentTrackable);
            }
        }

        public void Remove(T obj)
        {
            _currentTrackable.Remove(obj);
            
            foreach (var observer in _observers)
            {
                observer.HandleUpdate(_currentTrackable);
            }
        }
    }
}