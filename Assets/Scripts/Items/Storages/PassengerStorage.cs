using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Items.Base;
using UnityEngine;

namespace Items.Storages
{
    public enum StorageTag
    {
        Bag,
        Illegal,
        Pocket,
        All
    }

    public sealed class PassengerStorage : StorageAbstract
    {
        public override IReadOnlyCollection<ItemIdentifier> items => pocketStorages.SelectMany(x => x.items)
            .Concat(bagStorages.SelectMany(x => x.items))
            .Concat(illegalStorages.SelectMany(x => x.items))
            .ToArray();

        [SerializeField] private List<StorageAbstract> pocketStorages;
        [SerializeField] private List<StorageAbstract> bagStorages;
        [SerializeField] private List<StorageAbstract> illegalStorages;

        protected override void InitializeStorage()
        {

        }

        protected override bool TryAddItemInternal(params ItemIdentifier[] identifiers)
        {
            var placeCount = identifiers
                .GroupBy(item => GetStorageTag(item.GetDefinition().tag))
                .ToDictionary(
                    group => group.Key,
                    group => group.ToList()
                );
            
            var storages = Enum.GetValues(typeof(StorageTag))
                .Cast<StorageTag>().ToDictionary(
                    x => x,
                    x => GetStorages(x));
            
            var isExceed = placeCount.Any(x =>!storages[x.Key].Any(storage => storage.freeSpace == int.MaxValue) 
                                              && x.Value.Count > storages[x.Key].Sum(storage => storage.freeSpace));

            if (isExceed)
            {
                return false;
            }
            
            foreach (var kvp in placeCount)
            {
                var suitableStorages = storages[kvp.Key].Randomize();

                while (kvp.Value.Any())
                {
                    var storage = suitableStorages.First();
                    suitableStorages.Remove(storage);
                    
                    var placementBatch = kvp.Value.Take(storage.freeSpace).ToArray();

                    storage.TryAddItem(placementBatch);

                    foreach (var identifier in placementBatch)
                    {
                        kvp.Value.Remove(identifier);
                    }
                }
            }
            
            return true;
        }

        protected override bool TryRemoveItemInternal(params ItemIdentifier[] identifiers)
        {
            var selfItems = items;
            var storages = GetStorages(StorageTag.All);
            
            var removeCount = identifiers
                .GroupBy(item => item)
                .ToDictionary(
                    group => group.Key,
                    group => group.Count()
                );

            if (removeCount.Any(kvp =>
                    selfItems.Count(x => x.Equals(kvp.Key)) < kvp.Value))
            {
                return false;
            }

            foreach (var storage in storages)
            {
                var removeBatch = storage.items.Where(x =>
                {
                    if (removeCount.TryGetValue(x, out var count) && count > 0)
                    {
                        removeCount[x]--;
                        return true;
                    }

                    return false;
                }).ToArray();

                storage.TryRemoveItem(removeBatch);
            }

            return true;
        }

        private StorageTag GetStorageTag(ItemTag itemTag)
        {
            if ((itemTag & ItemTag.Bag) != 0)
            {
                return StorageTag.Bag;
            }

            if ((itemTag & ItemTag.Illegal) != 0)
            {
                return StorageTag.Illegal;
            }

            return StorageTag.Pocket;
        }

        private List<StorageAbstract> GetStorages(StorageTag itemTag)
        {
            switch (itemTag)
            {
                case StorageTag.Bag:
                    return bagStorages.ToList();
                case StorageTag.Illegal:
                    return pocketStorages.Concat(illegalStorages).ToList();
                case StorageTag.Pocket:
                    return pocketStorages.ToList();
            }

            return bagStorages
                .Concat(pocketStorages)
                .Concat(illegalStorages)
                .ToList();
        }
    }
}