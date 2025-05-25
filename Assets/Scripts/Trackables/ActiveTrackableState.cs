using System.Collections.Generic;
using Utils;
using Utils.Observable;

namespace Trackables
{
    public class ActiveTrackableState<T> : IObservable<IReadOnlyCollection<T>>, IWriteOnlyCollection<T>
    {
        protected HashSet<T> CurrentTrackable = new();
        protected HashSet<IObserver<IReadOnlyCollection<T>>> Observers = new();

        public void RegisterObserver(IObserver<IReadOnlyCollection<T>> observer, bool immediatelyUpdate)
        {
            Observers.Add(observer);

            if (immediatelyUpdate)
            {
                observer.HandleUpdate(CurrentTrackable);
            }
        }

        public void UnregisterObserver(IObserver<IReadOnlyCollection<T>> observer)
        {
            Observers.Remove(observer);
        }

        public bool TryGetState(out IReadOnlyCollection<T> state)
        {
            state = CurrentTrackable;
            
            return true;
        }
        
        public void Add(T obj)
        {
            CurrentTrackable.Add(obj);

            foreach (var observer in Observers)
            {
                observer.HandleUpdate(CurrentTrackable);
            }
        }

        public void Remove(T obj)
        {
            CurrentTrackable.Remove(obj);
            
            foreach (var observer in Observers)
            {
                observer.HandleUpdate(CurrentTrackable);
            }
        }

        public void Update(T obj)
        {
            throw new System.NotImplementedException();
        }
    }
}