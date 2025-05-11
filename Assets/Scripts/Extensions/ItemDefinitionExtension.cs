using Items.Base;

namespace Extensions
{
    public static class ItemDefinitionExtension
    {
        public static ItemDefinition GetDefinition(this ItemIdentifier identifier)
        {
            var database = ItemDatabase.instance;

            return database.GetDefinition(identifier);
        }
    }
}