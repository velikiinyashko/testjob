using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace pulse.Service
{
    public class InitializeService : IService
    {
        delegate Task GetEvent(CancellationToken cancellationToken = default);

        private record EventMenu(string Text, GetEvent Method);

        private Dictionary<ConsoleKey, EventMenu> _menu;

        public InitializeService()
        {
            _menu = new()
            {
                {ConsoleKey.D1, new("1) Инициализировать базу данных", CreateTable) }
            };
        }

        private void menu()
        {
            "Работа с базой".PrintLineColor(ConsoleColor.Green);
            "------------------------------".PrintLineColor(ConsoleColor.Magenta);
            foreach (var m in _menu)
                m.Value.Text.PrintLineColor(ConsoleColor.Yellow);
            "esc) Выход".PrintLineColor(ConsoleColor.Yellow);
            "------------------------------".PrintLineColor(ConsoleColor.Magenta);
            Console.WriteLine();
            Console.Write("Выберете пункт меню:");
        }

        public async Task PrintMenu(CancellationToken cancellationToken = default)
        {
            while (true)
            {
                Console.Clear();
                menu();
                var input = Console.ReadKey();

                if (input.Key == ConsoleKey.Escape)
                    return;

                if (_menu.ContainsKey(input.Key))
                    await _menu[input.Key].Method(cancellationToken);
            }
        }

        private async Task CreateTable(CancellationToken cancellationToken = default)
        {

            Console.Clear();
            "При инициализаци данные в базе будут уничтожены, вы уверены что хотите продолжить? [y/n]:".PrintLineColor(ConsoleColor.Red);
            var inpud = Console.ReadKey();

            if (inpud.Key == ConsoleKey.N)
                return;

            if (inpud.Key == ConsoleKey.Y)
                using (SqlConnection connection = new SqlConnection(Extension.Extension.GetConnectionString().ConnectionString))
                {
                    await connection.OpenAsync(cancellationToken);
                    SqlTransaction transaction = connection.BeginTransaction("init");
                    try
                    {
                        SqlCommand command = connection.CreateCommand();
                        command.Transaction = transaction;
                        command.Connection = connection;
                        command.CommandText = await File.ReadAllTextAsync(Path.Combine(Environment.CurrentDirectory, "Create.sql"), cancellationToken); ;
                        await command.ExecuteNonQueryAsync(cancellationToken);
                        
                        command.CommandText = await File.ReadAllTextAsync(Path.Combine(Environment.CurrentDirectory, "CreateView.sql"), cancellationToken);
                        await command.ExecuteNonQueryAsync(cancellationToken);
                        await transaction.CommitAsync(cancellationToken);
                        
                        Console.Clear();
                        "Создание таблиц завершено".PrintLineColor(ConsoleColor.Green);
                        "Нажмите любую клавишу для продолжения...: ".PrintLineColor(ConsoleColor.White);
                        Console.ReadKey();

                    }
                    catch (Exception ex)
                    {
                        Console.Clear();
                        ex.Message.PrintLineColor(ConsoleColor.Red);
                        Console.Write("Произошла ошибка при инициализации базы таблиц\r\nНажмите любую клавишу для продолжения...: ");
                        Console.ReadKey();
                        return;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

        }
    }
}
