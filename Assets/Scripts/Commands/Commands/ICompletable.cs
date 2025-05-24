using System;

namespace Commands.Commands
{
    public interface ICompletable
    {
        public bool isCompleted { get; }
        public event Action<bool> OnComplete;
    }
}