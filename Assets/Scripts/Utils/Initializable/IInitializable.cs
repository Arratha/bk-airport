namespace Utils.Initializable
{
    public interface IInitializable
    {
        public bool isInitialized { get; }

        public void Initialize();

        public void Deinitialize();
    }
}