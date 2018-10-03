namespace SimpleUnitOfWork
{
    public enum PredefinedWaitAndRetrySchedule
    {
        /// <summary>
        /// No retry attempts
        /// </summary>
        NoRetry,
        /// <summary>
        /// Default wait and retry schedule for a program with a UI
        /// </summary>
        DefaultWeb,
        /// <summary>
        /// Default wait and retry schedule for a batch program or background process
        /// </summary>
        DefaultJob
    }
}
