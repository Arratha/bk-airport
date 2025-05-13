using System;
using Items.Base;
using Items.Storages;

namespace Commands.Contexts
{
    public class TransferItemFromContext : ICommandContext
    {
        public StorageAbstract storage => _storage;
        private StorageAbstract _storage;

        public ItemIdentifier[] identifiers => _identifiers;
        private ItemIdentifier[] _identifiers;

        public Func<ItemIdentifier, bool> compareFunction => _compareFunction;
        private Func<ItemIdentifier, bool> _compareFunction;

        public TransferItemFromContext(StorageAbstract storage, params ItemIdentifier[] identifiers)
        {
            _storage = storage;
            _identifiers = identifiers;
        }

        public TransferItemFromContext(StorageAbstract storage, Func<ItemIdentifier, bool> compareFunction)
        {
            _storage = storage;
            _compareFunction = compareFunction;
        }
    }
}