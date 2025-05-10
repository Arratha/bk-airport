using System.IO;
using Items.Base;
using UnityEditor;
using UnityEngine;

namespace Editor.Items
{
    public static class ItemDefinitionCreator
    {
        [MenuItem("Assets/Create/Items/Item Definition")]
        public static void CreateItemDefinitionWithDialog()
        {
            var defaultName = "NewItemDefinition";
            var path = EditorUtility.SaveFilePanelInProject(
                "Create Item Definition",
                defaultName,
                "asset",
                "Specify the name and location for the new Item Definition"
            );

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var folderPath = Path.GetDirectoryName(path);
            var definitionName = Path.GetFileNameWithoutExtension(path);

            var definition = ScriptableObject.CreateInstance<ItemDefinition>();
            AssetDatabase.CreateAsset(definition, path);

            var identifier = ScriptableObject.CreateInstance<ItemIdentifier>();
            var identifierPath =
                AssetDatabase.GenerateUniqueAssetPath($"{folderPath}/{definitionName}_Identifier.asset");
            AssetDatabase.CreateAsset(identifier, identifierPath);

            var field = typeof(ItemDefinition).GetField("selfId",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
                field.SetValue(definition, identifier);

            EditorUtility.SetDirty(definition);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Selection.activeObject = definition;
        }
    }
}