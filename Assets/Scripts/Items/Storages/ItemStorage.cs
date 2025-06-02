using System.Collections.Generic;
using System.Linq;
using Items.Base;
using UnityEngine;

namespace Items.Storages
{
    //Basic storage
    public class ItemStorage : StorageAbstract
    {
        public override IReadOnlyCollection<ItemIdentifier> items => selfItems;
        [SerializeField] protected List<ItemIdentifier> selfItems;

        [Min(0), SerializeField] private int maxCount;

        public override ItemIdentifier GetFirstItem(bool invokeCallback = true)
        {
            var fistItem = selfItems.FirstOrDefault();

            if (TryRemoveItem(fistItem, invokeCallback))
            {
                return fistItem;
            }

            return null;
        }

        protected override void InitializeStorage()
        {
            if (selfItems == null)
            {
                selfItems = new List<ItemIdentifier>();
            }

            selfItems.RemoveAll(x => x == null);
        }

        protected override bool TryAddItemInternal(ItemIdentifier identifier)
        {
            selfItems.Add(identifier);

            return true;
        }

        protected override bool TryRemoveItemInternal(ItemIdentifier identifier)
        {
            if (!selfItems.Contains(identifier))
            {
                return false;
            }

            selfItems.Remove(identifier);
            return true;
        }

        protected override float GetFreeSpaceFloat()
        {
            if (maxCount == 0)
            {
                return Mathf.Infinity;
            }

            return maxCount - selfItems.Count;
        }
    }
}