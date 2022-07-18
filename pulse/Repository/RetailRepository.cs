using System.Data.SqlClient;

namespace pulse.Repository
{
    public class RetailRepository : IRepository<Retail>
    {
        public async Task<bool> Create(Retail entity, CancellationToken cancellationToken = default)
        {
            using (var _connection = new SqlConnection(Extension.Extension.GetConnectionString().ConnectionString))
            {
                await _connection.OpenAsync(cancellationToken);

                SqlTransaction transaction = _connection.BeginTransaction();
                try
                {
                    SqlCommand command = _connection.CreateCommand();
                    command.Connection = _connection;
                    command.Transaction = transaction;

                    command.CommandText = Extension.Extension.GetCreateQuery(entity);

                    await command.ExecuteNonQueryAsync(cancellationToken);

                    transaction.Commit();
                    Console.Clear();
                    $"Торговая точка {entity.Name} успешно добавлен в базу".PrintLineColor(ConsoleColor.Green);
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ex.Message.PrintLineColor(ConsoleColor.Red);
                    Console.Write("Нажмите любую клавишу для продолжения...: ");
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

                    command.CommandText = $"DELETE FROM Party WHERE RetailId={Id}";
                    await command.ExecuteNonQueryAsync(cancellationToken);

                    command.CommandText = $"DELETE FROM Stock WHERE RetailId={Id}";
                    await command.ExecuteNonQueryAsync(cancellationToken);

                    command.CommandText = $"DELETE FROM Retail WHERE RetailId={Id}";
                    await command.ExecuteNonQueryAsync(cancellationToken);

                    transaction.Commit();
                    Console.Clear();
                    $"Торговая точка успешно удалена из базы".PrintLineColor(ConsoleColor.Green);
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ex.Message.PrintLineColor(ConsoleColor.Red);
                    Console.Write("Нажмите любую клавишу для продолжения...: ");
                    Console.ReadKey();
                }
                finally
                {
                    _connection.Close();
                }
            }
            return false;
        }

        public async Task<List<Retail>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            List<Retail> _entity = new();
            using (var _connection = new SqlConnection(Extension.Extension.GetConnectionString().ConnectionString))
            {
                await _connection.OpenAsync(cancellationToken);
                SqlCommand command = _connection.CreateCommand();
                command.Connection = _connection;
                command.CommandText = "SELECT * FROM v_retail";

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            _entity.Add(new()
                            {
                                RetailId = reader.GetValue(0).ToInt(),
                                Name = reader.GetString(1),
                                City = reader.GetString(2),
                                Address = reader.GetString(3),
                                Phone = reader.GetString(4),
                                CountStocks = reader.GetValue(5).ToInt(),
                                CountParty = reader.GetValue(6).ToInt(),
                                SumPrice = reader.GetValue(7).ToDecimal()
                            });
                        }
                    }
                }


            }
            return _entity;
        }

        public Task<List<Retail>> GetAsync(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Retail entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
