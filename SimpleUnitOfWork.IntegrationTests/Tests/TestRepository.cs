using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace SimpleUnitOfWork.IntegrationTests.Tests
{
    public class TestRepository
    {
        private static DbConnection GetConnection()
        {
            var settings = new System.Configuration.AppSettingsReader();
            var connectionString = (string)settings.GetValue("DbConnectionString", typeof(string));
            return new SqlConnection(connectionString);
        }

        public async Task NonTransientFailure()
        {
            var sql = $"FaultyStoredProcedure";
            await GetConnection().ExecuteAsync(sql, commandType: CommandType.StoredProcedure);
        }

        public async Task InsertTable1Value(string value)
        {
            var sql = $"InsertTable1Value";
            await GetConnection().ExecuteAsync(sql, new { value }, commandType: CommandType.StoredProcedure);
        }

        public async Task InsertTable2Value(string value)
        {
            var sql = $"InsertTable2Value";
            await GetConnection().ExecuteAsync(sql, new { value }, commandType: CommandType.StoredProcedure);
        }

        public async Task TransientFailure()
        {
            var sql = $"TransientFailureStoredProcedure";
            await GetConnection().ExecuteAsync(sql, commandType: CommandType.StoredProcedure);
        }
    }
}
