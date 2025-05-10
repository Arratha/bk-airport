#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Items.Base
{
    public class ItemDefinition : ScriptableObject
    {
        public ItemIdentifier id => selfId;
        [HideInInspector, SerializeField] private ItemIdentifier selfId;

        public Item prefab;
        private Item _prefabCopy;

#if UNITY_EDITOR
        public void SelectIdentifier()
        {
            Selection.activeObject = selfId;
            EditorGUIUtility.PingObject(selfId);
        }

        private void OnValidate()
        {
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