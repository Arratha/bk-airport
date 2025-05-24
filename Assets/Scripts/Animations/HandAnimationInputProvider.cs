using Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.SimpleDI;

namespace Animations
{
    [RequireComponent(typeof(Animator))]
    public class HandAnimationInputProvider : MonoBehaviour
    {
        [SerializeField] private Side side;
        
        private InputActionReference _gripActionReference;

        private Animator _animator;

        private int _gripId;
        private int _triggerId;

        private const string GripName = "Grip";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _gripId = Animator.StringToHash(GripName);
            _gripActionReference = ServiceProvider.instance.Resolve<InputActionReference>((side, PlayerActions.Select));
        }

        private void OnEnable()
        {
            _gripActionReference.action.performed += OnGripActionPerformed;
            _gripActionReference.action.canceled += OnGripActionCancelled;
        }

        private void OnDisable()
        {
            _gripActionReference.action.performed -= OnGripActionPerformed;
            _gripActionReference.action.canceled -= OnGripActionCancelled;
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