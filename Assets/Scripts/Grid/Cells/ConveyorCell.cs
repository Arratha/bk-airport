using UnityEngine;

namespace Grid.Cells
{
    public class ConveyorCell : AttachableCell
    {
        public override bool isAvailable => base.isAvailable && _isInGrid;
        private bool _isInGrid;

        private Vector2 _gridSize;

        public ConveyorCell(Vector2 size, Vector3 position, Vector3 gridWorldPosition, Vector2 gridSize) : base(size,
            position, gridWorldPosition)
        {
            _gridSize = gridSize;
            _isInGrid = IsInGrid();
        }

        public void Move(Vector2 translation)
        {
            Position += translation;

            Attached.ForEach(x => x.position = x.position + new Vector3(translation.x, 0, translation.y));
            _isInGrid = IsInGrid();
        }

        private bool IsInGrid()
        {
            var halfCellSize = size / 2;
            var cellMin = position - halfCellSize;
            var cellMax = position + halfCellSize;
            var gridHalfSize = _gridSize / 2;

            if (cellMin.x < -gridHalfSize.x)
                return false;
            if (cellMax.x > gridHalfSize.x)
                return false;

            if (cellMin.y < -gridHalfSize.y)
                return false;
            if (cellMax.y > gridHalfSize.y)
                return false;

            return true;
        }
    }
}