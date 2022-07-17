namespace pulse.Service
{
    internal class StockService : IService
    {
        public string Header { private set; get; } = "Склады";
        private IRepository<Stock> _repository;

        public StockService()
        {
            _repository = new StockRepository();
        }

        private void menu()
        {
            Header.PrintLineColor(ConsoleColor.Green);
            "------------------------------".PrintLineColor(ConsoleColor.Magenta);
            "1) Вывести список складов".PrintLineColor(ConsoleColor.Yellow);
            "2) Добавить склад".PrintLineColor(ConsoleColor.Yellow);
            "3) Удалить склад".PrintLineColor(ConsoleColor.Yellow);
            "0) Выход".PrintLineColor(ConsoleColor.Yellow);
            "------------------------------".PrintLineColor(ConsoleColor.Magenta);
        }

        public async Task PrintMenu(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                Console.Clear();
                menu();
                Console.WriteLine();
                Console.Write("Выберете пункт меню:");

                var _inputKey = Console.ReadKey();

                switch (_inputKey.Key)
                {
                    case System.ConsoleKey.D1:
                        await _repository.GetAllAsync(cancellationToken);
                        break;
                    case System.ConsoleKey.D0:
                        Console.Clear();
                        "выход".PrintLineColor(ConsoleColor.Red);
                        return;
                    default:
                        return;
                }
            }
        }
    }
}
