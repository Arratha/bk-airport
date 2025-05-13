using System.Collections;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMoveController : MonoBehaviour
    {
        private CharacterController _characterController;
        private Transform _playerTransform;

        [SerializeField] private float movementSpeed = 5;
        [SerializeField] private float fallingSpeed = 10;

        private bool _isMoveLocked;

        public void SetPosition(Vector3 position)
        {
            StartCoroutine(SuspendMovement());

            transform.position = position;
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _playerTransform = transform;
        }

        private void FixedUpdate()
        {
            if (_isMoveLocked)
            {
                return;
            }

            Move();
        }

        private void Move()
        {
            var direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            direction = direction.normalized;

            var motion = _playerTransform.TransformDirection(direction) * Time.fixedDeltaTime * movementSpeed;

            if (!_characterController.isGrounded)
            {
                motion += Vector3.down * Time.fixedDeltaTime * fallingSpeed;
            }

            _characterController.Move(motion);
        }

        private IEnumerator SuspendMovement()
        {
            _isMoveLocked = true;

            yield return new WaitForFixedUpdate();

            _isMoveLocked = false;
        }
    }
}