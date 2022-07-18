using System.Data.SqlClient;

namespace pulse.Repository
{
    public class PartyRepository : IRepository<Party>
    {
        public async Task<bool> Create(Party entity, CancellationToken cancellationToken = default)
        {
            using (var _connection = new SqlConnection(GetConnectionString().ConnectionString))
            {
                await _connection.OpenAsync(cancellationToken);

                SqlTransaction transaction = _connection.BeginTransaction();
                try
                {
                    SqlCommand command = _connection.CreateCommand();
                    command.Connection = _connection;
                    command.Transaction = transaction;
                    command.CommandText = GetCreateQuery(entity);


                    await command.ExecuteNonQueryAsync(cancellationToken);

                    transaction.Commit();
                    Console.Clear();
                    $"Партия успешно добавлен в базу".PrintLineColor(ConsoleColor.Green);
                    Console.Write("Нажмите любую клавишу для продолжения...: ");
                    Console.ReadKey();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ex.Message.PrintLineColor(ConsoleColor.Red);
                    Console.Write("Произошла ошибка при создании таблиц\r\nНажмите любую клавишу для продолжения...: ");
                    Console.ReadKey();
                }
                finally
                {
                    _connection.Close();
                }
            }
            return false;
        }

        public async Task<bool> Delete(int Id, CancellationToken cancellationToken = default)
        {
            using (var _connection = new SqlConnection(GetConnectionString().ConnectionString))
            {
                await _connection.OpenAsync(cancellationToken);

                SqlTransaction transaction = _connection.BeginTransaction();
                try
                {
                    SqlCommand command = _connection.CreateCommand();
                    command.Connection = _connection;
                    command.Transaction = transaction;
                    command.CommandText = $"DELETE FROM Party WHERE PartyId = {Id}";


                    await command.ExecuteNonQueryAsync(cancellationToken);

                    transaction.Commit();
                    Console.Clear();
                    $"Партия успешно удалена".PrintLineColor(ConsoleColor.Green);
                    Console.Write("Нажмите любую клавишу для продолжения...: ");
                    Console.ReadKey();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ex.Message.PrintLineColor(ConsoleColor.Red);
                    Console.Write("Произошла ошибка при создании таблиц\r\nНажмите любую клавишу для продолжения...: ");
                    Console.ReadKey();
                }
                finally
                {
                    _connection.Close();
                }
            }
            return false;
        }

        public async Task<List<Party>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            List<Party> _products = new();
            try
            {
                using (var _connection = new SqlConnection(Extension.Extension.GetConnectionString().ConnectionString))
                {
                    await _connection.OpenAsync(cancellationToken);
                    SqlCommand command = _connection.CreateCommand();
                    command.Connection = _connection;
                    command.CommandText = "SELECT * FROM v_party";

                    using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                _products.Add(new()
                                {
                                    PartyId = reader.GetValue(0).ToInt(),
                                    ProductId = reader.GetValue(1).ToInt(),
                                    RetailId = reader.GetValue(2).ToInt(),
                                    StockId = reader.GetValue(3).ToInt(),
                                    Count = reader.GetValue(4).ToInt(),
                                    Price = reader.GetValue(5).ToDecimal(),
                                    RetailName = reader.GetString(6),
                                    StockName = reader.GetString(7),
                                    ProductName = reader.GetString(8),
                                });
                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                ex.Message.PrintLineColor(ConsoleColor.Red);
                Console.Write("Произошла ошибка при создании таблиц\r\nНажмите любую клавишу для продолжения...: ");
                Console.ReadKey();
            }
            return _products;
        }

        public Task<List<Party>> GetAsync(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Party entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
