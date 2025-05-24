using System;
using Commands.Contexts;

namespace Commands.Commands
{
    public class WaitCommand : ICommand
    {
        public event Action<bool> OnComplete;

        public bool isCompleted => _isCompleted;
        private bool _isCompleted;
        
        private float _timeRemains;
        
        public WaitCommand(WaitContext context)
        {
            _timeRemains = context.seconds;
        }

        public void Execute(float deltaTime)
        {
            if (_isCompleted)
            {
                return;
            }

            _timeRemains -= deltaTime;

            if (_timeRemains > 0)
            {
                return;
            }

            _isCompleted = true;
            OnComplete?.Invoke(true);
        }
    }
}