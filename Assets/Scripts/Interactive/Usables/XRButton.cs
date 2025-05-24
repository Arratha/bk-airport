using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Interactive.Usables
{
    [RequireComponent(typeof(XRBaseInteractable))]
    public class XRButton : UsableBehaviour
    {
        [SerializeField, Range(0.1f, 10)]
        private float selectThreshold;
        
        private XRBaseInteractable _interactable;

        private HashSet<XRPokeInteractor> _interactors = new();

        private bool _isPressed;

        private void Awake()
        {
            _interactable = GetComponent<XRBaseInteractable>();

            _interactable.hoverEntered.AddListener(OnHoverEntered);
            _interactable.hoverExited.AddListener(OnHoverExited);
        }

        private void Update()
        {
            var minPosition = selectThreshold + 1;

            if (_interactors.Any())
            {
                minPosition = _interactors
                    .Select(x => transform.InverseTransformPoint(x.attachTransform.position).y)
                    .Min();
            }

            if (minPosition <= selectThreshold && !_isPressed)
            {
                _isPressed = true;
                Use();
            }
            else if (minPosition > selectThreshold && _isPressed)
            {
                _isPressed = false;
                Cancel();
            }
        }

        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            if (args.interactorObject is XRPokeInteractor pokeInteractor)
            {
                _interactors.Add(pokeInteractor);
            }
        }

        private void OnHoverExited(HoverExitEventArgs args)
        {
            if (args.interactorObject is XRPokeInteractor pokeInteractor)
            {
                _interactors.Remove(pokeInteractor);
            }
        }

        private void OnDestroy()
        {
            _interactable.hoverEntered.RemoveListener(OnHoverEntered);
            _interactable.hoverExited.RemoveListener(OnHoverExited);
        }
    }
}