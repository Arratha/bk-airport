using System;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Utils
{
    public abstract class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    GetOrCreateInstance();
                }

                return _instance;
            }
        }

        private static T _instance;

        private const string Folder = "Singletons";

        private static void GetOrCreateInstance()
        {
#if UNITY_EDITOR
            var type = typeof(T).Name;
            var guids = AssetDatabase.FindAssets("t:" + type);

            CheckForMultipleInstances();

            if (guids.Length == 1)
            {
                GetInstance(guids[0]);
            }
            else
            {
                _instance = CreateInstance();
            }
#else
            var type = typeof(T).Name;
            var instances = Resources.LoadAll<T>(Folder);

            if (instances.Length == 0)
            {
                throw new Exception($"No instance of {type} found.");
            }

            if (instances.Length > 1)
            {
                throw new Exception($"More than one instance of {type} exists.");
            }

            _instance = instances[0];
#endif
        }


#if UNITY_EDITOR
        private void OnEnable()
        {
            CheckForMultipleInstances();
        }

        private static void GetInstance(string guid)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            _instance = AssetDatabase.LoadAssetAtPath<T>(path);
        }

        private static void CheckForMultipleInstances()
        {
            var type = typeof(T).Name;
            var guids = AssetDatabase.FindAssets("t:" + type);

            if (guids.Length <= 1)
            {
                return;
            }

            var paths = string.Join("\n", guids.Select(x => AssetDatabase.GUIDToAssetPath(x)));

            throw new Exception($"Several instances of type {typeof(T).Name} exists. Delete all but one.\n{paths}");
        }

        private static T CreateInstance()
        {
            var type = typeof(T).Name;

            var so = CreateInstance<T>();

            var directory = $"Assets/Resources/{Folder}";
            Directory.CreateDirectory(directory);


            var name = $"{type}.asset";
            var path = Path.Combine(directory, name);

            AssetDatabase.CreateAsset(so, path);
            AssetDatabase.SaveAssets();

            Debug.Log($"New object of type {type} was created at {path}");

            return so;
        }
#endif
    }
}