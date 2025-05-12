using System;
using Commands.Contexts;
using Passenger.Movables;
using UnityEngine;

namespace Commands.Commands
{
    public class MoveToCommand : ICommand
    {
        public event Action<bool> OnComplete;

        public bool isCompleted => _isCompleted;
        private bool _isCompleted;

        private IMovable _movable;

        private Vector3 _destination;

        public MoveToCommand(MoveToContext context, IMovable movable)
        {
            _movable = movable;
            _destination = context.destination;

            OnComplete += context.onComplete;
        }

        public void Execute(float deltaTime)
        {
            if (_isCompleted)
            {
                return;
            }

            var movablePosition = _movable.gameObject.transform.position;

            if (Vector3.Distance(movablePosition, _destination) <= 0.01f)
            {
                _movable.gameObject.transform.position = _destination;

                _isCompleted = true;
                OnComplete?.Invoke(true);

                return;
            }

            var direction = (_destination - movablePosition).normalized;

            _movable.Move(direction, deltaTime);
        }
    }
}