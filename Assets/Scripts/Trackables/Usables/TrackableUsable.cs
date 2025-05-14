using UnityEngine;
using Usables;
using Utils;
using Utils.Observable;
using Utils.SimpleDI;

namespace Trackables.Usables
{
    [RequireComponent(typeof(Collider))]
    public class TrackableUsable : UsableBehaviour, ILabeled
    {
        public new Collider collider
        {
            get
            {
                if (_collider == null)
                {
                    _collider = GetComponent<Collider>();
                }

                return _collider;
            }
        }

        private Collider _collider;

        public string label => selfLabel;
        [SerializeField] private string selfLabel;

        private IWriteOnlyCollection<TrackableUsable> _collection;

        private void Awake()
        {
            var serviceProvider = ServiceProvider.instance;
            _collection = serviceProvider.Resolve<IWriteOnlyCollection<TrackableUsable>>();
        }

        private void OnEnable()
        {
            collider.enabled = true;
            _collection.Add(this);
        }

        private void OnDisable()
        {
            collider.enabled = false;
            _collection.Remove(this);
        }

        protected override bool TryUse()
        {
            return enabled;
        }
    }
}