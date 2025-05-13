using System;
using System.Linq;
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
        
        public bool isDisposed => _isDisposed;
        private bool _isDisposed;

        private StorageAbstract _selfStorage; 
        private StorageAbstract _storageToGet;

        private ItemIdentifier[] _itemIDs;
        private Func<ItemIdentifier, bool> _compareFunction;
        
        public TransferItemFromCommand(TransferItemFromContext context, StorageAbstract selfStorage)
        {
            _selfStorage = selfStorage;
            _storageToGet = context.storage;

            _itemIDs = context.identifiers;
            _compareFunction = context.compareFunction;
        }

        public void Execute(float deltaTime)
        {
            if (_isCompleted)
            {
                return;
            }

            if (_compareFunction != null)
            {
                _itemIDs = _storageToGet.items.Where(x => _compareFunction(x)).ToArray();
            }
            
            if (!_storageToGet.TryRemoveItem(_itemIDs))
            {
                _isCompleted = true;
                OnComplete?.Invoke(false);
                
                return;
            }
            
            if (!_selfStorage.TryAddItem(_itemIDs))
            {
                _storageToGet.TryAddItem(_itemIDs);
                
                _isCompleted = true;
                OnComplete?.Invoke(false);
                
                return;
            }
            
            _isCompleted = true;
            OnComplete?.Invoke(true);
        }
        
        public void Dispose()
        {
            OnComplete = null;
        }
    }
}