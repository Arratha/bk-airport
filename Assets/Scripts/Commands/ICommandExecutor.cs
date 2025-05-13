using System;
using Commands.Commands;
using Commands.Contexts;

namespace Commands
{
    public interface ICommandExecutor : IDisposable
    {
        public ICompletable EnqueueCommand(ICommandContext context);

        public void StopCommands();
    }
}