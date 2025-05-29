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

    //Stores items in given types of storages
    //Storage for an item is defined by item tag
    public sealed class PassengerStorage : StorageAbstract
    {
        public override IReadOnlyCollection<ItemIdentifier> items =>
            GetStorages(StorageTag.All).SelectMany(x => x.items).ToList();

        [SerializeField] private List<StorageAbstract> pocketStorages;
        [SerializeField] private List<StorageAbstract> bagStorages;
        [SerializeField] private List<StorageAbstract> illegalStorages;
        
        public override ItemIdentifier GetFirstItem(bool invokeCallback = true)
        {
            var fistItem = items.FirstOrDefault();

            if (TryRemoveItem(fistItem, invokeCallback))
            {
                return fistItem;
            }

            return null;
        }
        
        protected override void InitializeStorage()
        {
            
        }

        protected override bool TryAddItemInternal(ItemIdentifier identifier)
        {
            var storageTag = GetStorageTag(identifier.GetDefinition().tag);
            var validStorages = GetStorages(storageTag).Randomize();
            var storage = validStorages.FirstOrDefault(x => x.TryAddItem(identifier));

            return storage != null;
        }

        protected override bool TryRemoveItemInternal(ItemIdentifier identifier)
        {
            var randomizedStorages = GetStorages(StorageTag.All).Randomize();
            var validStorages = randomizedStorages.Where(x => x.items.Contains(identifier));
            var storage = validStorages.FirstOrDefault(x => x.TryRemoveItem(identifier));

            return storage != null;
        }

        protected override float GetFreeSpaceFloat()
        {
            return GetStorages(StorageTag.All).Sum(x => x.freeSpace);
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