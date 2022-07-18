using System.Data.SqlClient;
namespace pulse.Repository
{

    public class ProductRepository : IRepository<Product>
    {
        public async Task<bool> Create(Product entity, CancellationToken cancellationToken = default)
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
                    $"Товар {entity.Name} успешно добавлен в базу продуктов".PrintLineColor(ConsoleColor.Green);
                    Console.Write("Нажмите любую клавишу для продолжения...: ");
                    Console.ReadKey();
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

                    command.CommandText = $"DELETE FROM Party WHERE ProductId={Id}";
                    await command.ExecuteNonQueryAsync(cancellationToken);

                    command.CommandText = $"DELETE FROM Product WHERE ProductId={Id}";
                    await command.ExecuteNonQueryAsync(cancellationToken);

                    transaction.Commit();
                    Console.Clear();
                    $"Товар успешно удален из базы".PrintLineColor(ConsoleColor.Green);
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

        public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            List<Product> _products = new List<Product>();
            using (var _connection = new SqlConnection(GetConnectionString().ConnectionString))
            {
                await _connection.OpenAsync(cancellationToken);
                SqlCommand command = _connection.CreateCommand();
                command.Connection = _connection;
                command.CommandText = "SELECT * FROM v_product";

                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            _products.Add(new()
                            {
                                ProductId = reader.GetValue(0).ToInt(),
                                Name = reader.GetValue(1).ToString(),
                                RetailId = reader.GetValue(2).ToInt(),
                                Count = reader.GetValue(3).ToInt()
                            }) ;
                        }
                    }
                }


            }
            return _products;

        }

        public Task<List<Product>> GetAsync(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Product entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
