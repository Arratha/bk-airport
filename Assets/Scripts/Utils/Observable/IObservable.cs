namespace Utils.Observable
{
    public interface IObservable<T>
    {
        public void RegisterObserver(IObserver<T> observer);

        public void UnregisterObserver(IObserver<T> observer);

        public T GetState();
    }
}