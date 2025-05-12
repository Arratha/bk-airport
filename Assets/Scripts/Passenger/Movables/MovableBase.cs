using UnityEngine;

namespace Passenger.Movables
{
    public class MovableBase : MonoBehaviour, IMovable
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rotationSpeed;

        public void Move(Vector3 direction, float deltaTime)
        {
            direction.Normalize();

            var modifiedSpeed = moveSpeed * deltaTime;
            var translation = direction * modifiedSpeed;

            transform.Translate(translation);
        }

        public void Rotate(float direction, float deltaTime)
        {
            direction = Mathf.Sign(direction);

            var modifiedSpeed = rotationSpeed * deltaTime;
            var rotation = direction * modifiedSpeed;

            transform.Rotate(Vector3.up, rotation);
        }
    }
}