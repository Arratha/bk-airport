using UnityEngine;

namespace Commands.Contexts
{
    public class MoveToContext : ICommandContext
    {
        public Vector3 destination => _destination;
        private Vector3 _destination;

        public MoveToContext(Vector3 destination)
        {
            _destination = destination;
        }
    }
}