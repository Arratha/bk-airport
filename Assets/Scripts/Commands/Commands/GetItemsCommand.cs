using System;
using Commands.Contexts;
using Items.Base;
using Items.Storages;

namespace Commands.Commands
{
    public class GetItemsCommand : ICommand
    {
        public event Action<bool> OnComplete;

        public bool isCompleted => _isCompleted;
        private bool _isCompleted;
        
        public bool isDisposed => _isDisposed;
        private bool _isDisposed;

        private StorageAbstract _selfStorage;

        private ItemIdentifier[] _itemIDs;

        public GetItemsCommand(GetItemsContext context, StorageAbstract selfStorage)
        {
            _selfStorage = selfStorage;

            _itemIDs = context.identifiers;
        }

        public void Execute(float deltaTime)
        {
            if (_isCompleted)
            {
                return;
            }

            if (!_selfStorage.TryAddItem(_itemIDs))
            {
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
         
            _isDisposed = true;
        }
    }
}