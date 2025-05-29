using Interactive.Usables;
using Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Utils.SimpleDI;

namespace Player
{
    public class DirectUser : MonoBehaviour
    {
        [SerializeField] private Side side;

        private InputActionReference _useActionReference;

        private XRDirectInteractor _direct;

        private IUsable _usable;

        private void Awake()
        {
            _direct = GetComponentInChildren<XRDirectInteractor>(true);

            _useActionReference = ServiceProvider.instance.Resolve<InputActionReference>((side, PlayerActions.Activate));
            _useActionReference.action.started += OnUsed;

            _direct.selectEntered.AddListener(OnSelected);
            _direct.selectExited.AddListener(OnDeselected);
        }

        private void OnSelected(SelectEnterEventArgs args)
        {
            if (args.interactableObject.transform.TryGetComponent<IUsable>(out var usable))
            {
                _usable = usable;
            }
        }

        private void OnDeselected(SelectExitEventArgs _)
        {
            _usable = null;
        }

        private void OnUsed(InputAction.CallbackContext _)
        {
            _usable?.Use();
        }

        private void OnDestroy()
        {
            _useActionReference.action.started -= OnUsed;

            if (_direct == null)
            {
                return;
            }

            _direct.selectEntered.RemoveListener(OnSelected);
            _direct.selectExited.RemoveListener(OnDeselected);
        }
    }
}