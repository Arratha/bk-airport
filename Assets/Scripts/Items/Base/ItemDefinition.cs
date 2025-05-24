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

    //Defines item id, tags and prefab
    //Id managed automatically
    public class ItemDefinition : ScriptableObject
    {
        public ItemIdentifier id => selfId;
        [HideInInspector, SerializeField] private ItemIdentifier selfId;

        public ItemTag tag => selfTag;
        [SerializeField] private ItemTag selfTag;
        
        public Item prefab;
        private Item _prefabCopy;
        
        public Item interactivePrefab;
        private Item _interactivePrefabCopy;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                return;
            }

            ValidatePrefab();
            ValidateInteractivePrefab();
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
        
        private void ValidateInteractivePrefab()
        {
            if (interactivePrefab == null && _interactivePrefabCopy == null)
            {
                return;
            }

            if (interactivePrefab == null)
            {
                _interactivePrefabCopy.SetPrefabId(null);
                _interactivePrefabCopy = null;
                return;
            }

            if (_interactivePrefabCopy == null)
            {
                interactivePrefab.SetPrefabId(selfId);
                _interactivePrefabCopy = interactivePrefab;
            }
        }
#endif
    }
}