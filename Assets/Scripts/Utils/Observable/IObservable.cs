namespace Utils.Observable
{
    public interface IObservable<T>
    {
        public void RegisterObserver(IObserver<T> observer, bool immediatelyUpdate = false);

        public void UnregisterObserver(IObserver<T> observer);

        public bool TryGetState(out T state);
    }
}