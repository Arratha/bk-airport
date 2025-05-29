using System;
using System.Linq;
using Trackables.Items;
using UnityEditor;
using UnityEngine;
using Utils.Zones;

namespace Editor.Trackables.Items
{
    [CustomEditor(typeof(MetallicTrackerAbstract), true)]
    public class MetallicTrackerEditor : UnityEditor.Editor
    {
        private static Type[] _zoneTypes;

        static MetallicTrackerEditor()
        {
            RefreshZoneTypes();
            EditorApplication.projectChanged += RefreshZoneTypes;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            var activator = (MetallicTrackerAbstract)target;

            foreach (var zoneType in _zoneTypes)
            {
                if (GUILayout.Button($"Add {zoneType.Name}"))
                {
                    var instance = (IZone)Activator.CreateInstance(zoneType);
                    activator.zone = instance;
                }
            }
        }

        private static void RefreshZoneTypes()
        {
            _zoneTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && typeof(IZone).IsAssignableFrom(t))
                .ToArray();
        }
    }
}