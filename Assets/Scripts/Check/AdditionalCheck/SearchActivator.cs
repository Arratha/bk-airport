using System.Linq;
using Interactive.Search;
using Items.Base;
using Items.Storages;
using UnityEngine;
using Utils.Observable;
using Utils.SimpleDI;

namespace Check.AdditionalCheck
{
    //Automatically disables interaction when not in AdditionalCheck mode
    //Requires both storage capacity and items to enable interaction
    //Transfers all items to inspection when searched
    [RequireComponent(typeof(StorageAbstract))]
    [RequireComponent(typeof(VibrationZone))]
    [RequireComponent(typeof(SearchItemSpawner))]
    public class SearchActivator : MonoBehaviour, IObserver<CheckType>
    {
        private StorageAbstract _storage;

        private VibrationZone _vibrationZone;
        private SearchItemSpawner _itemSpawner;

        private IObservableState<CheckType> _state;

        public void HandleUpdate(CheckType message)
        {
            var isEnabled = message == CheckType.AdditionalCheck && _storage.items.Any();

            _vibrationZone.enabled = isEnabled;
            _itemSpawner.enabled = isEnabled;
        }

        private void Awake()
        {
            _storage = GetComponent<StorageAbstract>();

            _vibrationZone = GetComponent<VibrationZone>();
            _itemSpawner = GetComponent<SearchItemSpawner>();

            _storage.OnItemAdded += HandleItemsChange;
            _storage.OnItemRemoved += HandleItemsChange;

            _state = ServiceProvider.instance.Resolve<IObservableState<CheckType>>();
            _state.RegisterObserver(this, true);
        }

        private void HandleItemsChange(ItemIdentifier[] identifiers)
        {
            if (_state.TryGetState(out var type))
            {
                var isEnabled = type == CheckType.AdditionalCheck && _storage.items.Any();

                _vibrationZone.enabled = isEnabled;
                _itemSpawner.enabled = isEnabled;
            }
        }

        private void OnDestroy()
        {
            _state.UnregisterObserver(this);

            if (_storage != null)
            {
                _storage.OnItemAdded -= HandleItemsChange;
                _storage.OnItemRemoved -= HandleItemsChange;
            }
        }
    }
}