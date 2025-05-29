using System.Collections.Generic;
using System.Linq;
using Extensions;
using Items.Base;
using Items.Storages.Placers;
using Unity.Mathematics;
using UnityEngine;

namespace Items.Storages
{
    public class AttachmentStorage : StorageAbstract
    {
        public override IReadOnlyCollection<ItemIdentifier> items => selfItems.Select(x => x.identifier).ToList();
        [SerializeField] protected List<Item> selfItems;

        [Min(0), SerializeField] private int maxCount;

        [Space, SerializeField] private bool tryCreateInteractive;

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

        protected override bool TryAddItemInternal(ItemIdentifier identifier)
        {
            var definition = identifier.GetDefinition();
            var prefab = tryCreateInteractive
                ? (definition.interactivePrefab != null ? definition.interactivePrefab : definition.prefab)
                : definition.prefab;

            var itemToAdd = Instantiate(prefab, transform);
            
            var itemTransform = itemToAdd.transform;
            
            if (!itemToAdd.TryGetComponent<AttachmentPoint>(out var point))
            {
                itemTransform.localPosition = Vector3.zero; 
                itemTransform.localRotation = quaternion.identity;
            }
            else
            {
                itemTransform.localPosition = point.positionOffset;
                itemTransform.localRotation = quaternion.Euler(point.rotationOffset);
            }

            selfItems.Add(itemToAdd);

            return true;
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
    }
}