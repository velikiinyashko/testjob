namespace pulse.Extension
{
    public static class Extension
    {

        public static System.Data.SqlClient.SqlConnectionStringBuilder GetConnectionString()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder connection = new();
            connection.ConnectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB";
            connection.AttachDBFilename = Path.Combine(Environment.CurrentDirectory, "pulse.mdf");
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
            //получение полей входящей сущности исключая поле идентификатора таблицы
            var properties = typeof(T).GetProperties().Where(q => q.Name != $"{typeof(T).Name}Id").ToList();

            //создаем коллекцию для значений 
            List<object> values = new List<object>();

            //проходимся по полям и получаем значения
            foreach (var prop in properties)
            {
                var value = prop.GetValue(obj);
                if (value.GetType() == typeof(string))
                    values.Add($"N\"{value}\"");
                else
                    values.Add(value);
            }

            //формируем sql запрос
            return $"INSERT INTO {typeof(T).Name} ({string.Join(',', properties.Select(s => s.Name).ToArray())}) VALUES ({string.Join(',', values)})";
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
