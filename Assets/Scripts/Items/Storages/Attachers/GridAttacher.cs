using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Grid;
using Grid.Cells;
using Items.Base;
using Items.Storages.Attachers.Placers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items.Storages.Attachers
{
    //Creates instances of given ids prefabs and attaches it to grid
    [RequireComponent(typeof(GridAbstract<AttachableCell>))]
    public class GridAttacher : StorageAttacherAbstract
    {
        protected IGrid<AttachableCell> Grid;
        protected Dictionary<Transform, AttachableCell> CellsInUse = new();
        
        [Tooltip("If set as Vector2.zero, it will be calculated automatically"), SerializeField]
        protected Vector2 cellSize;

        private bool _isAutomatic;

        protected override void OnInit()
        {
            GetGrid();

            _isAutomatic = cellSize == Vector2.zero;

            if (!_isAutomatic)
            {
                Grid.CreateCells(cellSize);
            }
        }

        private void GetGrid()
        {
            Grid = GetComponent<IGrid<AttachableCell>>();

            if (Grid == null)
            {
                Grid = GetComponent<IGrid<ConveyorCell>>();
            }

            if (Grid == null)
            {
                throw new NullReferenceException(nameof(Grid));
            }
        }

        protected sealed override void Attach(ItemIdentifier[] identifiers)
        {
            var newObjects = new List<GameObject>();

            foreach (var identifier in identifiers)
            {
                var prefab = identifier.GetDefinition().prefab;
                var instance = Instantiate(prefab, transform);
                items.Add(instance);
                newObjects.Add(instance.gameObject);
            }

            if (_isAutomatic && RearrangeIfNeeded(newObjects))
            {
                Place(items.Select(x => x.gameObject).ToList());
            }
            else
            {
                Place(newObjects);
            }
        }

        protected sealed override void Detach(ItemIdentifier[] identifiers)
        {
            foreach (var identifier in identifiers)
            {
                var instance = items.First(x => x.identifier.Equals(identifier));
                items.Remove(instance);

                if (CellsInUse.TryGetValue(instance.transform, out var cell))
                {
                    cell.Remove(instance.transform);
                    
                    if (cell.isEmpty)
                    {
                        CellsInUse.Remove(instance.transform);
                    }
                }

                Destroy(instance.gameObject);
            }
        }

        private bool RearrangeIfNeeded(List<GameObject> newObjects)
        {
            var shouldRearrange = false;

            if (TryGetSize(newObjects, out var newSize))
            {
                shouldRearrange |= cellSize.x < newSize.x && (cellSize.x = newSize.x) > 0;
                shouldRearrange |= cellSize.y < newSize.y && (cellSize.y = newSize.y) > 0;
            }

            shouldRearrange |= cellSize.x == 0 && (cellSize.x = 0.1f) > 0;
            shouldRearrange |= cellSize.y == 0 && (cellSize.y = 0.1f) > 0;

            Grid.CreateCells(cellSize);

            return shouldRearrange;
        }

        protected virtual void Place(List<GameObject> objectsToPlace)
        {
            var emptyCells = Grid.cells
                .Where(x => x.isAvailable)
                .OrderBy(cell => cell.position.y)
                .ThenBy(cell => cell.position.x)
                .ToList();

            foreach (var obj in objectsToPlace)
            {
                var cell = emptyCells.FirstOrDefault();
                emptyCells.Remove(cell);

                if (cell != null)
                {
                    PlaceObject(obj, cell);
                }
                else
                {
                    obj.SetActive(false);
                }
            }
        }

        protected void PlaceObject(GameObject obj, AttachableCell cell)
        {
            var offset = Vector3.zero;

            if (obj.TryGetComponent<AttachmentBounds>(out var bounds))
            {
                offset.x = Random.Range(-(cell.size.x - bounds.size.x) / 2,
                    (cell.size.x - bounds.size.x) / 2);
                offset.y = bounds.size.y / 2;
                offset.z = Random.Range(-(cell.size.y - bounds.size.z) / 2,
                    (cell.size.y - bounds.size.z) / 2);

                offset += obj.transform.position - bounds.centralPoint.position;
            }

            cell.Attach(obj.transform, offset);
            CellsInUse.Add(obj.transform, cell);
        }

        protected bool TryGetSize(List<GameObject> objects, out Vector2 size)
        {
            size = Vector2.zero;
            var result = false;

            foreach (var obj in objects)
            {
                if (TryGetSize(obj, out var newSize))
                {
                    size.x = Mathf.Max(size.x, newSize.x);
                    size.y = Mathf.Max(size.y, newSize.y);

                    result = true;
                }
            }

            return result;
        }

        private bool TryGetSize(GameObject obj, out Vector2 size)
        {
            if (obj.TryGetComponent<AttachmentBounds>(out var bounds))
            {
                size = new Vector2(bounds.size.x, bounds.size.z);
                return true;
            }

            size = Vector2.zero;
            return false;
        }
    }
}