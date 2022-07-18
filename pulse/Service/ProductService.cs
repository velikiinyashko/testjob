global using static pulse.Extension.Extension;
using System.Collections.ObjectModel;

namespace pulse.Service
{
    public class ProductService : IService
    {
        private string Header = "Товары: ";
        private IRepository<Product> _repository;
        private IRepository<Retail> _retailRepository;

        private ObservableCollection<Product> _products = new();

        Dictionary<ConsoleKey, GetEventRecord> _events;

        public ProductService()
        {
            _repository = new ProductRepository();
            _retailRepository = new RetailRepository();

            _events = new()
            {
                {ConsoleKey.D1, new GetEventRecord("1) Добавить товар", this.AddProduct) },
                {ConsoleKey.D2, new GetEventRecord("2) Просмотреть остатки по товарам", this.ViewProducts) },
                {ConsoleKey.D3, new GetEventRecord("3) Удалить товар (также удаляет все партии)", this.DeleteProduct) }
            };
        }

        /// <summary>
        /// Вывод списка аптек
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        private async Task GetAllRetail(CancellationToken cancellation = default)
        {
            var retails = await _retailRepository.GetAllAsync(cancellation);
            foreach (var item in retails)
                $"[{item.RetailId}] {item.Name}".PrintLineColor(ConsoleColor.White);
        }

        public async Task PrintMenu(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                Console.Clear();
                Header.PrintLineColor(ConsoleColor.Green);
                await GetAllProduct();

                _events.PrintMenu();

                Console.Write("Выберете пункт меню:");


                var _inputKey = Console.ReadKey();

                if (_inputKey.Key == ConsoleKey.Escape)
                    return;

                if (_events.ContainsKey(_inputKey.Key))
                    await _events[_inputKey.Key].Event(cancellationToken);
            }

        }

        /// <summary>
        /// Удаление товара
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task DeleteProduct(CancellationToken cancellationToken = default)
        {
            Console.Clear();
            await GetAllProduct(cancellationToken);
            Console.Write("\r\nИд продукта: ");

            var _productId = Console.ReadLine().ToInt();
            while (true)
            {
                try
                {
                    Console.Clear();
                    var item = _products.Where(q => q.ProductId == _productId).First();
                    $"[{item.ProductId}] {item.Name}".PrintLineColor(ConsoleColor.White);
                    "Вы действительно хотите удалить этот товар?\r\nЭто также удалит все партии с ним связаные! [n/y]".PrintLineColor(ConsoleColor.Red);
                    var input = Console.ReadKey();
                    if (input.Key == ConsoleKey.N)
                        return;

                    if (input.Key != ConsoleKey.Y)
                        continue;

                    var res = await _repository.Delete(_productId, cancellationToken);
                    if (res)
                        return;
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    ex.Message.PrintLineColor(ConsoleColor.Red);
                    Console.Write("Для продолжения нажмите любую клавишу...");
                    Console.ReadKey();
                    return;
                }
            }
        }

        /// <summary>
        /// Вывести список всех товаров
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task GetAllProduct(CancellationToken cancellationToken = default)
        {
            _products = new(await _repository.GetAllAsync(cancellationToken));
            foreach (var item in _products.Select(s => new { ProductId = s.ProductId, Name = s.Name }).Distinct().ToList())
                $"[{item.ProductId}] {item.Name}".PrintLineColor(ConsoleColor.White);

        }

        /// <summary>
        /// Просмотр товаров
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ViewProducts(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                Console.Clear();
                await GetAllRetail(cancellationToken);
                Console.Write("Ид торговой точки: ");
                var _retailId = Console.ReadLine().ToInt();

                Console.Clear();
                foreach (var item in _products.Where(q => q.RetailId == _retailId))
                    $"[{item.ProductId}] Наименование {item.Name}; Количество: {item.Count}".PrintLineColor(ConsoleColor.White);

                Console.Write("Для выхода нажмите 'q', либо продолжите просмотр");
                if (Console.ReadKey().Key == ConsoleKey.Q)
                    return;

            }
        }

        /// <summary>
        /// Добавление продукта
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
