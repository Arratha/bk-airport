using System;

namespace Commands.Commands
{
    public interface ICompletable : IDisposable
    {
        public bool isCompleted { get; }
        public bool isDisposed { get; }

        public event Action<bool> OnComplete;
    }
}