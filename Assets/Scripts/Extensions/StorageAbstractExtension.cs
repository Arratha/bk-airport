using System.Collections.Generic;
using System.Linq;
using Items.Base;
using Items.Storages;

namespace Extensions
{
    public static class StorageAbstractExtension
    {
        public static ItemTag GetTags(this StorageAbstract storage)
        {
            var definitions = storage.GetDefinitions();

            var tags = ItemTag.None;
            definitions.ForEach(x => tags |= x.tag);

            return tags;
        }

        public static List<ItemDefinition> GetDefinitions(this StorageAbstract storage)
        {
            return storage.items.Select(x => x.GetDefinition()).ToList();
        }
    }
}