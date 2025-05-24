using Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Utils.SimpleDI;

namespace Player
{
    public class ControlModeResolver : MonoBehaviour
    {
        [SerializeField] private Side side;

        private InputActionReference _reference;

        private XRDirectInteractor _direct;
        private XRRayInteractor _ray;

        private void Awake()
        {
            _direct = GetComponentInChildren<XRDirectInteractor>(true);
            _ray = GetComponentInChildren<XRRayInteractor>(true);

            _reference = ServiceProvider.instance.Resolve<InputActionReference>((side, PlayerActions.Activate));

            _reference.action.started += OnStarted;
            _reference.action.canceled += OnCancelled;

            ChangeControls(true);
        }

        public void OnStarted(InputAction.CallbackContext context)
        {
            ChangeControls(false);
        }


        public void OnCancelled(InputAction.CallbackContext context)
        {
            ChangeControls(true);
        }

        private void ChangeControls(bool isDirect)
        {
            _direct.enabled = isDirect;
            _ray.enabled = !isDirect;
        }

        private void OnDestroy()
        {
            _reference.action.started -= OnStarted;
            _reference.action.canceled -= OnCancelled;
        }
    }
}