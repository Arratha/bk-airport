using System.Collections.Generic;
using Grid.Cells;
using UnityEngine;

namespace Grid
{
    public class ConveyorGrid : GridAbstract<ConveyorCell>
    {
        public new IReadOnlyCollection<AttachableCell> cells => base.cells;
        
        public bool TryMove(Vector2 translation)
        {
            var translations = new List<Vector2>();

            foreach (var x in Cells)
            {
                var warp = Vector2.zero;

                if (!x.isEmpty && !IsInGrid(x.position + translation, x.size))
                {
                    return false;
                }
                else if (!x.isEmpty)
                {
                    warp = GetWarp(x.position + translation, x.size);
                }

                translations.Add(translation + new Vector2(warp.x * gridSize.x, warp.y * gridSize.y));
            }

            for (var i = 0; i < Cells.Count; i++)
            {
                Cells[i].Move(translations[i]);
            }

            return true;
        }

        private bool IsInGrid(Vector2 position, Vector2 size)
        {
            var halfCellSize = size / 2;
            var cellMin = position - halfCellSize;
            var cellMax = position + halfCellSize;
            var gridHalfSize = gridSize / 2;

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

        private Vector2 GetWarp(Vector2 position, Vector2 size)
        {
            var halfCellSize = size / 2;
            var cellMin = position - halfCellSize;
            var cellMax = position + halfCellSize;
            var gridHalfSize = gridSize / 2;

            var result = Vector2.zero;

            if (cellMax.x <= -gridHalfSize.x)
                result.x = 1;
            if (cellMin.x >= gridHalfSize.x)
                result.x = -1;

            if (cellMax.y <= -gridHalfSize.y)
                result.y = 1;
            if (cellMin.y >= gridHalfSize.y)
                result.y = -1;

            return result;
        }

        protected override ConveyorCell CreateCell(Vector2 size, Vector2 position)
        {
            return new ConveyorCell(size, position, transform.position, gridSize);
        }

        protected override void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 0, gridSize.y));

            if (Cells == null)
            {
                return;
            }

            Cells.ForEach(cell =>
            {
                Gizmos.color = cell.isAvailable ? Color.cyan : Color.red;
                var position = transform.position + new Vector3(cell.position.x, 0, cell.position.y);
                Gizmos.DrawWireCube(position, new Vector3(cell.size.x, 0, cell.size.y));
            });
        }

    }
}