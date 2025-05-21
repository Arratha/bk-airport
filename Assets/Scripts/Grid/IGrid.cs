using System.Collections.Generic;
using Grid.Cells;
using UnityEngine;

namespace Grid
{
    public interface IGrid<out TCell> where TCell : Cell
    {
        public IReadOnlyCollection<TCell> cells { get; }

        public void CreateCells(Vector2 cellSize);

        public void CreateCells(int rows, int columns);

        public void ClearCells();
    }
}