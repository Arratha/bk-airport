using Items.Base;
using Items.Storages.Placers;
using Unity.Mathematics;
using UnityEngine;

namespace Items.Storages.Attachers
{
    public class AttacherStorageSimple : AttacherStorageAbstract
    {
        protected override bool TryAddItemInternal(ItemIdentifier identifier)
        {
            var prefab = GetPrefab(identifier);

            var itemToAdd = Instantiate(prefab, transform);

            var itemTransform = itemToAdd.transform;

            if (!itemToAdd.TryGetComponent<AttachmentPoint>(out var point))
            {
                itemTransform.localPosition = Vector3.zero;
                itemTransform.localRotation = quaternion.identity;
            }
            else
            {
                itemTransform.localPosition = point.positionOffset;
                itemTransform.localRotation = quaternion.Euler(point.rotationOffset);
            }

            selfItems.Add(itemToAdd);

            InvokeObjectAdded(itemToAdd);
            
            return true;
        }
    }
}