using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Airport.Project.Scripts.Interactables.Vibrations
{
    public class VibrationsSource : MonoBehaviour
    {
        [SerializeField] private float amplitude;
        [SerializeField] private float radius;

        private Dictionary<XRNode, bool> _isInRange = new()
        {
            { XRNode.LeftHand, false },
            { XRNode.RightHand, false }
        };

        private void Update()
        {
            ProcessController(XRNode.LeftHand);
            ProcessController(XRNode.RightHand);
        }

        private void ProcessController(XRNode node)
        {
            var device = InputDevices.GetDeviceAtXRNode(node);

            var isInRange = device.TryGetFeatureValue(CommonUsages.devicePosition, out var position)
                            && Vector3.Distance(transform.position, position) <= radius;

            if (_isInRange[node] == isInRange)
            {
                return;
            }

            _isInRange[node] = isInRange;

            if (isInRange)
            {
                StartCoroutine(ContinuousShaking(node));
            }
            else
            {
                Shake(node, 0);
            }
        }

        private IEnumerator ContinuousShaking(XRNode node)
        {
            while (_isInRange[node])
            {
                Shake(node, 1);

                yield return new WaitForSeconds(1);
            }
        }

        private void Shake(XRNode node, float duration)
        {
            var device = InputDevices.GetDeviceAtXRNode(node);

            if (device.TryGetHapticCapabilities(out var capabilities) &&
                capabilities.supportsImpulse)
            {
                device.SendHapticImpulse(0u, amplitude, duration);
            }
        }
    }
}