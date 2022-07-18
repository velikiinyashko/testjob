namespace pulse.Interface
{
    public interface IRepository<T>
    {
        public Task<List<T>> GetAsync(int Id, CancellationToken cancellationToken = default);
        public Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        public Task<bool> Create(T entity, CancellationToken cancellationToken = default);
        public Task<bool> Update(T entity, CancellationToken cancellationToken = default);
        public Task<bool> Delete(int Id, CancellationToken cancellationToken = default);
    }
}