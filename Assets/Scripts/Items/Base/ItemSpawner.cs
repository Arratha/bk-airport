using UnityEngine;

namespace Items.Base
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private ItemIdentifier id;

        private void Awake()
        {
            Instantiate();
        }

        [ContextMenu(nameof(Instantiate))]
        private void Instantiate()
        {
            var definition = ItemDatabase.instance.TryGetDefinition(id);

            Instantiate(definition.prefab);
        }
    }
}