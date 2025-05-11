using System.Collections.Generic;
using System.Linq;
using Items;
using Items.Base;

namespace Extensions
{
    public static class ItemStorageExtension
    {
        public static ItemTags GetTags(this ItemStorage storage)
        {
            var definitions = storage.GetDefinitions();

            var tags = ItemTags.None;
            definitions.ForEach(x => tags |= x.tags);

            return tags;
        }

        public static List<ItemDefinition> GetDefinitions(this ItemStorage storage)
        {
            return storage.items.Select(x => x.GetDefinition()).ToList();
        }
    }
}