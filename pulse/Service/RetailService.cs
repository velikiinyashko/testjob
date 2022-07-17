namespace pulse.Service
{
    public class RetailService : IService
    {
        public string Header => "Торговые точки";

        private readonly IRepository<Retail> _repository;

        private void menu()
        {
            Header.PrintLineColor(ConsoleColor.Green);
            "------------------------------".PrintLineColor(ConsoleColor.Magenta);
            "1) Вывести список действующих точек".PrintLineColor(ConsoleColor.Yellow);
            "2) Добавить новую точку".PrintLineColor(ConsoleColor.Yellow);
            "3) Удалить точку".PrintLineColor(ConsoleColor.Yellow);
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
