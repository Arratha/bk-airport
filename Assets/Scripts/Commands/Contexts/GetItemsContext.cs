using Items.Base;

namespace Commands.Contexts
{
    public class GetItemsContext : ICommandContext
    {
        public ItemIdentifier[] identifiers => _identifiers;
        private ItemIdentifier[] _identifiers;

        public GetItemsContext(params ItemIdentifier[] identifiers)
        {
            _identifiers = identifiers;
        }
    }
}