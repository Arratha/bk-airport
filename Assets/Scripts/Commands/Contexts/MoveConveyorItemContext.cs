using Check.MainCheck.Conveyor;
using Items.Base;

namespace Commands.Contexts
{
    public class MoveConveyorItemContext : ICommandContext
    {
        public Item itemToMove => _itemToMove;
        private Item _itemToMove;

        public InteractableConveyor conveyor => _conveyor;
        private InteractableConveyor _conveyor;

        public MoveConveyorItemContext(Item item, InteractableConveyor conveyor)
        {
            _itemToMove = item;
            _conveyor = conveyor;
        }
    }
}