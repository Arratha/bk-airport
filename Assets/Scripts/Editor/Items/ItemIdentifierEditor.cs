using Items.Base;
using UnityEditor;
using UnityEngine;

namespace Editor.Items
{
    [CustomEditor(typeof(ItemIdentifier))]
    public class ItemIdentifierEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Select Definition"))
            {
                var id = (ItemIdentifier)target;
                var definition = GetDefinition(id);

                if (definition == null)
                {
                    Debug.LogWarning("No definition uses this id, it should be deleted.");
                    return;
                }

                Selection.activeObject = definition;
                EditorGUIUtility.PingObject(definition);
            }
            
            if (GUILayout.Button("Select Prefab"))
            {
                var id = (ItemIdentifier)target;
                var definition = GetDefinition(id);

                if (definition == null)
                {
                    Debug.LogWarning("No definition uses this id, it should be deleted.");
                    return;
                }

                if (definition.prefab == null)
                {
                    Debug.LogWarning("Definition has no prefab attached to it.");
                    return;
                }

                Selection.activeObject = definition.prefab;
                EditorGUIUtility.PingObject(definition.prefab);
            }
        }

        private ItemDefinition GetDefinition(ItemIdentifier id)
        {
            var type = typeof(ItemDefinition).Name;
            var guids = AssetDatabase.FindAssets("t:" + type);

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var definition = AssetDatabase.LoadAssetAtPath<ItemDefinition>(path);

                if (definition.id.Equals(id))
                {
                    return definition;
                }
            }

            return null;
        }
    }
}