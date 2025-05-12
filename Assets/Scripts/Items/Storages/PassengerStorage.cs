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

        protected override bool TryAddItemInternal(ItemIdentifier identifier)
        {
            var availableStorages = GetSuitableStorages(identifier).Randomize();

            while (availableStorages.Any())
            {
                var currentStorage = availableStorages.First();

                if (currentStorage.TryAddItem(identifier))
                {
                    return true;
                }

                availableStorages.Remove(currentStorage);
            }

            return false;
        }

        protected override bool TryRemoveItemInternal(ItemIdentifier identifier)
        {
            var availableStorages = GetSuitableStorages(identifier);

            while (availableStorages.Any())
            {
                var currentStorage = availableStorages.First();

                if (currentStorage.TryRemoveItem(identifier))
                {
                    return true;
                }
                
                availableStorages.Remove(currentStorage);
            }

            return false;
        }

        private List<StorageAbstract> GetSuitableStorages(ItemIdentifier identifier)
        {
            var result = new List<StorageAbstract>();
            var definition = identifier.GetDefinition();

            if ((definition.tags & ItemTags.Bag) != 0)
            {
                result.AddRange(bagStorages);
                
                return result;
            }

            if ((definition.tags & ItemTags.Illegal) == 0)
            {
                result.AddRange(pocketStorages);

                return result;
            }
            
            if ((definition.tags & ItemTags.Illegal) != 0)
            {
                result.AddRange(pocketStorages);
                result.AddRange(illegalStorages);

                return result;
            }

            return null;
        }
    }
}