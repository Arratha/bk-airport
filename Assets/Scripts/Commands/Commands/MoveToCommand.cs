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
        
        public bool isDisposed => _isDisposed;
        private bool _isDisposed;

        private IMovable _movable;

        private Vector3 _destination;

        public MoveToCommand(MoveToContext context, IMovable movable)
        {
            _movable = movable;
            _destination = context.destination;
        }

        public void Execute(float deltaTime)
        {
            if (_isCompleted)
            {
                return;
            }

            if (_movable.MoveTo(_destination, deltaTime))
            {
                _isCompleted = true;
                OnComplete?.Invoke(true);
            }
        }

        public void Dispose()
        {
            OnComplete = null;
        }
    }
}