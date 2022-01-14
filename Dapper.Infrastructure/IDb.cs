using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper.Infrastructure
{
    public interface IDb<TEntity>
    {
        Task<int> ExecuteAsync(string sql, TEntity entity);
        Task<IList<TEntity>> SelectAsync(string sql);
    }
}