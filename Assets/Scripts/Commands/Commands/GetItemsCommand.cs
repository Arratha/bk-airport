using System;
using System.Collections.Generic;
using System.Linq;
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

        private StorageAbstract _selfStorage;

        private List<ItemIdentifier> _itemIDs;

        public GetItemsCommand(GetItemsContext context, StorageAbstract selfStorage)
        {
            _selfStorage = selfStorage;

            _itemIDs = context.identifiers.Where(x => x != null).ToList();
        }

        public void Execute(float deltaTime)
        {
            if (_isCompleted)
            {
                return;
            }

            var notAddedItems = new List<ItemIdentifier>();

            _itemIDs.ForEach(x =>
            {
                if (!_selfStorage.TryAddItem(x))
                {
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