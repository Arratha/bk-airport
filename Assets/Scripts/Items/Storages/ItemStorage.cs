using System.Collections.Generic;
using Items.Base;
using UnityEngine;

namespace Items.Storages
{
    public class ItemStorage : StorageAbstract
    {
        public override IReadOnlyCollection<ItemIdentifier> items => selfItems;
        [SerializeField] protected List<ItemIdentifier> selfItems;

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
            if (selfItems.Contains(identifier))
            {
                selfItems.Remove(identifier);
                return true;
            }

            return false;
        }
    }
}