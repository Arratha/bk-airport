using System.Collections.Generic;
using System.Linq;
using Items.Base;
using Items.Storages.Placers;
using Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using Utils.SimpleDI;

namespace Check.MainCheck.Conveyor
{
    public class InteractableConveyor : UsableDrivenConveyor
    {
        [Space, SerializeField] private float speedModifier;
        
        private InputActionReference _leftGrabReference;
        private InputActionReference _rightGrabReference;

        private XRDirectInteractor _leftInteractor;
        private XRDirectInteractor _rightInteractor;

        private Item _grabbedItem;
        private Side _grabbedSide;
        private Vector3 _grabbedOffset;

        protected override void OnInit()
        {
            base.OnInit();

            var serviceProvider = ServiceProvider.instance;

            _leftInteractor = serviceProvider.Resolve<XRDirectInteractor>(Side.Left);
            _rightInteractor = serviceProvider.Resolve<XRDirectInteractor>(Side.Right);

            _leftGrabReference = serviceProvider.Resolve<InputActionReference>((Side.Left, PlayerActions.Select));
            _rightGrabReference = serviceProvider.Resolve<InputActionReference>((Side.Right, PlayerActions.Select));
        }

        protected override void FixedUpdate()
        {
            var objectsToMove = new List<Item>();

            if (shouldMove)
            {
                var selfBounds = new Bounds(transform.position, new Vector3(size.x, Mathf.Infinity, size.y));

                foreach (var kvp in ItemBounds)
                {
                    if (!kvp.Value.bounds.Intersects(selfBounds))
                    {
                        continue;
                    }

                    objectsToMove.Add(kvp.Key);
                }

                grid.Move(speed * Time.fixedDeltaTime, objectsToMove);
            }

            if (_grabbedItem == null || objectsToMove.Contains(_grabbedItem))
            {
                return;
            }

            var interactor = _grabbedSide == Side.Left ? _leftInteractor : _rightInteractor;

            var worldSpeedDirection = transform.TransformDirection(new Vector3(speed.x, 0, speed.y)).normalized;

            var interactorCoord = Vector3.Dot(interactor.transform.position, worldSpeedDirection);
            var grabbedCoord = Vector3.Dot(_grabbedItem.transform.position + _grabbedOffset, worldSpeedDirection);

            var sign = Mathf.Sign(interactorCoord - grabbedCoord);
            var resultingSpeed = Mathf.Min(Mathf.Abs(interactorCoord - grabbedCoord),
                speedModifier * speed.magnitude);

            grid.Move(speed.normalized * sign * resultingSpeed * Time.fixedDeltaTime, new List<Item> { _grabbedItem });
        }

        private void HandleGrab(InputAction.CallbackContext context)
        {
            if (_grabbedItem != null)
            {
                return;
            }

            var side = context.action.id == _leftGrabReference.action.id ? Side.Left : Side.Right;
            var interactor = side == Side.Left ? _leftInteractor : _rightInteractor;

            var interactorPosition = interactor.transform.position;

            var grabbedBounds = ItemBounds.FirstOrDefault(x => x.Value.bounds.Contains(interactorPosition));

            if (grabbedBounds.Equals(new KeyValuePair<Item, AttachmentBounds>()))
            {
                return;
            }

            _grabbedSide = side;
            _grabbedItem = grabbedBounds.Key;
            _grabbedOffset = interactorPosition - _grabbedItem.transform.position;
            
            Debug.Log(_grabbedItem);
        }

        private void HandleRelease(InputAction.CallbackContext context)
        {
            if (_grabbedItem == null)
            {
                return;
            }

            var side = context.action.id == _leftGrabReference.action.id ? Side.Left : Side.Right;

            if (side != _grabbedSide)
            {
                return;
            }

            _grabbedItem = null;
        }

        private void OnEnable()
        {
            _leftGrabReference.action.started += HandleGrab;
            _leftGrabReference.action.canceled += HandleRelease;

            _rightGrabReference.action.started += HandleGrab;
            _rightGrabReference.action.canceled += HandleRelease;
        }

        private void OnDisable()
        {
            _leftGrabReference.action.started -= HandleGrab;
            _leftGrabReference.action.canceled -= HandleRelease;

            _rightGrabReference.action.started -= HandleGrab;
            _rightGrabReference.action.canceled -= HandleRelease;
        }
    }
}