namespace Utils.Observable
{
    public interface IObservableState<T> : IObservable<T>, IObserver<T>
    {

    }
}