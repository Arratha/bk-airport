using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Animations
{
    [RequireComponent(typeof(XRBaseInteractable))]
    public class PokeButtonAnimator : MonoBehaviour
    {
        [SerializeField] private Transform movableTransform;

        [Space, SerializeField, Range(0.1f, 10)]
        private float maxDistanceInLocalCoords;

        [SerializeField, Range(0.1f, 1)] private float maxReturnTime;

        private float _initialPosition;
        private float _speed;

        private XRBaseInteractable _interactable;

        private HashSet<XRPokeInteractor> _interactors = new();

        private void Awake()
        {
            _interactable = GetComponent<XRBaseInteractable>();

            _initialPosition = movableTransform.localPosition.y;

            _speed = maxDistanceInLocalCoords / maxReturnTime;
            
            _interactable.hoverEntered.AddListener(OnHoverEntered);
            _interactable.hoverExited.AddListener(OnHoverExited);
        }

        private void Update()
        {
            var currentPosition = movableTransform.localPosition.y;
            var newPosition = _initialPosition;

            if (_interactors.Any())
            {
                newPosition = _interactors
                    .Select(x => transform.InverseTransformPoint(x.attachTransform.position).y)
                    .Min();
            }

            if (Mathf.Approximately(currentPosition, newPosition))
            {
                return;
            }

            if (newPosition > currentPosition)
            {
                newPosition = Mathf.MoveTowards(currentPosition, newPosition, _speed * Time.deltaTime);
            }

            movableTransform.localPosition =
                Mathf.Clamp(newPosition, _initialPosition - maxDistanceInLocalCoords, _initialPosition) *
                Vector3.up;
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