using System;
using System.Data.SqlClient;

namespace SimpleUnitOfWork
{
    internal static class Extensions
    {
        internal static bool IsTransient(this Exception ex)
        {
            switch (ex)
            {
                case SqlException exception:
                    return TransientErrors.Contains(exception.Number);
                case TimeoutException _:
                    return true;
            }

            return false;
        }
    }
}
