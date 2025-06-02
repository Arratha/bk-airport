using System;
using Check.MainCheck.Conveyor;
using Commands.Contexts;
using Items.Base;

namespace Commands.Commands
{
    public class MoveConveyorItemCommand : ICommand
    {
        public event Action<bool> OnComplete;

        public bool isCompleted => _isCompleted;
        private bool _isCompleted;

        private Item _itemToMove;
        private InteractableConveyor _conveyor;

        public MoveConveyorItemCommand(MoveConveyorItemContext context)
        {
            _itemToMove = context.itemToMove;
            _conveyor = context.conveyor;
        }

        public void Execute(float deltaTime)
        {
            if (_isCompleted)
            {
                return;
            }

            var conveyorResult = _conveyor.QueueItemMoveToConveyor(_itemToMove);

            if (conveyorResult != QueueItemMoveResult.Queued)
            {
                _isCompleted = true;
                OnComplete?.Invoke(conveyorResult == QueueItemMoveResult.InPosition);
            }
        }
    }
}