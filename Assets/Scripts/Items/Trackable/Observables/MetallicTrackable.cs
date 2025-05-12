using Extensions;
using Items.Base;
using Items.Storages;
using UnityEngine;

namespace Items.Trackable.Observables
{
    [RequireComponent(typeof(StorageAbstract))]
    [DisallowMultipleComponent]
    public class MetallicTrackable : TrackableAbstract<MetallicTrackable>
    {
        private StorageAbstract _storage;

        protected override void OnInit()
        {
            _storage = GetComponent<StorageAbstract>();

            _storage.OnItemAdded += OnAddItem;
            _storage.OnItemRemoved += OnRemoveItem;

            enabled = HasMetallic();
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
            var definition = identifier.GetDefinition();

            if ((definition.tags & ItemTags.Metallic) != 0)
            {
                enabled = true;
            }
        }

        private void OnRemoveItem(ItemIdentifier identifier)
        {
            var definition = identifier.GetDefinition();

            if ((definition.tags & ItemTags.Metallic) == 0)
            {
                return;
            }

            if (!HasMetallic())
            {
                enabled = false;
            }
        }

        private bool HasMetallic()
        {
            return (_storage.GetTags() & ItemTags.Metallic) != 0;
        }
    }
}