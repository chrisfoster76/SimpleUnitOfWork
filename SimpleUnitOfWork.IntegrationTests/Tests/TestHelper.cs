namespace SimpleUnitOfWork.IntegrationTests.Tests
{
    public static class TestHelper
    {
        public static string GetConnectionString()
        {
            var settings = new System.Configuration.AppSettingsReader();
            var connectionString = (string)settings.GetValue("DbConnectionString", typeof(string));
            return connectionString;
        }
    }
}
