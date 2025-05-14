using System;
using Grid;
using UnityEngine;

namespace Development
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private Vector2 speed;
        [SerializeField] private Vector2 cellSize;
        [SerializeField] private ConveyorGrid grid;

        [ContextMenu(nameof(SetCells))]
        private void SetCells()
        {
            grid.CreateCells(cellSize);
        }

        private void FixedUpdate()
        {
            grid.TryMove(speed);
        }
    }
}