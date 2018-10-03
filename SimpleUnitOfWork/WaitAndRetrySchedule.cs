using System;
using System.Text;

namespace SimpleUnitOfWork
{
    public class WaitAndRetrySchedule
    {
        public int RetryCount { get; set; }
        public Func<int, TimeSpan> SleepDurationProvider { get; set; }
        public override string ToString()
        {
            var result = new StringBuilder("WaitAndRetrySchedule: ");

            if (RetryCount == 0)
            {
                result.Append("No retries");
            }
            else
            {
                result.Append($"{RetryCount} retries at intervals of ");
                for(var r=0; r < RetryCount; r++)
                {
                    var t = SleepDurationProvider.Invoke(r);
                    result.Append($"[{t}]");
                    if (r < RetryCount - 1) { result.Append(",");}
                }
            }
            
            return result.ToString();
        }
    }
}
