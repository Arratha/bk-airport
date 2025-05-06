using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

namespace Airport.Interactables.Spawners
{
    public class MagicHatSpawner : MonoBehaviour
    {
        [SerializeField] private InputActionReference leftGripAction;
        [SerializeField] private InputActionReference rightGripAction;

        [Space, SerializeField] private XRDirectInteractor leftController;
        [SerializeField] private XRDirectInteractor rightController;

        [Space, SerializeField] private float radius;

        [Space, SerializeField] private XRGrabInteractable prefab;

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

        private void OnEnable()
        {
            leftGripAction.action.performed += OnLeftGrip;
            rightGripAction.action.performed += OnRightGrip;
        }

        private void OnDisable()
        {
            leftGripAction.action.performed -= OnLeftGrip;
            rightGripAction.action.performed -= OnRightGrip;
        }

        private void OnLeftGrip(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() < 0.5f || !_isInRange[XRNode.LeftHand])
            {
                return;
            }

            PutObject(XRNode.LeftHand);
        }

        private void OnRightGrip(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() < 0.5f || !_isInRange[XRNode.RightHand])
            {
                return;
            }

            PutObject(XRNode.RightHand);
        }

        private void ProcessController(XRNode node)
        {
            var targetController = node == XRNode.LeftHand ? leftController : rightController;

            _isInRange[node] = Vector3.Distance(transform.position, targetController.transform.position) <= radius;
        }

        private void PutObject(XRNode node)
        {
            var targetController = node == XRNode.LeftHand ? leftController : rightController;

            if (targetController.hasSelection)
            {
                return;
            }

            var interactable = Instantiate(prefab);

            targetController.interactionManager.SelectEnter(
                targetController,
                interactable as IXRSelectInteractable
            );
        }
    }
}