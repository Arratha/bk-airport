using System.Collections.Generic;
using Grid.Cells;
using UnityEngine;

namespace Grid
{
    public abstract class GridAbstract<T> : MonoBehaviour, IGrid<T> where T : Cell
    {
        [SerializeField] protected Vector2 gridSize;

        public IReadOnlyCollection<T> cells => Cells;
        protected List<T> Cells = new();

        public void CreateCells(Vector2 cellSize)
        {
            ClearCells();

            var rows = Mathf.FloorToInt(gridSize.x / cellSize.x);
            var columns = Mathf.FloorToInt(gridSize.y / cellSize.y);
            CreateCells(rows, columns, cellSize);
        }

        public void CreateCells(int rows, int columns)
        {
            ClearCells();
            CreateCells(rows, columns, new Vector2(gridSize.x / rows, gridSize.y / columns));
        }

        public void ClearCells()
        {
            Cells.Clear();
        }

        private void CreateCells(int rows, int columns, Vector2 cellSize)
        {
            var startingX = cellSize.x * (1 - rows) / 2;
            var posY = cellSize.y * (1 - columns) / 2;

            for (var y = 0; y < columns; y++)
            {
                var posX = startingX;

                for (var x = 0; x < rows; x++)
                {
                    Cells.Add(CreateCell(cellSize, new Vector3(posX, posY)));
                    posX += cellSize.x;
                }

                posY += cellSize.y;
            }
        }

        protected abstract T CreateCell(Vector2 size, Vector2 position);

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(gridSize.x, 0, gridSize.y));

            if (Cells == null)
            {
                return;
            }

            Gizmos.color = Color.cyan;

            Cells.ForEach(cell =>
            {
                var position = transform.position + new Vector3(cell.position.x, 0, cell.position.y);
                Gizmos.DrawWireCube(position, new Vector3(cell.size.x, 0, cell.size.y));
            });
        }
    }
}