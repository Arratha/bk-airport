namespace Utils.Observable
{
    public class ObservableState<T> : Observable<T>, IObservableState<T>
    {
        public ObservableState()
        {

        }

        public ObservableState(T initialState)
        {
            CurrentState = initialState;
        }

        public void HandleUpdate(T message)
        {
            CurrentState = message;

            UpdateObservers();
        }
    }
}