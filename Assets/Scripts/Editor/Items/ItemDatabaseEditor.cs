using Items.Base;
using UnityEditor;
using UnityEngine;

namespace Editor.Items
{
    [CustomEditor(typeof(ItemDatabase))]
    public class ItemDatabaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Fill Items"))
            {
                ((ItemDatabase)target).FillIn();
            }
        }
    }
}