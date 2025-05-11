using UnityEngine;

namespace Utils.Observable
{
    public interface IObserverBehaviour<T>
    {
        public GameObject gameObject { get; }
        
        public void OnAdd(IObservableBehaviour<T> observable);

        public void OnRemove(IObservableBehaviour<T> observable);

        public void OnUpdate(T message);
    }
}