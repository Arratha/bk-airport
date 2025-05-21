using UnityEngine;
using UnityEngine.InputSystem;

namespace Animations
{
    [RequireComponent(typeof(Animator))]
    public class HandAnimationInputProvider : MonoBehaviour
    {
        [SerializeField] private InputActionReference gripAction;

        private Animator _animator;

        private int _gripId;
        private int _triggerId;

        private const string GripName = "Grip";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _gripId = Animator.StringToHash(GripName);
        }

        private void OnEnable()
        {
            gripAction.action.performed += OnGripActionPerformed;
            gripAction.action.canceled += OnGripActionCancelled;
        }

        private void OnDisable()
        {
            gripAction.action.performed -= OnGripActionPerformed;
            gripAction.action.canceled -= OnGripActionCancelled;
        }

        private void OnGripActionPerformed(InputAction.CallbackContext context)
        {
            _animator.SetFloat(_gripId, context.ReadValue<float>());
        }

        private void OnGripActionCancelled(InputAction.CallbackContext context)
        {
            _animator.SetFloat(_gripId, 0);
        }
    }
}