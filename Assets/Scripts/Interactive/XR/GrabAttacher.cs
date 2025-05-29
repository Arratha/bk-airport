using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Interactive.XR
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class GrabAttacher : MonoBehaviour
    {
        private XRGrabInteractable _interactable;

        private Transform _transform;

        private Vector3 _initialPosition;
        private Quaternion _initialRotation;

        private void Awake()
        {
            _interactable = GetComponent<XRGrabInteractable>();

            _transform = transform;
            _initialPosition = _transform.localPosition;
            _initialRotation = _transform.localRotation;
        }

        private void OnDeselect(SelectExitEventArgs args)
        {
            _transform.localPosition = _initialPosition;
            _transform.localRotation = _initialRotation;
        }

        private void OnEnable()
        {
            _interactable.selectExited.AddListener(OnDeselect);
        }

        private void OnDestroy()
        {
            if (_interactable == null)
            {
                return;
            }

            _interactable.selectExited.RemoveListener(OnDeselect);
        }
    }
}