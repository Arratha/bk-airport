using System;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace Player
{
    public class BodyPartPlacer : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float heightOffset;

        private Transform _originTransform;
        private Transform _cameraTransform;

        private bool _isUpdate;
        private bool _isPreRenderer;
        
        private void Awake()
        {
            var xrOrigin = GetComponentInParent<XROrigin>();
            var originCamera = xrOrigin.Camera;
            
            _originTransform = xrOrigin.transform;
            _cameraTransform = originCamera.transform;

            var trackedPoseDriver = originCamera.GetComponent<TrackedPoseDriver>();
            _isUpdate = trackedPoseDriver.updateType == TrackedPoseDriver.UpdateType.Update ||
                        trackedPoseDriver.updateType == TrackedPoseDriver.UpdateType.UpdateAndBeforeRender;
            _isPreRenderer = trackedPoseDriver.updateType == TrackedPoseDriver.UpdateType.BeforeRender ||
                             trackedPoseDriver.updateType == TrackedPoseDriver.UpdateType.UpdateAndBeforeRender;
        }

        private void Update()
        {
            OnUpdate();
        }

        private void OnUpdate()
        {
            if (!_isUpdate)
            {
                return;
            }

            UpdatePosition();
        }

        private void OnPreRenderer()
        {
            if (!_isPreRenderer)
            {
                return;
            }
            
            UpdatePosition();
        }

        private void OnEnable()
        {
            Application.onBeforeRender += OnPreRenderer;
        }

        private void OnDisable()
        {
            Application.onBeforeRender -= OnPreRenderer;
        }

        private void UpdatePosition()
        {
            var originPosition = _originTransform.position;
            var cameraPosition = _cameraTransform.position;

            var targetHeight = originPosition.y +
                               Mathf.Lerp(0, cameraPosition.y - originPosition.y, heightOffset);

            var targetPosition = new Vector3(cameraPosition.x, targetHeight, cameraPosition.z);

            transform.position = targetPosition;
        }
    }
}