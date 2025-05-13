using TMPro;
using UnityEngine;
using Usables;
using Utils;
using Utils.Observable;
using Utils.SimpleDI;

namespace UI
{
    public class UsableCanvas : MonoBehaviour, IObserver<FocusedUsable>
    {
        [SerializeField] private TextMeshProUGUI usableLabel;

        private IObservableState<FocusedUsable> _state;
        private const string DefaultLabel = "Использовать";

        public void HandleUpdate(FocusedUsable message)
        {
            if (message.usable == null)
            {
                usableLabel.enabled = false;
                return;
            }

            usableLabel.enabled = true;

            if (message.usable is ILabeled labeled)
            {
                usableLabel.text = labeled.label;
                return;
            }

            usableLabel.text = DefaultLabel;
        }

        private void Awake()
        {
            _state = ServiceProvider.instance.Resolve<IObservableState<FocusedUsable>>();
            _state.RegisterObserver(this);
        }

        private void OnDestroy()
        {
            _state.UnregisterObserver(this);
        }
    }
}