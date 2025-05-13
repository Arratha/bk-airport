using UnityEngine;

namespace Passenger.Movables
{
    public class MovableBase : MonoBehaviour, IMovable
    {
        [SerializeField] private float moveSpeed;

        public bool MoveTo(Vector3 destination, float deltaTime)
        {
            var modifiedSpeed = moveSpeed * deltaTime;

            if (Vector3.Distance(transform.position, destination) <= modifiedSpeed)
            {
                transform.position = destination;

                return true;
            }

            var direction = destination - transform.position;
            direction.Normalize();

            transform.Translate(direction * modifiedSpeed);
            return false;
        }

        public void Rotate(float direction, float deltaTime)
        {

        }
    }
}