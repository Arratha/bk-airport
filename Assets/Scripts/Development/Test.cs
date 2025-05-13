using Commands.Contexts;
using Items;
using Items.Base;
using Items.Storages;
using Passenger;
using UnityEngine;

namespace Development
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private Transform destination;
        [SerializeField] private PassengerController controller;
        
        
        [Space, SerializeField] private StorageAbstract storage;
        [SerializeField] private ItemIdentifier identifier;
        [SerializeField] private ItemsPreset preset;

        [ContextMenu(nameof(EnqueueTransferFrom))]
        private void EnqueueTransferFrom()
        {
            controller.EnqueueCommand(new TransferItemFromContext(storage, identifier));
        }
        
        [ContextMenu(nameof(EnqueueTransferTo))]
        private void EnqueueTransferTo()
        {
            controller.EnqueueCommand(new TransferItemToContext(storage, identifier));
        }

        [ContextMenu(nameof(EnqueueMoveCommand))]
        private void EnqueueMoveCommand()
        {
            controller.EnqueueCommand(new MoveToContext(destination.position));
        }

        [ContextMenu(nameof(StopCommands))]
        private void StopCommands()
        {
            controller.StopCommands();
        }

        [ContextMenu(nameof(Add))]
        private void Add()
        {
            Debug.Log(storage.TryAddItem(identifier));
        }

        [ContextMenu(nameof(AddPreset))]
        private void AddPreset()
        {
            foreach (var item in preset.items)
            {
                Debug.Log(storage.TryAddItem(item));
            }
        }

        [ContextMenu(nameof(Remove))]
        private void Remove()
        {
            Debug.Log(storage.TryRemoveItem(identifier));
        }
    }

}