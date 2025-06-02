using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Interactive.Activators.Conditions;

namespace Interactive.Activators
{
    public class SimpleActivator : MonoBehaviour
    {
        [SerializeField] private List<MonoBehaviour> managedComponents;
        [SerializeReference] private List<ICondition> conditions;

        public void AddCondition(ICondition condition)
        {
            if (Application.isPlaying && !condition.isInitialized)
            {
                condition.Initialize();
            }

            if (conditions == null)
            {
                conditions = new List<ICondition>();
            }

            conditions.Add(condition);
        }

        private void Start()
        {
            conditions.ForEach(x => x.Initialize());
            conditions.ForEach(x => x.OnChanged += UpdateState);
            UpdateState();
        }

        private void UpdateState()
        {
            var isDisabled = conditions.Any(x => !x.isSatisfied);

            managedComponents.ForEach(x => x.enabled = !isDisabled);
        }

        private void OnDestroy()
        {
            conditions.ForEach(x => x.Deinitialize());
        }
    }
}