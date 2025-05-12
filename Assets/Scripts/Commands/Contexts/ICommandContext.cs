using System;

namespace Commands.Contexts
{
    public interface ICommandContext
    {
        public Action<bool> onComplete { get; }
    }
}