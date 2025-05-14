using System.Linq;
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

        private void OnAddItem(params ItemIdentifier[] identifiers)
        {
            if (identifiers.Any(x => (x.GetDefinition().tag & ItemTag.Metallic) != 0))
            {
                enabled = true;
            }
        }

        private void OnRemoveItem(params ItemIdentifier[] identifiers)
        {
            if (!HasMetallic())
            {
                enabled = false;
            }
        }

        private bool HasMetallic()
        {
            return (_storage.GetTags() & ItemTag.Metallic) != 0;
        }
    }
}