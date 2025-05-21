using System.Linq;
using Extensions;
using Items.Base;
using Items.Storages.Attachers.Placers;
using UnityEngine;

namespace Items.Storages.Attachers
{
    //Creates instances of given ids prefabs and attaches it to self transform
    public class StorageAttacher : StorageAttacherAbstract
    {
        protected override void Attach(ItemIdentifier[] identifiers)
        {
            foreach (var identifier in identifiers)
            {
                var prefab = identifier.GetDefinition().prefab;
                var instance = Instantiate(prefab, transform);

                if (instance.TryGetComponent<AttachmentPoint>(out var attachmentPoint))
                {
                    instance.transform.localPosition = attachmentPoint.point.position - instance.transform.position;
                }
                else
                {
                    instance.transform.localPosition = Vector3.zero;
                }

                items.Add(instance);
            }
        }

        protected override void Detach(ItemIdentifier[] identifiers)
        {
            foreach (var identifier in identifiers)
            {
                var instance = items.First(x => x.identifier.Equals(identifier));
                items.Remove(instance);

                Destroy(instance.gameObject);
            }
        }
    }
}