namespace Usables
{
    public class FocusedUsable
    {
        public IUsable usable => _usable;
        private IUsable _usable;

        public FocusedUsable(IUsable usable)
        {
            _usable = usable;
        }
    }
}