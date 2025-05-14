using Grid.Cells;
using UnityEngine;

namespace Grid
{
    public class AttachableGrid : GridAbstract<AttachableCell>
    {
        protected override AttachableCell CreateCell(Vector2 size, Vector2 position)
        {
            return new AttachableCell(size, position, transform.position);
        }
    }
}