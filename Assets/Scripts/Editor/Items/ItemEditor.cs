using Items.Base;
using UnityEditor;
using UnityEngine;

namespace Editor.Items
{
    [CustomEditor(typeof(Item))]
    public class ItemEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Select Definition"))
            {
                var prefab = PrefabUtility.GetCorrespondingObjectFromSource((Item)target);

                if (prefab == null)
                {
                    Debug.LogWarning("This item has no prefab.");
                    return;
                }

                var definition = GetDefinition(prefab);

                if (definition == null)
                {
                    Debug.LogWarning("No definition uses this prefab.");
                    return;
                }

                Selection.activeObject = definition;
                EditorGUIUtility.PingObject(definition);
            }
            
            if (GUILayout.Button("Select Identifier"))
            {
                var item = (Item)target;

                if (item.identifier == null)
                {
                    Debug.LogWarning("This item has no identifier.");
                    return;
                }

                Selection.activeObject = item.identifier;
                EditorGUIUtility.PingObject(item.identifier);
            }
        }

        private ItemDefinition GetDefinition(Item prefab)
        {
            var type = typeof(ItemDefinition).Name;
            var guids = AssetDatabase.FindAssets("t:" + type);

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var definition = AssetDatabase.LoadAssetAtPath<ItemDefinition>(path);

                if (prefab.Equals(definition.prefab))
                {
                    return definition;
                }
            }

            return null;
        }
    }
}