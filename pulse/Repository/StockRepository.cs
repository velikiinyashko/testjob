namespace pulse.Repository
{
    public class StockRepository : IRepository<Stock>
    {
        public Task<bool> Create(Stock entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Stock>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Stock> GetAsync(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Stock entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
