using System.Linq;
using Items.Base;
using Items.Storages;
using UnityEngine;
using Usables;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.AdditionalCheck
{
    [RequireComponent(typeof(StorageAbstract))]
    [RequireComponent(typeof(IUsable))]
    public class SearchProvider : MonoBehaviour, IObserver<CheckType>
    {
        private StorageAbstract _storage;
        private IUsable _usable;

        private IObservableState<CheckType> _state;

        public void HandleUpdate(CheckType message)
        {
            _usable.enabled = message == CheckType.AdditionalCheck && _storage.items.Any();
        }

        private void Awake()
        {
            _storage = GetComponent<StorageAbstract>();
            _storage.OnItemAdded += HandleItemsChange;
            _storage.OnItemRemoved += HandleItemsChange;
            
            _usable = GetComponent<IUsable>();
            _usable.OnUsed += HandleUsed;
            
            _state = ServiceProvider.instance.Resolve<IObservableState<CheckType>>();
            _state.RegisterObserver(this);
            
            HandleUpdate(_state.GetState());
        }

        private void HandleItemsChange(ItemIdentifier[] identifiers)
        {
            _usable.enabled = _state.GetState() == CheckType.AdditionalCheck && _storage.items.Any();
        }

        private void HandleUsed()
        {
            var items = _storage.items.ToArray();
            _storage.TryRemoveItem(items);

            var state = ServiceProvider.instance.Resolve<IObservableState<SearchItems>>();
            state.HandleUpdate(new SearchItems { items = items });
        }

        private void OnDestroy()
        {
            _state.UnregisterObserver(this);

            if (_storage != null)
            {
                _storage.OnItemAdded -= HandleItemsChange;
                _storage.OnItemRemoved -= HandleItemsChange;
            }

            if (_usable != null)
            {
                _usable.OnUsed -= HandleUsed;
            }
        }
    }
}