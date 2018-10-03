using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using NLog;
using Polly;
using Polly.Retry;

namespace SimpleUnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WaitAndRetrySchedule _retrySchedule;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private Action<int> _onRetryAction;

        public UnitOfWork()
        {
            _retrySchedule = PredefinedWaitAndRetryScheduleHelper.GetWaitAndRetrySchedule(PredefinedWaitAndRetrySchedule.NoRetry);
            _logger.Info(_retrySchedule);
        }

        public UnitOfWork(WaitAndRetrySchedule waitAndRetrySchedule)
        {
            _retrySchedule = waitAndRetrySchedule;
            _logger.Info(_retrySchedule);
        }

        public UnitOfWork(PredefinedWaitAndRetrySchedule schedule)
        {
            _retrySchedule = PredefinedWaitAndRetryScheduleHelper.GetWaitAndRetrySchedule(schedule);
            _logger.Info($"Predefined: [{schedule}] - {_retrySchedule}");
        }

        public async Task ExecuteAsync(Func<Task> action)
        {
            var policy = GetRetryPolicy();

            try
            {
                await policy.ExecuteAsync(async () =>
                {
                    //operations must not promote the transaction to a distributed transaction, eg:
                    //    · When you have multiple connections to different databases.
                    //    · When you have nested connections to the same database.
                    //    · When the ambient transaction is a distributed transaction, and you don’t declare a TransactionScopeOption.RequiresNew.
                    //    · When you invoke another resource manager with a database connection.
                    using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        await action();
                        transaction.Complete();
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"Unit of Work invocation failed: {ex.Message}");
            }
        }

        public UnitOfWork OnRetry(Action<int> action)
        {
            _onRetryAction = action;
            return this;
        }

        private RetryPolicy GetRetryPolicy()
        {
            return Policy
                .Handle<Exception>(ex => ex.IsTransient())
                .WaitAndRetryAsync(_retrySchedule.RetryCount, retryAttempt => _retrySchedule.SleepDurationProvider(retryAttempt),
                    (exception, timespan, retryCount, context) =>
                    {
                        _logger.Warn($"Unit of Work error: ({exception.Message}). Retrying... attempt {retryCount})");
                        _onRetryAction?.Invoke(retryCount);
                    }
                );
        }
    }
}
