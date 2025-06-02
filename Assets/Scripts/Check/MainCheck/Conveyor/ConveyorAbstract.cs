using System.Collections.Generic;
using Items.Base;
using Items.Storages.Attachers;
using Items.Storages.Placers;
using UnityEngine;

namespace Check.MainCheck.Conveyor
{
    //Places object on grid and moves it
    //The move condition should be defined in inheritors
    public abstract class ConveyorAbstract : MonoBehaviour
    {
        [SerializeField] protected GridMovableStorage grid;

        [Space, SerializeField] protected Vector2 size;
        [SerializeField] protected Vector2 speed;

        [Space, SerializeField] protected bool shouldMove;

        protected Dictionary<Item, AttachmentBounds> ItemBounds = new();

        private void Awake()
        {
            foreach (var itemObject in grid.itemObjects)
            {
                ItemBounds.Add(itemObject, itemObject.GetComponent<AttachmentBounds>());
            }

            grid.OnItemObjectAdded += HandleAddItem;
            grid.OnItemObjectRemoved += HandleRemoveItem;

            OnInit();
        }

        protected virtual void OnInit()
        {

        }

        protected virtual void FixedUpdate()
        {
            if (!shouldMove)
            {
                return;
            }

            var selfBounds = new Bounds(transform.position, new Vector3(size.x, Mathf.Infinity, size.y));

            var objectsToMove = new List<Item>();
            foreach (var kvp in ItemBounds)
            {
                if (!kvp.Value.bounds.Intersects(selfBounds))
                {
                    continue;
                }

                objectsToMove.Add(kvp.Key);
            }

            grid.Move(speed * Time.fixedDeltaTime, objectsToMove);
        }

        protected virtual void HandleAddItem(Item item)
        {
            var attachmentBounds = item.GetComponent<AttachmentBounds>();

            ItemBounds.Add(item, attachmentBounds);
        }

        protected virtual void HandleRemoveItem(Item item)
        {
            ItemBounds.Remove(item);
        }

        private void OnDestroy()
        {
            if (grid != null)
            {
                grid.OnItemObjectAdded += HandleAddItem;
                grid.OnItemObjectRemoved += HandleRemoveItem;
            }

            HandleDestroy();
        }

        protected virtual void HandleDestroy()
        {

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawCube(transform.position, new Vector3(size.x, 0, size.y));
        }
    }
}