using System;
using Commands.Contexts;
using Items.Base;
using Items.Storages;

namespace Commands.Commands
{
    public class TransferItemFromCommand : ICommand
    {
        public event Action<bool> OnComplete;

        public bool isCompleted => _isCompleted;
        private bool _isCompleted;

        private StorageAbstract _selfStorage; 
        private StorageAbstract _storageToGet;

        private ItemIdentifier _itemId;
        
        public TransferItemFromCommand(TransferItemFromContext context, StorageAbstract selfStorage)
        {
            _selfStorage = selfStorage;
            _storageToGet = context.storage;

            _itemId = context.identifier;
            
            OnComplete += context.onComplete;
        }

        public void Execute(float deltaTime)
        {
            if (_isCompleted)
            {
                return;
            }

            if (!_storageToGet.TryRemoveItem(_itemId))
            {
                _isCompleted = true;
                OnComplete?.Invoke(false);
                
                return;
            }
            
            if (!_selfStorage.TryAddItem(_itemId))
            {
                _storageToGet.TryAddItem(_itemId);
                
                _isCompleted = true;
                OnComplete?.Invoke(false);
                
                return;
            }
            
            _isCompleted = true;
            OnComplete?.Invoke(true);
        }
    }
}