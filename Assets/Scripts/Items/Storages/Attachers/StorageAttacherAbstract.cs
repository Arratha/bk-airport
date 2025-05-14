using System.Collections.Generic;
using System.Linq;
using Items.Base;
using UnityEngine;

namespace Items.Storages.Attachers
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(StorageAbstract))]
    public abstract class StorageAttacherAbstract : MonoBehaviour
    {
        private StorageAbstract _storage;

        [SerializeField] protected List<Item> items = new();

        private void Awake()
        {
            OnInit();
            
            _storage = GetComponent<StorageAbstract>();
            Attach(_storage.items.ToArray());
            _storage.TryAddItem(items.Select(x => x.identifier).ToArray(), false);
        }

        protected virtual void OnInit()
        {
            
        }

        protected abstract void Attach(ItemIdentifier[] identifiers);

        protected abstract void Detach(ItemIdentifier[] identifiers);

        private void OnEnable()
        {
            _storage.OnItemAdded += Attach;
            _storage.OnItemRemoved += Detach;
        }

        private void OnDisable()
        {
            if (_storage == null)
            {
                return;
            }

            _storage.OnItemAdded -= Attach;
            _storage.OnItemRemoved -= Detach;
        }
    }
}