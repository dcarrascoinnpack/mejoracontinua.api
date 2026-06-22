using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace MejoraContinua.Api.Data;

public class DbConnectionFactory
{
    private readonly IConfiguration _configuration;

    public DbConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public MySqlConnection CreateConnection()
    {
        var connectionString =
            _configuration.GetConnectionString("MySql");

        return new MySqlConnection(connectionString);
    }
}
