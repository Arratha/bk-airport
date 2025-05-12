using System;
using Commands.Contexts;
using Items.Base;
using Items.Storages;

namespace Commands.Commands
{
    public class TransferItemToCommand : ICommand
    {
        public event Action<bool> OnComplete;

        public bool isCompleted => _isCompleted;
        private bool _isCompleted;

        private StorageAbstract _selfStorage; 
        private StorageAbstract _storageToTransfer;

        private ItemIdentifier _itemId;
        
        public TransferItemToCommand(TransferItemToContext context, StorageAbstract selfStorage)
        {
            _selfStorage = selfStorage;
            _storageToTransfer = context.storage;

            _itemId = context.identifier;
            
            OnComplete += context.onComplete;
        }

        public void Execute(float deltaTime)
        {
            if (_isCompleted)
            {
                return;
            }

            if (!_selfStorage.TryRemoveItem(_itemId))
            {
                _isCompleted = true;
                OnComplete?.Invoke(false);
                
                return;
            }
            
            if (!_storageToTransfer.TryAddItem(_itemId))
            {
                _selfStorage.TryAddItem(_itemId);
                
                _isCompleted = true;
                OnComplete?.Invoke(false);
                
                return;
            }
            
            _isCompleted = true;
            OnComplete?.Invoke(true);
        }
    }
}