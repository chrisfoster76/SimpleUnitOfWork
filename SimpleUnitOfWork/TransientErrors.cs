using System.Collections.Generic;

namespace SimpleUnitOfWork
{
    internal static class TransientErrors
    {
        internal static readonly IList<int> Numbers = new List<int>
        {
            // https://docs.microsoft.com/en-us/azure/sql-database/sql-database-develop-error-messages
            // https://docs.microsoft.com/en-us/azure/sql-database/sql-database-connectivity-issues
            4060, 40197, 40501, 40613, 49918, 49919, 49920, 11001,
            -2, 20, 64, 233, 10053, 10054, 10060, 40143
        };

        internal static bool Contains(int number)
        {
            return Numbers.Contains(number);
        }
    }
}
