using UnityEngine;
using Utils;
using Utils.SimpleDI;

namespace Trackables
{
    public abstract class TrackableAbstract<T> : MonoBehaviour where T : TrackableAbstract<T>
    {
        private IWriteOnlyCollection<T> _state;

        private void Awake()
        {
            _state = ServiceProvider.instance.Resolve<IWriteOnlyCollection<T>>();

            OnInit();
        }

        protected virtual void OnInit()
        {

        }

        private void OnEnable()
        {
            _state.Add((T)this);
        }

        private void OnDisable()
        {
            _state.Remove((T)this);
        }
    }
}