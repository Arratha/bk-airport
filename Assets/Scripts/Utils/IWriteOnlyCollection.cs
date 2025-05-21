namespace Utils
{
    public interface IWriteOnlyCollection<T>
    {
        public void Add(T obj);

        public void Remove(T obj);
    }
}