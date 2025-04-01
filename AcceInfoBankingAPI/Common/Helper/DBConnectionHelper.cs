using Npgsql;
using Dapper;

namespace AcceInfoBankingAPI.Common.Helper
{
    public class DBConnectionHelper
    {
        private readonly string _connectionString;

        public DBConnectionHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("PostgreSqlConnection");
        }
        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        // 🔹 SELECT Query - returns list
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? parameters = null)
        {
            using var conn = GetConnection();
            return await conn.QueryAsync<T>(sql, parameters);
        }

        // 🔹 SELECT Query - returns single
        public async Task<T?> QuerySingleAsync<T>(string sql, object? parameters = null)
        {
            using var conn = GetConnection();
            return await conn.QuerySingleOrDefaultAsync<T>(sql, parameters);
        }

        // 🔹 INSERT/UPDATE/DELETE
        public async Task<int> ExecuteAsync(string sql, object? parameters = null)
        {
            using var conn = GetConnection();
            return await conn.ExecuteAsync(sql, parameters);
        }
    }
}
