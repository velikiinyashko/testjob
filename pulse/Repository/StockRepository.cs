using System.Data.SqlClient;

namespace pulse.Repository
{
    public class StockRepository : IRepository<Stock>
    {
        public async Task<bool> Create(Stock entity, CancellationToken cancellationToken = default)
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
                    $"Склад {entity.Name} успешно добавлен в базу".PrintLineColor(ConsoleColor.Green);
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ex.Message.PrintLineColor(ConsoleColor.Red);
                    Console.Write("Произошла ошибка при сохранении данных\r\nНажмите любую клавишу для продолжения...: ");
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

                    command.CommandText = $"DELETE FROM Party WHERE StockId={Id}";
                    await command.ExecuteNonQueryAsync(cancellationToken);

                    command.CommandText = $"DELETE FROM Stock WHERE StockId={Id}";
                    await command.ExecuteNonQueryAsync(cancellationToken);

                    transaction.Commit();
                    Console.Clear();
                    $"Склад успешно удален из базы".PrintLineColor(ConsoleColor.Green);
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ex.Message.PrintLineColor(ConsoleColor.Red);
                    Console.Write("Произошла ошибка при удалении данных\r\nНажмите любую клавишу для продолжения...: ");
                    Console.ReadKey();
                }
                finally
                {
                    _connection.Close();
                }
            }
            return false;
        }

        public async Task<List<Stock>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            List<Stock> _entity = new();
            using (var _connection = new SqlConnection(GetConnectionString().ConnectionString))
            {
                await _connection.OpenAsync(cancellationToken);
                SqlCommand command = _connection.CreateCommand();
                command.Connection = _connection;
                command.CommandText = "SELECT * FROM Stock";

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            _entity.Add(new()
                            {
                                StockId = reader.GetValue(0).ToInt(),
                                RetailId = reader.GetValue(1).ToInt(),
                                Name = reader.GetString(2)
                            });
                        }
                    }
                }


            }
            return _entity;
        }

        public async Task<List<Stock>> GetAsync(int Id, CancellationToken cancellationToken = default)
        {
            List<Stock> _entity = new();
            using (var _connection = new SqlConnection(Extension.Extension.GetConnectionString().ConnectionString))
            {
                await _connection.OpenAsync(cancellationToken);
                SqlCommand command = _connection.CreateCommand();
                command.Connection = _connection;
                command.CommandText = $"SELECT * FROM Stock WHERE StockId = {Id}";

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            _entity.Add(new()
                            {
                                StockId = reader.GetValue(0).ToInt(),
                                RetailId = reader.GetValue(1).ToInt(),
                                Name = reader.GetString(2),
                            });
                        }
                    }
                }


            }
            return _entity;
        }

        public Task<bool> Update(Stock entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
