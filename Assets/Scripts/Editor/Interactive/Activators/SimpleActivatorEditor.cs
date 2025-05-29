using System;
using System.Linq;
using Interactive.Activators;
using Interactive.Activators.Conditions;
using UnityEditor;
using UnityEngine;

namespace Editor.Interactive.Activators
{
    [CustomEditor(typeof(SimpleActivator))]
    public class SimpleActivatorEditor : UnityEditor.Editor
    {
        private static Type[] _conditionTypes;

        static SimpleActivatorEditor()
        {
            RefreshConditionTypes();
            EditorApplication.projectChanged += RefreshConditionTypes;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space();

            var activator = (SimpleActivator)target;


            foreach (var conType in _conditionTypes)
            {
                if (GUILayout.Button($"Add {conType.Name}"))
                {
                    var condition = (ICondition)Activator.CreateInstance(conType);
                    activator.AddCondition(condition);
                }
            }
        }

        private static void RefreshConditionTypes()
        {
            _conditionTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && !t.IsAbstract && typeof(ICondition).IsAssignableFrom(t))
                .ToArray();
        }
    }
}