using System.Collections.Generic;
using Items.Base;
using UnityEngine;

namespace Items.Storages
{
    //Basic storage
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

        protected override bool TryAddItemInternal(params ItemIdentifier[] identifiers)
        {
            foreach (var identifier in identifiers)
            {
                selfItems.Add(identifier);
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
            }

            return true;
        }
    }
}