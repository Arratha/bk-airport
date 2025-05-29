using Runtime;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Utils.SimpleDI;

namespace Interactive.XR
{
    public class VibrationZone : MonoBehaviour
    {
        [SerializeField, Range(0.01f, 10)] private float radius;
        [SerializeField] private Vector3 offset;

        [Space, SerializeField] private float amplitude = 1f;
        [SerializeField] private float duration = 0.1f;

        private XRDirectInteractor _leftInteractor;
        private XRDirectInteractor _rightInteractor;

        private bool _leftInside;
        private bool _rightInside;

        private void Awake()
        {
            var serviceProvider = ServiceProvider.instance;

            _leftInteractor = serviceProvider.Resolve<XRDirectInteractor>(Side.Left);
            _rightInteractor = serviceProvider.Resolve<XRDirectInteractor>(Side.Right);
        }

        private void Update()
        {
            CheckController(_leftInteractor, XRNode.LeftHand, ref _leftInside);
            CheckController(_rightInteractor, XRNode.RightHand, ref _rightInside);
        }

        private void CheckController(XRDirectInteractor interactor, XRNode node, ref bool previousInside)
        {
            var distance = Vector3.Distance(interactor.transform.position, transform.position + offset);
            var isInside = distance <= radius;

            if (isInside == previousInside)
            {
                return;
            }

            previousInside = isInside;

            if (isInside)
            {
                Vibrate(node);
            }
        }

        private void Vibrate(XRNode node)
        {
            Debug.Log($"Controller {node} is vibrating with amplitude {amplitude} and duration {duration}");
            
            var device = InputDevices.GetDeviceAtXRNode(node);

            if (device.TryGetHapticCapabilities(out var capabilities) && capabilities.supportsImpulse)
            {
                device.SendHapticImpulse(0u, amplitude, duration);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + offset, radius);
        }
    }
}