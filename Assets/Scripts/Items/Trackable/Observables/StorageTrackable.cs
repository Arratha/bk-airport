using System.Linq;
using Items.Base;
using Items.Storages;
using UnityEngine;

namespace Items.Trackable.Observables
{
    [RequireComponent(typeof(StorageAbstract))]
    [DisallowMultipleComponent]
    public class StorageTrackable : TrackableAbstract<StorageTrackable>
    {
        private StorageAbstract _storage;

        protected override void OnInit()
        {
            _storage = GetComponent<StorageAbstract>();

            _storage.OnItemAdded += OnAddItem;
            _storage.OnItemRemoved += OnRemoveItem;

            enabled = _storage.items.Any();
        }

        private void OnDestroy()
        {
            if (_storage == null)
            {
                return;
            }

            _storage.OnItemAdded -= OnAddItem;
            _storage.OnItemRemoved -= OnRemoveItem;
        }

        private void OnAddItem(ItemIdentifier identifier)
        {
            enabled = true;
        }

        private void OnRemoveItem(ItemIdentifier identifier)
        {
            enabled = _storage.items.Any();
        }
    }
}