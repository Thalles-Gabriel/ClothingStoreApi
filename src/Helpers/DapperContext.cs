using System.Data;
using Npgsql;

namespace ClothingStore.API.Helpers;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connection;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        var db = Environment.GetEnvironmentVariable("db_database");
        var host = Environment.GetEnvironmentVariable("db_host");
        var user = Environment.GetEnvironmentVariable("db_user");
        var password = Environment.GetEnvironmentVariable("db_password");
        _connection = new NpgsqlConnectionStringBuilder()
        {
            Database = db,
            Host = host,
            Username = user,
            Password = password
        }.ConnectionString;
    }

    public IDbConnection NewConnection() => new NpgsqlConnection(_connection);
}
