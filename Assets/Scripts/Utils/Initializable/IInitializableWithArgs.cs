namespace Utils.Initializable
{
    public interface IInitializable<TArgs>
    {
        public bool isInitialized { get; }

        public void Initialize(TArgs args);

        public void Deinitialize();
    }
}