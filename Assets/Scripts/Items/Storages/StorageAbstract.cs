using System;
using System.Collections.Generic;
using System.Linq;
using Items.Base;
using UnityEngine;

namespace Items.Storages
{
    public abstract class StorageAbstract : MonoBehaviour
    {
        public abstract IReadOnlyCollection<ItemIdentifier> items { get; }

        public event Action<ItemIdentifier[]> OnItemAdded;
        public event Action<ItemIdentifier[]> OnItemRemoved;

        public int freeSpace => maxItems - items.Count;
        [Tooltip("MaxItems less than 1 counts as infinity"), SerializeField]
        private int maxItems;

        public bool TryAddItem(params ItemIdentifier[] identifiers)
        {
            if (identifiers == null
                || identifiers.Any(x => x == null))
            {
                return false;
            }

            if (GetFreeSpace() < identifiers.Length)
            {
                return false;
            }

            if (TryAddItemInternal(identifiers))
            {
                OnItemAdded?.Invoke(identifiers);
                return true;
            }

            return false;
        }

        public bool TryRemoveItem(params ItemIdentifier[] identifiers)
        {
            if (identifiers == null 
                || identifiers.Any(x => x == null))
            {
                return false;
            }

            if (TryRemoveItemInternal(identifiers))
            {
                OnItemRemoved?.Invoke(identifiers);
                return true;
            }

            return false;
        }

        private void Awake()
        {
            InitializeStorage();

            if (GetFreeSpace() < 0)
            {
                Debug.LogError($"Items count is exceeded.", gameObject);
            }
        }

        protected abstract void InitializeStorage();

        protected abstract bool TryAddItemInternal(params ItemIdentifier[] identifiers);

        protected abstract bool TryRemoveItemInternal(params ItemIdentifier[] identifiers);

        private int GetFreeSpace()
        {
            if (maxItems <= 0)
            {
                return int.MaxValue;
            }

            return maxItems - items.Count;
        }
    }
}