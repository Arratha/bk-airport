using UnityEngine;

namespace Passenger.Movables
{
    public interface IMovable
    {
        public bool MoveTo(Vector3 destination, float deltaTime);

        public void Rotate(float direction, float deltaTime);
    }
}