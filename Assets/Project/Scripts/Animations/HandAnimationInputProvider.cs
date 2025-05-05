using UnityEngine;
using UnityEngine.InputSystem;

namespace Airport.Animations
{
    [RequireComponent(typeof(Animator))]
    public class HandAnimationInputProvider : MonoBehaviour
    {
        [SerializeField] private InputActionReference gripAction;
        [SerializeField] private InputActionReference triggerAction;

        private Animator _animator;

        private int _gripId;
        private int _triggerId;

        private const string GripName = "Grip";
        private const string TriggerName = "Trigger";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _gripId = Animator.StringToHash(GripName);
            _triggerId = Animator.StringToHash(TriggerName);

            gripAction.action.performed += OnGripActionPerformed;
            triggerAction.action.performed += OnTriggerActionPerformed;
        }

        private void OnEnable()
        {
            gripAction.action.performed += OnGripActionPerformed;
            triggerAction.action.performed += OnTriggerActionPerformed;
        }

        private void OnDisable()
        {
            gripAction.action.performed -= OnGripActionPerformed;
            triggerAction.action.performed -= OnTriggerActionPerformed;
        }

        private void OnGripActionPerformed(InputAction.CallbackContext context)
        {
            _animator.SetFloat(_gripId, context.ReadValue<float>());
        }

        private void OnTriggerActionPerformed(InputAction.CallbackContext context)
        {
            _animator.SetFloat(_triggerId, context.ReadValue<float>());
        }
    }
}