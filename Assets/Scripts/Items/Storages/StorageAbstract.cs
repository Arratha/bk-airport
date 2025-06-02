using System;
using System.Collections.Generic;
using Items.Base;
using UnityEngine;

namespace Items.Storages
{
    //Stores items by their ids
    [DisallowMultipleComponent]
    public abstract class StorageAbstract : MonoBehaviour
    {
        public abstract IReadOnlyCollection<ItemIdentifier> items { get; }

        public event Action<ItemIdentifier> OnItemAdded;
        public event Action<ItemIdentifier> OnItemRemoved;

        //We use float cause it can be infinite
        public float freeSpace => GetFreeSpaceFloat();

        public bool TryAddItem(ItemIdentifier identifier, bool invokeCallback = true)
        {
            if (identifier == null)
            {
                return false;
            }

            if (GetFreeSpaceFloat() < 1)
            {
                return false;
            }

            if (!TryAddItemInternal(identifier))
            {
                return false;
            }

            if (invokeCallback)
            {
                OnItemAdded?.Invoke(identifier);
            }

            return true;
        }

        public bool TryRemoveItem(ItemIdentifier identifier, bool invokeCallback = true)
        {
            if (identifier == null)
            {
                return false;
            }

            if (!TryRemoveItemInternal(identifier))
            {
                return false;
            }

            if (invokeCallback)
            {
                OnItemRemoved?.Invoke(identifier);
            }

            return true;
        }

        public abstract ItemIdentifier GetFirstItem(bool invokeCallback = true);

        private void Awake()
        {
            InitializeStorage();

            if (GetFreeSpaceFloat() < 0)
            {
                Debug.LogError($"Items count is exceeded.", gameObject);
            }
        }

        protected abstract void InitializeStorage();

        protected abstract bool TryAddItemInternal(ItemIdentifier identifier);

        protected abstract bool TryRemoveItemInternal(ItemIdentifier identifier);

        protected abstract float GetFreeSpaceFloat();
    }
}