using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pulse.Repository
{
    public class PartyRepository : IRepository<Party>
    {
        public async Task<bool> Create(Party entity, CancellationToken cancellationToken = default)
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

        public Task<bool> Delete(int Id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Party>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
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
