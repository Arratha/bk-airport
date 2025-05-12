using System;
using Items.Base;
using Items.Storages;

namespace Commands.Contexts
{
    public class TransferItemToContext : ICommandContext
    {
        public Action<bool> onComplete => _onComplete;
        private Action<bool> _onComplete;

        public StorageAbstract storage => _storage;
        private StorageAbstract _storage;

        public ItemIdentifier identifier => _identifier;
        private ItemIdentifier _identifier;

        public TransferItemToContext(Action<bool> onComplete, StorageAbstract storage, ItemIdentifier identifier)
        {
            _onComplete = onComplete;
            _storage = storage;
            _identifier = identifier;
        }
    }
}