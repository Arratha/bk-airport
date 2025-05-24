using System.Collections;
using Runtime;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Utils.SimpleDI;

namespace Interactive.Search
{
    public class VibrationZone : MonoBehaviour
    {
        [SerializeField, Range(0.01f, 10)] private float radius;
        [SerializeField] private Vector3 offset;
        [Space, SerializeField] private float amplitude = 1f;
        [SerializeField] private float baseVibrationInterval = 0.5f;
        [SerializeField] private float baseVibrationDuration = 0.1f;

        private XRDirectInteractor _leftInteractor;
        private XRDirectInteractor _rightInteractor;

        private Coroutine _leftVibrationCoroutine;
        private Coroutine _rightVibrationCoroutine;

        private void Awake()
        {
            var serviceProvider = ServiceProvider.instance;
            
            _leftInteractor = serviceProvider.Resolve<XRDirectInteractor>(Side.Left);
            _rightInteractor = serviceProvider.Resolve<XRDirectInteractor>(Side.Right);
        }

        private void Update()
        {
            CheckController(_leftInteractor, XRNode.LeftHand, ref _leftVibrationCoroutine);
            CheckController(_rightInteractor, XRNode.RightHand, ref _rightVibrationCoroutine);
        }

        private void CheckController(XRDirectInteractor interactor, XRNode node, ref Coroutine vibrationCoroutine)
        {
            if (interactor == null) return;

            var distance = Vector3.Distance(interactor.transform.position, transform.position + offset);
            var isInZone = distance <= radius;

            if (isInZone && vibrationCoroutine == null)
            {
                Vibrate(node, CalculateAmplitude(distance), GetRandomized(baseVibrationDuration));
                vibrationCoroutine = StartCoroutine(VibrationRoutine(node));
            }
            else if (!isInZone && vibrationCoroutine != null)
            {
                Vibrate(node, 0, 0);
                StopCoroutine(vibrationCoroutine);
                vibrationCoroutine = null;
            }
        }

        private IEnumerator VibrationRoutine(XRNode node)
        {
            while (true)
            {
                var interval = GetRandomized(baseVibrationInterval);

                yield return new WaitForSeconds(interval);

                var interactor = node == XRNode.LeftHand ? _leftInteractor : _rightInteractor;

                if (interactor == null)
                {
                    break;
                }

                var currentDistance = Vector3.Distance(interactor.transform.position, transform.position + offset);

                Vibrate(node, CalculateAmplitude(currentDistance), GetRandomized(baseVibrationDuration));
            }
        }

        private float CalculateAmplitude(float distance)
        {
            return Mathf.Lerp(amplitude, 0.1f, Mathf.Clamp01(distance / radius));
        }

        private float GetRandomized(float value)
        {
            return value * Random.Range(0.7f, 1.3f);
        }

        private void Vibrate(XRNode node, float strength, float duration)
        {
            var device = InputDevices.GetDeviceAtXRNode(node);

            if (device.TryGetHapticCapabilities(out var capabilities) && capabilities.supportsImpulse)
            {
                device.SendHapticImpulse(0u, Mathf.Clamp01(strength), duration);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position + offset, radius);
        }
    }
}