using Items.Base;
using UnityEditor;
using UnityEngine;

namespace Editor.Items
{
    public class ModificationProcessor : AssetModificationProcessor
    {
        private static AssetDeleteResult OnWillDeleteAsset(string path, RemoveAssetOptions options)
        {
            var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            if (asset is ItemDefinition definition)
            {
                ItemDatabase.instance.DeleteDefinition(definition);

                var idPath = AssetDatabase.GetAssetPath(definition.id);

                if (!string.IsNullOrEmpty(idPath))
                {
                    AssetDatabase.DeleteAsset(idPath);
                }
            }

            if (asset is ItemIdentifier identifier)
            {
                var canBeDeleted = ItemDatabase.instance.TryGetDefinition(identifier) == null;

                return canBeDeleted ? AssetDeleteResult.DidNotDelete : AssetDeleteResult.FailedDelete;
            }

            return AssetDeleteResult.DidNotDelete;
        }
    }
}