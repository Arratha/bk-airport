using Commands.Commands;
using Commands.Contexts;

namespace Commands
{
    public interface ICommandExecutor
    {
        public ICompletable EnqueueCommand(ICommandContext context);

        public void StopCommands();
    }
}