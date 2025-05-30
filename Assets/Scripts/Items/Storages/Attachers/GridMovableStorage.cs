using System.Collections.Generic;
using Extensions;
using Items.Base;
using UnityEngine;

namespace Items.Storages.Attachers
{
    public class GridMovableStorage : GridStorage
    {
        public void Move(Vector2 translation)
        {
            Move(translation, selfItems);
        }

        public void Move(Vector2 translation, List<Item> itemsToMove)
        {
            if (translation == Vector2.zero) return;

            itemsToMove.RemoveAll(x => !selfItems.Contains(x));

            var direction = new Vector3(translation.x, 0, translation.y).normalized;
            var totalDistance = new Vector3(translation.x, 0, translation.y).magnitude;

            foreach (var item in itemsToMove)
            {
                var visited = new HashSet<Item>();

                var pushChain = BuildPushChain(item, direction, visited);
                var maxTranslation = ComputeMaxTranslation(direction, pushChain, totalDistance);

                if (maxTranslation > 0f)
                {
                    var moveVector = direction * maxTranslation;
                    foreach (var obj in pushChain)
                    {
                        obj.transform.position += moveVector;
                        ItemBounds[obj] = new Bounds(ItemBounds[obj].center + moveVector, ItemBounds[obj].size);
                    }
                }
            }
        }

        private List<Item> BuildPushChain(Item root, Vector3 direction, HashSet<Item> visited)
        {
            var chain = new List<Item>();
            var queue = new Queue<Item>();

            queue.Enqueue(root);
            visited.Add(root);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                chain.Add(current);

                var currentBounds = ItemBounds[current];
                var movedBounds = new Bounds(currentBounds.center + direction * 0.001f, currentBounds.size);

                foreach (var other in selfItems)
                {
                    if (visited.Contains(other) || other == current) continue;

                    var otherBounds = ItemBounds[other];

                    if (movedBounds.FullyIntersects(otherBounds))
                    {
                        visited.Add(other);
                        queue.Enqueue(other);
                    }
                }
            }

            return chain;
        }

        private float ComputeMaxTranslation(Vector3 direction, List<Item> pushChain, float maxDistance)
        {
            var allowedDistance = maxDistance;

            foreach (var item in pushChain)
            {
                var bounds = ItemBounds[item];

                var gridLimit = DistanceToGridEdge(bounds, direction);
                allowedDistance = Mathf.Min(allowedDistance, gridLimit);

                foreach (var other in selfItems)
                {
                    if (pushChain.Contains(other)) continue;

                    var otherBounds = ItemBounds[other];
                    var obstacleDist = DistanceToObstacle(bounds, otherBounds, direction);

                    if (obstacleDist >= 0f)
                    {
                        allowedDistance = Mathf.Min(allowedDistance, obstacleDist);
                    }
                }
            }

            return allowedDistance;
        }

        private float DistanceToGridEdge(Bounds bounds, Vector3 direction)
        {
            var min = bounds.min - transform.position;
            var max = bounds.max - transform.position;
            var halfWidth = size.x / 2;
            var halfHeight = size.y / 2;

            var distance = float.PositiveInfinity;

            if (direction.x > 0)
                distance = Mathf.Min(distance, halfWidth - max.x);
            else if (direction.x < 0)
                distance = Mathf.Min(distance, min.x + halfWidth);

            if (direction.z > 0)
                distance = Mathf.Min(distance, halfHeight - max.z);
            else if (direction.z < 0)
                distance = Mathf.Min(distance, min.z + halfHeight);

            return Mathf.Max(0f, distance);
        }

        private float DistanceToObstacle(Bounds a, Bounds b, Vector3 direction)
        {
            if (!BoundsOverlapInOrthogonalAxis(a, b, direction)) return -1f;

            if (direction.x > 0 && a.max.x <= b.min.x)
                return b.min.x - a.max.x;
            if (direction.x < 0 && a.min.x >= b.max.x)
                return a.min.x - b.max.x;

            if (direction.z > 0 && a.max.z <= b.min.z)
                return b.min.z - a.max.z;
            if (direction.z < 0 && a.min.z >= b.max.z)
                return a.min.z - b.max.z;

            return -1f;
        }

        private bool BoundsOverlapInOrthogonalAxis(Bounds a, Bounds b, Vector3 direction)
        {
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                return a.max.z > b.min.z && a.min.z < b.max.z;
            else
                return a.max.x > b.min.x && a.min.x < b.max.x;
        }
    }
}