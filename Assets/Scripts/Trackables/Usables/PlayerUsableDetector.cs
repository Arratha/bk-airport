using System.Linq;
using UnityEngine;
using Usables;
using Utils.Observable;
using Utils.SimpleDI;

namespace Trackables.Usables
{
    //Search for closest usable in front of player
    //Updates focused usable state
    //If any usable is focused allows interaction with it
    public class PlayerUsableDetector : UsableDetector
    {
        [SerializeField] private Transform cameraTransform;

        [Space, SerializeField] private float distance = 5;
        [SerializeField] private float angle = 30;

        private TrackableUsable _currentUsable;

        private IObservableState<FocusedUsable> _state;

        protected override void OnInit()
        {
            var serviceProvider = ServiceProvider.instance;
            _state = serviceProvider.Resolve<IObservableState<FocusedUsable>>();
        }

        private void Update()
        {
            SearchForUsable();
            Use();
        }

        private void SearchForUsable()
        {
            if (Usables == null)
            {
                return;
            }

            TrackableUsable tempUsable = null;

            var currentUsables = Usables.Where(IsVisible).Where(CanHit).ToArray();

            var minAngle = Mathf.Infinity;

            foreach (var currentUsable in currentUsables)
            {
                var difference = currentUsable.transform.position - cameraTransform.position;
                var difAngle = Vector3.Angle(difference, cameraTransform.forward);

                if (difAngle < minAngle)
                {
                    minAngle = difAngle;
                    tempUsable = currentUsable;
                }
            }

            if (_currentUsable != tempUsable)
            {
                _currentUsable = tempUsable;
                _state.HandleUpdate(new FocusedUsable(_currentUsable));
            }
        }

        private void Use()
        {
            if (_currentUsable == null)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                _currentUsable.Use();
            }
        }

        protected override void HandleDisable()
        {
            _currentUsable = null;
            _state.HandleUpdate(new FocusedUsable(_currentUsable));
        }

        private bool IsVisible(TrackableUsable usable)
        {
            var difference = usable.collider.bounds.center - cameraTransform.position;

            if (difference.magnitude > distance)
            {
                return false;
            }

            var difAngle = Vector3.Angle(difference, cameraTransform.forward);

            if (difAngle > angle)
            {
                return false;
            }

            return true;
        }

        private bool CanHit(TrackableUsable usable)
        {
            if (Physics.Linecast(cameraTransform.position, usable.collider.bounds.center, out var hit))
            {
                return hit.collider == usable.collider;
            }

            return false;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying
                || Usables == null)
            {
                return;
            }

            Gizmos.color = Color.red;

            var currentUsables = Usables.Where(IsVisible).Where(CanHit).ToArray();

            foreach (var currentUsable in currentUsables)
            {
                Gizmos.DrawLine(cameraTransform.position, currentUsable.collider.bounds.center);
            }
        }
    }
}