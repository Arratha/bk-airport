using System;
using System.Collections.Generic;
using Items.Base;
using Items.Storages;

namespace Commands.Contexts
{
    public class TransferItemToContext : ICommandContext
    {
        public StorageAbstract storageToPut => _storageToPut;
        private StorageAbstract _storageToPut;
        
        public Func<StorageAbstract, List<ItemIdentifier>> selectItems => _selectItems;
        private Func<StorageAbstract, List<ItemIdentifier>> _selectItems;

        public TransferItemToContext(StorageAbstract storageToPut, Func<StorageAbstract, List<ItemIdentifier>> selectItems)
        {
            _storageToPut = storageToPut;
            _selectItems = selectItems;
        }
    }
}