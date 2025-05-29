using System;
using Items.Base;
using Items.Storages;
using UnityEngine;

namespace Interactive.Activators.Conditions
{
    [Serializable]
    public class StorageHasItems : ICondition
    {
        public bool isSatisfied => storage.items.Count > 0;

        [SerializeField] private StorageAbstract storage;

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
            if (!_isInitialized)
            {
                Debug.LogError($"The object {this} is not initialized.");
                return;
            }

            _isInitialized = false;

            if (storage == null)
            {
                return;
            }

            storage.OnItemAdded -= HandleUpdate;
            storage.OnItemRemoved -= HandleUpdate;
        }

        private void HandleUpdate(ItemIdentifier[] items)
        {
            OnChanged?.Invoke();
        }
    }
}