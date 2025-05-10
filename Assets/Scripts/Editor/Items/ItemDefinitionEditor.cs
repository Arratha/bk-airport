using Items.Base;
using UnityEditor;
using UnityEngine;

namespace Editor.Items
{
    [CustomEditor(typeof(ItemDefinition))]
    public class ItemDefinitionEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            if (GUILayout.Button("Select Identifier"))
            {
                ((ItemDefinition)target).SelectIdentifier();
            }
        }
    }
}