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

        protected override bool TryAddItemInternal(ItemIdentifier identifier)
        {
            selfItems.Add(identifier);
            AddItemInstance(identifier);

            return true;
        }

        protected override bool TryRemoveItemInternal(ItemIdentifier identifier)
        {
            if (selfItems.Contains(identifier))
            {
                selfItems.Remove(identifier);
                RemoveItemInstance(identifier);

                return true;
            }

            return false;
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