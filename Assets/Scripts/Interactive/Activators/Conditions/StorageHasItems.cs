using System;
using Items.Base;
using Items.Storages;
using UnityEngine;

namespace Interactive.Activators.Conditions
{
    [Serializable]
    public class StorageHasItems : ICondition
    {
        public bool isSatisfied => !isInversed == storage.items.Count > 0;

        [SerializeField] private StorageAbstract storage;
        [SerializeField] private bool isInversed;

        public bool isInitialized => _isInitialized;
        private bool _isInitialized;

        public event Action OnChanged;

        public void Initialize()
        {
            if (_isInitialized)
            {
                Debug.LogError($"The object {this} has already been initialized. Re-initialization is not possible.");
                return;
            }

            _isInitialized = true;

            storage.OnItemAdded += HandleUpdate;
            storage.OnItemRemoved += HandleUpdate;
        }

        public void Deinitialize()
        {
            _isInitialized = false;

            if (storage == null)
            {
                return;
            }

            storage.OnItemAdded -= HandleUpdate;
            storage.OnItemRemoved -= HandleUpdate;
        }

        private void HandleUpdate(ItemIdentifier items)
        {
            OnChanged?.Invoke();
        }
    }
}