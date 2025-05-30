using System.Collections.Generic;
using System.Linq;
using Extensions;
using Items.Base;
using Items.Storages.Placers;
using UnityEngine;

namespace Items.Storages.Attachers
{
    public enum Horizontal
    {
        Left,
        Right
    }

    public enum Vertical
    {
        Bottom,
        Top
    }

    //TODO: refactor and optimize
    public class GridStorage : AttacherStorageAbstract
    {
        [Space, SerializeField] protected Vector2 size;

        [Space, SerializeField] private bool prioritizeHorizontal;
        [SerializeField] private Horizontal horizontalPlacement;
        [SerializeField] private Vertical verticalPlacement;

        [Space, SerializeField] private bool drawLines;

        protected Dictionary<Item, Bounds> ItemBounds = new();

        protected override void InitializeStorage()
        {
            base.InitializeStorage();

            selfItems.ForEach(x =>
            {
                if (!x.TryGetComponent<AttachmentBounds>(out var attachmentBounds))
                {
                    return;
                }

                ItemBounds.Add(x, attachmentBounds.bounds);
            });
        }

        protected override bool TryAddItemInternal(ItemIdentifier identifier)
        {
            var prefab = GetPrefab(identifier);

            if (!prefab.TryGetComponent<AttachmentBounds>(out var attachmentBounds))
            {
                return false;
            }

            var bounds = attachmentBounds.bounds;
            bounds.center = Vector3.up * (bounds.size.y / 2 + transform.position.y);

            if (!TryFindValidPosition(bounds, out var validBoundsPosition))
            {
                return false;
            }

            var itemToAdd = Instantiate(prefab);

            var itemTransform = itemToAdd.transform;
            itemTransform.SetParent(transform);

            bounds.center = validBoundsPosition;
            itemTransform.position = validBoundsPosition - (attachmentBounds.bounds.center + prefab.transform.position);

            selfItems.Add(itemToAdd);
            ItemBounds.Add(itemToAdd, bounds);

            InvokeObjectAdded(itemToAdd);
            
            return true;
        }

        protected override bool TryRemoveItemInternal(ItemIdentifier identifier)
        {
            var itemToRemove = selfItems.FirstOrDefault(x => x.identifier.Equals(identifier));

            if (itemToRemove == null)
            {
                return false;
            }

            ItemBounds.Remove(itemToRemove);

            selfItems.Remove(itemToRemove);
            Destroy(itemToRemove.gameObject);

            InvokeObjectRemoved(itemToRemove);

            return true;
        }

        private bool TryFindValidPosition(Bounds bounds, out Vector3 position)
        {
            var obstacles = ItemBounds.Values.ToHashSet();

            var candidatePositions = GenerateCandidatePositions(new Vector2(bounds.size.x, bounds.size.z));

            foreach (var candidate in candidatePositions)
            {
                position = new Vector3(candidate.x, bounds.center.y, candidate.y);
                bounds.center = position;

                if (obstacles.All(x => !x.FullyIntersects(bounds)))
                {
                    return true;
                }
            }

            position = Vector3.zero;
            return false;
        }

        private IEnumerable<Vector2> GenerateCandidatePositions(Vector2 itemSize)
        {
            var halfSize = itemSize / 2;
            halfSize.x *= horizontalPlacement == Horizontal.Left ? 1 : -1;
            halfSize.y *= verticalPlacement == Vertical.Bottom ? 1 : -1;

            var gridPosition = new Vector2(transform.position.x, transform.position.z);
            var modifiedGridMin = gridPosition - size / 2 - halfSize;
            var modifiedGridMax = gridPosition + size / 2 - halfSize;

            var horizontalLines = GetLines(false);
            var verticalLines = GetLines(true);
            var validPositions =
                horizontalLines.SelectMany(x => verticalLines.Select(y => new Vector2(x, y) + halfSize).Where(vector =>
                {
                    if (vector.x < modifiedGridMin.x || vector.x > modifiedGridMax.x)
                    {
                        return false;
                    }

                    if (vector.y < modifiedGridMin.y || vector.y > modifiedGridMax.y)
                    {
                        return false;
                    }

                    return true;
                }));

            return validPositions.OrderBy(first => OrderFunc(first, false))
                .ThenBy(second => OrderFunc(second, true));
        }

        private HashSet<float> GetLines(bool vertical)
        {
            var result = new HashSet<float>();

            var gridPosition = vertical ? transform.position.z : transform.position.x;
            var gridDimension = vertical ? size.y : size.x;

            result.Add(gridPosition - gridDimension / 2);
            result.Add(gridPosition + gridDimension / 2);

            foreach (var bounds in ItemBounds.Values)
            {
                var min = vertical ? bounds.min.z : bounds.min.x;
                var max = vertical ? bounds.max.z : bounds.max.x;

                result.Add(min);
                result.Add(max);
            }

            return result;
        }

        private float OrderFunc(Vector2 vector, bool isInversed)
        {
            var isVertical = prioritizeHorizontal == isInversed;

            var result = isVertical ? vector.y : vector.x;

            var sign = isVertical
                ? verticalPlacement == Vertical.Bottom ? 1 : -1
                : horizontalPlacement == Horizontal.Left ? 1 : -1;

            return result * sign;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;

            var gridPosition = transform.position;
            var gridDimensions = new Vector3(size.x, 0, size.y);

            Gizmos.DrawWireCube(gridPosition, gridDimensions);

            if (!drawLines)
            {
                return;
            }

            Gizmos.color = new Color(1, 0, 0.5f, 1);

            var horizontalLines = GetLines(false);
            var verticalLines = GetLines(true);

            var gridMin = gridPosition - gridDimensions / 2;
            var gridMax = gridPosition + gridDimensions / 2;

            foreach (var x in horizontalLines)
            {
                Gizmos.DrawLine(new Vector3(x, gridPosition.y, gridMin.z), new Vector3(x, gridPosition.y, gridMax.z));
            }

            foreach (var y in verticalLines)
            {
                Gizmos.DrawLine(new Vector3(gridMin.x, gridPosition.y, y), new Vector3(gridMax.x, gridPosition.y, y));
            }
        }
    }
}