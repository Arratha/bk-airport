using System;
using UnityEngine;

namespace Commands.Contexts
{
    public class MoveToContext : ICommandContext
    {
        public Action<bool> onComplete => _onComplete;
        private Action<bool> _onComplete;

        public Vector3 destination => _destination;
        private Vector3 _destination;

        public MoveToContext(Action<bool> onComplete, Vector3 destination)
        {
            _onComplete = onComplete;
            _destination = destination;
        }
    }
}