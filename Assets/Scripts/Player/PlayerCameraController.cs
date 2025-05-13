using UnityEngine;

namespace Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        [SerializeField] private Transform cameraTransform;
        private Transform _playerTransform;

        [Space, SerializeField] private float minAngle = -45;
        [SerializeField] private float maxAngle = 35;

        [SerializeField] private float mouseSensitivity;

        private Vector2 _rotation;

        public void SetRotation(Vector2 rotation)
        {
            _rotation = new Vector2(Mathf.Clamp(rotation.x, -1 * maxAngle, -1 * minAngle),
                rotation.y);

            cameraTransform.localRotation = Quaternion.Euler(_rotation.x, 0, 0);

            _playerTransform.eulerAngles = Vector3.up * rotation.y;
        }

        private void Awake()
        {
            _playerTransform = transform;
        }

        private void LateUpdate()
        {
            Rotate();
        }

        private void Rotate()
        {
            var axis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"))
                       * mouseSensitivity;

            var newRotation = new Vector2(_rotation.x - axis.y, _rotation.y + axis.x);

            SetRotation(newRotation);
        }
    }
}