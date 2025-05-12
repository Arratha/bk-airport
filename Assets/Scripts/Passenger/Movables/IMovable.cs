using UnityEngine;

namespace Passenger.Movables
{
    public interface IMovable
    {
        public GameObject gameObject { get; }

        public void Move(Vector3 direction, float deltaTime);

        public void Rotate(float direction, float deltaTime);
    }
}