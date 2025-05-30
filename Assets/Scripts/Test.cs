using Items;
using Items.Storages;
using UnityEngine;

namespace DefaultNamespace
{
    public class Test : MonoBehaviour
    {
        [SerializeField] private ItemsPreset preset;
        [SerializeField] private StorageAbstract storage;

        [ContextMenu(nameof(Add))]
        private void Add()
        {
            foreach (var itemIdentifier in preset.items)
            {
                storage.TryAddItem(itemIdentifier);
            }
        }
    }
}