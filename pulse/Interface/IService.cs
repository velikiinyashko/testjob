namespace pulse.Interface
{
    public interface IService
    {
        public string Header { get; }
        public Task PrintMenu(CancellationToken cancellationToken = default);

    }
}
