namespace Commands.Commands
{
    public interface ICommand : ICompletable
    {
        public void Execute(float deltaTime);
    }
}