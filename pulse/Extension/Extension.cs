namespace pulse.Extension
{
    public static class Extension
    {

        public delegate Task GetEvent(CancellationToken cancellationToken = default);
        public record GetEventRecord(string Name, GetEvent Event);

        public static void PrintMenu(this Dictionary<ConsoleKey, GetEventRecord> menu)
        {
            "\r\n------------------------------".PrintLineColor(ConsoleColor.Magenta);
            foreach (var item in menu)
                item.Value.Name.PrintLineColor(ConsoleColor.Yellow);
            "esc) выход".PrintLineColor(ConsoleColor.Yellow);
            "------------------------------\r\n".PrintLineColor(ConsoleColor.Magenta);
        }

        public static System.Data.SqlClient.SqlConnectionStringBuilder GetConnectionString()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder connection = new();
            connection.ConnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB";
            connection.AttachDBFilename = @"C:\USERS\VELIK\SOURCE\REPOS\TESTJOB\PULSE\PULSE.MDF";
            connection.IntegratedSecurity = true;

            return connection;

        }

        /// <summary>
        /// Получение запроса на вставку единичного объекта данных через рефлексию
        /// </summary>
        /// <typeparam name="T">класс сущности</typeparam>
        /// <param name="obj">Сущность с данными для вставки</param>
        /// <returns>Возвращает запрос INSERT в виде строки</returns>
        public static string GetCreateQuery<T>(T obj) where T : class
        {
            //создаем коллекцию для значений 
            List<object> values = new();
            List<object> properties = new();

            //проходимся по полям и получаем значения
            foreach (var p in typeof(T).GetProperties().ToList())
            {
                var value = p.GetValue(obj);
                if (value != null)
                {
                    properties.Add(p.Name);
                    if (value.GetType() == typeof(string))
                        values.Add($"N'{value}'");
                    else if (value.GetType() == typeof(decimal))
                        values.Add(value.ToString().Replace('.', ','));
                    else
                        values.Add(value);
                }
            }

            //формируем sql запрос
            return $"INSERT INTO {typeof(T).Name} ({string.Join(',', properties)}) VALUES ({string.Join(',', values)})";
        }

        public static decimal ToDecimal(this object obj)
        {
            try
            {
                if (obj == null)
                    return 0;

                decimal.TryParse(obj.ToString().Replace('.', ','), out decimal result);
                return result;

            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static int ToInt(this object obj)
        {
            try
            {
                if (obj == null)
                    return 0;

                int.TryParse(obj.ToString(), out var result);
                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
