using System.Collections.Generic;
using Items.Base;
using UnityEngine;

namespace Check.MainCheck.Conveyor
{
    public enum QueueItemMoveResult
    {
        NotFound,
        InPosition,
        Queued
    }

    public class InteractableConveyor : UsableDrivenConveyor
    {
        [Space, SerializeField] private float speedModifier;

        private List<Item> _queuedItems = new();

        protected override void FixedUpdate()
        {
            var selfTransform = transform;
            var selfBounds = new Bounds(selfTransform.position, new Vector3(size.x, Mathf.Infinity, size.y));

            if (shouldMove)
            {
                var conveyorMove = new List<Item>();

                foreach (var kvp in ItemBounds)
                {
                    if (!kvp.Value.bounds.Intersects(selfBounds))
                    {
                        continue;
                    }

                    conveyorMove.Add(kvp.Key);
                }

                grid.Move(speed * Time.fixedDeltaTime, conveyorMove);
            }

            var worldSpeedDirection = transform.TransformDirection(new Vector3(speed.x, 0, speed.y)).normalized;

            var conveyorCoord = Vector3.Dot(selfBounds.center, worldSpeedDirection);

            foreach (var item in _queuedItems)
            {
                var itemBounds = ItemBounds[item];

                if (itemBounds.bounds.Intersects(selfBounds))
                {
                    continue;
                }

                var itemCoord = Vector3.Dot(itemBounds.bounds.center, worldSpeedDirection);

                var sign = Mathf.Sign(conveyorCoord - itemCoord);
                var resultingSpeed = Mathf.Min(Mathf.Abs(conveyorCoord - itemCoord),
                    speedModifier * speed.magnitude);

                grid.Move(speed.normalized * sign * resultingSpeed * Time.fixedDeltaTime,
                    new List<Item> { item });
            }

            _queuedItems.Clear();
        }

        public QueueItemMoveResult QueueItemMoveToConveyor(Item item)
        {
            if (!ItemBounds.TryGetValue(item, out var attachmentBounds))
            {
                return QueueItemMoveResult.NotFound;
            }

            var selfBounds = new Bounds(transform.position, new Vector3(size.x, Mathf.Infinity, size.y));

            if (attachmentBounds.bounds.Intersects(selfBounds))
            {
                return QueueItemMoveResult.InPosition;
            }

            _queuedItems.Add(item);

            return QueueItemMoveResult.Queued;
        }

        protected override void HandleRemoveItem(Item item)
        {
            base.HandleRemoveItem(item);

            _queuedItems.Remove(item);
        }
    }
}