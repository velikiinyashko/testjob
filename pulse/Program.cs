global using pulse.Extension;
global using pulse.Interface;
global using pulse.Model;
global using pulse.Repository;
global using pulse.Service;
internal class Program
{

    record menuProperty(string Text, IService Service);


    private static async Task Main(string[] args)
    {
        Dictionary<ConsoleKey, menuProperty> _modules = new()
        {
            { ConsoleKey.D1, new("1) Работа с товарами", new ProductService()) },
            { ConsoleKey.D2, new("2) Работа с торговыми точками", new RetailService()) },
            { ConsoleKey.D0, new("0) Инициализация базы", new InitializeService())}
        };


        while (true)
        {
            Console.Clear();
            " Меню".PrintLineColor(ConsoleColor.Green);
            "------------------------------".PrintLineColor(ConsoleColor.Magenta);
            foreach (var module in _modules)
                module.Value.Text.PrintLineColor(ConsoleColor.Yellow);
            "------------------------------".PrintLineColor(ConsoleColor.Magenta);

            Console.Write("Выберете пункт меню:");
            var input = Console.ReadKey();

            if (input.Key == ConsoleKey.Escape)
                break;

            if (_modules.ContainsKey(input.Key))
               await _modules[input.Key].Service.PrintMenu();
            
        }

        "Всего доброго!".PrintLineColor(ConsoleColor.Green);
    }
}