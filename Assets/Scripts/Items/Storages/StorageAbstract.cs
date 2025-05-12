using System;
using System.Collections.Generic;
using Items.Base;
using UnityEngine;

namespace Items.Storages
{
    public abstract class StorageAbstract : MonoBehaviour
    {
        public abstract IReadOnlyCollection<ItemIdentifier> items { get; }

        public event Action<ItemIdentifier> OnItemAdded;
        public event Action<ItemIdentifier> OnItemRemoved;
        
        [Tooltip("MaxItems less than 1 counts as infinity"), SerializeField]
        private int maxItems;

        public bool TryAddItem(ItemIdentifier identifier)
        {
            if (identifier == null || GetFreeSpace() < 1)
            {
                return false;
            }

            if (TryAddItemInternal(identifier))
            {
                OnItemAdded?.Invoke(identifier);
                return true;
            }

            return false;
        }

        public bool TryRemoveItem(ItemIdentifier identifier)
        {
            if (identifier == null)
            {
                return false;
            }

            if (TryRemoveItemInternal(identifier))
            {
                OnItemRemoved?.Invoke(identifier);
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

        protected abstract bool TryAddItemInternal(ItemIdentifier identifier);

        protected abstract bool TryRemoveItemInternal(ItemIdentifier identifier);

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