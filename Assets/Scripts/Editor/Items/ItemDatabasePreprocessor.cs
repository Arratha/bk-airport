using Items.Base;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Editor.Items
{
    public class ItemDatabasePreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }

        public void OnPreprocessBuild(BuildReport report)
        {
            var database = ItemDatabase.instance;

            database.FillIn();
            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();
        }
    }
}