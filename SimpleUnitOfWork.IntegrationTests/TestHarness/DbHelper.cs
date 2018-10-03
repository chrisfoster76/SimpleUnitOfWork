using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace SimpleUnitOfWork.IntegrationTests.TestHarness
{
    public static class DbHelper
    {
        private static DbConnection GetConnection()
        {
            var settings = new System.Configuration.AppSettingsReader();
            var connectionString = (string)settings.GetValue("DbConnectionString", typeof(string));
            return new SqlConnection(connectionString);
        }

        public static async Task ClearDown()
        {
            var sql = $"ClearDown";
            await GetConnection().ExecuteAsync(sql, commandType: CommandType.StoredProcedure);
        }

        public static async Task<string> GetTable1Value()
        {
            var sql = $"GetTable1Value";
            var result = await GetConnection().QueryAsync<string>(sql, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }

        public static async Task<string> GetTable2Value()
        {
            var sql = $"GetTable2Value";
            var result = await GetConnection().QueryAsync<string>(sql, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
    }
}
