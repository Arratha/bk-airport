using System;
using System.Collections.Generic;
using System.Linq;
using Items.Base;
using UnityEngine;

namespace Items.Storages
{
    [DisallowMultipleComponent]
    public abstract class StorageAbstract : MonoBehaviour
    {
        public abstract IReadOnlyCollection<ItemIdentifier> items { get; }

        public event Action<ItemIdentifier[]> OnItemAdded;
        public event Action<ItemIdentifier[]> OnItemRemoved;

        public int freeSpace => GetFreeSpace();

        [Tooltip("MaxItems less than 1 counts as infinity"), SerializeField]
        private int maxItems;

        public bool TryAddItem(ItemIdentifier[] identifiers, bool invokeCallback = true)
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
                if (invokeCallback)
                {
                    OnItemAdded?.Invoke(identifiers);
                }

                return true;
            }

            return false;
        }

        public bool TryRemoveItem(ItemIdentifier[] identifiers, bool invokeCallback = true)
        {
            if (identifiers == null
                || identifiers.Any(x => x == null))
            {
                return false;
            }

            if (TryRemoveItemInternal(identifiers))
            {
                if (invokeCallback)
                {
                    OnItemRemoved?.Invoke(identifiers);
                }

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