#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using UnityEngine;

namespace Items.Base
{
    [Flags]
    public enum ItemTags
    {
        None = 0,
        Illegal = 1 << 0,
        Metallic = 1 << 1
    }

    public class ItemDefinition : ScriptableObject
    {
        public ItemIdentifier id => selfId;
        [HideInInspector, SerializeField] private ItemIdentifier selfId;

        public ItemTags tags => selfTags;
        [SerializeField] private ItemTags selfTags;
        
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