using System;
using System.Collections.Generic;
using Items.Base;
using Items.Storages;

namespace Commands.Contexts
{
    public class TransferItemFromContext : ICommandContext
    {
        public StorageAbstract storageToGet => _storageToGet;
        private StorageAbstract _storageToGet;

        public ItemIdentifier[] identifiers => _identifiers;
        private ItemIdentifier[] _identifiers;

        public Func<StorageAbstract, List<ItemIdentifier>> selectItems => _selectItems;
        private Func<StorageAbstract, List<ItemIdentifier>> _selectItems;

        public TransferItemFromContext(StorageAbstract storageToGet,
            Func<StorageAbstract, List<ItemIdentifier>> selectItems)
        {
            _storageToGet = storageToGet;
            _selectItems = selectItems;
        }
    }
}