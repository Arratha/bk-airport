using System.Collections.Generic;
using System.Linq;
using Extensions;
using Items.Base;
using UnityEngine;

namespace Items.Storages
{
    public class AttachmentStorage : ItemStorage
    {
        [SerializeField] private List<Item> attachedItems;

        protected override void InitializeStorage()
        {
            if (selfItems == null)
            {
                selfItems = new List<ItemIdentifier>();
            }

            if (attachedItems == null)
            {
                attachedItems = new List<Item>();
            }

            selfItems.RemoveAll(x => x == null);
            attachedItems.RemoveAll(x => x == null);

            attachedItems.ForEach(x => x.gameObject.transform.SetParent(transform));

            var attachedIDs = attachedItems.Select(x => x.identifier).ToList();

            selfItems.ForEach(x => AddItemInstance(x));
            selfItems.AddRange(attachedIDs);
        }

        protected override bool TryAddItemInternal(params ItemIdentifier[] identifiers)
        {
            foreach (var identifier in identifiers)
            {
                selfItems.Add(identifier);
                AddItemInstance(identifier);
            }

            return true;
        }

        protected override bool TryRemoveItemInternal(params ItemIdentifier[] identifiers)
        {
            var removeCount = new Dictionary<ItemIdentifier, int>();
            
            foreach (var identifier in identifiers)
            {
                if (removeCount.TryGetValue(identifier, out var count))
                {
                    removeCount[identifier] = count + 1;
                }
                else
                {
                    removeCount[identifier] = 1;
                }
            }

            foreach (var kvp in removeCount)
            {
                var actualCount = 0;
                
                foreach (var item in selfItems)
                {
                    if (item.Equals(kvp.Key)) actualCount++;
                }

                if (actualCount < kvp.Value)
                {
                    return false;
                }
            }

            foreach (var identifier in identifiers)
            {
                selfItems.Remove(identifier);
                RemoveItemInstance(identifier);
            }

            return true;
        }

        protected virtual void AddItemInstance(ItemIdentifier identifier)
        {
            var prefab = identifier.GetDefinition().prefab;
            var instance = Instantiate(prefab, transform);

            attachedItems.Add(instance);
        }

        protected virtual void RemoveItemInstance(ItemIdentifier identifier)
        {
            var instance = attachedItems.FirstOrDefault(x => x.identifier);

            if (instance == null)
            {
                Debug.LogError($"No instance of item with id {identifier} was found.", gameObject);
                return;
            }

            attachedItems.Remove(instance);
            instance.Destroy();
        }
    }
}