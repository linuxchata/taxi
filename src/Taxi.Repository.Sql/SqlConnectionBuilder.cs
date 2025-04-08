using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Taxi.Repository.Sql;

public static class SqlConnectionBuilder
{
    public static IDbConnection GetConnection(IConfiguration configuration)
    {
        var connectionString = configuration["ConnectionStrings:SqlConnection"];
        return new SqlConnection(connectionString);
    }
}
