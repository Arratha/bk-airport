using Runtime;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using Utils.SimpleDI;

namespace Trackables.Items
{
    [RequireComponent(typeof(IXRSelectInteractable))]
    [RequireComponent(typeof(AudioSource))]
    public class HandheldMetallicTracker : MetallicTrackerAbstract
    {
        private AudioSource _source;

        [Space, SerializeField] private float pitchRandomFactor = 0.02f;
        private float _pitch;

        [Space, SerializeField] private float amplitude = 1f;
        [SerializeField] private float duration = 0.1f;

        private IXRSelectInteractable _interactable;

        private XRDirectInteractor _leftInteractor;
        private XRDirectInteractor _rightInteractor;
        
        private XRNode? _interactorNode;
        
        protected override void OnInit()
        {
            var serviceProvider = ServiceProvider.instance;

            _leftInteractor = serviceProvider.Resolve<XRDirectInteractor>(Side.Left);
            _rightInteractor = serviceProvider.Resolve<XRDirectInteractor>(Side.Right);
            
            _source = GetComponent<AudioSource>();
            _pitch = _source.pitch;

            _interactable = GetComponent<IXRSelectInteractable>();

            _interactable.selectEntered.AddListener(HandleSelected);
            _interactable.selectExited.AddListener(HandleDeselected);

            enabled = _interactable.isSelected;
        }

        protected override void OnDetected()
        {
            if (_source.isPlaying)
            {
                return;
            }

            _source.pitch = _pitch * (1 + Random.Range(-pitchRandomFactor, pitchRandomFactor));
            _source.Play();

            Vibrate();
        }

        private void HandleSelected(SelectEnterEventArgs args)
        {
            enabled = true;
            _interactorNode = null;

            var interactor = args.interactorObject;

            if (interactor.Equals(_leftInteractor))
            {
                _interactorNode = XRNode.LeftHand;
                return;
            }

            if (interactor.Equals(_rightInteractor))
            {
                _interactorNode = XRNode.RightHand;
            }
        }

        private void HandleDeselected(SelectExitEventArgs args)
        {
            enabled = false;
        }

        private void Vibrate()
        {
            if (_interactorNode == null)
            {
                return;
            }

            Debug.Log($"Controller {_interactorNode} is vibrating with amplitude {amplitude} and duration {duration}");

            var device = InputDevices.GetDeviceAtXRNode((XRNode)_interactorNode);

            if (device.TryGetHapticCapabilities(out var capabilities) && capabilities.supportsImpulse)
            {
                device.SendHapticImpulse(0u, amplitude, duration);
            }
        }

        private void OnDestroy()
        {
            if (_interactable == null)
            {
                return;
            }

            _interactable.selectEntered.RemoveListener(HandleSelected);
            _interactable.selectExited.RemoveListener(HandleDeselected);
        }
    }
}