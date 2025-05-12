using System;

namespace Commands.Commands
{
    public interface ICommand
    {
        public event Action<bool> OnComplete;

        public bool isCompleted { get; }

        public void Execute(float deltaTime);
    }
}