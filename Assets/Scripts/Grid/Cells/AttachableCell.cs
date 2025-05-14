using System.Collections.Generic;
using UnityEngine;

namespace Grid.Cells
{
    public class AttachableCell : Cell
    {
        public virtual bool isAvailable => isEmpty;
        public bool isEmpty => Attached.Count == 0;
        protected List<Transform> Attached = new();

        private Vector3 _gridWorldPosition;

        public AttachableCell(Vector2 size, Vector3 position, Vector3 gridWorldPosition) : base(size, position)
        {
            _gridWorldPosition = gridWorldPosition;
        }

        public void Attach(Transform attachable, Vector3 offset = new())
        {
            Attached.Add(attachable);

            attachable.SetParent(null);
            attachable.position = _gridWorldPosition + new Vector3(Position.x, 0, Position.y) + offset;
        }

        public void Remove(Transform attachable)
        {
            Attached.Remove(attachable);
        }
    }
}