using Dapper.Core.Entities;
using Dapper.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IConfiguration configuration;
        private IDb<Product> _db;

        public ProductRepository(IConfiguration configuration,IDb<Product> db)
        {
            _db = db;
            this.configuration = configuration;
        }

        public async Task<int> AddAsync(Product entity)
        {
            entity.AddedOn = DateTime.Now;

            var sql = "Insert into Products (Name,Description,Barcode,Rate,AddedOn,ModifiedOn) VALUES (@Name,@Description,@Barcode,@Rate,@AddedOn,@ModifiedOn)";

            var result = await _db.ExecuteAsync(sql, entity);
            return result;

        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Products WHERE Id = @Id";

            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { Id = id });
                return result;
            }
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            var sql = "SELECT * FROM Products";
            var result = await _db.SelectAsync(sql);
            return result.ToList();

        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Products WHERE Id = @Id";

            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryFirstOrDefaultAsync<Product>(sql, new { Id = id });

                return result;
            }
        }

        public async Task<int> UpdateAsync(Product entity)
        {
            var sql = "UPDATE Products SET Name = @Name, Description = @Description, Barcode = @Barcode, Rate = @Rate, ModifiedOn = @ModifiedOn  WHERE Id = @Id";

            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);

                return result;
            }
        }
    }
}
