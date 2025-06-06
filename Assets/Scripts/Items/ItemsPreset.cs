using System.Collections.Generic;
using Items.Base;
using UnityEngine;

namespace Items
{
    //Collection of items used by passenger generators
    [CreateAssetMenu(menuName = "Items/Preset")]
    public class ItemsPreset : ScriptableObject
    {
        public IReadOnlyCollection<ItemIdentifier> items => selfItems;
        [SerializeField] private List<ItemIdentifier> selfItems;
    }
}