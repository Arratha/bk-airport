using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Items.Base;
using UnityEngine;

namespace Items.Storages.Attachers
{
    public abstract class AttacherStorageAbstract : StorageAbstract
    {
        public override IReadOnlyCollection<ItemIdentifier> items => selfItems.Select(x => x.identifier).ToList();
        public IReadOnlyCollection<Item> itemObjects => selfItems;
        [SerializeField] protected List<Item> selfItems;

        [Min(0), SerializeField] private int maxCount;

        [Space, SerializeField] private bool tryCreateInteractive;

        public event Action<Item> OnItemObjectAdded;
        public event Action<Item> OnItemObjectRemoved;
        
        public override ItemIdentifier GetFirstItem(bool invokeCallback = true)
        {
            var fistItem = selfItems.FirstOrDefault();

            if (fistItem == null)
            {
                return null;
            }

            if (TryRemoveItem(fistItem.identifier, invokeCallback))
            {
                return fistItem.identifier;
            }

            return null;
        }

        protected override void InitializeStorage()
        {
            if (selfItems == null)
            {
                selfItems = new List<Item>();
            }

            selfItems.RemoveAll(x => x == null);
        }

        protected override bool TryRemoveItemInternal(ItemIdentifier identifier)
        {
            var itemToRemove = selfItems.FirstOrDefault(x => x.identifier.Equals(identifier));

            if (itemToRemove == null)
            {
                return false;
            }

            selfItems.Remove(itemToRemove);
            Destroy(itemToRemove.gameObject);

            InvokeObjectRemoved(itemToRemove);
            
            return true;
        }
        
        protected override float GetFreeSpaceFloat()
        {
            if (maxCount == 0)
            {
                return Mathf.Infinity;
            }

            return maxCount;
        }

        protected void InvokeObjectAdded(Item item)
        {
            OnItemObjectAdded?.Invoke(item);
        }

        protected void InvokeObjectRemoved(Item item)
        {
            OnItemObjectRemoved?.Invoke(item);
        }
        
        protected Item GetPrefab(ItemIdentifier identifier)
        {
            var definition = identifier.GetDefinition();

            if (tryCreateInteractive && definition.interactivePrefab != null)
            {
                return definition.interactivePrefab;
            }

            return definition.prefab;
        }
    }
}