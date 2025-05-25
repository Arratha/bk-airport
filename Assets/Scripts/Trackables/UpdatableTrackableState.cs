using UnityEngine;
using Utils.Observable;

namespace Trackables
{
    public class UpdatableTrackableState<T> : ActiveTrackableState<T>, IObserver<T>
    {
        public void HandleUpdate(T message)
        {
            if (CurrentTrackable.Contains(message))
            {
                Debug.LogError($"You trying to update element not present in Trackables: {message}");
                return;
            }

            foreach (var observer in Observers)
            {
                observer.HandleUpdate(CurrentTrackable);
            }
        }
    }
}