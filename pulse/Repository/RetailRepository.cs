namespace pulse.Repository
{
    public class RetailRepository : IRepository<Retail>
    {
        public Task<bool> Create(Retail entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Retail>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Retail> GetAsync(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Retail entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
