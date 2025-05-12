using System.Linq;
using Extensions;
using Items.Base;
using Items.Storages;
using Items.Trackable.Observables;
using Unity.VisualScripting;
using UnityEngine;

namespace Items.Trackable
{
    [RequireComponent(typeof(StorageAbstract))]
    [DisallowMultipleComponent]
    public class TrackableResolver : MonoBehaviour
    {
        private StorageAbstract _storage;

        private void Awake()
        {
            _storage = GetComponent<StorageAbstract>();

            TrackStorage();
            TrackMetallic();
        }

        private void OnDestroy()
        {
            if (_storage == null)
            {
                return;
            }

            _storage.OnItemAdded -= OnAddStorage;
            _storage.OnItemAdded -= OnAddMetallic;
        }

        private void TrackStorage()
        {
            if (_storage.items.Any())
            {
                this.AddComponent<StorageTrackable>();
                return;
            }

            _storage.OnItemAdded += OnAddStorage;
        }

        private void TrackMetallic()
        {
            if ((_storage.GetTags() & ItemTags.Metallic) != 0)
            {
                this.AddComponent<MetallicTrackable>();
                return;
            }

            _storage.OnItemAdded += OnAddMetallic;
        }

        private void OnAddStorage(ItemIdentifier identifier)
        {
            Debug.Log(1);
            
            _storage.OnItemAdded -= OnAddStorage;
            
            this.AddComponent<StorageTrackable>();
        }

        private void OnAddMetallic(ItemIdentifier identifier)
        {
            var definition = identifier.GetDefinition();
            
            if ((definition.tags & ItemTags.Metallic) == 0)
            {
                return;
            }
            
            _storage.OnItemAdded -= OnAddMetallic;
            
            this.AddComponent<MetallicTrackable>();
        }
    }
}