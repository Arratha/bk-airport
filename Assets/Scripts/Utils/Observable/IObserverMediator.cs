namespace Utils.Observable
{
    public interface IObserverMediator<T>
    {
        public void RegisterObserver(IObserverBehaviour<T> observer);

        public void UnregisterObserver(IObserverBehaviour<T> observer);

        public void RegisterObservable(IObservableBehaviour<T> observable);

        public void UnregisterObservable(IObservableBehaviour<T> observable);
    }
}