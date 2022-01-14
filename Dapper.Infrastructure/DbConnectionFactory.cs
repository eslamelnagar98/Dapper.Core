using System;
using System.Data.SqlClient;

namespace Dapper.Infrastructure
{
    public class DbConnectionFactory 
    {
        public Func<SqlConnection> Connection { get; set; }       
        public DbConnectionFactory(Func<SqlConnection> connection)
        {
            Connection = connection;
        }
    }
}
