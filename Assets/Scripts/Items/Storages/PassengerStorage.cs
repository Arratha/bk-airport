using System.Collections.Generic;
using System.Linq;
using Extensions;
using Items.Base;
using UnityEngine;

namespace Items.Storages
{
    public sealed class PassengerStorage : StorageAbstract
    {
        public override IReadOnlyCollection<ItemIdentifier> items
        {
            get
            {
                var result = new List<ItemIdentifier>();

                result.AddRange(pocketStorages.SelectMany(x => x.items));
                result.AddRange(bagStorages.SelectMany(x => x.items));
                result.AddRange(illegalStorages.SelectMany(x => x.items));

                return result;
            }
        }

        [SerializeField] private List<StorageAbstract> pocketStorages;
        [SerializeField] private List<StorageAbstract> bagStorages;
        [SerializeField] private List<StorageAbstract> illegalStorages;

        protected override void InitializeStorage()
        {

        }

        protected override bool TryAddItemInternal(params ItemIdentifier[] identifiers)
        {
            var bagIdentifiers = identifiers
                .Where(x => (x.GetDefinition().tags & ItemTags.Bag) != 0)
                .ToList();
            var pocketIdentifiers = identifiers
                .Where(x => (x.GetDefinition().tags & ItemTags.Bag) == 0 &&
                            (x.GetDefinition().tags & ItemTags.Illegal) == 0)
                .ToList();
            var illegalIdentifiers = identifiers
                .Where(x => (x.GetDefinition().tags & ItemTags.Bag) == 0 &&
                            (x.GetDefinition().tags & ItemTags.Illegal) != 0)
                .ToList();

            var storagesBag = GetSuitableStorages(ItemTags.Bag);
            var storagesPocket = GetSuitableStorages(ItemTags.None);
            var storagesIllegal = GetSuitableStorages(ItemTags.Illegal);

            var freeSpaceBag = storagesBag.Sum(x => x.freeSpace);
            var freeSpacePocket = storagesPocket.Sum(x => x.freeSpace);
            var freeSpaceIllegal = storagesIllegal.Sum(x => x.freeSpace);

            var canAddBags = freeSpaceBag >= bagIdentifiers.Count;
            var canAddPockets = freeSpacePocket >= pocketIdentifiers.Count;
            var canAddIllegals = (freeSpaceIllegal + freeSpacePocket) >= illegalIdentifiers.Count;

            if (!canAddBags || !canAddPockets || !canAddIllegals)
            {
                return false;
            }

            PlaceItemsInStorage(bagIdentifiers, storagesBag);
            PlaceItemsInStorage(pocketIdentifiers, storagesPocket);
            PlaceItemsInStorage(illegalIdentifiers, storagesIllegal);
            return false;
        }

        protected override bool TryRemoveItemInternal(params ItemIdentifier[] identifiers)
        {
            var selfItems = items;
            var availableStorages = GetSuitableStorages();

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
                var storage = availableStorages.First(x => x.items.Contains(identifier));

                storage.TryRemoveItem(identifier);
            }

            return true;
        }

        private void PlaceItemsInStorage(List<ItemIdentifier> identifiers, List<StorageAbstract> storages)
        {
            var availableStorages = new List<StorageAbstract>(storages);

            
            while (identifiers.Any())
            {
                var identifier = identifiers.First();

                var storage = availableStorages.First();

                if (storage.TryAddItem(identifier))
                {
                    identifiers.Remove(identifier);
                }
                else
                {
                    availableStorages.Remove(storage);
                }
            }
        }

        private List<StorageAbstract> GetSuitableStorages(ItemTags? itemTags = null)
        {
            var result = new List<StorageAbstract>();

            if (itemTags == null)
            {
                result.AddRange(bagStorages);
                result.AddRange(pocketStorages);
                result.AddRange(pocketStorages);

                return result;
            }

            if ((itemTags & ItemTags.Bag) != 0)
            {
                result.AddRange(bagStorages);

                return result;
            }

            if ((itemTags & ItemTags.Illegal) != 0)
            {
                result.AddRange(pocketStorages);
                result.AddRange(illegalStorages);

                return result;
            }

            result.AddRange(pocketStorages);

            return result;
        }
    }
}