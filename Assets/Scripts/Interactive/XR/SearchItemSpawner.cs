using Extensions;
using Items.Storages;
using Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Utils.SimpleDI;

namespace Interactive.XR
{
    public class SearchItemSpawner : MonoBehaviour
    {
        [SerializeField] private StorageAbstract storage;
        
        [SerializeField, Range(0.01f, 10)] private float radius;
        [SerializeField] private Vector3 offset;
        
        private XRDirectInteractor _leftInteractor;
        private XRDirectInteractor _rightInteractor;

        private InputActionReference _leftGrabReference;
        private InputActionReference _rightGrabReference;

        private bool _isLeftInRadius;
        private bool _isRightInRadius;

        private void Awake()
        {
            var serviceProvider = ServiceProvider.instance;

            storage = GetComponent<StorageAbstract>();
            
            _leftInteractor = serviceProvider.Resolve<XRDirectInteractor>(Side.Left);
            _rightInteractor = serviceProvider.Resolve<XRDirectInteractor>(Side.Right);

            _leftGrabReference = serviceProvider.Resolve<InputActionReference>((Side.Left, PlayerActions.Select));
            _rightGrabReference = serviceProvider.Resolve<InputActionReference>((Side.Right, PlayerActions.Select));
        }

        private void Update()
        {
            _isLeftInRadius = IsInZone(_leftInteractor.transform);
            _isRightInRadius = IsInZone(_rightInteractor.transform);
        }

        private void HandleGrab(InputAction.CallbackContext context)
        {
            var side = context.action.id == _leftGrabReference.action.id ? Side.Left : Side.Right;
            var inRadius = side == Side.Left ? _isLeftInRadius : _isRightInRadius;

            if (!inRadius)
            {
                return;
            }

            var targetInteractor = side == Side.Left ? _leftInteractor : _rightInteractor;

            if (targetInteractor.hasSelection)
            {
                return;
            }

            var item = storage.GetFirstItem();

            if (item == null)
            {
                return;
            }

            var instance = Instantiate(item.GetDefinition().interactivePrefab);
            
            var instanceTransform = instance.transform;
            
            instanceTransform.SetParent(transform);
            instanceTransform.position = targetInteractor.attachTransform.position;
            instanceTransform.rotation = targetInteractor.attachTransform.rotation;

            if (instance.TryGetComponent<IXRSelectInteractable>(out var interactable))
            {
                targetInteractor.interactionManager.SelectEnter(
                    targetInteractor,
                    interactable
                );
            }
        }

        private bool IsInZone(Transform target)
        {
            var distance = Vector3.Distance(target.position, transform.position + offset);
            return distance <= radius;
        }
        
        private void OnEnable()
        {
            _leftGrabReference.action.started += HandleGrab;
            _rightGrabReference.action.started += HandleGrab;
        }

        private void OnDisable()
        {
            _leftGrabReference.action.started -= HandleGrab;
            _rightGrabReference.action.started -= HandleGrab;
        }
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position + offset, radius);
        }
    }
}