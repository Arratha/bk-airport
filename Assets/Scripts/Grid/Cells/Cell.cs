using UnityEngine;

namespace Grid.Cells
{
    public class Cell
    {
        public Vector2 size => Size;
        protected Vector2 Size;

        public Vector2 position => Position;
        protected Vector2 Position;

        public Cell(Vector2 size, Vector2 position)
        {
            Size = size;
            Position = position;
        }
    }
}