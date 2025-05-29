using System;
using System.Collections.Generic;
using System.Linq;
using Commands.Contexts;
using Items.Base;
using Items.Storages;
using UnityEngine;

namespace Commands.Commands
{
    public class TransferItemToCommand : ICommand
    {
        public event Action<bool> OnComplete;

        public bool isCompleted => _isCompleted;
        private bool _isCompleted;

        private StorageAbstract _storageToGet;
        private StorageAbstract _storageToPut;

        private List<ItemIdentifier> _itemIDs;

        private Func<StorageAbstract, List<ItemIdentifier>> _selectItems;

        public TransferItemToCommand(TransferItemToContext context, StorageAbstract storageToGet)
        {
            _storageToGet = storageToGet;
            _storageToPut = context.storageToPut;

            _selectItems = context.selectItems;
        }

        public void Execute(float deltaTime)
        {
            if (_isCompleted)
            {
                return;
            }

            if (_itemIDs == null)
            {
                _itemIDs = _selectItems(_storageToGet).Where(x => x != null).ToList();
            }

            var notAddedItems = new List<ItemIdentifier>();

            _itemIDs.ForEach(x =>
            {
                if (!_storageToGet.TryRemoveItem(x))
                {
                    Debug.LogError($"Cannot remove {x}");
                    notAddedItems.Add(x);
                    return;
                }

                if (!_storageToPut.TryAddItem(x))
                {
                    Debug.LogError($"Cannot add {x}");
                    _storageToGet.TryAddItem(x, false);
                    notAddedItems.Add(x);
                }
            });

            _itemIDs = notAddedItems;

            
            if (_itemIDs.Any())
            {
                return;
            }

            _isCompleted = true;
            OnComplete?.Invoke(true);
        }
    }
}