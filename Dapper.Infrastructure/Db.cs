using Dapper.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Dapper.Infrastructure
{
    public class Db<TEntity> : IDb<TEntity>
    {
        private readonly DbConnectionFactory _dbConnectionFactory;
        private readonly IConfiguration _configuration;

        public Db(IConfiguration configuration)
        {
            _dbConnectionFactory = new(() =>new SqlConnection(_configuration.GetConnectionString("DefaultConnection")));
            _configuration = configuration;
        }

        private async Task<T> CommandAsync<T>(Func<SqlConnection, SqlTransaction, Task<T>> command)
        {
            using (var connection = _dbConnectionFactory.Connection?.Invoke())
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await command?.Invoke(connection, transaction);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                }
            }
        }


        public async Task<IList<TEntity>> SelectAsync(string sql)
        {
            return await CommandAsync<IList<TEntity>>(async (conn, trn) =>
            {
                var result = (await conn.QueryAsync<TEntity>(sql, null, trn)).ToList();
                return result;
            });
        }

        public async Task<int> ExecuteAsync(string sql, TEntity entity)
        {
            return await CommandAsync(async (conn, transaction) =>
            {
                var result = await conn.ExecuteAsync(sql, entity, transaction);
                return result;
            });
        }

        public async Task<int> UpdateAsync(string sql, TEntity entity)
        {
            return await CommandAsync<int>(async (conn, transaction) =>
            {
                var result = await conn.ExecuteAsync(sql, entity, transaction);
                return result;
            });
        }

    }
}
