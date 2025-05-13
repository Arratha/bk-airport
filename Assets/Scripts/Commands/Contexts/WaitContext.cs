namespace Commands.Contexts
{
    public class WaitContext : ICommandContext
    {
        public float seconds => _seconds;
        private float _seconds;

        public WaitContext(float seconds)
        {
            _seconds = seconds;
        }
    }
}