using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Interactive.Usables
{
    [RequireComponent(typeof(XRBaseInteractable))]
    public class XRUsable : UsableBehaviour
    {
        public override event Action OnUsed
        {
            add
            {
                base.OnUsed += value;

                if (_interactable != null)
                {
                    _interactable.enabled = enabled;
                }
            }
            remove
            {
                base.OnUsed -= value;

                if (_interactable != null)
                {
                    _interactable.enabled = enabled && HasListeners();
                }
            }
        }

        public override event Action OnCancelled
        {
            add
            {
                base.OnCancelled += value;

                if (_interactable != null)
                {
                    _interactable.enabled = enabled;
                }
            }
            remove
            {
                base.OnCancelled -= value;

                if (_interactable != null)
                {
                    _interactable.enabled = enabled && HasListeners();
                }
            }
        }

        private XRBaseInteractable _interactable;

        private void Awake()
        {
            _interactable = GetComponent<XRBaseInteractable>();

            _interactable.selectEntered.AddListener(HandleSelectEntered);
            _interactable.selectExited.AddListener(HandleSelectExited);
        }

        private void OnDestroy()
        {
            if (_interactable == null)
            {
                return;
            }

            _interactable.selectEntered.AddListener(HandleSelectEntered);
            _interactable.selectExited.AddListener(HandleSelectExited);
        }

        private void HandleSelectEntered(SelectEnterEventArgs _)
        {
            Use();
        }

        private void HandleSelectExited(SelectExitEventArgs _)
        {
            Cancel();
        }

        private void OnEnable()
        {
            if (_interactable == null)
            {
                return;
            }

            _interactable.enabled = HasListeners();
        }

        private void OnDisable()
        {
            if (_interactable == null)
            {
                return;
            }

            _interactable.enabled = false;
        }
    }
}