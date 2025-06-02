using Extensions;
using Items.Base;
using UnityEngine;

namespace Trackables.Items
{
    [RequireComponent(typeof(Item))]
    [DisallowMultipleComponent]
    public class MetallicItemTrackable : MetallicTrackableAbstract
    {
        protected override void OnInit()
        {
            var item = GetComponent<Item>();
            var definition = item.identifier.GetDefinition();

            if ((definition.tag & ItemTag.Metallic) == 0)
            {
                Destroy(this);
            }
        }
    }
}