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
        public string Header => "Инициализация базы";


        private void menu()
        {
            Header.PrintLineColor(ConsoleColor.Green);
            "------------------------------".PrintLineColor(ConsoleColor.Magenta);
            "1) Инициализировать базу данных".PrintLineColor(ConsoleColor.Yellow);           
            "0) Выход".PrintLineColor(ConsoleColor.Yellow);
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

                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        CreateTable();
                        continue;
                    case ConsoleKey.D0:
                        return;
                    default:
                        continue;
                }
            }
        }

        private void CreateTable()
        {
            using(SqlConnection connection = new SqlConnection(Extension.Extension.GetConnectionString().ConnectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction("init");
                var query = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "Create.sql"));
                try
                {
                    SqlCommand command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.Connection = connection;
                    command.CommandText = query;

                    command.ExecuteNonQuery();

                    transaction.Commit();
                    Console.Clear();
                    "Создание таблиц завершено".PrintLineColor(ConsoleColor.Green);
                    "Нажмите любую клавишу для продолжения...: ".PrintLineColor(ConsoleColor.White);
                    Console.ReadKey();

                }
                catch(Exception ex)
                {
                    Console.Clear();
                    ex.Message.PrintLineColor(ConsoleColor.Red);
                    Console.Write("Произошла ошибка при создании таблиц\r\nНажмите любую клавишу для продолжения...: ");
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
