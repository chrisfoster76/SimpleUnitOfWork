using System;

namespace SimpleUnitOfWork
{
    internal static class PredefinedWaitAndRetryScheduleHelper
    {
        internal static WaitAndRetrySchedule GetWaitAndRetrySchedule(PredefinedWaitAndRetrySchedule schedule)
        {
            if (schedule == PredefinedWaitAndRetrySchedule.NoRetry)
            {
                return new WaitAndRetrySchedule();
            }

            if (schedule == PredefinedWaitAndRetrySchedule.DefaultWeb)
            {
                return new WaitAndRetrySchedule
                {
                    RetryCount = 3,
                    SleepDurationProvider = retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                };
            }

            if (schedule == PredefinedWaitAndRetrySchedule.DefaultJob)
            {
                return new WaitAndRetrySchedule
                {
                    RetryCount = 6,
                    SleepDurationProvider = retryAttempt => TimeSpan.FromSeconds(10 * (retryAttempt+1))
                };
            }

            throw new InvalidOperationException("Invalid schedule");
        }
    }
}
