namespace Utils.Observable
{
    public interface IObserver<T>
    {
        public void HandleUpdate(T message);
    }
}