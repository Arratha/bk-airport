using Extensions;
using Items.Base;
using Items.Storages;
using UnityEngine;

namespace Trackables.Items
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

            enabled = (_storage.GetTags() & ItemTag.Metallic) != 0;
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

        private void OnAddItem(ItemIdentifier identifiers)
        {
            if ((identifiers.GetDefinition().tag & ItemTag.Metallic) != 0)
            {
                enabled = true;
            }
        }

        private void OnRemoveItem(ItemIdentifier identifiers)
        {
            if ((identifiers.GetDefinition().tag & ItemTag.Metallic) == 0)
            {
                return;
            }

            enabled = (_storage.GetTags() & ItemTag.Metallic) != 0;
        }
    }
}