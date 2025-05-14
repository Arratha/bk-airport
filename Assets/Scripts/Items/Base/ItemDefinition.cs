using System;
using UnityEngine;

namespace Items.Base
{
    [Flags]
    public enum ItemTag
    {
        None = 0,
        Illegal = 1 << 0,
        Metallic = 1 << 1,
        Bag = 1 << 2
    }   

    public class ItemDefinition : ScriptableObject
    {
        public ItemIdentifier id => selfId;
        [HideInInspector, SerializeField] private ItemIdentifier selfId;

        public ItemTag tag => selfTag;
        [SerializeField] private ItemTag selfTag;
        
        public Item prefab;
        private Item _prefabCopy;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }

            ValidatePrefab();
        }

        private void ValidatePrefab()
        {
            if (prefab == null && _prefabCopy == null)
            {
                return;
            }

            if (prefab == null)
            {
                _prefabCopy.SetPrefabId(null);
                _prefabCopy = null;
                return;
            }

            if (_prefabCopy == null)
            {
                prefab.SetPrefabId(selfId);
                _prefabCopy = prefab;
            }
        }
#endif
    }
}