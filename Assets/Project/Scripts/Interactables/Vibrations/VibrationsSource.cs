using System.Collections;
using UnityEngine;
using UnityEngine.XR;

namespace Airport.Interactables.Vibrations
{
    public class VibrationsSource : MonoBehaviour
    {
        [SerializeField] private float duration = 0.2F;
        [SerializeField] private float amplitude;
        [SerializeField] private float radius;
        [SerializeField] private Transform leftHand;
        [SerializeField] private Transform rightHand;

        private float _leftFactor;
        private float _rightFactor;
        
        private float _leftFactorOld;
        private float _rightFactorOld;

        private void Awake()
        {
            StartCoroutine(ContinuousShaking());
        }

        private void Update()
        {
            ProcessController(XRNode.LeftHand);
            ProcessController(XRNode.RightHand);
        }

        private void ProcessController(XRNode node)
        {
            var target = node == XRNode.LeftHand ? leftHand : rightHand;
            var distance = Vector3.Distance(transform.position, target.position);
            var factor = 1 - Mathf.Clamp01(distance / radius);

            if (node == XRNode.LeftHand)
            {
                _leftFactor = factor;
            }
            else
            {
                _rightFactor = factor;
            }
        }

        private IEnumerator ContinuousShaking()
        {
            while (true)
            {
                if (!Mathf.Approximately(_leftFactor, _leftFactorOld))
                {
                    Shake(XRNode.LeftHand, amplitude * _leftFactor);
                }

                if (!Mathf.Approximately(_rightFactorOld, _rightFactor))
                {
                    Shake(XRNode.RightHand, amplitude * _rightFactor);
                }

                _leftFactorOld = _leftFactor;
                _rightFactorOld = _rightFactor;
                
                yield return new WaitForSeconds(duration * 2.3F);
            }
        }

        private void Shake(XRNode node, float factor)
        {
            var device = InputDevices.GetDeviceAtXRNode(node);

            if (device.TryGetHapticCapabilities(out var capabilities) &&
                capabilities.supportsImpulse)
            {
                device.SendHapticImpulse(0u, factor, duration);
            }
        }
    }
}