namespace pulse.Service
{
    public class ProductService : IService
    {
        public string Header { private set; get; } = "Товары: ";
        private IRepository<Product> _repository;
        delegate Task GetEvent(CancellationToken cancellationToken = default);
        record GetEventRecord(string Name, GetEvent Event);
        Dictionary<ConsoleKey, GetEventRecord> _events;

        public ProductService()
        {
            _repository = new ProductRepository();
            _events = new()
            {
                {ConsoleKey.D1, new GetEventRecord("1) Добавить товар", this.AddProduct) },
                {ConsoleKey.D2, new GetEventRecord("2) Удалить товар (также удаляет все партии)", this.DeleteProduct) }
            };
        }

        private void menu()
        {

        }

        public async Task PrintMenu(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                Console.Clear();
                Header.PrintLineColor(ConsoleColor.Green);
                await GetAllProduct();
                "------------------------------".PrintLineColor(ConsoleColor.Magenta);
                foreach (var item in _events)
                    item.Value.Name.PrintLineColor(ConsoleColor.Yellow);
                "------------------------------".PrintLineColor(ConsoleColor.Magenta);
                Console.WriteLine();
                Console.Write("Выберете пункт меню:");


                var _inputKey = Console.ReadKey();

                if (_inputKey.Key == ConsoleKey.Escape)
                    return;

                if (_events.ContainsKey(_inputKey.Key))
                    await _events[_inputKey.Key].Event(cancellationToken);
            }

        }

        private async Task DeleteProduct(CancellationToken cancellationToken = default)
        {

        }

        private async Task GetAllProduct(CancellationToken cancellationToken = default)
        {
            var products = await _repository.GetAllAsync(cancellationToken);
            foreach (var item in products)
                $"[{item.ProductId}] {item.Name}".PrintLineColor(ConsoleColor.White);

        }

        private async Task AddProduct(CancellationToken cancellationToken = default)
        {
            Console.Clear();
            Console.Write("Введите название продукта: ");
            var productName = Console.ReadLine();

            await _repository.Create(new()
            {
                Name = productName
            }, cancellationToken);

            return;
        }

    }
}
